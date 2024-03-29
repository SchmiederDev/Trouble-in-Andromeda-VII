using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyArchetype Archetype;

    public string EnemyName;
    public int StructuralIntegrity;
    public int ImpactMagnitude;
    public int XP;

    private string WeaponTag = "SpaceShipWeapon";
    private string PlayerTag = "PlayerShip";

    public delegate void OnSufferedDamage();
    public OnSufferedDamage sufferedDamage;

    private void Awake()
    {
        TransferDataFromArchetype();
    }

    private void TransferDataFromArchetype()
    {
        EnemyName = Archetype.EnemyName;
        StructuralIntegrity = Archetype.StructuralIntegrity;
        ImpactMagnitude = Archetype.ImpactMagnitude;
        XP = Archetype.XP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject CollidingObject = collision.gameObject;        
        HitEnemy(CollidingObject);
    }

    public virtual void HitEnemy(GameObject CollidingObject)
    {
        if (CollidingObject.tag == WeaponTag)
        {
            //int damage = TheGame.theGameInst.PlayerUnionFighter.ActiveWeapon.FirePower;            

            Phaser UnionPhaser = CollidingObject.GetComponent<Phaser>();
            SpaceWeapon IncomingPhaser = UnionPhaser.Get_SpaceShipWeapon();

            int damage = IncomingPhaser.FirePower;

            StructuralIntegrity -= damage;

            if (StructuralIntegrity <= 0)
            {
                sufferedDamage.Invoke();
                StartCoroutine(DestroyEnemy_OnHit());
            }
        }

    }

    private IEnumerator DestroyEnemy_OnHit()
    {
        yield return new WaitForSeconds(0.28f);
        if(!TheGame.theGameInst.ActiveLevel.hasGatherObjective)
            TheGame.theGameInst.PlayerUnionFighter.Gain_XP(XP);

        TheGame.theGameInst.Remove_Enemy(gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == PlayerTag)
        {
            TheGame.theGameInst.PlayerUnionFighter.SufferedCollison(ImpactMagnitude);
            TheGame.theGameInst.audioManager.PlaySound("ShipCollision");
        }
    }

}
