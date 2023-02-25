using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField]
        private Enemy enemy;

        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            _slider.maxValue = enemy.MaxHealth;
            _slider.value = enemy.CurrentHealth;
        }

        private void Update()
        {
            // TODO: Only update when enemy health changes
            _slider.value = enemy.CurrentHealth;
        }
    }
}