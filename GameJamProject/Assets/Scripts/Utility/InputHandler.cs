using UnityEngine;

namespace Jam
{
    // Handles single player input for standard 18 inputs. Assumes buttons are pressed instead of held. 
    public class InputHandler : Singleton<InputHandler>
    {
        private float horizontalAxis;
        private float verticalAxis;
        private bool fire1Pressed;
        private bool fire2Pressed;
        private bool fire3Pressed;
        private bool jumpPressed;
        private float mouseX;
        private float mouseY;
        private float mouseScrollWheel;
        private bool submitPressed;
        private bool cancelPressed;

        private bool fire1Released;
        private bool fire2Released;
        private bool fire3Released;
        private bool jumpReleased;
        private bool submitReleased;
        private bool cancelReleased;

        private bool fire1Held;
        private bool fire2Held;
        private bool fire3Held;
        private bool jumpHeld;
        private bool submitHeld;
        private bool cancelHeld;

        public float HorizontalAxis
        {
            get
            {
                return horizontalAxis;
            }
        }

        public float VerticalAxis
        {
            get
            {
                return verticalAxis;
            }
        }

        public bool Fire1Pressed
        {
            get
            {
                return fire1Pressed;
            }
        }

        public bool Fire2Pressed
        {
            get
            {
                return fire2Pressed;
            }
        }

        public bool Fire3Pressed
        {
            get
            {
                return fire3Pressed;
            }
        }

        public bool JumpPressed
        {
            get
            {
                return jumpPressed;
            }
        }

        public float MouseX
        {
            get
            {
                return mouseX;
            }
        }

        public float MouseY
        {
            get
            {
                return mouseY;
            }
        }

        public float MouseScrollWheel
        {
            get
            {
                return mouseScrollWheel;
            }
        }

        public bool SubmitPressed
        {
            get
            {
                return submitPressed;
            }
        }

        public bool CancelPressed
        {
            get
            {
                return cancelPressed;
            }
        }

        public bool Fire1Released
        {
            get
            {
                return fire1Released;
            }
        }

        public bool Fire2Released
        {
            get
            {
                return fire2Released;
            }
        }

        public bool Fire3Released
        {
            get
            {
                return fire3Released;
            }
        }

        public bool JumpReleased
        {
            get
            {
                return jumpReleased;
            }
        }

        public bool SubmitReleased
        {
            get
            {
                return submitReleased;
            }
        }

        public bool CancelReleased
        {
            get
            {
                return cancelReleased;
            }
        }

        public bool Fire1Held
        {
            get
            {
                return fire1Held;
            }
        }

        public bool Fire2Held
        {
            get
            {
                return fire2Held;
            }
        }

        public bool Fire3Held
        {
            get
            {
                return fire3Held;
            }
        }

        public bool JumpHeld
        {
            get
            {
                return jumpHeld;
            }
        }

        public bool SubmitHeld
        {
            get
            {
                return submitHeld;
            }
        }

        public bool CancelHeld
        {
            get
            {
                return cancelHeld;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Axes
            horizontalAxis = Input.GetAxis("Horizontal");
            verticalAxis = Input.GetAxis("Vertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel"); 

            // Button Presses
            fire1Pressed = Input.GetButtonDown("Fire1");
            fire2Pressed = Input.GetButtonDown("Fire2");
            fire3Pressed = Input.GetButtonDown("Fire3");
            jumpPressed = Input.GetButtonDown("Jump");
            submitPressed = Input.GetButtonDown("Submit");
            cancelPressed = Input.GetButtonDown("Cancel");

            // Button Holds
            fire1Held = Input.GetButton("Fire1");
            fire2Held = Input.GetButton("Fire2");
            fire3Held = Input.GetButton("Fire3");
            jumpHeld = Input.GetButton("Jump");
            submitHeld = Input.GetButton("Submit");
            cancelHeld = Input.GetButton("Cancel");

            // Button Releases
            fire1Released = Input.GetButtonUp("Fire1");
            fire2Released = Input.GetButtonUp("Fire2");
            fire3Released = Input.GetButtonUp("Fire3");
            jumpReleased = Input.GetButtonUp("Jump");
            submitReleased = Input.GetButtonUp("Submit");
            cancelReleased = Input.GetButtonUp("Cancel");

            // Testing
            if (JumpPressed)
                Debug.Log("Jump Pressed");
            if (JumpHeld)
                Debug.Log("Jump Held");
            if (jumpReleased)
                Debug.Log("Jump Released"); 
        }
    }
}


