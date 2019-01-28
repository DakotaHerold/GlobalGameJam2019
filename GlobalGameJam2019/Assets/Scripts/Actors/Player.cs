using Jam;
using System;
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
    private Vector3 velocity; 
    private float mouseX, mouseY;
    private Camera cam;
    private Vector2 mouseLocation;
    private Vector3 direction;
    private BoxCollider2D collisionBox;
    private Collider2D[] tempItems;

    [SerializeField]
    private List<Collider2D> itemsNear;

    private MeshRenderer lightConeRenderer;
    private FieldOfView fov;
    private bool flashlightOn;
    private bool colliding;
    private int itemColMask = 1 << 8;

    [SerializeField]
    private PlayerAnimState animState;
    private int health = 4;
    [SerializeField]
    private float speed = 0.002f;
    private float rotationSpeed = 4.0f;
    private float flashDistance = 2.5f;

    private bool takingDamage = false;
    private SpriteRenderer spriteRend;

    private bool flashlightActive; 

    private Rigidbody2D rb;
    private CircleCollider2D collider;

    private Animator animController;
    [SerializeField]
    private Vector3 movingFlashlightLocalPos;

    private GameObject flashlightObj; 
    // Awake is called before first frame update
    void Awake()
    {
        flashlightObj = transform.GetChild(0).gameObject; 
        animController = GetComponent<Animator>(); 
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

        ItemRadiusCheck();
        FlashLightUpdate(); 
        MouseRotationUpdate(); 



        

        // state handling
        //if (velocity.x != 0 || velocity.y != 0)
        //{
        //    animState = PlayerAnimState.walking;
        //}
        //else
        //{
        //    animState = PlayerAnimState.standing;
        //}
    }

    private void FixedUpdate()
    {
        MovementInput(); 
    }

    private void FlashLightUpdate()
    {
        if (GameManager.Instance.CurrentState != GameManager.GAME_STATE.RUNNING)
            return; 

        if(InputHandler.Instance.JumpPressed)
        {
            GameManager.Instance.GetAudioManager().PlayFlashlightSFX(); 
        }
        else if(InputHandler.Instance.JumpReleased)
        {
            GameManager.Instance.GetAudioManager().PlayFlashlightSFX();
        }

        if (InputHandler.Instance.JumpHeld)
        {
            // Toggle Flashlight
            //ToggleFlashlight(); 
            ShowCone(); 
        }
        else
        {
            HideCone();
        }
    }

    private void ShowCone()
    {
        flashlightActive = true;
        lightConeRenderer.enabled = true;
        fov.enabled = true;
    }

    private void HideCone()
    {
        flashlightActive = false;
        lightConeRenderer.enabled = false;
        fov.enabled = false; 
    }

    private void ToggleFlashlight()
    {
        if(flashlightActive)
        {
            ShowCone(); 
        }
        else
        {
            HideCone(); 
        }
        // TODO: Play flash light click sound here
    }


    private void MouseRotationUpdate()
    {
        if (GameManager.Instance.CurrentState != GameManager.GAME_STATE.RUNNING)
        {
            transform.up = Vector3.up; 
            return;
        }

        // get mouse position
        mouseLocation = cam.ScreenToWorldPoint(InputHandler.Instance.MousePos);

        // get direction you want to point at
        Vector2 direction = (mouseLocation - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
    }

    private void MovementInput()
    {
        if (GameManager.Instance == null)
            return; 

        if (GameManager.Instance.CurrentState != GameManager.GAME_STATE.RUNNING)
            return;

        // Update velocity
        velocity.x += InputHandler.Instance.HorizontalAxis * speed * Time.deltaTime;

        velocity.y += InputHandler.Instance.VerticalAxis * speed * Time.deltaTime;

        Vector2 newPos = rb.position + new Vector2(velocity.x, velocity.y);
        rb.MovePosition(newPos);

        // Update animation 
        if (InputHandler.Instance.HorizontalAxis != 0 || InputHandler.Instance.VerticalAxis != 0)
        {
            animController.SetBool("Moving", true);
            flashlightObj.transform.localPosition = movingFlashlightLocalPos;
        }
        else
        {
            animController.SetBool("Moving", false);
            flashlightObj.transform.localPosition = Vector3.zero; 
        }
        // Zero out for next frame
        velocity = Vector3.zero;
        
    }

    public void FlashlightCheck()
    {
        List<Transform> visibleTargets = fov.visibleTargets;
        //Debug.Log(visibleTargets.Count); 
        foreach(Transform t in visibleTargets)
        {
            //Debug.Log(t.gameObject.name); 
            Ghost ghost = t.gameObject.GetComponent<Ghost>();
            Item item = t.gameObject.GetComponent<Item>();

            if(flashlightActive)
            {
                if (item != null)
                {
                    GameManager.Instance.GetItemManager().ResetHauntedItem(item);
                }
                else if (ghost != null)
                {
                    ghost.SpotGhost();
                }
            }
            
        }
        fov.visibleTargets.Clear();
    }

    private void ItemRadiusCheck()
    {
        tempItems = Physics2D.OverlapCircleAll(transform.position, fov.viewRadius, itemColMask);


        foreach (var i in tempItems)
        {
            Item baseItem = i.gameObject.GetComponent<Item>(); 

            if(baseItem != null)
            {
                if (baseItem is Collectible || baseItem is Fixable)
                {
                    continue;
                }
                else
                {
                    if (!itemsNear.Contains(i))
                    {
                        itemsNear.Add(i);
                        AddNewHauntedItem();
                    }
                }
            }
        }

        for (int j = itemsNear.Count - 1; j > -1; j--)
        {
            bool matches = false;

            for (int k = 0; k < tempItems.Length; k++)
            {
                if (itemsNear[j] == tempItems[k])
                {
                    matches = true;
                }
            }
            if (matches != true)
            {
                RemoveHauntedItem(itemsNear[j].gameObject.GetComponent<Item>()); 
                itemsNear.Remove(itemsNear[j]);
            }
        }
    }

    private void AddNewHauntedItem()
    {
        GameManager.Instance.NewVicinityItem(); 
    }

    private void RemoveHauntedItem(Item item)
    {
        GameManager.Instance.RemoveHauntedItem(item);
    }

    public List<Item> GetVicinityItems()
    {
        List<Item> result = new List<Item>(); 
        foreach(Collider2D c in itemsNear)
        {
            result.Add(c.gameObject.GetComponent<Item>()); 
        }
        return result;
    }

    public void BrightenFlashlight()
    {
        fov.viewRadius += 0.25f;
        fov.viewAngle += 5.0f; 
    }

    public void DimFlashlight()
    {
        fov.viewRadius -= 0.2f;
        fov.viewAngle -= 2.5f;
    }

    public void TakeDamage()
    {
        if (takingDamage)
            return;

        
        health--;
        // TODO: Flash player, give momentary invulnerability
        if (health <= 0)
            GameManager.Instance.GameOver(); 
        else
        {
            DimFlashlight();
            StartCoroutine(DamageRoutine()); 
        }

    }

    IEnumerator DamageRoutine()
    {
        takingDamage = true; 
        Color baseColor = spriteRend.color;
        for (int i = 0; i < 5; ++i)
        {
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(.1f);
            spriteRend.color = Color.red;
            yield return new WaitForSeconds(.1f);
        }
        spriteRend.color = baseColor;
        takingDamage = false; 
    }

    public void Reset()
    {
        collider = GetComponent<CircleCollider2D>(); 
        velocity = Vector3.zero;
        rb = GetComponent<Rigidbody2D>();       
        spriteRend = GetComponent<SpriteRenderer>();
        lightConeRenderer = GetComponentInChildren<MeshRenderer>();
        lightConeRenderer.sortingLayerName = SortingLayer.layers[3].name;
        //Debug.Log(render.sortingLayerName);
        fov = GetComponent<FieldOfView>();
        flashlightOn = false;
        cam = Camera.main;
        health = 3;
        collisionBox = GetComponent<BoxCollider2D>();
        itemsNear = new List<Collider2D>();

        HideCone();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), collider);
        }
    }
}
