﻿using Jam;
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
    private Collider2D[] tempItems;

    [SerializeField]
    private List<Collider2D> itemsNear;

    private MeshRenderer render;
    private FieldOfView fov;
    private bool flashlightOn;
    private bool colliding;
    private int itemColMask = 1 << 9;

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
        render = GetComponentInChildren<MeshRenderer>();
        render.sortingLayerName = SortingLayer.layers[3].name;
        Debug.Log(render.sortingLayerName);
        fov = GetComponent<FieldOfView>();
        flashlightOn = false;
        cam = Camera.main;
        health = 3;
        collisionBox = GetComponent<BoxCollider2D>();
        itemsNear = new List<Collider2D>();
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

        tempItems = Physics2D.OverlapCircleAll(transform.position, fov.viewRadius, itemColMask);


        foreach(var i in tempItems)
        {
            if(!itemsNear.Contains(i))
            {
                itemsNear.Add(i);
            }
        }

        for(int j = itemsNear.Count - 1; j > -1; j--)
        {
            bool matches = false;

            for (int k = 0; k < tempItems.Length; k++)
            {
                if (itemsNear[j] == tempItems[k])
                {
                    matches = true;
                }
            }
            if(matches != true)
            {
                itemsNear.Remove(itemsNear[j]);
            }
        }
            //Debug.Log("item near");

        if (!colliding)
        {
            transform.Translate(velocityX, velocityY, 0, Space.World);
        }
        else
        {
            transform.Translate(-velocityX * 5, -velocityY * 5, 0, Space.World);
        }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        colliding = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
        colliding = false;
    }

    public List<Collider2D> GetItems()
    {
        return itemsNear;
    }
}
