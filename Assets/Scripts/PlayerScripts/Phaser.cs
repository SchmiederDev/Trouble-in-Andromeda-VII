using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : MonoBehaviour
{
    [SerializeField]
    private Transform PhaserTransform;

    [SerializeField]
    private float speed = 25f;

    private float YPos = 0f;

    private float MapEnd = 9.0f;


    // Start is called before the first frame update
    void Start()
    {
        PhaserTransform = GetComponent<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void FixedUpdate()
    {
        PhaserTransform.Translate(0, speed * Time.deltaTime, 0);
    }

    private void CheckPosition()
    {
        YPos = PhaserTransform.position.y;

        if(YPos >= MapEnd)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
