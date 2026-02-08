using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OduLib.Systems.MapDesign
{
    [ExecuteInEditMode]
    public class PinVisualizationView : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private MapPinDatabase _pinDatabase;
        [SerializeField] private Transform _pinCreatorAnchor;

        [Header("Prefabs")]
        [SerializeField] private GameObject _pinPrefab;
        [SerializeField] private PinRenderObject _uiPinPrefab;

        [Header("UI")]
        [SerializeField] private Transform _uiPinParent;
        [SerializeField] private Camera _pinCamera;
        [SerializeField] private Vector3 _worldOffset;

        [Header("Pin Info Panel")]
        [SerializeField] private GameObject _pinInfoContent;
        [SerializeField] private Image _pinInfoImage;
        [SerializeField] private TMP_Text _pinInfoNameLabel;
        [SerializeField] private TMP_Text _pinInfoDescriptionLabel;
        [SerializeField] private Button _pinInfoCloseButton;

        [Header("Debug")]
        [SerializeField] private Map _currentMap;
        [SerializeField] private bool _render;
        [SerializeField] private bool _createPin;

        private List<GameObject> _instantiatedPins;
        private List<GameObject> _instantiatedUiPins;

        private void Update()
        {
            if (_render)
            {
                RenderAllPins();
                _render = false;
            }

            if (_createPin)
            {
                CreatePin();
                _createPin = false;
            }
        }

        private void CreatePin()
        {
            _pinDatabase.CreatePin(_currentMap, "Test", "This is a pin test", null, _pinCreatorAnchor.position);
        }

        public void RenderAllPins()
        {
            SetupPinInfoPanel();
            ClearInstantiatedLists();
            InstantiateAndSetupPins();
        }

        private void InstantiateAndSetupPins()
        {
            var currentPins = _pinDatabase.GetPinsFromMap(_currentMap);

            foreach (var pin in currentPins)
            {
                var instantiatedPin = Instantiate(_pinPrefab, pin.Position, Quaternion.identity);
                var position = instantiatedPin.transform.position;

                position.y = 15;
                instantiatedPin.transform.position = position;

                _instantiatedPins.Add(instantiatedPin);

                var uiPin = Instantiate(_uiPinPrefab, _uiPinParent);
                var uiPinRect = uiPin.GetComponent<RectTransform>();
                uiPinRect.position = _pinCamera.WorldToScreenPoint(pin.Position + _worldOffset);

                uiPin.Setup(pin, (pin) => RenderPinInfo(pin));
                _instantiatedUiPins.Add(uiPin.gameObject);
            }
        }

        private void ClearInstantiatedLists()
        {
            _instantiatedPins ??= new List<GameObject>();
            _instantiatedUiPins ??= new List<GameObject>();

            foreach (var instantiatedPin in _instantiatedPins)
            {
                DestroyImmediate(instantiatedPin);
            }

            foreach (var uiPin in _instantiatedUiPins)
            {
                DestroyImmediate(uiPin);
            }
        }

        private void SetupPinInfoPanel()
        {
            _pinInfoCloseButton.onClick.RemoveAllListeners();
            _pinInfoCloseButton.onClick.AddListener(() => HidePinInfo());
        }

        private void RenderPinInfo(MapPin pinData)
        {
            _pinInfoContent.SetActive(true);

            _pinInfoNameLabel.SetText(pinData.Name);
            _pinInfoDescriptionLabel.SetText(pinData.Description);
        }

        private void HidePinInfo()
        {
            _pinInfoContent.SetActive(false);
        }
    }
}