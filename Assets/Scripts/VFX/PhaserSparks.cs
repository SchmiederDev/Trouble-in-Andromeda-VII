using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserSparks : MonoBehaviour
{
    [SerializeField]
    private Animator SparksAnimator;

    // Start is called before the first frame update
    void Start()
    {
        SparksAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject IncomingObject = collision.gameObject;
        Debug.Log("colliding object: " + IncomingObject.name);

        if (IncomingObject.tag == "SpaceShipWeapon")
        {
            SparksAnimator.SetTrigger("WasHit");
            TheGame.theGameInst.audioManager.PlaySound("LaserHit_01");
        }
            
    }

}
