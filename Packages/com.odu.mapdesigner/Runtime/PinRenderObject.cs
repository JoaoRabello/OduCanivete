using System;
using UnityEngine;
using UnityEngine.UI;

namespace OduLib.Systems.MapDesign
{
    public class PinRenderObject : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;

        private MapPin _pinData;

        public void Setup(MapPin pinData, Action<MapPin> onSelect)
        {
            _pinData = pinData;

            _selectButton.onClick.RemoveAllListeners();
            _selectButton.onClick.AddListener(() => OnClick(onSelect));
        }

        private void OnClick(Action<MapPin> select)
        {
            select?.Invoke(_pinData);
        }
    }
}
