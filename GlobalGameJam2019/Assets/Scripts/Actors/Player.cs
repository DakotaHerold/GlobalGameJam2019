using Jam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // enums for states
    private enum PlayerAnimState
    {
        standing = 0,
        walking = 1,
        hit = 2
    }

    // attributes
    private float velocityX, velocityY;
    private float mouseX, mouseY;
    private Camera cam;
    private Vector2 mouseLocation;
    private Vector3 direction;
    private BoxCollider2D collisionBox;
    private bool flashlightOn;

    // flashlight attributes
    private float viewRadius;
    private float viewAngle;

    [SerializeField]
    private PlayerAnimState animState;
    [SerializeField]
    private int health;
    [SerializeField]
    private float speed = 2.0f;
    private float rotationSpeed = 4.0f;
    private float flashDistance = 2.5f;

    // Awake is called before first frame update
    void Awake()
    {
        flashlightOn = false;
        cam = Camera.main;
        health = 3;
        collisionBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse position
        mouseLocation = cam.ScreenToWorldPoint(InputHandler.Instance.MousePos);

        // handle movement input
        velocityX += InputHandler.Instance.HorizontalAxis * speed;
        velocityX *= Time.deltaTime;

        velocityY += InputHandler.Instance.VerticalAxis * speed;
        velocityY *= Time.deltaTime;

        transform.Translate(velocityX, velocityY, 0, Space.World);

        //**** slow rotation down for polish?

        // get direction you want to point at
        Vector2 direction = (mouseLocation - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;

        // state handling
        if (velocityX != 0 || velocityY != 0)
        {
            animState = PlayerAnimState.walking;
        }
        else
        {
            animState = PlayerAnimState.standing;
        }

    }
}
