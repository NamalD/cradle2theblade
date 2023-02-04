using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        public void SetMoveSpeed(float moveHorizontal)
        {
            animator.SetFloat(MoveSpeed, Math.Abs(moveHorizontal));
        }

        public void SetAttacking()
        {
            animator.SetTrigger(Attacking);
        }
    }
}