using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class Item : MonoBehaviour
    {
        public enum SHAKE_AXIS
        {
            None = 1,
            X = 2,
            Y = 4,
            Z = 8
        }


        private string itemName;
        // public item category
        public GameObject ItemManager;
        private bool isMessy;
        public bool IsMessy { get => isMessy; }
        protected bool shouldShake;
        protected bool shouldRotate;


        protected bool rotateLeft;
        protected bool rotating;
        protected float duration = .9f;
        protected float currentTime;
        protected float rotationSpeed = 100f;

        protected Vector3 startPos;
        protected Vector3 startScale;
        protected Quaternion startRot;

        // Shake variables
        protected float xShakeFrequency = 10.0f;  // Speed of sine movement - initial value: 20f
        protected float xShakeMagnitude = 3f;   // Size of sine movement - initial value: 0.5f
        protected float yShakeFrequency = 10.0f;
        protected float yShakeMagnitude = 3f;
        protected float scaleShakeFrequency = 3f;
        protected float scaleShakeMagnitude = 10f;

        private bool xNegateSin;
        protected bool yNegateSin;
        protected bool scaleNegateSin;

        protected bool stoppingShake;
        protected bool stoppingRotate;

        protected float stopDuration = 0.1f; 

        [EnumFlag]
        private SHAKE_AXIS shakeAxes;

        // Awake is called before Start
        protected virtual void Awake()
        {
            startPos = transform.position;
            startScale = transform.localScale;
            startRot = transform.rotation;

            // Testing
            //Shake(SHAKE_AXIS.Z | SHAKE_AXIS.X | SHAKE_AXIS.Y, 5f, 3f, false);
            //Shake(SHAKE_AXIS.Y, 11f, 4f, false);
            //Rotate();
        }

        // Update is called once per frame
        protected virtual void Update()
        {

            if (shouldShake == true)
            {
                ShakeItem(Time.deltaTime, Time.time);
            }
            else if(stoppingShake)
            {
                SlowShake(Time.deltaTime); 
            }

            if (shouldRotate == true)
            {
                RotateItem(Time.deltaTime);
            }
            else if(stoppingRotate)
            {
                SlowRotate(Time.deltaTime); 
            }
        }

        //Remove item from game world (A collectable)?
        public void Delete()
        {
            // TODO: Update based on item manager handling
            Destroy(this);
        }

        private void SlowShake(float deltaTime)
        {
            bool scaleComplete = false;
            bool transformComplete = false; 
            float lerp = deltaTime / stopDuration;

            // Stop  scale shake
            float scale = Mathf.Lerp(transform.localScale.x, startScale.x, lerp);
            transform.localScale = new Vector3(scale, scale, transform.localScale.z);

            // Round to nearest tenth to stop the scale lerp
            if (Mathf.Round((scale * 10f)) / 10f == startScale.x)
            {
                transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
                scaleComplete = true; 
            }

            transform.position = Vector3.Lerp(transform.position, startPos, lerp);

            // Round to nearest tenth to stop vector lerp
            if (Mathf.Round((transform.position.x * 10f)) / 10f == startPos.x)
            {
                transform.position = startPos;
                transformComplete = true;
            }

            if(transformComplete && scaleComplete)
            {
                stoppingShake = false; 
            }
        }

        private void ShakeItem(float deltaTime, float totalTime)
        {
            // Shouldn't be shaking
            if (shakeAxes == SHAKE_AXIS.None)
            {
                Debug.LogWarning("Shake axes on " + gameObject.name + " was none and attempted to shake."); 
                return;
            }

            Vector3 shakeOffset = Vector3.zero; 
            float xSinValue = Mathf.Sin(totalTime * xShakeFrequency) * xShakeMagnitude;
            float ySinValue = Mathf.Sin(totalTime * yShakeFrequency) * yShakeMagnitude; 

            if (shakeAxes.HasFlag(SHAKE_AXIS.X))
            {
                if (!xNegateSin)
                    shakeOffset.x += xSinValue;
                else
                    shakeOffset.x -= xSinValue; 
            }
            if (shakeAxes.HasFlag(SHAKE_AXIS.Y))
            {
                if (!yNegateSin)
                    shakeOffset.y += ySinValue;
                else
                    shakeOffset.y -= ySinValue; 
            }
            transform.position = (startPos + shakeOffset);

            // Z is not visible in 2D so adjust scale to compensate for same effect. 
            if (shakeAxes.HasFlag(SHAKE_AXIS.Z))
            {
                float scaleSinValue = Mathf.Sin(totalTime * scaleShakeFrequency) * scaleShakeMagnitude; 
                float newScale = (scaleNegateSin) ? -scaleSinValue : scaleSinValue;  
                transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
            }
            
           
            
        }

        private void SlowRotate(float deltaTime)
        {
            float lerp = deltaTime / stopDuration;
            Quaternion newRot = Quaternion.Lerp(transform.rotation, startRot, lerp);
            transform.rotation = newRot;
            if (Mathf.Round((newRot.z * 10f)) / 10f == startRot.z)
            {
                transform.rotation = startRot;
                stoppingRotate = false; 
            }
        }

        private void RotateItem(float deltaTime)
        {
            //gameObject.transform.Rotate(Vector3.forward * Time.deltaTime * speed); //if this rotates around but we can't see it change it to vector3.right

            //If the time elapsed is less than the time desired to rotate
            if (currentTime < duration)
            {
                rotating = true;
            }
            // We've rotated this direction for the desired time - rotate the other direction.
            else
            {
                rotateLeft = !rotateLeft;
                currentTime = 0;
                duration = 1.8f;
            }

            //If object is currently rotating
            if (rotating == true)
            {
                float direction;

                // If we're rotating left
                if (rotateLeft)
                {
                    direction = -1f;
                    direction *= rotationSpeed;
                }

                //We're rotating right
                else
                {
                    direction = 1;
                    direction *= rotationSpeed;
                }


                gameObject.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 5), direction * Time.deltaTime);
                currentTime += deltaTime;

            }
        }

        // Public methods ------------------------------------
        public void Rotate()
        {
            shouldRotate = true;
            stoppingRotate = false; 
        }

        public void StopRotate()
        {
            shouldRotate = false;
            stoppingRotate = true; 
        }

        /// <summary>
        /// Begins shaking on the specified axis or axes. For varying frequency and magnitudes, multiple calls per axis must be made
        /// </summary>
        /// <param name="axes">The axis or axes to be shook</param>
        /// <param name="frequency">The rate of the sin wave</param>
        /// <param name="magnitude">The distance of the sin wave</param>
        /// <param name="negate">Start negative or positive in sin wave</param>
        public void Shake(SHAKE_AXIS axes, float frequency = 10.0f, float magnitude = 3.0f, bool negate = false)
        {
            if(axes.HasFlag(SHAKE_AXIS.X))
            {
                xShakeFrequency = frequency;
                xShakeMagnitude = magnitude;
                xNegateSin = negate; 
            }
            if (axes.HasFlag(SHAKE_AXIS.Y))
            {
                yShakeFrequency = frequency;
                yShakeMagnitude = magnitude;
                yNegateSin = negate; 
            }
            if (axes.HasFlag(SHAKE_AXIS.Z))
            {
                scaleShakeFrequency = frequency;
                scaleShakeMagnitude = magnitude;
                scaleNegateSin = negate; 
            }

            shakeAxes |= axes;
            shouldShake = true;
            stoppingShake = false; 
        }

        public void StopShake()
        {
            shakeAxes = SHAKE_AXIS.None;
            shouldShake = false;
            stoppingShake = true; 
        }

        protected virtual void OnMouseDown()
        {
            // TODO, handle callbacks
            Debug.Log(gameObject.name + " Pressed."); 
        }

    }
}