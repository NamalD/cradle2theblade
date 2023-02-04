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

        void Update()
        {
            // TODO: Limit attacks
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _combat.Attack();
            }

            _movement.UpdateMovement(Input.GetAxisRaw("Horizontal"));
        }
    }
}