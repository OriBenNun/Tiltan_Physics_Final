using UnityEngine;

namespace PhysicsSystem
{
    public class Spring : MonoBehaviour
    {
        private SpringConfigSo _springConfigSo;
        private float _kFactor;
        private float _maximumX;
        private float _minimumX;
        
        private float _currentX;

        public void InitSpring(SpringConfigSo configSo)
        {
            _springConfigSo = configSo;
            _kFactor = configSo.theKFactor;
            _maximumX = configSo.maximumDisplacement;
            _minimumX = configSo.minimumDisplacement;

            _currentX = _minimumX;
        }

        public bool AddDisplacement(float amountToAdd)
        {
            if (_currentX + amountToAdd > _maximumX || _currentX + amountToAdd < _minimumX)
            {
                return false;
            }

            _currentX += amountToAdd;
            return true;
        }
        
        public float GetForceAndReleaseTension()
        {
            var force = _kFactor * _currentX;
            _currentX = _minimumX;
            return force;
        }

        public float GetCurrentTensionNormalized() => (_currentX - _minimumX) / (_maximumX - _minimumX);
    }
}