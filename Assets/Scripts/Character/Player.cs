using Objects;
using Traits;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Character
{
    /// <summary>
    /// 
    /// </summary>
    public struct ControllerInput
    {
        public struct Button
        {
            public bool Pressed;
            public bool Held;
            public bool Released;
        }
        
        public Vector2 Movement;
        public Vector2 Aim;

        public Button Up;
        public Button Down;
        public Button Left;
        public Button Right;

        public Button A;
        public Button B;
        public Button X;
        public Button Y;

        public Button L;
        public Button R;

        public Button Start;
        public Button Select;
    }
    
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public CharacterController2D Controller { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected bool Listening = true;
        
        /// <summary>
        /// 
        /// </summary>
        public ControllerInput ControllerInput;

        /// <summary>
        /// 
        /// </summary>
        private enum Directions
        {
            Right,
            Left
        }
        
        /// <summary>
        /// 
        /// </summary>
        private Directions Direction = Directions.Right;

        /// <summary>
        /// 
        /// </summary>
        public AudioClip LandSound;

        /// <summary>
        /// 
        /// </summary>
        public Checkpoint Checkpoint;

        //////////////////////
        ///// GAME HOOKS /////
        //////////////////////

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            // reference components
            Controller = GetComponent<CharacterController2D>();

            // determine the facing direction
            Direction = transform.localScale.x > 0f ? Directions.Right : Directions.Left;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            HandleInput();
        }

        /////////////////
        ///// INPUT /////
        /////////////////

        /// <summary>
        /// 
        /// </summary>
        public void StartListening()
        {
            Listening = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopListening()
        {
            Listening = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsListening()
        {
            return Listening;
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleInput()
        {
            ControllerInput.Movement = Vector2.ClampMagnitude(new Vector2(
                Input.GetAxis("Left Stick Horizontal") + (Input.GetButton("Left") ? -1f : 0f) + (Input.GetButton("Right") ? 1f : 0f),
                Input.GetAxis("Left Stick Vertical") + (Input.GetButton("Up") ? -1f : 0f) + (Input.GetButton("Down") ? 1f : 0f)
            ), 1f);

            ControllerInput.Aim = new Vector2(
                Input.GetAxis("Left Stick Horizontal") + Input.GetAxis("Right Stick Horizontal"),
                Input.GetAxis("Left Stick Vertical") + Input.GetAxis("Right Stick Vertical")
            ).normalized;

            ControllerInput.Up.Pressed = Input.GetButtonDown("Up");
            ControllerInput.Up.Held = Input.GetButton("Up");
            ControllerInput.Up.Released = Input.GetButtonUp("Up");
            
            ControllerInput.Down.Pressed = Input.GetButtonDown("Down");
            ControllerInput.Down.Held = Input.GetButton("Down");
            ControllerInput.Down.Released = Input.GetButtonUp("Down");
            
            ControllerInput.Left.Pressed = Input.GetButtonDown("Left");
            ControllerInput.Left.Held = Input.GetButton("Left");
            ControllerInput.Left.Released = Input.GetButtonUp("Left");
            
            ControllerInput.Right.Pressed = Input.GetButtonDown("Right");
            ControllerInput.Right.Held = Input.GetButton("Right");
            ControllerInput.Right.Released = Input.GetButtonUp("Right");

            ControllerInput.A.Pressed = Input.GetButtonDown("A");
            ControllerInput.A.Held = Input.GetButton("A");
            ControllerInput.A.Released = Input.GetButtonUp("A");
            
            ControllerInput.B.Pressed = Input.GetButtonDown("B");
            ControllerInput.B.Held = Input.GetButton("B");
            ControllerInput.B.Released = Input.GetButtonUp("B");
            
            ControllerInput.X.Pressed = Input.GetButtonDown("X");
            ControllerInput.X.Held = Input.GetButton("X");
            ControllerInput.X.Released = Input.GetButtonUp("X");
            
            ControllerInput.Y.Pressed = Input.GetButtonDown("Y");
            ControllerInput.Y.Held = Input.GetButton("Y");
            ControllerInput.Y.Released = Input.GetButtonUp("Y");

            ControllerInput.L.Pressed = Input.GetButtonDown("L");
            ControllerInput.L.Held = Input.GetButton("L");
            ControllerInput.L.Released = Input.GetButtonUp("L");
            
            ControllerInput.R.Pressed = Input.GetButtonDown("R");
            ControllerInput.R.Held = Input.GetButton("R");
            ControllerInput.R.Released = Input.GetButtonUp("R");
            
            ControllerInput.Start.Pressed = Input.GetButtonDown("Start");
            ControllerInput.Start.Held = Input.GetButton("Start");
            ControllerInput.Start.Released = Input.GetButtonUp("Start");
            
            ControllerInput.Select.Pressed = Input.GetButtonDown("Select");
            ControllerInput.Select.Held = Input.GetButton("Select");
            ControllerInput.Select.Released = Input.GetButtonUp("Select");
            
            SendMessage("OnInput", this, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void OnInput(Player player)
        {
            if (!player.IsListening())
            {
                return;
            }
            
            if (Controller.State.IsAiming())
            {
                FaceDirection(ControllerInput.Aim);
            }
            else
            {
                FaceDirection(ControllerInput.Movement);
            }
        }
        
        //////////////////
        ///// FACING /////
        //////////////////

        /// <summary>
        /// 
        /// </summary>
        protected void FaceDirection(Vector2 direction)
        {
            if (direction.x > 0f && Direction != Directions.Right)
            {
                Flip();
            }
            else if (direction.x < 0f && Direction != Directions.Left)
            {
                Flip();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Flip()
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );

            Direction = transform.localScale.x > 0f ? Directions.Right : Directions.Left;
        }

        /////////////////////////
        ///// SOUND EFFECTS /////
        /////////////////////////

        /// <summary>
        /// 
        /// </summary>
        public void OnLand()
        {
            if (LandSound)
            {
                AudioSource.PlayClipAtPoint(LandSound, transform.position);
            }
        }

        /////////////////////
        ///// ANIMATION /////
        /////////////////////
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        public void OnAnimate(Animator animator)
        {
            // set moving animation parameter
            animator.SetBool("Moving", (
                Mathf.Abs(ControllerInput.Movement.x) > 0f &&
                Controller.State.IsGrounded() &&
                !Controller.State.IsAiming() &&
                !Controller.State.IsCrouching()
            ));
            
            // set the movement animation parameters
            animator.SetFloat("Horizontal Movement", ControllerInput.Movement.x);
            animator.SetFloat("Vertical Movement", ControllerInput.Movement.y);
        }

        /////////////////
        ///// DEATH /////
        /////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damagable"></param>
        public void OnDeath(Damagable damagable)
        {
            if (Checkpoint && Checkpoint.Active)
            {
                Checkpoint.Respawn(this);

                damagable.Revive();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}