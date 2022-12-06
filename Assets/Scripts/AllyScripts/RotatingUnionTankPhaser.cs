using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingUnionTankPhaser : Phaser
{
    
    [SerializeField]
    private Transform ParentTransform;

    private Rigidbody2D PhaserRB;

    private Quaternion StartingRotation;

    Vector3 Direction;

    // Start is called before the first frame update
    void Start()
    {
        PhaserTransform = GetComponent<Transform>();
        PhaserRB = GetComponent<Rigidbody2D>();
        ParentTransform = GetComponentInParent<Transform>().parent;
        StartingRotation = ParentTransform.rotation;
        PhaserTransform.rotation = StartingRotation;
        CalculateDirection();
    }

    private void FixedUpdate()
    {
        PhaserTransform.rotation = StartingRotation;
        PhaserRB.MovePosition(PhaserRB.position + (Vector2)Direction * speed * Time.deltaTime);
    }

    private void CalculateDirection()
    {
        Direction = ParentTransform.position - PhaserTransform.position;
        Direction.Normalize();
        Direction *= -1f;
    }

}
