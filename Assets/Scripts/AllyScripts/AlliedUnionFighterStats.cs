using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AlliedUnionFighterStats : AllyHealth
{
    const string allyDestroyedMessage = "Damn! We lost a ship!";

    [SerializeField]
    SimpleOnSceneAllySpawner mySpawner;

    [SerializeField]
    private Animator UnionShipAnimator;
    [SerializeField]
    private Light2D ShipEngineLight;

    [SerializeField]
    private float destructionTime = 0.25f;

    public override void Start()
    {
        base.Start();
        UnionShipAnimator = GetComponent<Animator>();
        ShipEngineLight = GetComponentInChildren<Light2D>();
        mySpawner = GetComponentInParent<SimpleOnSceneAllySpawner>();
        mySpawner.shouldRespawn = false;

    }

    public override void CheckHealthStatus()
    {
        if (allyHealth < 0)
        {
            mySpawner.shouldRespawn = true;
            Destroy(ShipEngineLight);
            UnionShipAnimator.SetBool("WasDestroyed", true);            
            TheGame.theGameInst.Set_FlashMessage(allyDestroyedMessage);
            StartCoroutine(DestroyUnionShip());
        }
    }

    IEnumerator DestroyUnionShip()
    {
        yield return new WaitForSeconds(destructionTime);
        TheGame.theGameInst.Remove_Ally(gameObject);
        Destroy(gameObject);
    }
}
