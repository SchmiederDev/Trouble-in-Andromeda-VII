using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheGame : MonoBehaviour
{
    public static TheGame theGameInst;

    public GameTimer Timer;

    public bool MissionCanBegin = false;
    public delegate void OnMissionCanBegin();
    public OnMissionCanBegin onMissionCanBegin;

    public delegate void OnLevelLoad();
    public OnLevelLoad onLevelLoad;

    public UnionFighter PlayerUnionFighter;

    private int totalXP = 0;
    private const int maxXP = 5000000;

    public List<GameObject> Enemies;
    public List<GameObject> Pickables;

    [SerializeField]
    private List<GameObject> ActiveEnemies;

    [SerializeField]
    public List<GameObject> ActiveAllies;

    public List<Level> GameLevels;

    [SerializeField]
    public Level ActiveLevel { get; private set; }

    private int MaxLevelIndex;

    [SerializeField]
    private ObjectiveStarter LevelObjectiveStarter;

    [SerializeField]
    private StandardSpawner CollectableSpawner;

    [SerializeField]
    private EnemySpawner MainEnemySpawner;

    public AudioManager audioManager;

    [SerializeField]
    private BarSlider IntegritySlider;

    [SerializeField]
    private BarSlider ShieldSlider;

    [SerializeField]
    private BarSlider EnergySlider;

    private int CurrentLevelThreshold = 500;
    private int CurrentLevelIndex = 0;

    private bool ReadyForNextLevel = false;

    private FlashText FlashMessage;

    public delegate void OnActiveEnemiesChanged();
    public OnActiveEnemiesChanged onActiveEnemiesChanged;

    public delegate void OnActiveAlliesChanged();
    public OnActiveAlliesChanged onActiveAlliesChanged;

    // Start is called before the first frame update
    void Awake()
    {
        if (theGameInst == null)
        {
            theGameInst = this;
        }

        else 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Init_TheGame();
    }

    private void Init_TheGame()
    {
        FlashMessage = GetComponentInChildren<FlashText>();
        Timer = GetComponentInChildren<GameTimer>();
        Timer.Init_Timer();
        MaxLevelIndex = SceneManager.sceneCountInBuildSettings;
        audioManager = GetComponentInChildren<AudioManager>();
        Init_GameElements_UI_Association();
        ActiveEnemies = new List<GameObject>();
        FirstLevelLoad();
    }

    private void Init_GameElements_UI_Association()
    {
        IntegritySlider.Set_MaxBarValue(PlayerUnionFighter.IntegrityMax);
        ShieldSlider.Set_MaxBarValue(PlayerUnionFighter.ShieldPowerMax);
        EnergySlider.Set_MaxBarValue(PlayerUnionFighter.EnergyMax);

        LevelObjectiveStarter = GetComponentInChildren<ObjectiveStarter>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        CheckLevelState();
    }

    private void UpdateStats()
    {
        IntegritySlider.Set_BarValue(PlayerUnionFighter.StructuralIntegrity);
        ShieldSlider.Set_BarValue(PlayerUnionFighter.ShieldPower);
        EnergySlider.Set_BarValue(PlayerUnionFighter.Energy);
    }

    private void CheckLevelState()
    {
        if(ActiveLevel.MissionObjectiveTime)
        {
            if (Timer.minutes >= ActiveLevel.TimeThreshold)
            {
                if (ActiveLevel.hasGatherObjective)
                {
                    if (PlayerUnionFighter.Get_XP() >= ActiveLevel.XPThreshold)
                    {
                        Debug.Log("Mission goal accomplished");
                        ReadyForNextLevel = true;                        
                    }
                        
                    else
                    {
                        StartLevel();
                        ReadyForNextLevel = false;                        
                    }
                }

                else 
                    ReadyForNextLevel = true;
            }

            else
                ReadyForNextLevel = false;
        }

        else
        {
            if (PlayerUnionFighter.Get_XP() >= CurrentLevelThreshold)
                ReadyForNextLevel = true;

            else
                ReadyForNextLevel = false;
        }

        if(ReadyForNextLevel)
        {
            CurrentLevelIndex++;

            if(CurrentLevelIndex < SceneManager.sceneCountInBuildSettings)
                LoadNextLevel();

            else
                CurrentLevelIndex = MaxLevelIndex;
        }
    }

    private void FirstLevelLoad()
    {
        CollectableSpawner = GetComponentInChildren<StandardSpawner>();
        MainEnemySpawner = GetComponentInChildren<EnemySpawner>();
        ActiveLevel = GameLevels[0];
        CurrentLevelThreshold = ActiveLevel.XPThreshold;
        PlayerUnionFighter.ResetWeapons();
        Init_Spawners();
    }

    public void StartLevel()
    {
        onLevelLoad.Invoke();

        ReadyForNextLevel = false;
        MissionCanBegin = false;
        onMissionCanBegin.Invoke();

        Timer.StopAllCoroutines();
        Timer.Reset_Timer();
        Timer.Init_Timer();

        SetActiveLevel();
        PlayerUnionFighter.Reset_UnionFighter_OnLevelLoad();

        Reset_Spawners();
        Init_Spawners();

        Reset_LevelObjective();

        SceneManager.LoadScene(CurrentLevelIndex);
        Start_MissionObjective();
    }

    private void LoadNextLevel()
    {
        int checkSum = totalXP + PlayerUnionFighter.Get_XP();

        if (checkSum <= maxXP)
            totalXP += PlayerUnionFighter.Get_XP();
        else
            totalXP = maxXP;

        Save();

        StartLevel();        
    }

    public bool LoadGame()
    {
        SaveGame loadedGame = SaveSystem.Load();

        if(loadedGame != null)
        {
            if (CurrentLevelIndex != loadedGame.LevelsPlayed)
            {
                CurrentLevelIndex = loadedGame.LevelsPlayed;
                StartLevel();
            }

            else
            {
                Debug.Log("Current level already loaded.");
            }

            return true;

        }
        else
        {
            Debug.Log("No savegame found.");
            return false;
        }
    }

    private void Save()
    {
        SaveGame LastSave = SaveSystem.Load();

        if(LastSave != null)
        {
            if (LastSave.LevelsPlayed < CurrentLevelIndex)
            {
                SaveGame gameToSave = new SaveGame(totalXP, CurrentLevelIndex);
                SaveSystem.Save(gameToSave);
            }
        }

        else
        {
            SaveGame gameToSave = new SaveGame(totalXP, CurrentLevelIndex);
            SaveSystem.Save(gameToSave);
        }

    }

    private void SetActiveLevel()
    {        
        ActiveLevel = GameLevels[CurrentLevelIndex];
        CurrentLevelThreshold = ActiveLevel.XPThreshold;
    }

    private void Init_Spawners()
    {
        CollectableSpawner.Init_SpawnObjects(ActiveLevel.LevelCollectables);
        MainEnemySpawner.Init_EnemiesToSpawn(ActiveLevel.LevelEnemies);
    }

    private void Reset_Spawners()
    {
        ActiveEnemies.Clear();

        MainEnemySpawner.StopAllCoroutines();
        MainEnemySpawner.Clear_Lists();

        CollectableSpawner.StopAllCoroutines();
        CollectableSpawner.Clear_Lists();
    }

    private void Start_MissionObjective()
    {
        LevelObjectiveStarter.StartFade();
        LevelObjectiveStarter.Start_MissionObjective();
    }

    public string Get_CurrentMissionObjective()
    {
        return ActiveLevel.LevelObjective;
    }

    private void Reset_LevelObjective()
    {
        LevelObjectiveStarter.StopAllCoroutines();
        LevelObjectiveStarter.StopCoroutines_InObjectiveTxtChild();
    }

    public void Add_Enemy(GameObject NewEnemyOnScene)
    {
        ActiveEnemies.Add(NewEnemyOnScene);

        if(onActiveEnemiesChanged != null)
            onActiveEnemiesChanged.Invoke();
    }

    public void Add_EnemyRange(List<GameObject> NewEnemiesOnScene)
    {
        ActiveEnemies.AddRange(NewEnemiesOnScene);

        if(onActiveEnemiesChanged!=null)
            onActiveEnemiesChanged.Invoke();
    }

    public void Remove_Enemy(GameObject EnemyToRemove)
    {
        ActiveEnemies.Remove(EnemyToRemove);

        if(onActiveEnemiesChanged != null)
            onActiveEnemiesChanged.Invoke();
    }

    public void Add_Ally(GameObject NewAllyOnScene)
    {
        ActiveAllies.Add(NewAllyOnScene);

        if(onActiveAlliesChanged != null)
            onActiveAlliesChanged.Invoke();
    }

    public void Add_AllyRange(List<GameObject> NewAlliesOnScene)
    {
        ActiveAllies.AddRange(NewAlliesOnScene);

        if(onActiveAlliesChanged != null)
            onActiveAlliesChanged.Invoke();
    }

    public void Remove_Ally(GameObject AllyToRemove)
    {
        ActiveAllies.Remove(AllyToRemove);

        if(onActiveAlliesChanged != null)
            onActiveAlliesChanged.Invoke();
    }

    public int CountEnemiesOfType(string enemyTypeName)
    {
        string cloneName = enemyTypeName + "(Clone)";
        List<GameObject> TempEnemyList = ActiveEnemies.FindAll(EnemyElement => EnemyElement.name == cloneName);
        int numberOfEnemies = TempEnemyList.Count;
        return numberOfEnemies;
    }

    public int Get_ActiveEnemies()
    {
        return ActiveEnemies.Count;
    }

    public List<GameObject> Get_ActiveEnemiesGameObjects()
    {
        return ActiveEnemies;
    }

    public bool CheckIfEnemyYPosIsClear(Vector3 NextEnemyPos)
    {
        float minNegativeDistance = NextEnemyPos.y - MainEnemySpawner.MinEnemyToEnemyDistance;
        float maxPositiveDistance = NextEnemyPos.y + MainEnemySpawner.MinEnemyToEnemyDistance;

        bool IsSpaceOccupied = ActiveEnemies.TrueForAll(EnemyElement => EnemyElement.transform.position.y < minNegativeDistance || EnemyElement.transform.position.y > maxPositiveDistance);
        return IsSpaceOccupied;
    }

    public void Set_FlashMessage(string message)
    {        
        FlashMessage.MessageChanged.Invoke(message);
    }

}
