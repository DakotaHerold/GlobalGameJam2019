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
    private Collider2D[] tempItems;

    [SerializeField]
    private List<Collider2D> itemsNear;

    private MeshRenderer render;
    private FieldOfView fov;
    private bool flashlightOn;
    private bool colliding;
    private int itemColMask = 1 << 8;

    [SerializeField]
    private PlayerAnimState animState;
    private int health = 4;
    [SerializeField]
    private float speed = 2.0f;
    private float rotationSpeed = 4.0f;
    private float flashDistance = 2.5f;

    private bool takingDamage = false;
    private SpriteRenderer spriteRend;

    private FieldOfView flashlight; 

    // Awake is called before first frame update
    void Awake()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameManager.Instance.CurrentState);

        ItemRadiusCheck();
        if (GameManager.Instance.CurrentState != GameManager.GAME_STATE.RUNNING)
            return; 

        // get mouse position
        mouseLocation = cam.ScreenToWorldPoint(InputHandler.Instance.MousePos);
       


        if (!colliding)
        {
            // handle movement input
            velocityX += InputHandler.Instance.HorizontalAxis * speed;
            velocityX *= Time.deltaTime;

            velocityY += InputHandler.Instance.VerticalAxis * speed;
            velocityY *= Time.deltaTime;

            transform.Translate(velocityX, velocityY, 0, Space.World);
        }
        else
        {
            
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

        //FlashlightCheck(); 
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

            if (item != null)
            {
                GameManager.Instance.GetItemManager().RemoveHauntedItem(item); 
            }
            else if(ghost != null)
            {
                ghost.SpotGhost(); 
            }
        }
    }

    private void ItemRadiusCheck()
    {
        tempItems = Physics2D.OverlapCircleAll(transform.position, fov.viewRadius, itemColMask);


        foreach (var i in tempItems)
        {
            if (!itemsNear.Contains(i))
            {
                itemsNear.Add(i);
                AddNewHauntedItem(); 
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
            return;

        Debug.Log("collision");
        colliding = true;

        Ghost ghost = collision.gameObject.GetComponent<Ghost>(); 
        if (ghost == null)
        {
            Vector3 collisionDif = collision.transform.position - transform.position;
            float distance = .1f;

            Vector3 newDirection = -collisionDif.normalized;

            transform.position += (newDirection * distance);

            colliding = false;
        }
        else
        {
            colliding = false;
        }

        colliding = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
            return;

        Debug.Log("exit");
        colliding = false;
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
        flashlight.viewRadius += 0.25f;
        flashlight.viewAngle += 5.0f; 
    }

    public void DimFlashlight()
    {
        flashlight.viewRadius -= 0.2f;
        flashlight.viewAngle -= 2.5f;
    }

    public void TakeDamage()
    {
        if (takingDamage)
            return;

        
        health--;
        // TODO: Flash player, give momentary invulnerability
        if (health < 0)
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
        flashlight = GetComponentInChildren<FieldOfView>();
        spriteRend = GetComponent<SpriteRenderer>();
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
}
