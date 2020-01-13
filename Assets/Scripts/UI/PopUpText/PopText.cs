using System;
using UnityEngine;
using TMPro;

namespace RPG.UI.PopUpText
{
    public class PopText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textValue = null;
        [SerializeField] bool isPositive = true;

        string modifier = "+";

        public void SetValue(float amount)
        {
            if (!isPositive) { modifier = ""; }
            textValue.text = String.Format("{0}{1}", modifier, amount);
        }
    }
}
