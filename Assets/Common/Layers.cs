using UnityEngine;

namespace Common
{
    public class Layers : MonoBehaviour
    {
        public static Layers Instance { get; } = new GameObject().AddComponent<Layers>();

        public int Scatter { get; private set; }

        public int Floor { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitialiseLayers();
        }
        
        private void InitialiseLayers()
        {
            Scatter = LayerMask.NameToLayer("Scatter");
            Floor = LayerMask.NameToLayer("Floor");
        }
    }
}