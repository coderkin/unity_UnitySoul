using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CF
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;
        private Color enableColor;
        private Color unEnableColor;
        private Image fill;

        private void Awake()
        {
            fill = transform.Find("Fill").GetComponent<Image>();
            enableColor = fill.color;
        }

        public void SetMaxStamina(int maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }

        public void SetCurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
        }

        public void EnableStaminaBar(bool isEnable)
        {
            if(isEnable)
            {
                fill.color = enableColor;
            }else
            {
                fill.color = Color.gray;
            }
        }
    }
}
