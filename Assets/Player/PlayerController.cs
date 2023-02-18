using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCombat))]
    public class PlayerController : MonoBehaviour
    {
        private GameObject _player;
        private PlayerMovement _movement;
        private PlayerCombat _combat;
        private GrapplingGun.GrapplingGun _grapplingGun;

        private void Awake()
        {
            _player = gameObject;
            _movement = GetComponent<PlayerMovement>();
            _combat = GetComponent<PlayerCombat>();
            _grapplingGun = GetComponent<GrapplingGun.GrapplingGun>();
        }

        // TODO: Particle effect when player is moving fast
        private void Update()
        {
            // TODO: Bounce back if something collides with player
            // TODO: Limit attacks
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _combat.Attack();
            }
            
            // TODO: Pistol
            // TODO: Grapple
            
            _movement.UpdateMovement(Input.GetAxisRaw("Horizontal"));
        }

        public void MoveToScene(Scene scene)
        {
            SceneManager.MoveGameObjectToScene(_player, scene);
        }
    }
}