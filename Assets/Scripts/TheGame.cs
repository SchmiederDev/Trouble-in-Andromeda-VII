using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheGame : MonoBehaviour
{
    public static TheGame theGameInst;

    public GameTimer Timer;
    public AudioManager audioManager;

    public UnionFighter PlayerUnionFighter;

    private int totalXP = 0;
    private const int maxXP = 5000000;


    private int MaxLevelIndex;    
    private int CurrentLevelIndex = 0;
    private int CurrentLevelThreshold = 500;

    public List<Level> GameLevels;
    
    [SerializeField]
    public Level ActiveLevel { get; private set; }
    private bool ReadyForNextLevel = false;


    public delegate void OnLevelLoad();
    public OnLevelLoad onLevelLoad;

    public bool MissionCanBegin = false;
    public delegate void OnMissionCanBegin();
    public OnMissionCanBegin onMissionCanBegin;


    [SerializeField]
    private ObjectiveStarter LevelObjectiveStarter;
    private FlashText FlashMessage;

    [SerializeField]
    private BarSlider IntegritySlider;
    [SerializeField]
    private BarSlider ShieldSlider;
    [SerializeField]
    private BarSlider EnergySlider;
    

    [SerializeField]
    private StandardSpawner CollectableSpawner;
    [SerializeField]
    private EnemySpawner MainEnemySpawner;


    public List<GameObject> Enemies;
    public List<GameObject> Pickables;


    [SerializeField]
    private List<GameObject> ActiveEnemies;
    [SerializeField]
    public List<GameObject> ActiveAllies;


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

    private void Start()
    {
        Show_CurrentLevel();
    }

    private void Init_TheGame()
    {
        MaxLevelIndex = SceneManager.sceneCountInBuildSettings;

        InitTimerFirstTime();
        FetchAudioManager();
        Init_GameElements_UI_Association();
        Init_EnemiesAndAllies();
        FirstLevelLoad();
    }

    private void InitTimerFirstTime()
    {
        Timer = GetComponentInChildren<GameTimer>();
        Timer.Init_Timer();
    }

    private void FetchAudioManager()
    {
        audioManager = GetComponentInChildren<AudioManager>();
    }

    private void Init_GameElements_UI_Association()
    {
        IntegritySlider.Set_MaxBarValue(PlayerUnionFighter.IntegrityMax);
        ShieldSlider.Set_MaxBarValue(PlayerUnionFighter.ShieldPowerMax);
        EnergySlider.Set_MaxBarValue(PlayerUnionFighter.EnergyMax);

        LevelObjectiveStarter = GetComponentInChildren<ObjectiveStarter>();
        FlashMessage = GetComponentInChildren<FlashText>();
    }

    private void Init_EnemiesAndAllies()
    {
        ActiveEnemies = new List<GameObject>();
        ActiveAllies = new List<GameObject>();
    }

    private void FirstLevelLoad()
    {
        SetFirstLevel();
        PlayerUnionFighter.ResetWeapons();
        FetchSpawners();
        Init_Spawners();
    }

    private void SetFirstLevel()
    {
        ActiveLevel = GameLevels[0];
        CurrentLevelThreshold = ActiveLevel.XPThreshold;
    }

    private void FetchSpawners()
    {
        CollectableSpawner = GetComponentInChildren<StandardSpawner>();
        MainEnemySpawner = GetComponentInChildren<EnemySpawner>();
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

            if(CurrentLevelIndex < MaxLevelIndex)
                LoadNextLevel();

            else
                CurrentLevelIndex = MaxLevelIndex;
        }
    }

    

    public void StartLevel()
    {
        SceneManager.LoadScene(CurrentLevelIndex, LoadSceneMode.Single);

        onLevelLoad.Invoke();

        ReadyForNextLevel = false;
        MissionCanBegin = false;
        onMissionCanBegin.Invoke();

        RestartTimer();

        SetActiveLevel();
        PlayerUnionFighter.Reset_UnionFighter_OnLevelLoad();

        Reset_Spawners();
        Init_Spawners();

        Reset_LevelObjective();

        Start_MissionObjective();

        Show_CurrentLevel();
    }

    public void RestartLevel()
    {
        onLevelLoad.Invoke();

        MissionCanBegin = false;
        onMissionCanBegin.Invoke();

        RestartTimer();

        PlayerUnionFighter.Reset_UnionFighter_OnLevelLoad();

        Reset_Spawners();
        Init_Spawners();

        Start_MissionObjective();

        Show_CurrentLevel();
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

    private void Save()
    {
        SaveGame LastSave = SaveSystem.Load();

        if (LastSave != null)
        {
            int lastGameLevel = MaxLevelIndex - 1;

            if (LastSave.LevelsPlayed < CurrentLevelIndex && CurrentLevelIndex < lastGameLevel)
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

    public bool LoadGame()
    {
        SaveGame loadedGame = SaveSystem.Load();

        if(loadedGame != null)
        {
            if (CurrentLevelIndex != loadedGame.LevelsPlayed)
            {
                CurrentLevelIndex = loadedGame.LevelsPlayed;
                PlayerUnionFighter.LoadWeapons(loadedGame.unlockedWeapons);
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

    private void RestartTimer()
    {
        Timer.StopAllCoroutines();
        Timer.Reset_Timer();
        Timer.Init_Timer();
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

    private void Reset_LevelObjective()
    {
        LevelObjectiveStarter.StopAllCoroutines();
        LevelObjectiveStarter.StopCoroutines_InObjectiveTxtChild();
    }

    public void Set_FlashMessage(string message)
    {
        FlashMessage.MessageChanged.Invoke(message);
    }

    private void Show_CurrentLevel()
    {
        int levelCount = CurrentLevelIndex + 1;
        string levelMessage = "Level: " + levelCount + " - " + ActiveLevel.LevelName;
        Set_FlashMessage(levelMessage);
    }

    public int GetLevelIndex()
    {
        return CurrentLevelIndex;
    }

    public string Get_CurrentMissionObjective()
    {
        return ActiveLevel.LevelObjective;
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
    
}
