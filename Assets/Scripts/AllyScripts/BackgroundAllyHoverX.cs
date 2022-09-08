using UnityEngine;

public class BackgroundAllyHoverX : MonoBehaviour
{
    Rigidbody2D AlliedShipRB;

    [SerializeField]
    private float hoverMax = 0.5f;    
    [SerializeField]
    private float hoverMin = 0.5f;

    [SerializeField]
    private float hoverRangeMax;
    [SerializeField]
    private float hoverRangeMin;

    [SerializeField]
    private float hoverRate = 0.5f;
    Vector2 MovementX;

    [SerializeField]
    private float movementSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        AlliedShipRB = GetComponent<Rigidbody2D>();
        hoverRangeMax = AlliedShipRB.position.x + hoverMax;
        hoverRangeMin = AlliedShipRB.position.x - hoverMin;
        MovementX.x = hoverRate;
        
    }

    private void FixedUpdate()
    {
        Vector2 NextShipPosition = AlliedShipRB.position + MovementX * movementSpeed * Time.deltaTime;

        if(NextShipPosition.x >= hoverRangeMax || NextShipPosition.x <= hoverRangeMin)
            SwitchYDirection();

        AlliedShipRB.MovePosition(NextShipPosition);
    }

    private void SwitchYDirection()
    {
        MovementX.x *= -1f;
    }
}
