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
                TheGame.theGameInst.audioManager.PlaySound("PickUpSound");
                Destroy(gameObject);
            }
                
        }
    }
}
