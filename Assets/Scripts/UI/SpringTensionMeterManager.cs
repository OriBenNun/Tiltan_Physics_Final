using System;
using Game.Cannon;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpringTensionMeterManager : MonoBehaviour
    {
        [SerializeField] private Color regularColor;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private float changeColorToHighlightDuration = 0.4f;
        [SerializeField] private float changeColorToRegularDuration = 0.2f;
        [SerializeField] private Image addTensionButtonImage;
        [SerializeField] private Image removeTensionButtonImage;
        [SerializeField] private Image tensionMeterFillImage;
        
        private void Start()
        {
            CannonController.OnSpringChanged += HandleOnSpringChange;
            
            UpdateTensionMeter(0);
        }

        private void OnDestroy()
        {
            CannonController.OnSpringChanged -= HandleOnSpringChange;
        }

        private void HandleOnSpringChange(SpringAction action, float tensionNormalized)
        {
            UpdateTensionMeter(tensionNormalized);
            switch (action)
            {
                case SpringAction.AddTension:
                    ChangeColor(addTensionButtonImage, highlightedColor, changeColorToHighlightDuration);
                    ChangeColor(removeTensionButtonImage, regularColor, changeColorToRegularDuration);
                    break;
                case SpringAction.SubtractTension:
                    ChangeColor(removeTensionButtonImage, highlightedColor, changeColorToHighlightDuration);
                    ChangeColor(addTensionButtonImage, regularColor, changeColorToRegularDuration);
                    break;
                case SpringAction.None:
                    ChangeColor(removeTensionButtonImage, regularColor, changeColorToRegularDuration);
                    ChangeColor(addTensionButtonImage, regularColor, changeColorToRegularDuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        private void UpdateTensionMeter(float tensionNormalized)
        {
            tensionMeterFillImage.fillAmount = tensionNormalized;
        }

        private void ChangeColor(Image image, Color color, float duration)
        {
            image.CrossFadeColor(color, duration, true, true);
        }
    }
}
