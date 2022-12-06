using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurpFlyByXRanger : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D AlienXRangerRB;

    [SerializeField]
    private Animator XRangerAnimator;

    [SerializeField]
    private float movementSpeed = 7.5f;

    [SerializeField]
    Vector2 MovementX;

    private Vector2 StartingPos;

    float mapStart = -17f;
    float mapEnd_X = 18f;
    float minYPos = 0;
    int XDirection = 1;

    Enemy EnemySelf;
    

    private void Awake()
    {
        EnemySelf = GetComponent<Enemy>();
        EnemySelf.sufferedDamage += DestroyXFighterOnHit;        
    }

    // Start is called before the first frame update
    void Start()
    {
        AlienXRangerRB = GetComponent<Rigidbody2D>();
        XRangerAnimator = GetComponent<Animator>();
        CheckStartingPosition();
    }

    private void Update()
    {
        CheckRangerPosition();
    }

    private void FixedUpdate()
    {
        MoveXRanger();
    }

    private void CheckStartingPosition()
    {
        if(AlienXRangerRB.position.y < minYPos)
            StartingPos = new Vector2(mapStart, minYPos);

        else
            StartingPos = new Vector2(mapStart, AlienXRangerRB.position.y);

        AlienXRangerRB.position = StartingPos;
    }

    private void CheckRangerPosition()
    {
        if (AlienXRangerRB.position.x > mapEnd_X)
            DestroyXFighterOutOfRange();
    }

    private void MoveXRanger()
    {
        MovementX = new Vector2(AlienXRangerRB.position.x + XDirection * movementSpeed * Time.deltaTime, 0);
        AlienXRangerRB.MovePosition(MovementX);
    }

    private void DestroyXFighterOnHit()
    {
        XRangerAnimator.SetBool("WasDestroyed", true);
        TheGame.theGameInst.audioManager.PlaySound("AsteroidHit");
    }

    // Maybe use coroutine instead - if there needs to be time to do other things...
    private void DestroyXFighterOutOfRange()
    {
        TheGame.theGameInst.Remove_Enemy(gameObject);
        Destroy(gameObject);
    }
    
}
