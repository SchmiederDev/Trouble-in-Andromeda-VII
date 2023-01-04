using UnityEngine;
using System;

public class UnionFighterController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D FighterRB;

    private FighterSteeringMode steeringMode;

    private bool ControlsEnabled = false;

    private Vector2 MovementInput;
    private float InputX = 0;
    private float InputY = 0;

    [SerializeField]
    private float movementSpeed = 15f;
    [SerializeField]
    private float YAcceleration = 0.01f;

    [SerializeField]
    private float rotationSpeed = 300f;

    [SerializeField]
    private float hoverMaxPos = -6.45f;
    [SerializeField]
    private float hoverMinPos = -6.55f;

    private float levelBorderX = 17f;
    private float levelBorderY = 8.5f;

    private static Vector2 StartingPosDefault = new Vector2(0f, -6.5f);
    private static Vector2 HoverYStartingPos = new Vector2(-17f, 0f);
    private static Vector2 RotateStartingPos = new Vector2(0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        FighterRB = GetComponent<Rigidbody2D>();
        steeringMode = FighterSteeringMode.DefaultHoverX;
        TheGame.theGameInst.onMissionCanBegin += Enable_Controls;
        TheGame.theGameInst.onLevelLoad += Reset_FighterPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlsEnabled)
        {   
            
            InputX = Input.GetAxisRaw("Horizontal");
            InputY = Input.GetAxisRaw("Vertical");

            ControlMovement_AccordingTo_SteeringMode();            
        }        
    }
    private void ControlMovement_AccordingTo_SteeringMode()
    {
        switch (steeringMode)
        {
            case FighterSteeringMode.DefaultHoverX:
                MovementInput.x = InputX;
                MovementInput.y = YAcceleration;
                break;

            case FighterSteeringMode.FreeMovementXY:
                MovementInput.x = InputX;
                MovementInput.y = InputY;
                break;

            case FighterSteeringMode.HoverY:
                MovementInput.x = InputX;
                MovementInput.y = InputY;
                FighterRB.rotation = -90f;
                break;

            case FighterSteeringMode.Rotate:
                FighterRB.freezeRotation = false;
                break;
        }
    }

    private void Set_SteeringMode_PressingKey()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            steeringMode++;

            int steeringModeIndex = (int)steeringMode;
            int steeringModeLength = Enum.GetNames(typeof(FighterSteeringMode)).Length;
            if (steeringModeIndex > steeringModeLength - 1)
                steeringMode = FighterSteeringMode.DefaultHoverX;

            Reset_FighterPosition();
            FighterRB.freezeRotation = true;
        }
    }

    private void FixedUpdate()
    {        
        if(ControlsEnabled)
            MoveUnionFighter();
    }   

    private void MoveUnionFighter()
    {
        if (steeringMode == FighterSteeringMode.DefaultHoverX)
        {
            MoveDefaultHoverX();
        }

        else if (steeringMode == FighterSteeringMode.FreeMovementXY || steeringMode == FighterSteeringMode.HoverY)
        {
            MoveFreelyXY();
        }

        else if (steeringMode == FighterSteeringMode.Rotate)
        {
            MoveRotating();
        }
    }

    private void MoveDefaultHoverX()
    {
        Vector2 NextFighterPos = FighterRB.position + MovementInput * movementSpeed * Time.deltaTime;

        if (NextFighterPos.x < levelBorderX && NextFighterPos.x > -(levelBorderX))
        {
            if (NextFighterPos.y >= hoverMaxPos || NextFighterPos.y <= hoverMinPos)
                SwitchYDirection();

            FighterRB.MovePosition(NextFighterPos);

            Check_YPosition();
        }
    }

    private void MoveFreelyXY()
    {
        Vector2 NextFighterPos = FighterRB.position + MovementInput * movementSpeed * Time.deltaTime;

        if (NextFighterPos.x < levelBorderX && NextFighterPos.x > -(levelBorderX) && NextFighterPos.y < levelBorderY && NextFighterPos.y > -(levelBorderY))
            FighterRB.MovePosition(NextFighterPos);
    }

    private void MoveRotating()
    {
        float NextRotation;

        if (InputX != 0 && InputY == 0)
            NextRotation = FighterRB.rotation - InputX * rotationSpeed * Time.deltaTime;

        else if (InputX == 0 && InputY != 0)
            NextRotation = FighterRB.rotation - InputY * rotationSpeed * Time.deltaTime;

        else
            NextRotation = FighterRB.rotation - InputX * rotationSpeed * Time.deltaTime;

        FighterRB.MoveRotation(NextRotation);
    }

    private void Enable_Controls()
    {
        ControlsEnabled = TheGame.theGameInst.MissionCanBegin;

        if (!ControlsEnabled)
            FighterRB.constraints = RigidbodyConstraints2D.FreezeAll;

        else
        {
            if (steeringMode != FighterSteeringMode.Rotate)
                FighterRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            else
                FighterRB.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    private void Check_YPosition()
    {
        if(FighterRB.position.y >= hoverMaxPos || FighterRB.position.y <= hoverMinPos)
        {
            float currentX = FighterRB.position.x;
            FighterRB.position = new Vector2(currentX, StartingPosDefault.y);
        }
    }    

    private void Reset_FighterPosition()
    {
        FighterRB.rotation = 0f;

        switch (steeringMode)
        {
            case FighterSteeringMode.DefaultHoverX:
                FighterRB.position = StartingPosDefault;
                break;

            case FighterSteeringMode.FreeMovementXY:
                FighterRB.position = StartingPosDefault;
                break;

            case FighterSteeringMode.HoverY:
                FighterRB.position = HoverYStartingPos;
                break;

            case FighterSteeringMode.Rotate:
                FighterRB.position = RotateStartingPos;
                break;
        }
    }

    private void SwitchYDirection()
    {
        MovementInput.y *= -1f;
    }

    private enum FighterSteeringMode
    {
        DefaultHoverX,
        FreeMovementXY,
        HoverY,
        Rotate
    }

}
