using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HeavyAttackChargeCount : MonoBehaviour
    {
        [SerializeField]
        private PlayerController player;

        [SerializeField]
        private Text countText;
    
        // Start is called before the first frame update
        void Start()
        {
            countText.text = "0";
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = player.transform.position;
        
            // TODO: Only update when this changes
            countText.text = player.HeavyAttackCharges.ToString();
        }
    }
}
