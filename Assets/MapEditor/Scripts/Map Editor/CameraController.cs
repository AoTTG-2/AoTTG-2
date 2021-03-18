using UnityEngine;

namespace MapEditor
{
    public class CameraController : MonoBehaviour
    {
        #region Fields

        //A self-reference to the singleton instance of this script
        public static CameraController Instance { get; private set; }

        //The default speed the camera moves at (assuming the movement axes are both 1)
        [SerializeField] private float defaultMovementSpeed = 100f;

        //The factor with which the movement speeds up or slows down when a speed modifier key is held
        [SerializeField] private float speedMultiplier = 5f;

        //Stores the three different speeds the camera can move at
        private AdjustableSpeed movementSpeed;

        //The speed the camera rotates at
        [SerializeField] private float rotateSpeed = 5f;

        //The multiplier for the distance of the camera from the selection bounds radius when focusing
        [SerializeField] private float focusDistMultiplier = 2f;

        #endregion

        #region Delegates

        public delegate void OnFocusEvent();

        public event OnFocusEvent OnFocus;

        #endregion

        #region Instantiation

        //Disable the fog on distant objects
        void OnPreRender()
        {
            RenderSettings.fog = false;
        }

        //Instantiate the adjustable speed class
        private void Awake()
        {
            //Set this script as the only instance of the SelectionHandle script
            if (Instance == null)
                Instance = this;

            movementSpeed = new AdjustableSpeed(defaultMovementSpeed, speedMultiplier);
        }

        #endregion

        #region Update

        //Running in Update instead of LateUpdate so that the camera updates before the selection handle
        private void Update()
        {
            //If the 'F' key was pressed, translate the camera to show the current selection
            if (Input.GetKeyDown(KeyCode.F) &&
                ObjectSelection.Instance.GetSelectionCount() != 0)
            {
                FocusCamera();
            }
            //Otherwise if the editor is in fly mode, update the camera position and rotation
            else if (EditorManager.Instance.CurrentMode == EditorMode.Fly)
            {
                TranslateCamera();
                RotateCamera();
            }
        }

        private void TranslateCamera()
        {
            //Get the current movement speed
            float movSpeed = this.movementSpeed.GetSpeed() * Time.deltaTime;

            //Get the amount to translate on the x and z axis
            float xDisplacement = Input.GetAxisRaw("Horizontal") * movSpeed;
            float zDisplacement = Input.GetAxisRaw("Vertical") * movSpeed;

            //Get the amount to translate on the y axis
            float yDisplacement = 0;

            //If only the left mouse button is pressed, move the camera down
            if (Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
                yDisplacement = -movSpeed;
            //If only the right mouse button is pressed, move the camera up
            else if (Input.GetButton("Fire2") && !Input.GetButton("Fire1"))
                yDisplacement = movSpeed;

            //Translate the camera on the x and z axes in self space
            transform.Translate(xDisplacement, 0, zDisplacement, Space.Self);
            //Translate the camera on the y axis in world space
            transform.Translate(0, yDisplacement, 0, Space.World);
        }

        private void RotateCamera()
        {
            //Find how much the camera should be rotated on the x and y axes, then add the current rotations to them
            float xRotation = (Input.GetAxis("Mouse Y") * -rotateSpeed) + transform.rotation.eulerAngles.x;
            float yRotation = (Input.GetAxis("Mouse X") * rotateSpeed) + transform.rotation.eulerAngles.y;

            //Restrict the camera angle so it doesn't flip
            if (xRotation > 90 && xRotation < 180)
                xRotation = 90;
            if (xRotation < 270 && xRotation > 180)
                xRotation = 270;

            //Set the new rotation of the camera
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }

        private void FocusCamera()
        {
            //Get the center and radius of the selection bounding sphere
            Bounds selectionBounds = ObjectSelection.Instance.SelectionBounds;
            Vector3 boundsCenter = selectionBounds.center;
            float boundsRadius = selectionBounds.extents.magnitude;

            //Expand the radius to keep the camera further away from the selection
            boundsRadius *= focusDistMultiplier;

            //The normalized vector pointing towards the camera along the camera's line of sight
            Vector3 lookVector = (-transform.forward).normalized;

            //Translate the camera to the intersection point between the look vector and the expanded bounding sphere
            transform.position = boundsCenter + (lookVector * boundsRadius);

            //Notify all listeners that the camera was focused
            OnFocus?.Invoke();
        }

        #endregion
    }
}