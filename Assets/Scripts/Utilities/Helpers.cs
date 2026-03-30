using System.Collections.Generic;
using UnityEngine;

// using UnityEngine.EventSystems;

namespace DungTran31.Utilities
{
    public static class Helpers
    {
        private static Camera _camera;

        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

        public static Camera Camera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }

        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        /*
        Example Implementation
        IEnumerator Bad()
        {
            for(var i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }

        IEnumerator Good()
        {
            for(var i = 0; i < 100; i++)
            {
                yield return Helpers.GetWait(0.01f);
            }
        }
        */

        /*private static PointerEventData _eventDataCurrentPosition;
        private static List<RayCastResult> _results;
        public static bool IsOverUi() {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { _eventDataCurrentPosition = Input.mousePosition };
            _results = new List<RayCastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }*/
        // IsOverUi() tương đương với EventSystem.current.IsPointerOverGameObject() có sẵn trong unity

        /*
        Example Implementation
        [SerializeField] private TextMeshProUGUI _text;
        void Update()
        {
            _text.text = Helpers.IsOverUi() ? "Over UI" : "Home free baby";
        }
        */

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
            return result;
        }

        /*
        Example Implementation
        thường sử dụng để đưa 1 object 3D vào canvas
        Gán script vào object 3D
        [SerializeField] private RectTransform _followTarget;

        void Update()
        {
            transform.position = Helpers.GetWorldPositionOfCanvasElement(_followTarget);
        }
        */

        public static void DeleteChildren(this Transform t)
        {
            foreach (Transform child in t) Object.Destroy(child.gameObject);
        }
    }
}