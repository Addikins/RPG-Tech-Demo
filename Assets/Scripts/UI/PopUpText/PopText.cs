using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI.PopUpText
{
    public class PopText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textValue = null;

        public void SetValue(float amount)
        {
            textValue.text = String.Format("{0}", amount);
        }
    }
}
