using UnityEngine;

namespace PhysicsSystem
{
    public class PhysicsManager : MonoBehaviour
    {
        [SerializeField] private float timeScale = 1f;

        public float GetTimeScale() => timeScale;
        public float GetDeltaTimeScale() => Time.deltaTime * timeScale;
        public float GetFixedDeltaTimeScale() => Time.fixedDeltaTime * timeScale;
    }
}