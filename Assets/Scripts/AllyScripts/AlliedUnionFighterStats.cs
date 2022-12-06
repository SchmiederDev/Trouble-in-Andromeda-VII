using UnityEngine;

public class AlliedUnionFighterStats : AllyHealth
{
    const string allyDestroyedMessage = "Damn! We lost a ship!";

    [SerializeField]
    SimpleOnSceneAllySpawner mySpawner;

    public override void Start()
    {
        base.Start();
        mySpawner = GetComponentInParent<SimpleOnSceneAllySpawner>();
        mySpawner.shouldRespawn = false;

    }

    public override void CheckHealthStatus()
    {
        if (allyHealth < 0)
        {
            mySpawner.shouldRespawn = true;
            TheGame.theGameInst.Remove_Ally(gameObject);
            Destroy(gameObject);
            TheGame.theGameInst.Set_FlashMessage(allyDestroyedMessage);
        }
    }
}
