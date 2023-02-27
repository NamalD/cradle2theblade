using Common;
using Player.Gun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCombat))]
    [RequireComponent(typeof(GunBehaviour))]
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        private GameObject _player;
        private Health _health;
        private PlayerMovement _movement;
        private PlayerCombat _combat;
        private GrapplingGun.GrapplingGun _grapplingGun;
        private GunBehaviour _gun;

        private void Awake()
        {
            _player = gameObject;
            _health = GetComponent<Health>();
            _movement = GetComponent<PlayerMovement>();
            _combat = GetComponent<PlayerCombat>();
            _grapplingGun = GetComponent<GrapplingGun.GrapplingGun>();
            _gun = GetComponent<GunBehaviour>();
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
            
            // TODO: Limit rate of fire
            if (Input.GetKeyDown(KeyCode.F))
            {
                _gun.Fire();
            }
            
            // TODO: Move Grapple inputs to this controller
            
            _movement.UpdateMovement(Input.GetAxisRaw("Horizontal"));
        }

        public void MoveToScene(Scene scene)
        {
            SceneManager.MoveGameObjectToScene(_player, scene);
        }
    }
}