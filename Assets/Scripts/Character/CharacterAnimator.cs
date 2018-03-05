using Physics;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Body))]
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected Body Body;
        
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
            Body = GetComponent<Body>();
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
            Animator.SetBool("Grounded", Body.State.IsGrounded());

            // set the velocity animation parameters
            Animator.SetFloat("Horizontal Velocity", Body.Velocity.x);
            Animator.SetFloat("Vertical Velocity", Body.Velocity.y);
        }
    }
}