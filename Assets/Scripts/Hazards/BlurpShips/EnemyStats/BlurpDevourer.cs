using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurpDevourer : Enemy
{
    [SerializeField]
    Enemy EnemySelf;

    [SerializeField]
    Enemy BattleTower_01;
    [SerializeField]
    Enemy BattleTower_02;

    [SerializeField]
    BarSlider Devourer_HealthBar;

    [SerializeField]
    BarSlider BattleTower01_HealthBar;
    [SerializeField]
    BarSlider BattleTower02_HealthBar;

    private void Start()
    {
        EnemySelf = GetComponent<Enemy>();
        EnemySelf.sufferedDamage += DestroyDevourerOnHit;
        Reset_Names();

        BattleTower_01.sufferedDamage += Destroy_BattleTower_01;
        BattleTower_02.sufferedDamage += Destroy_BattleTower_02;

        List<GameObject> theseEnemies = new List<GameObject>();
        theseEnemies.Add(gameObject);
        theseEnemies.Add(BattleTower_01.gameObject);
        theseEnemies.Add(BattleTower_02.gameObject);

        TheGame.theGameInst.Add_EnemyRange(theseEnemies);
    }

    private void Reset_Names()
    {
        BattleTower_01.EnemyName = BattleTower_01.EnemyName + "_01";
        BattleTower_02.EnemyName = BattleTower_02.EnemyName + "_02";
    }

    private void Update()
    {
        BattleTower01_HealthBar.Set_BarValue(BattleTower_01.StructuralIntegrity);
        BattleTower02_HealthBar.Set_BarValue(BattleTower_02.StructuralIntegrity);
    }

    public override void HitEnemy(GameObject CollidingObject)
    {
        if(BattleTower_01.StructuralIntegrity <= 0 && BattleTower_02.StructuralIntegrity <= 0)
        {
            base.HitEnemy(CollidingObject);
            Devourer_HealthBar.Set_BarValue(EnemySelf.StructuralIntegrity);
        }
    }

    private void DestroyDevourerOnHit()
    {

    }

    private void Destroy_BattleTower_01()
    {

    }

    private void Destroy_BattleTower_02()
    {

    }
}
