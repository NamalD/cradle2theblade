using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class EnemyHealthBar : MonoBehaviour
    {
        private Health _health;
        private Slider _slider;

        private void Awake()
        {
            _health = GetComponentInParent<Health>();
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            _slider.maxValue = _health.MaxHealth;
            _slider.value = _health.CurrentHealth;
        }

        private void Update()
        {
            // TODO: Only update when enemy health changes
            _slider.value = _health.CurrentHealth;
        }
    }
}