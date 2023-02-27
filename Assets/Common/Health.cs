using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;

        public UnityEvent onDeath;

        public int MaxHealth => maxHealth;

        public int CurrentHealth { get; private set; }

        private void Start()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
                onDeath.Invoke();
        }
    }
}