using System.Collections;
using UnityEngine;

public class AllyHealth : MonoBehaviour
{
    
    public int allyHealth = 100;
    private BarSlider HealthBar;

    private float restartLevelTransition = 0.5f;

    const string civilianShipMessage = "Oh, no the civilian ship was destroyed! Game Over!";

    // Start is called before the first frame update
    public virtual void Start()
    {
        HealthBar = GetComponentInChildren<BarSlider>();
        HealthBar.Set_MaxBarValue(allyHealth);
        HealthBar.Set_BarValue(allyHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "AlienPhaser")
        {
            EnemyYPhaser phaser = collision.gameObject.GetComponent<EnemyYPhaser>();
            SpaceWeapon EnemyWeapon = phaser.GetEnemyWeapon();
            allyHealth -= EnemyWeapon.FirePower;
            HealthBar.Set_BarValue(allyHealth);
            CheckHealthStatus();
        }
    }

    public virtual void CheckHealthStatus()
    {
        if (allyHealth < 0)
            StartCoroutine(RestartLevel());
            
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(restartLevelTransition);
        TheGame.theGameInst.Set_FlashMessage(civilianShipMessage);
        TheGame.theGameInst.StartLevel();
    }
}
