using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class Ghost : MonoBehaviour
    {

        Vector3 target;
        private float minMoveSpeed = 0.5f;
        private float maxMoveSpeed = 3.0f;
        private float currentMoveSpeed = 1f;
        private float rotationSpeed = 3.0f;

        private float attackRadius = 0.25f;
        private float forgetRadius = 5.0f; 

        private bool allowedToSeek = true;

        private GhostManager ghostManager;

        [SerializeField]
        private Sprite activeSprite;
        [SerializeField]
        private Sprite deathSprite;

        private SpriteRenderer spriteRend; 

        private void Awake()
        {
            spriteRend = GetComponent<SpriteRenderer>(); 
            spriteRend.sprite = activeSprite; 

            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }


            // Testing
            //gameObject.SetActive(true);
            //EnableSeek();
        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                target = GameManager.Instance.GetPlayer().transform.position;

                if (allowedToSeek)
                {
                    if(GameManager.Instance.CurrentState == GameManager.GAME_STATE.RUNNING)
                        SeekTarget();
                }
            }

          
        }

        private void SeekTarget()
        {
            //Find distance
            Vector3 direction = target - transform.position;

            // Check if we're too far away to seek 
            if(direction.magnitude >= forgetRadius)
            {
                return; 
            }

            //Check if we are within the radius
            if (direction.magnitude < attackRadius)
            {
                ReachedPlayer(); 
                return;
            }
            // Move to the target
            // TODO: Slow movespeed 
            transform.position = Vector3.MoveTowards(transform.position, target, currentMoveSpeed * Time.deltaTime);
            //Rotate to the target
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        public void ReachedPlayer()
        {
            DisableSneak();
            gameObject.SetActive(false);
            GameManager.Instance.DealPlayerDamage(); 
            if(ghostManager != null)
            {
                ghostManager.SetGhostInactive(this); 
            }
        }

        public void EnableSeek()
        {
            allowedToSeek = true;
        }

        public void DisableSneak()
        {
            allowedToSeek = false;
        }

        public void SpawnGhost()
        {
            spriteRend.sprite = activeSprite;
            gameObject.SetActive(true);
            EnableSeek();
        }

        public void SetGhostManager(GhostManager manager)
        {
            ghostManager = manager; 
        }

        public void SpotGhost()
        {
            StartCoroutine(DeathRoutine()); 
        }

        IEnumerator DeathRoutine()
        {
            spriteRend.sprite = deathSprite;
            DisableSneak();
            yield return new WaitForSeconds(0.75f);
            gameObject.SetActive(false);
            if (ghostManager != null)
            {
                ghostManager.SetGhostInactive(this);
            }
        }
    }
}
