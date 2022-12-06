using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTowerPhaser : EnemyYPhaser
{
    
    [SerializeField]
    private Transform ParentTransform;

    private Rigidbody2D PhaserRB;

    private Quaternion StartingRotation;

    Vector3 Direction;

    // Start is called before the first frame update
    void Awake()
    {
        PhaserRB = GetComponent<Rigidbody2D>();
        PhaserTransform = GetComponent<Transform>();
        ParentTransform = GetComponentInParent<Transform>().parent;
        StartingRotation = ParentTransform.rotation;
        PhaserTransform.rotation = StartingRotation;
        CalculateDirection();
    }

    public void CalculateDirection()
    {
        Direction = PhaserTransform.position - ParentTransform.position;
        Direction.Normalize();
    }

    private void FixedUpdate()
    {
        PhaserTransform.rotation = StartingRotation;
        PhaserRB.MovePosition(PhaserRB.position + (Vector2)Direction * speed * Time.deltaTime);
    }
}
