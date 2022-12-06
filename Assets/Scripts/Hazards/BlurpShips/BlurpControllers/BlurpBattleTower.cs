using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurpBattleTower : MonoBehaviour
{
    private Rigidbody2D BattleTowerRB;
    private string battleTowerName;

    private BlurpDevourer Devourer;

    [SerializeField]
    private float rotationMin = -35f;

    [SerializeField]
    private float rotationMax = 35f;

    private float towerAngle;

    [SerializeField]
    private float rotationSpeed = 50f;
    private float rotationDirection = 1f;

    // Start is called before the first frame update
    void Start()
    {
        battleTowerName = gameObject.name; 
        Devourer = GetComponentInParent<BlurpDevourer>();
        BattleTowerRB = GetComponent<Rigidbody2D>();
        towerAngle = rotationMin;
        BattleTowerRB.rotation = towerAngle;
    }

    private void FixedUpdate()
    {

        if (BattleTowerRB.rotation > rotationMax || BattleTowerRB.rotation < rotationMin)
            rotationDirection *= -1f;

        towerAngle += rotationSpeed * rotationDirection * Time.deltaTime;
        
        BattleTowerRB.MoveRotation(towerAngle);
    }
}
