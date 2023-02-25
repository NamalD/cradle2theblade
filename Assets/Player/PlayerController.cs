using Projectile;
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

        // TODO: Extract gun behaviour
        [SerializeField]
        private ProjectileBehaviour projectilePrefab;

        [SerializeField]
        private Transform gunFirePoint;

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
            if (Input.GetKeyDown(KeyCode.F))
            {
                // TODO: Create a gun fire position which is independent from the grapple
                Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
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