using Jam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // attributes
    private float velocityX, velocityY;
    private Camera cam;
    private Vector2 mouseLocation;
    private Vector3 direction;

    [SerializeField]
    private int health;
    [SerializeField]
    private float mouseX, mouseY;
    [SerializeField]
    private float speed = 2.0f;

    private float rotationSpeed = 4.0f;

    private BoxCollider2D collisionBox;

    // Awake is called before first frame update
    void Awake()
    {
        cam = Camera.main;
        health = 3;
        collisionBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse position
        mouseLocation = cam.ScreenToWorldPoint(InputHandler.Instance.MousePos);
        //direction = mouseLocation - transform.position;

        // handle movement input
        velocityX += InputHandler.Instance.HorizontalAxis * speed;
        velocityX *= Time.deltaTime;

        velocityY += InputHandler.Instance.VerticalAxis * speed;
        velocityY *= Time.deltaTime;

        transform.Translate(velocityX, velocityY, 0, Space.World);

        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

        //**** slow rotation down for polish

        // get direction you want to point at
        Vector2 direction = (mouseLocation - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;

    }
}
