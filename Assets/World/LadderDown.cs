using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World
{
    public class LadderDown : MonoBehaviour
    {
        [SerializeField]
        private Transform ladderPosition;

        [SerializeField]
        private string nextScene;

        // TODO: Combine these together (player controller => game object => layer)
        private GameObject _player;

        [SerializeField]
        private LayerMask playerLayer;

        [SerializeField]
        private PlayerController playerController;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            var collision = Physics2D.OverlapBox(
                ladderPosition.position,
                ladderPosition.localScale,
                angle: 0,
                // TODO: Use layer from player game object instead
                playerLayer);
            
            // TODO: Display UI hint on collision
            if (collision && Input.GetKeyDown(KeyCode.E))
            {
                // TODO: Create enum for scene names and make this a parameter
                StartCoroutine(LoadNextScene(nextScene));
            }
        }

        private IEnumerator LoadNextScene(string nextSceneName)
        {
            DontDestroyOnLoad(_player);
            
            var loadOperation = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
            while (!loadOperation.isDone)
                yield return null;

            var newScene = SceneManager.GetSceneByName(nextSceneName);
            SceneManager.SetActiveScene(newScene);
            playerController.MoveToScene(newScene);
        }
    }
}