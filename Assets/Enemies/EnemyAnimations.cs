using UnityEngine;

namespace Enemies
{
    public class EnemyAnimations : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int IsDead = Animator.StringToHash("IsDead");
        
        public void TriggerHurt()
        {
            animator.SetTrigger(Hurt);
        }
        
        public void TriggerDeath()
        {
            animator.SetBool(IsDead, true);
        }
    }
}