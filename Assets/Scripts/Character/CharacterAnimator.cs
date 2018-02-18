using UnityEngine;

namespace Character
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected CharacterController2D Character;
        
        /// <summary>
        /// 
        /// </summary>
        protected Animator Animator;

        /// <summary>
        /// 
        /// </summary>
        protected Player Player;
        
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Character = GetComponent<CharacterController2D>();
            Animator = GetComponent<Animator>();
            Player = GetComponent<Player>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            Animate();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Animate()
        {
            SendMessage("OnAnimate", Animator, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        public void OnAnimate(Animator animator)
        {
            // set gounded animation parameter
            Animator.SetBool("Grounded", Character.State.IsGrounded());

            // set the velocity animation parameters
            Animator.SetFloat("Horizontal Velocity", Character.Velocity.x);
            Animator.SetFloat("Vertical Velocity", Character.Velocity.y);
        }
    }
}