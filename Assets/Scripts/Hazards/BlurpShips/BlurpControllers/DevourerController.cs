using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevourerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D DevourerRB;

    private Vector2 DevourerMovement;
    private Vector2 NextPos;

    private float maxRotation = 180f;
    private float minRotation = 0f;

    [SerializeField]
    private float minXPos = -3f;

    [SerializeField]
    private float maxXPos = 9f;

    bool hasReachedMaxPos = false;

    [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField]
    private float rotationSpeed = 5f;

    private float rotationDirection = -1f;

    [SerializeField]
    private float hoverSpeed = 2.5f;

    private float directionX = 1f;

    // Start is called before the first frame update
    void Start()
    {
        DevourerRB = GetComponent<Rigidbody2D>();
        DevourerMovement.x = directionX;
    }

    private void FixedUpdate()
    {       

        if (!hasReachedMaxPos)
        {            
            CheckXPos();
            MoveDevourer();
        }

        else
            RotateDevourer();
        
    }

    private void MoveDevourer()
    {
        DevourerRB.MovePosition(DevourerRB.position + DevourerMovement * movementSpeed * Time.deltaTime);
    }

    private void CheckXPos()
    {
        NextPos = DevourerRB.position + DevourerMovement * movementSpeed * Time.deltaTime;
        if (NextPos.x > maxXPos || NextPos.x < minXPos)
            hasReachedMaxPos = true;
    }

    private void SwitchXDirection()
    {
        directionX *= -1f;
        DevourerMovement.x = directionX;
    }

    private void SwitchRotationDirection()
    {
        rotationDirection *= -1f;
    }

    private void RotateDevourer()
    {
        float nextrotation = DevourerRB.rotation += rotationDirection * rotationSpeed * Time.deltaTime;

        if(rotationDirection < 0f)
        {
            if (nextrotation > minRotation)
                DevourerRB.MoveRotation(nextrotation);
            else
                Turn();
        }

        else
        {
            if (nextrotation < maxRotation)
                DevourerRB.MoveRotation(nextrotation);
            else
                Turn();
        }
    }

    private void Turn()
    {
        SwitchXDirection();
        hasReachedMaxPos = false;
        SwitchRotationDirection();
    }
}
