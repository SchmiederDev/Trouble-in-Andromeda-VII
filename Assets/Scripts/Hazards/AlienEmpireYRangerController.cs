using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEmpireYRangerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D YRangerRB;

    [SerializeField]
    private Animator YRangerAnim;

    [SerializeField]
    private float movementSpeed = 5f;

    private float YDirection = -1f;
    private Vector2 movement;

    [SerializeField]
    private float rotationRate = 30f;

    private const float minY = -11f;
    

    // Start is called before the first frame update
    void Start()
    {
        YRangerRB = GetComponent<Rigidbody2D>();
        YRangerAnim = GetComponent<Animator>();
        Set_RotationDirection();
        movement.y = YDirection;
    }

    private void FixedUpdate()
    {
        YRangerRB.MovePosition(YRangerRB.position + movement * movementSpeed * Time.deltaTime);
        YRangerRB.MoveRotation(YRangerRB.rotation -= rotationRate * Time.deltaTime);
        CheckMinYPos();
    }

    private void Set_RotationDirection()
    {
        if (YRangerRB.position.x < 0)
            rotationRate *= -1f;
    }

    private void CheckMinYPos()
    {
        if (YRangerRB.position.y < minY)
            Destroy(gameObject);
    }
}
