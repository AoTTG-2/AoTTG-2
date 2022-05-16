using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tooltip
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI HeaderField;
        public TextMeshProUGUI ContentField;

        public LayoutElement LayoutElement;
        public int CharacterWrapLimit;

        public RectTransform RectTransform;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void SetText(string content, string header = "")
        {
            HeaderField.gameObject.SetActive(!string.IsNullOrWhiteSpace(header));
            ContentField.text = content;
            HeaderField.text = header;

            var headerLength = HeaderField.text?.Length;
            var contentLength = ContentField.text.Length;

            LayoutElement.enabled = headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit;
        }

        private void Update()
        {
            if (Application.isEditor)
            {
                var headerLength = HeaderField.text.Length;
                var contentLength = ContentField.text.Length;

                LayoutElement.enabled = headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit;
            }

            Vector2 position = UnityEngine.Input.mousePosition;
            transform.position = position;
        }
    }
}
