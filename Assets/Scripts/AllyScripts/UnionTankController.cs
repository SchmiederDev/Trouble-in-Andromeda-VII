using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionTankController : MonoBehaviour
{
    private Rigidbody2D TankRB;

    [SerializeField]
    private float destinationY = -7.2f;

    private Vector2 MovementDir = new Vector2(0, 1);

    [SerializeField]
    private float glidingSpeed = 1f;

    [SerializeField]
    private float hoverSpeed = 1.25f;

    private float hoverMax = -7.25f;
    private float hoverMin = -7.15f;

    [SerializeField]
    private float rotationSpeed = 2.5f;

    [SerializeField]
    private float minAngle = -15f;

    [SerializeField]
    private float maxAngle = 15f;

    [SerializeField]
    private float positivePosThreshold = 0f;

    [SerializeField]
    private float negativePosThreshhold = 6.5f;

    private bool IsGlidingIn = true;

    private List<GameObject> TargetEnemies;

    public string targetPriority;

    GameObject Target;

    private bool rotatesClockwise = true;

    private bool hasTarget = false;

    UnionTankInitializer UT_Initializer;


    // Start is called before the first frame update
    void Start()
    {
        UT_Initializer = GetComponentInParent<UnionTankInitializer>();
        rotationSpeed = UT_Initializer.rotationSpeed;

        TargetEnemies = new List<GameObject>();

        TankRB = GetComponent<Rigidbody2D>();

        TheGame.theGameInst.onActiveEnemiesChanged += Update_TargetEnemies;
    }

    private void Update()
    {
        if(Target != null)
        {

            if(Target.transform.rotation.z > 0)
                rotatesClockwise = true;

            else
                rotatesClockwise = false;

        }
    }

    private void FixedUpdate()
    {
        if (IsGlidingIn)
            FlyToBattleDestination();

        else
        {
            if (hasTarget)
            {
                Rotate();
                Hover();
            }

            else
                Hover();
        }
    }    

    private void Update_TargetEnemies()
    {
        TargetEnemies = TheGame.theGameInst.Get_ActiveEnemiesGameObjects();        
        FindTarget();
    }

    private void FindTarget()
    {        
        Target = TargetEnemies.Find(EnemyElement => EnemyElement.name == targetPriority);

        if (Target != null)
        {
            hasTarget = true;
        }

        else
            hasTarget = false;
    }

    private void FlyToBattleDestination()
    {
        if (TankRB.position.y < destinationY)
            TankRB.MovePosition(TankRB.position + MovementDir * glidingSpeed * Time.deltaTime);
        else
            IsGlidingIn = false;
    }

    private void Hover()
    {
        if (TankRB.position.y >= hoverMax || TankRB.position.y <= hoverMin)
            SwitchDirection();

        TankRB.MovePosition(TankRB.position + MovementDir * hoverSpeed * Time.deltaTime);
    }

    private void SwitchDirection()
    {
        MovementDir.y *= -1f;
    }

    private void Rotate()
    {
        if (rotatesClockwise)
        {
            if (Target.transform.position.x > positivePosThreshold)
                RotateClockWise();
        }
        else
        {
            if (Target.transform.position.x < negativePosThreshhold)
                RotateCounterClockWise();
        }
    }

    private void RotateClockWise()
    {
        float nextRotation = TankRB.rotation - rotationSpeed * Time.deltaTime;

        if (nextRotation > minAngle)
            TankRB.MoveRotation(nextRotation);

    }

    private void RotateCounterClockWise()
    {
        float nextRotation = TankRB.rotation + rotationSpeed * Time.deltaTime;

        if (nextRotation < maxAngle)
            TankRB.MoveRotation(nextRotation);
    }
}
