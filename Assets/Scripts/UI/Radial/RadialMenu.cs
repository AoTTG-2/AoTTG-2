using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Radial
{
    public class RadialMenu : MonoBehaviour
    {
        public FlareRadialMenu FlareMenu;

        public RadialElement RadialElementPrefab;
        public float GapWidthDegree = 1f;

        protected RadialElement[] Pieces;

        public int Distance = 200;
        public Text Label;
        public Interactable Selected;

        protected RadialMenu Parent;

        private bool IsReady { get; set; }

        private void OnEnable()
        {
            GameCursor.ApplyCursorMode();
        }

        protected virtual void OnDisable()
        {
            GameCursor.ApplyCameraMode();
        }

        protected virtual void Start()
        {
            var flares = Instantiate(RadialElementPrefab, transform);
            flares.NextMenu = FlareMenu;

            Pieces = new[]
            {
                flares
            };
            
            StartCoroutine(SpawnButtons());
        }

        protected virtual IEnumerator SpawnButtons()
        {
            var stepLength = 360f / Pieces.Length;
            var iconDist = Vector3.Distance(RadialElementPrefab.Icon.transform.position, RadialElementPrefab.CakePiece.transform.position);
            
            for (int i = 0; i < Pieces.Length; i++)
            {
                //Pieces[i] = Instantiate(RadialElementPrefab, transform);
                //set root element
                Pieces[i].transform.localPosition = Vector3.zero;
                Pieces[i].transform.localRotation = Quaternion.identity;

                //set cake piece
                Pieces[i].CakePiece.fillAmount = 1f / Pieces.Length - GapWidthDegree / 360f;
                Pieces[i].CakePiece.transform.localPosition = Vector3.zero;
                Pieces[i].CakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + GapWidthDegree / 2f + i * stepLength);
                Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);

                //set icon
                Pieces[i].Icon.transform.localPosition = Pieces[i].CakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
                //Pieces[i].Icon.sprite = Data.Elements[i].Icon;

                Pieces[i].Animate();

                yield return new WaitForSeconds(0.05f);
            }

            IsReady = true;
        }

        protected virtual void OnElementClicked(RadialElement element, int index)
        {

        }

        private void Update()
        {
            if (!IsReady) return;
            var stepLength = 360f / Pieces.Length;
            var mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, UnityEngine.Input.mousePosition - transform.position, Vector3.forward) + stepLength / 2f);
            var activeElement = (int) (mouseAngle / stepLength);
            for (int i = 0; i < Pieces.Length; i++)
            {
                if (Pieces[i].CakePiece == null) continue;
                if (i == activeElement)
                    Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.75f);
                else
                    Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
            }

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (Pieces[activeElement].NextMenu != null)
                {
                    var newSubRing = Instantiate(Pieces[activeElement].NextMenu, transform.parent).GetComponent<RadialMenu>();
                    newSubRing.Parent = this;
                    for (var j = 0; j < newSubRing.transform.childCount; j++)
                        Destroy(newSubRing.transform.GetChild(j).gameObject);
                }
                else
                {
                    OnElementClicked(Pieces[activeElement], activeElement);
                }
                gameObject.SetActive(false);
            }
        }

        private float NormalizeAngle(float a) => (a + 360f) % 360f;
    }
}
