using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyYPhaser : MonoBehaviour
{    
    private string PlayerTag = "PlayerShip";
    private string AllyTag = "AllyShip";

    [SerializeField]
    private SpaceWeapon EnemyWeapon;

    [SerializeField]
    public Transform PhaserTransform;

    public float speed = 25f;

    private float YPos = 0f;

    private float MapEnd = -9.0f;


    // Start is called before the first frame update
    void Start()
    {
        PhaserTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void FixedUpdate()
    {
        PhaserTransform.Translate(0, -speed * Time.deltaTime, 0);
    }

    private void CheckPosition()
    {
        YPos = PhaserTransform.position.y;

        if (YPos <= MapEnd)
        {
            Destroy(gameObject);
        }
    }

    public SpaceWeapon GetEnemyWeapon()
    {
        return EnemyWeapon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject CollisionObject = collision.gameObject;

        if(CollisionObject.tag == PlayerTag)
        {
            TheGame.theGameInst.PlayerUnionFighter.SufferDamage(EnemyWeapon.FirePower, EnemyWeapon.ShieldBreachCapacity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == PlayerTag || collision.gameObject.tag == AllyTag)
        {
            Destroy(gameObject);
        }
    }
}
