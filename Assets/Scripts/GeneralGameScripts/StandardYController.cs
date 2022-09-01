using UnityEngine;

public class StandardYController : MonoBehaviour
{
    [SerializeField]
    private Transform MovingObjectTransform;

    [SerializeField]
    private float speed = 2.5f;

    private float YPos = 0f;

    private float MapEnd = -9.0f;


    // Start is called before the first frame update
    void Start()
    {
        MovingObjectTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void FixedUpdate()
    {
        MovingObjectTransform.Translate(0, -speed * Time.deltaTime, 0);
    }

    private void CheckPosition()
    {
        YPos = MovingObjectTransform.position.y;

        if (YPos <= MapEnd)
        {
            Destroy(gameObject);
        }
    }
}
