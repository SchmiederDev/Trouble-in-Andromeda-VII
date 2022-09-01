using System;
using UnityEngine;

public class AlienEmpireXController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D AlienXFighterRB;

    [SerializeField]
    private Animator XFighterAnimator;

    [SerializeField]
    private float movementSpeed = 2.5f;

    [SerializeField]
    private float PlayerX;
    private int directionX;

    [SerializeField]
    private float nextAlienX;

    [SerializeField]
    Vector2 MovementX;

    float mapEnd_X = 17f;

    Enemy EnemySelf;

    private void Awake()
    {
        EnemySelf = GetComponent<Enemy>();
        EnemySelf.sufferedDamage += DestroyXFighter;
    }

    // Start is called before the first frame update
    void Start()
    {
        AlienXFighterRB = GetComponent<Rigidbody2D>();
        XFighterAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Update_PlayerDirection_and_AlienPosition();
    }

    private void FixedUpdate()
    {
        if(Check_PlayerDirection_and_AlienPosition())
            MoveXFighter();
    }

    private void Update_PlayerDirection_and_AlienPosition()
    {
        PlayerX = TheGame.theGameInst.PlayerUnionFighter.FighterPosition.x;
        directionX = Math.Sign(Input.GetAxisRaw("Horizontal"));                      
    }

    private bool Check_PlayerDirection_and_AlienPosition()
    {
        nextAlienX = AlienXFighterRB.position.x + directionX * movementSpeed * Time.deltaTime;

        if (nextAlienX < mapEnd_X && nextAlienX > -(mapEnd_X))
        {
            if(directionX < 0)
            {
                if (AlienXFighterRB.position.x >= PlayerX)
                {
                    return true;
                }

                else return false;
            }

            else
            {
                if (AlienXFighterRB.position.x <= PlayerX)
                {
                    return true;
                }

                else return false;
            }
        }

        else return false;
    }

    private void MoveXFighter()
    {
        MovementX = new Vector2(nextAlienX, 0);
        AlienXFighterRB.MovePosition(MovementX);
    }

    private void DestroyXFighter()
    {
        XFighterAnimator.SetBool("IsDestroyed", true);
        TheGame.theGameInst.audioManager.PlaySound("AsteroidHit");
    }
}
