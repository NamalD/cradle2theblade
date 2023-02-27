using Enemies;
using UnityEngine;

namespace World
{
    public class GameManager : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField]
        private Transform enemySpawnPoint;
        
        [SerializeField]
        private Enemy enemyPrefab;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        public void SpawnEnemy()
        {
            var enemy = Instantiate(enemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
            enemy.Health.onDeath.AddListener(SpawnEnemy);
        }
    }
}