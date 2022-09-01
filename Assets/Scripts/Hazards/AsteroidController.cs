using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField]
    private Transform AsteroidTransform;

    [SerializeField]
    private Animator AsteroidAnimator;

    [SerializeField]
    private float speed = 25f;

    private float YPos = 0f;

    private float MapEnd = -9.0f;

    private string WeaponTag = "SpaceShipWeapon";

    Enemy EnemySelf;

    private void Awake()
    {
        EnemySelf = GetComponent<Enemy>();
        EnemySelf.sufferedDamage += DestroyAsteroid;
    }

    // Start is called before the first frame update
    void Start()
    {
        AsteroidTransform = GetComponent<Transform>();
        AsteroidAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void FixedUpdate()
    {
        AsteroidTransform.Translate(0, -speed * Time.deltaTime, 0);
    }

    private void CheckPosition()
    {
        YPos = AsteroidTransform.position.y;

        if (YPos <= MapEnd)
        {
            TheGame.theGameInst.Remove_Enemy(gameObject);
            Destroy(gameObject);
        }
    }

    private void DestroyAsteroid()
    {
        AsteroidAnimator.SetBool("IsDestroyed", true);
        TheGame.theGameInst.audioManager.PlaySound("AsteroidHit");
    }
}
