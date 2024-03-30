using UnityEngine;

namespace PhysicsSystem
{
    public class PhysicsManager : MonoBehaviour
    {
        [SerializeField] private float timeScale = 1f;

        public float GetTimeScale() => timeScale;
    }
}