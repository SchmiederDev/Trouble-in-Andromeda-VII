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
    private int playerDirectionX;

    [SerializeField]
    private float nextAlienX;

    private float alienXDirection = 1f;

    [SerializeField]
    Vector2 MovementX;

    float mapEnd_X = 17f;

    private bool IsFollowing = false;

    [SerializeField]
    private float LineOfSightRange = 0.5f;

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
        UpdatePlayerPosition();

        if (!IsFollowing)
            CheckFighterPosition();
        else
            GetPlayerInput();        
    }

    private void FixedUpdate()
    {      

        if (IsFollowing)
        {
            if (Check_PlayerDirection_and_AlienPosition())
                FollowUnionFighter();
        }

        else
        {
            Range();            
        }
            
    }

    private void UpdatePlayerPosition()
    {
        PlayerX = TheGame.theGameInst.PlayerUnionFighter.FighterPosition.x;
    }

    private void GetPlayerInput()
    {        
        playerDirectionX = Math.Sign(Input.GetAxisRaw("Horizontal"));                      
    }

    private bool Check_PlayerDirection_and_AlienPosition()
    {
        nextAlienX = AlienXFighterRB.position.x + playerDirectionX * movementSpeed * Time.deltaTime;

        if (nextAlienX < mapEnd_X && nextAlienX > -(mapEnd_X))
        {
            if(playerDirectionX < 0)
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

    private void FollowUnionFighter()
    {
        MovementX = new Vector2(nextAlienX, 0);
        AlienXFighterRB.MovePosition(MovementX);
    }

    private void Range()
    {
        MovementX = new Vector2(AlienXFighterRB.position.x + alienXDirection * movementSpeed * Time.deltaTime, 0);
        AlienXFighterRB.MovePosition(MovementX);
    }

    private void CheckFighterPosition()
    {
        if (alienXDirection > 0)
        {
            if (AlienXFighterRB.position.x > mapEnd_X)
                SwapDirection();
        }

        else
        {
            if (AlienXFighterRB.position.x < -(mapEnd_X))
                SwapDirection();
        }

        IsFollowing = CheckLineOfSight();
    }

    private void SwapDirection()
    {
        alienXDirection *= -1;
    }

    private bool CheckLineOfSight()
    {
        float negativeLineOfSight = AlienXFighterRB.position.x - LineOfSightRange;
        float positiveLineOfSight = AlienXFighterRB.position.x + LineOfSightRange;              
        
        if (PlayerX >= negativeLineOfSight || PlayerX <= positiveLineOfSight)
            return true;
        else
            return false;
    }

    private void DestroyXFighter()
    {
        XFighterAnimator.SetBool("WasDestroyed", true);
        TheGame.theGameInst.audioManager.PlaySound("AsteroidHit");
    }
}
