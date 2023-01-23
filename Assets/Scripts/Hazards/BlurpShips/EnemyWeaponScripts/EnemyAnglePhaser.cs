using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnglePhaser : MonoBehaviour
{
    private string PlayerTag = "PlayerShip";

    [SerializeField]
    private SpaceWeapon EnemyWeapon;

    [SerializeField]
    private Transform PhaserTransform;

    private Rigidbody2D PhaserRB;

    [SerializeField]
    private float speed = 25f;

    private float YPos = 0f;

    private float MapEnd = -9.0f;

    private Vector3 PlayerPos;
    private Vector3 Direction;
    private float angle2Player;

    // Start is called before the first frame update
    void Start()
    {
        PhaserTransform = GetComponent<Transform>();
        PhaserRB = GetComponent<Rigidbody2D>();
        Calculate_Angle2Player();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void FixedUpdate()
    {
        PhaserRB.MovePosition(PhaserRB.position + (Vector2)Direction * speed * Time.deltaTime);
    }

    private void Calculate_Angle2Player()
    {
        PlayerPos = TheGame.theGameInst.PlayerUnionFighter.FighterPosition;
        Direction = PlayerPos - PhaserTransform.position;            
        Direction.Normalize();
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        if (angle >= 0)
        {
            angle2Player = angle + 90f;
            PhaserRB.rotation = angle2Player;
        }

        else
        {
            angle2Player = angle - 90f;
            PhaserRB.rotation = angle2Player;
        }
        
    }

    private void CheckPosition()
    {
        YPos = PhaserTransform.position.y;

        if (YPos <= MapEnd)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject CollisionObject = collision.gameObject;

        if (CollisionObject.tag == PlayerTag)
        {
            TheGame.theGameInst.PlayerUnionFighter.SufferDamage(EnemyWeapon.FirePower, EnemyWeapon.ShieldBreachCapacity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerTag)
        {
            Destroy(gameObject);
        }
    }
}
