using UnityEngine;

namespace PhysicsSystem
{
    [CreateAssetMenu(menuName = "Springs", fileName = "New_SpringConfigSo", order = 0)]
    public class SpringConfigSo : ScriptableObject
    {
        public float theKFactor = 1f;
        public float minimumDisplacement;
        public float maximumDisplacement = 4f;
    }
}