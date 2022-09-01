using UnityEngine;

public class UnionFighterController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D FighterRB;

    private bool ControlsEnabled = false;

    private Vector2 MovementInput;
    private float InputX = 0;


    [SerializeField]
    private float movementSpeed = 15f;
    [SerializeField]
    private float YAcceleration = 0.01f;

    [SerializeField]
    private float hoverMaxPos = -6.45f;
    [SerializeField]
    private float hoverMinPos = -6.55f;


    private static Vector2 StartingPos = new Vector2(0f, -6.5f);
    private float levelBorderX = 17f;


    // Start is called before the first frame update
    void Start()
    {
        FighterRB = GetComponent<Rigidbody2D>();
        MovementInput.y = YAcceleration;
        TheGame.theGameInst.onMissionCanBegin += Enable_Controls;
        TheGame.theGameInst.onLevelLoad += Reset_FighterPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlsEnabled)
        {
            InputX = Input.GetAxisRaw("Horizontal");
            MovementInput.x = InputX;
        }        
    }

    private void FixedUpdate()
    {
        Check_YPosition();

        Vector2 NextFighterPos = FighterRB.position + MovementInput * movementSpeed * Time.deltaTime;

        if (NextFighterPos.x < levelBorderX && NextFighterPos.x > -(levelBorderX))
        {
            if (NextFighterPos.y >= hoverMaxPos || NextFighterPos.y <= hoverMinPos)
                SwitchYDirection();

            FighterRB.MovePosition(NextFighterPos);
        }       

    }

    private void Enable_Controls()
    {
        ControlsEnabled = TheGame.theGameInst.MissionCanBegin;
    }

    private void Check_YPosition()
    {
        if(FighterRB.position.y >= hoverMaxPos || FighterRB.position.y <= hoverMinPos)
        {
            float currentX = FighterRB.position.x;
            FighterRB.position = new Vector2(currentX, StartingPos.y);
        }
    }

    private void Reset_FighterPosition()
    {
        FighterRB.position = StartingPos;
    }

    private void SwitchYDirection()
    {
        MovementInput.y *= -1f;
    }

}
