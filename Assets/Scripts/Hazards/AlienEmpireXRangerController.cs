using System;
using UnityEngine;

public class AlienEmpireXRangerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D AlienXRangerRB;

    [SerializeField]
    private Animator XRangerAnimator;

    [SerializeField]
    private float movementSpeed = 7.5f;

    [SerializeField]
    Vector2 MovementX;

    float mapEnd_X = 17f;
    int XDirection = 1;

    Enemy EnemySelf;

    private void Awake()
    {
        EnemySelf = GetComponent<Enemy>();
        EnemySelf.sufferedDamage += DestroyXFighter;
    }

    // Start is called before the first frame update
    void Start()
    {        
        AlienXRangerRB = GetComponent<Rigidbody2D>();
        XRangerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckRangerPosition();
    }

    private void FixedUpdate()
    {
        MoveXRanger();
    }

    private void CheckRangerPosition()
    {
        if(XDirection > 0)
        {
            if (AlienXRangerRB.position.x > mapEnd_X)
                SwapDirection();
        }

        else
        {
            if (AlienXRangerRB.position.x < -(mapEnd_X))
                SwapDirection();
        }
    }

    private void SwapDirection()
    {
        XDirection *= -1;
    }


    private void MoveXRanger()
    {
        MovementX = new Vector2(AlienXRangerRB.position.x + XDirection * movementSpeed * Time.deltaTime, 0);
        AlienXRangerRB.MovePosition(MovementX);
    }

    private void DestroyXFighter()
    {
        XRangerAnimator.SetBool("IsDestroyed", true);
        TheGame.theGameInst.audioManager.PlaySound("AsteroidHit");
    }
}
