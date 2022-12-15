using UnityEngine;

public class PhaserSparks : MonoBehaviour
{
    [SerializeField]
    private Animator SparksAnimator;

    const string weaponTag = "SpaceShipWeapon";
    const string enemyPhaserTag = "AlienPhaser";

    // Start is called before the first frame update
    void Start()
    {
        SparksAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject IncomingObject = collision.gameObject;

        if (IncomingObject.tag == weaponTag || IncomingObject.tag == enemyPhaserTag)
        {
            SparksAnimator.SetTrigger("WasHit");
            TheGame.theGameInst.audioManager.PlaySound("LaserHit_01");
        }
            
    }

}
