using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public CollectableItem ItemAttached;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject CollisionObject = collision.gameObject;

        if(CollisionObject.tag == "PlayerShip")
        {
            if (TheGame.theGameInst.PlayerUnionFighter.PickUpItem(ItemAttached))
            {
                if (TheGame.theGameInst.ActiveLevel.hasGatherObjective == true && TheGame.theGameInst.ActiveLevel.gatherType == ItemAttached.target)
                    TheGame.theGameInst.PlayerUnionFighter.Gain_XP(ItemAttached.charge);

                TheGame.theGameInst.audioManager.PlaySound("PickUpSound");
                Destroy(gameObject);
            }
                
        }
    }
}
