using UnityEngine;

namespace PhysicsSystem
{
    public class Spring : MonoBehaviour
    {
        private SpringConfigSo springConfigSo;
        private float kFactor;
        private float maximumX;
        private float minimumX;
        
        private float currentX;

        // Here's how you can apply Hooke's Law to your cannon charging mechanic:
        // Initialize Spring Parameters: Set up parameters such as the spring constant (k),
        // the maximum displacement (x max), and the minimum and maximum shoot forces.
        // Calculate Displacement: Track how long the shoot button is held down and use this information
        // to calculate the displacement of the "spring" (charging mechanic).
        // You can use a formula like x=k⋅t, where t is the time the shoot button is held down.
        // Calculate Force: Apply Hooke's Law to calculate the force to be applied to the projectile using
        // the displacement calculated in the previous step. Shoot Projectile: Use the calculated force to shoot
        // the projectile.
        

        public void InitSpring(SpringConfigSo configSo)
        {
            springConfigSo = configSo;
            kFactor = configSo.theKFactor;
            maximumX = configSo.maximumDisplacement;
            minimumX = configSo.minimumDisplacement;

            currentX = minimumX;
        }

        public bool AddDisplacement(float amountToAdd)
        {
            if (currentX + amountToAdd > maximumX || currentX + amountToAdd < minimumX)
            {
                return false;
            }

            currentX += amountToAdd;
            Debug.Log($"Spring: {springConfigSo.springName} Current X: {currentX}");
            return true;
        }
        
        public float GetForceAndReleaseTension()
        {
            var force = kFactor * currentX;
            currentX = minimumX;
            return force;
        }
    }
}