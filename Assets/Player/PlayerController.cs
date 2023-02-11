using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCombat))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement _movement;
        private PlayerCombat _combat;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _combat = GetComponent<PlayerCombat>();
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
    }
}