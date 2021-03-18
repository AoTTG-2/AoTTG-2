using System.Collections;
using System.Drawing;
//Used for manipulating the cursor position (Windows)
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapEditor
{
    public class SelectionHandle : MonoBehaviour
    {
        #region Serialized Data Members
        //Meshes used for the handle lines and plane drag box
        private Mesh handleLineMesh;
        private Mesh handleTriangleMesh;

        //Meshes used for the tool handle axis ends
        [SerializeField] private Mesh coneMesh;
        [SerializeField] private Mesh cubeMesh;

        //Materials used for the tool handle gizmo
        [SerializeField] private Material handleOpaqueMaterial;
        [SerializeField] private Material rotateLineMaterial;
        [SerializeField] private Material handleTransparentMaterial;

        //The sizes for the tool handle meshes
        [SerializeField] private float handleBoxSize = .25f;
        [SerializeField] private float handleSize = 10f;
        [SerializeField] private float capSize = .07f;

        //The width around the tool handle that can be interacted with
        [SerializeField] int handleInteractWidth = 10;
        //The padding around the edges of the window past which the mouse will be moved back into the window
        [SerializeField] int windowPadding = 5;

        //Adjusts the speed of handle movement when rotating or translating 
        [SerializeField] private float defaultTranslationSpeed = 1f;
        [SerializeField] private float defaultRotationSpeed = 3f;
        [SerializeField] private float defaultScaleSpeed = 1f;
        //The fast and slow speeds are calculated from the default speed and the multiplier
        [SerializeField] private float speedMultiplier = 50f;

        //The maximum distance away from the origin an object can be
        [SerializeField] private float maxDistance = 10000000f;
        #endregion

        #region Non-Serialized Data Members
        //A self-reference to the singleton instance of this script
        public static SelectionHandle Instance { get; private set; }

        //Stores the position and rotation information of the tool handle gizmo
        private Matrix4x4 handleMatrix;
        //Transform of the parent GameObject
        private Transform parentTransform;
        //Main camera in the scene
        private Camera mainCamera;

        //The current tool being used
        private Tool currentTool;
        //When the tool is set, rebuild the gizmo mesh
        public Tool Tool
        {
            get { return currentTool; }

            set
            {
                if (currentTool != value)
                {
                    currentTool = value;
                    RebuildGizmoMesh(Vector3.one);
                }
            }
        }

        //Stores the three different speeds for each tool
        private AdjustableSpeed translationSpeed;
        private AdjustableSpeed rotationSpeed;
        private AdjustableSpeed scaleSpeed;

        //Save the handle displacements for when they need to be returned
        private Vector3 prevPosition;
        private float rotationDisplacement;
        private Vector3 prevScale;
        private Vector3 scale;
        private float prevCursorDist;
        private float currCursorDist;

        //The current position of the mouse. Used instead of Input.mousePosition because it can be modified
        private Vector2 currentMousePosition;
        //Used to keep track of the displacement of the mouse between frames
        private Vector2 prevMousePosition = Vector2.zero;
        //The distance the mouse moved between the previous and the current frame
        private Vector2 mouseDisplacement = Vector2.zero;
        //The amount to offset the on-screen cursor to get the hypothetical unconstrained position. Used in the plane drag and scale all tools
        private Vector2 mouseOffest = Vector2.zero;

        ///Persistent variables used by the rotation tool
        //The angle displacement of the rotation handle since the drag started
        private float axisAngle = 0f;
        //Determines if the latest rotation was positive or negative
        private float sign;
        //The vector in screen-space representing the tangent line of the rotation handle that was clicked
        private Vector2 clickTangent;

        //The position and rotation of the handle when it was clicked. 
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        //The scale of the handle when it was released
        private Vector3 endScale;

        //Persistent variable used by the translation tool
        private float cameraDist;

        //Determines if the handle is being interacted with or not
        private bool draggingHandle;
        //In how many directions is the handle able to move
        private int draggingAxes = 0;
        //The transform of the rotation handle used when using the rotate tool
        private TransformData handleOrigin = TransformData.Identity;

        //Determines if the handle should be displayed and interactable
        private bool hidden = false;

        //The octant of the camera relative to the tool handle
        public Vector3 viewOctant { get; private set; }
        //The octant of the camera in the previous frame
        private Vector3 previousOctant;

        //Delegates for getting and setting the transform of the selection handle
        public Vector3 Position
        {
            get { return parentTransform.position; }
            set { parentTransform.position = value; }
        }
        public Quaternion Rotation
        {
            get { return parentTransform.rotation; }
            set { parentTransform.rotation = value; }
        }
        public Vector3 Scale
        {
            get { return parentTransform.localScale; }
            set { parentTransform.localScale = value; }
        }
        #endregion

        #region Initialization
        private void Awake()
        {
            //Set this script as the only instance of the SelectionHandle script
            if (Instance == null)
                Instance = this;

            //Hide the handle by default
            hidden = true;
            parentTransform = gameObject.GetComponent<Transform>();
            mainCamera = Camera.main;

            //Instantiate the adjustable speed classes
            translationSpeed = new AdjustableSpeed(defaultTranslationSpeed, speedMultiplier, true, false);
            rotationSpeed = new AdjustableSpeed(defaultRotationSpeed, speedMultiplier, true, false);
            scaleSpeed = new AdjustableSpeed(defaultScaleSpeed, speedMultiplier, true, false);

            //The selection handle is in translate mode by default
            Tool = Tool.Translate;

            //Create the meshes needed for the tool handle
            handleLineMesh = new Mesh();
            CreateHandleLineMesh(ref handleLineMesh, Vector3.one);
            handleTriangleMesh = new Mesh();
            CreateHandleTriangleMesh(ref handleTriangleMesh, Vector3.one);
            //Set the default size of the handle mesh
            scale = Vector3.one;
        }
        #endregion

        #region Delegates
        public delegate void OnHandleMoveEvent();
        public event OnHandleMoveEvent OnHandleMove;

        public delegate void OnHandleBeginEvent();
        public event OnHandleBeginEvent OnHandleBegin;

        public delegate void OnHandleFinishEvent();
        public event OnHandleFinishEvent OnHandleFinish;
        #endregion

        #region Imported Functions
        //Import external windows functions for getting and setting the cursor position
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out int x, out int y);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        #endregion

        #region Drag Orientation Class
        private class DragOrientation
        {
            public Vector3 origin;
            //The primary axis the handle is being dragged along in local coordinates (x, y, or z)
            public Vector3 localAxis;
            //The arbitrary axis the handle is being dragged along in world coordinates
            public Vector3 worldAxis;
            public Vector3 offset;
            public Plane plane;

            public DragOrientation()
            {
                origin = Vector3.zero;
                worldAxis = Vector3.zero;
                offset = Vector3.zero;
                plane = new Plane(Vector3.up, Vector3.zero);
            }
        }

        private DragOrientation drag = new DragOrientation();
        #endregion Drag Class

        #region Update
        //Check for any mouse inputs and handle interactions
        private void Update()
        {
            //Don't display or interact with the handle if it is hidden
            if (!hidden)
            {
                //Don't check for handle interactions if the handle is hidden or the editor is not in edit mode
                if (EditorManager.Instance.CurrentMode != EditorMode.Edit)
                    return;

                //Save the current mouse position
                currentMousePosition = Input.mousePosition;
                //Calculate the mouse displacement
                mouseDisplacement = currentMousePosition - prevMousePosition;
                //Save the position of the mouse for the next frame
                prevMousePosition = currentMousePosition;

                //While the tool handle is being dragged, make sure the mouse stays within the window bounds
                if (GetDragging() && !Input.GetMouseButtonUp(0))
                    ConstrainMouse();
                //If the mouse is pressed, check if the handle was clicked
                if (Input.GetMouseButtonDown(0))
                    CheckInteract();

                //Check if the handle is being dragged
                if (draggingHandle)
                {
                    //If the mouse is released, finish interacting with the handle
                    if (Input.GetMouseButtonUp(0))
                        OnFinishHandleMovement();
                    //If the mouse is pressed, interact with the handle
                    else if (Input.GetMouseButton(0))
                        InteractHandle();
                }
            }
        }

        //Rebuild the gizmos after all camera movements are finished
        private void LateUpdate()
        {
            //Update the octant the camera is in relative to the tool handle
            previousOctant = viewOctant;
            viewOctant = GetViewOctant();

            //Update the gizmo mesh and scale
            RebuildGizmoMesh(scale);
            RebuildGizmoMatrix();
        }
        #endregion

        #region Handle Methods
        private bool CheckHandleActivated(Vector2 mousePosition, out Axis plane)
        {
            plane = Axis.None;

            if (Tool == Tool.Translate || Tool == Tool.Scale)
            {
                float handleScreenSize = HandleUtility.GetHandleSize(parentTransform.position, mainCamera.transform.position, handleSize);

                // center
                Vector2 cen = mainCamera.WorldToScreenPoint(parentTransform.position);
                // up
                Vector2 up = mainCamera.WorldToScreenPoint((parentTransform.position + (parentTransform.up + parentTransform.up * capSize * 4f) * handleScreenSize));
                // right
                Vector2 right = mainCamera.WorldToScreenPoint((parentTransform.position + (parentTransform.right + parentTransform.right * capSize * 4f) * handleScreenSize));
                // forward
                Vector2 forward = mainCamera.WorldToScreenPoint((parentTransform.position + (parentTransform.forward + parentTransform.forward * capSize * 4f) * handleScreenSize));
                // First check if the plane boxes have been activated
                Vector2 p_right = (cen + ((right - cen) * viewOctant.x) * handleBoxSize);
                Vector2 p_up = (cen + ((up - cen) * viewOctant.y) * handleBoxSize);
                Vector2 p_forward = (cen + ((forward - cen) * viewOctant.z) * handleBoxSize);

                //x plane
                if (HandleUtility.PointInPolygon(new Vector2[] { cen, p_up, p_up, (p_up+p_forward) - cen,
                                                                (p_up + p_forward) - cen, p_forward, p_forward, cen },
                                                                mousePosition))
                {
                    plane = Axis.Y | Axis.Z;
                }
                //y plane
                else if (HandleUtility.PointInPolygon(new Vector2[] { cen, p_right, p_right, (p_right+p_forward)-cen,
                                                                    (p_right + p_forward)-cen, p_forward, p_forward, cen },
                                                                    mousePosition))
                {
                    plane = Axis.X | Axis.Z;
                }
                //z plane
                else if (HandleUtility.PointInPolygon(new Vector2[] { cen, p_up, p_up, (p_up + p_right) - cen,
                                                                    (p_up + p_right) - cen, p_right, p_right, cen },
                                                                    mousePosition))
                {
                    plane = Axis.X | Axis.Y;
                }
                //x axis
                else if (HandleUtility.DistancePointLineSegment(mousePosition, cen, up) < handleInteractWidth)
                    plane = Axis.Y;
                //y axis
                else if (HandleUtility.DistancePointLineSegment(mousePosition, cen, right) < handleInteractWidth)
                    plane = Axis.X;
                //z axis
                else if (HandleUtility.DistancePointLineSegment(mousePosition, cen, forward) < handleInteractWidth)
                    plane = Axis.Z;
                else
                    return false;

                return true;
            }
            else
            {
                Vector3[][] vertices = HandleMesh.GetRotationVertices(16, 1f);

                float best = Mathf.Infinity;

                Vector2 cur, prev = Vector2.zero;
                plane = Axis.X;

                for (int i = 0; i < 3; i++)
                {
                    cur = mainCamera.WorldToScreenPoint(vertices[i][0]);

                    for (int n = 0; n < vertices[i].Length - 1; n++)
                    {
                        prev = cur;
                        cur = mainCamera.WorldToScreenPoint(handleMatrix.MultiplyPoint3x4(vertices[i][n + 1]));

                        float dist = HandleUtility.DistancePointLineSegment(mousePosition, prev, cur);

                        if (dist < best && dist < handleInteractWidth)
                        {
                            Vector3 viewDir = (handleMatrix.MultiplyPoint3x4((vertices[i][n] + vertices[i][n + 1]) * .5f) - mainCamera.transform.position).normalized;
                            Vector3 nrm = parentTransform.TransformDirection(vertices[i][n]).normalized;

                            if (Vector3.Dot(nrm, viewDir) > .5f)
                                continue;

                            best = dist;

                            switch (i)
                            {
                                case 0: // Y
                                    plane = Axis.Y; // Axis.X | Axis.Z;
                                    break;

                                case 1: // Z
                                    plane = Axis.Z;// Axis.X | Axis.Y;
                                    break;

                                case 2: // X
                                    plane = Axis.X;// Axis.Y | Axis.Z;
                                    break;
                            }
                        }
                    }
                }

                if (best < handleInteractWidth + .1f)
                    return true;
            }

            return false;
        }

        private void CheckInteract()
        {
            //Don't check for handle interactions if it is hidden or if the cursor is over the UI
            if (hidden || EventSystem.current.IsPointerOverGameObject(-1))
                return;

            //The axis or axes along which the tool handle is being dragged
            Axis plane;

            //Check if the tool handle was clicked
            draggingHandle = CheckHandleActivated(currentMousePosition, out plane);

            //If the tool handle wasn't clicked, don't start the interaction
            if (!draggingHandle)
                return;

            //Save the current transform of the tool handle
            handleOrigin.SetTRS(parentTransform);

            //Reset the axes being dragged
            drag.worldAxis = Vector3.zero;
            drag.localAxis = Vector3.zero;
            draggingAxes = 0;

            //Set the relevant variables based on the drag plane
            SetDragData(plane);

            //Save the handle position and its distance to the camera
            if (Tool == Tool.Translate)
            {
                originalPosition = parentTransform.position;
                cameraDist = (mainCamera.transform.position - parentTransform.position).magnitude;
            }
            //Save the current rotation, reset the total displacement and save the angle of the cursor click
            if (Tool == Tool.Rotate)
            {
                originalRotation = parentTransform.rotation;
                axisAngle = 0f;
                clickTangent = GetClickTangent();
            }
            //Reset the handle size and prime it for scaling
            else
            {
                prevScale = Vector3.one;
                scale = Vector3.one;
                currCursorDist = GetMouseHandleDist(currentMousePosition);
            }

            //Capture the cursor while dragging
            EditorManager.Instance.CaptureCursor();
            //Notify all listeners that the tool handle was activated
            OnHandleBegin?.Invoke();
        }

        private void InteractHandle()
        {
            //Set the starting point of the drag to the position of the handle
            drag.origin = parentTransform.position;

            //Reset the persistent variables of each tool
            prevPosition = parentTransform.position;
            rotationDisplacement = 0f;
            prevScale = scale;
            prevCursorDist = currCursorDist;

            //Only rotate the handle if the mouse was moved
            if (mouseDisplacement.magnitude > 0f)
            {
                switch (Tool)
                {
                    case Tool.Translate:
                        //If the plane translate is selected, use the whole hit point as the position of the handle
                        if (draggingAxes > 1)
                        {
                            //Get the position under the cursor but on the movement plane
                            Vector3 planeHit = GetMovementPlaneHit();

                            //If the position is not valid, don't move the tool handle
                            for (int axis = 0; axis < 3; axis++)
                                if (float.IsNaN(planeHit[axis]))
                                    return;

                            //If the point is valid, move the tool handle to the point under the cursor
                            parentTransform.position = planeHit - drag.offset;
                        }
                        //If only one axis is selected, use the component of the mouse displacement parallel to the drag axis
                        else
                        {
                            //Get the displacement of the mouse in the handle's local space along the drag axis
                            Vector3 translationVector = GetDragDisplacement(mouseDisplacement);
                            //Scale the translation vector by the translation speed and distance to camera
                            translationVector *= cameraDist / 1000 * translationSpeed.GetSpeed();

                            //Translate the tool handle
                            parentTransform.Translate(translationVector, Space.Self);

                            //If any of the axes of the object went out of bounds, set it back to the maximum valid value
                            for (int axis = 0; axis < 3; axis++)
                            {
                                if (Mathf.Abs(parentTransform.position[axis]) > maxDistance)
                                {
                                    //Get the sign of the current position
                                    float positionSign = Mathf.Sign(parentTransform.position[axis]);

                                    //Set the position to back in bounds
                                    Vector3 fixedPosition = parentTransform.position;
                                    fixedPosition[axis] = maxDistance * positionSign;
                                    parentTransform.position = fixedPosition;
                                }
                            }
                        }
                        break;

                    case Tool.Rotate:
                        //Project the mouse displacement onto the tangent vector to get the component tangent to the rotation handle
                        Vector2 tangentDisplacement = ProjectBontoA(mouseDisplacement, clickTangent);
                        //Use the dot product between the tangent displacement and click tangent to get the sign of the rotation
                        sign = Vector2.Dot(tangentDisplacement, clickTangent) > 0 ? 1f : -1f;

                        //Use the magnitude of the displacement as the angle displacement
                        float angleDisplacement = tangentDisplacement.magnitude * sign;
                        //Add the displacement to the angle after scaling it by the rotation speed
                        rotationDisplacement = angleDisplacement / 10 * rotationSpeed.GetSpeed();
                        axisAngle += rotationDisplacement;

                        //Rotate the tool handle
                        parentTransform.rotation = Quaternion.AngleAxis(axisAngle, drag.worldAxis) * handleOrigin.Rotation;

                        break;

                    //If the tool isn't translate or rotate, it is scale
                    default:
                        //Stores the scale factor of each axis
                        Vector3 scaleVector;

                        //If all axes are being dragged, scale based on the distance between the cursor and tool handle
                        if (draggingAxes > 1)
                        {
                            //Get the distance in screen space between the tool handle and the cursor
                            currCursorDist = GetMouseHandleDist(currentMousePosition + mouseOffest);
                            //Calculate the displacement of the distance since last frame
                            float displacement = currCursorDist - prevCursorDist;
                            //Multiply the drag axis by the displacement
                            scaleVector = new Vector3(displacement, displacement, displacement);
                        }
                        //If only one axis is being dragged, only use the mouse displacement parallel to the axis being dragged
                        else
                            scaleVector = GetDragDisplacement(mouseDisplacement);

                        //Scale the vector by the translation speed and distance to camera
                        scaleVector *= scaleSpeed.GetSpeed() / 100;

                        //Scale the axis of the scale vector by the scale displacement
                        for (int axis = 0; axis < 3; axis++)
                            if (scaleVector[axis] != 0)
                                scale[axis] += scaleVector[axis];

                        break;
                }
            }

            //Notify all listeners that the handle was moved
            OnHandleMove?.Invoke();
        }

        //If the mouse moves too close to the edges of the window, move it to the opposite side
        private void ConstrainMouse()
        {
            //Get the position of the cursor relative to the window
            Vector2Int mousePosition = new Vector2Int((int)currentMousePosition.x, (int)currentMousePosition.y);
            //The amount the cursor needs to be moved to keep it inside the window
            Vector2 positionOffset = Vector2.zero;

            //Check if the mouse is out of the window horizontally
            if (mousePosition.x < windowPadding)
                positionOffset.x = Screen.width - (2 * windowPadding) - positionOffset.x;
            else if (mousePosition.x > Screen.width - windowPadding)
                positionOffset.x = -Screen.width + (2 * windowPadding) + positionOffset.x;

            //Check if the mouse is out of window vertically
            if (mousePosition.y < windowPadding)
                positionOffset.y = Screen.height - (2 * windowPadding) - positionOffset.y;
            else if (mousePosition.y > Screen.height - windowPadding)
                positionOffset.y = -Screen.height + (2 * windowPadding) + positionOffset.y;

            //If the mouse needs to be moved, set its position and calculate the mouse displacement
            if (positionOffset.x != 0 || positionOffset.y != 0)
            {
                //The location of the cursor relative to the screen
                Vector2Int cursorLocation;
                int x;
                int y;

                //Get the cursor location relative to the screen, add the offset, and set the new mouse position
                GetCursorPos(out x, out y);
                cursorLocation = new Vector2Int(x, y);
                SetCursorPos(cursorLocation.x + (int)positionOffset.x, cursorLocation.y - (int)positionOffset.y);

                //Add the offset to the mouse position so that the mouse displacement doesn't include the mouse repositioning
                currentMousePosition += positionOffset;
                prevMousePosition = currentMousePosition;

                //Store the total offset between the on-screen cursor and the hypothetical unconstrained cursor
                mouseOffest -= positionOffset;
            }
        }
        #endregion

        #region Helper Methods
        //Get the octant to display the planes in based on camera position and tool dragging status
        private Vector3 GetViewOctant()
        {
            //If the tool is not being dragged, calculate the current octant
            if (!draggingHandle)
            {
                //Convert the camera position to the local position of the tool handle
                Vector3 localCameraPos = parentTransform.InverseTransformPoint(mainCamera.transform.position);
                //Calculate the current octant
                return HandleUtility.GetViewOctant(localCameraPos);
            }

            //If it is being dragged, use the octant the camera was in before the drag
            return previousOctant;
        }

        //Find the point the mouse is over on the plane the handle is moving along
        private Vector3 GetMovementPlaneHit()
        {
            //Create a ray originating from the camera and passing through the cursor
            Ray ray = mainCamera.ScreenPointToRay(currentMousePosition + mouseOffest);

            //Find the position the cursor over on the corresponding plane
            if (drag.plane.Raycast(ray, out float distToHit))
                return ray.GetPoint(distToHit);

            //Otherwise return NAN to indicate a failure to get the point
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }

        private Vector3 GetClickVector()
        {
            //A plane representing the axis currently being dragged
            Plane movementPlane = new Plane();
            //The point where the ray intersected the plane
            Vector3 hitPoint;

            //Create a ray originating from the camera and passing through the cursor
            Ray ray = mainCamera.ScreenPointToRay(currentMousePosition);
            //The distance from the camera to the hit point
            float distToHit;

            //Set the movement plane based on the axis being dragged
            movementPlane.SetNormalAndPosition(drag.worldAxis, parentTransform.position);

            //Find the plane hit point
            if (movementPlane.Raycast(ray, out distToHit))
                hitPoint = ray.GetPoint(distToHit);
            //If the plane and ray don't intersect, return a zero vector
            else
                return Vector3.zero;

            //Return the position of hit point relative to the tool handle
            return hitPoint - parentTransform.position;
        }

        //Find the vector orthogonal to the given vector but in the same plane (counter-clockwise)
        private Vector3 GetOrthInPlane(Vector3 originalVector)
        {
            return Vector3.Cross(drag.worldAxis, originalVector);
        }

        //Find the vector from the handle origin to where the handle was clicked
        private Vector2 GetClickTangent()
        {
            //Convert both the handle position to screen space
            Vector2 screenPosHandle = mainCamera.WorldToScreenPoint(parentTransform.position);
            //The 3D vector tangent to the rotation handle at the click point
            Vector3 tangentVector3;

            //Get the vector starting at the handle origin and ending at the camera
            Vector3 cameraVector = (mainCamera.transform.position - parentTransform.position).normalized;

            //If the dragging plane is nearly orthogonal to the camera, calculate the tangent vector using the drag plane normal
            //Temporary fix until parts of the rotation handle further from the camera aren't interactable
            if (Mathf.Abs(Vector3.Dot(cameraVector, drag.worldAxis)) <= 0.05f)
            {
                //Use the vector orthogonal to both the camera vector and movement plane normal as the tangent vector
                tangentVector3 = Vector3.Cross(drag.worldAxis, cameraVector);
            }
            //Otherwise calculate the tangent vector using the click vector
            else
            {
                //Get the 3D vector representing the point on the rotation handle that was clicked
                Vector3 clickVector3 = GetClickVector();
                //Find the vector that is tangent to the rotation handle and in the movement plane
                tangentVector3 = GetOrthInPlane(clickVector3);
            }

            //Calculate the position of the end of the tangent vector in screen space
            Vector2 screenPosTangent = mainCamera.WorldToScreenPoint(parentTransform.position + tangentVector3);
            //Get the tangent vector in screen space by subtracting the screen tangent position by the screen handle position
            return screenPosTangent - screenPosHandle;
        }

        //Calculate the projection of one 2D vector onto another
        private Vector2 ProjectBontoA(Vector2 B, Vector3 A)
        {
            //The scalar to multiply the onto vector by
            float scalar = Vector2.Dot(A, B) / Mathf.Pow(A.magnitude, 2);

            //Scale the onto vector to represent the projection of the target vector
            return scalar * A;
        }

        //Use the displacement of the mouse in screen space to calculate the corresponding displacement
        //in the local space of the tool handle along the drag axis
        private Vector3 GetDragDisplacement(Vector2 mouseDisplacement)
        {
            //Convert the position of the tool handle to screen space
            Vector2 screenHandlePos = mainCamera.WorldToScreenPoint(parentTransform.position);
            //Convert the drag axis to screen space
            Vector2 screenDragAxis = mainCamera.WorldToScreenPoint(parentTransform.position + drag.worldAxis);

            //Get the drag axis vector in screen space by subtracting the drag axis tail point from the head point
            Vector2 screenDragVector = screenDragAxis - screenHandlePos;
            //Get the component of the mouse displacement parallel to the drag axis
            Vector2 screenDisplacement = ProjectBontoA(mouseDisplacement, screenDragVector);
            //Use the dot product between the drag vector and the screen displacement to get the sign of the translation
            float displacementSign = Vector2.Dot(screenDragVector, screenDisplacement) > 0 ? 1f : -1f;

            //Multiply the drag axis by the displacement magnitude to get the vector to translate by and return the result
            return drag.localAxis * screenDisplacement.magnitude * displacementSign;
        }

        //Return the distance between the tool handle and the mouse in screen space
        private float GetMouseHandleDist(Vector2 mouseDisplacement)
        {
            //Convert the position of the tool handle to screen space
            Vector2 screenHandlePos = mainCamera.WorldToScreenPoint(parentTransform.position);

            //Return the magnitude of the difference between the mouse and the handle position
            return (mouseDisplacement - screenHandlePos).magnitude;
        }

        //Sets the appropriate variables according to which axes are being dragged
        private void SetDragData(Axis plane)
        {
            Vector3 a, b;
            drag.offset = Vector3.zero;

            Ray ray = mainCamera.ScreenPointToRay(currentMousePosition);

            if ((plane & Axis.X) == Axis.X)
            {
                draggingAxes++;
                drag.worldAxis = parentTransform.right.normalized;
                drag.localAxis = Vector3.right;
                drag.plane.SetNormalAndPosition(parentTransform.right.normalized, parentTransform.position);
            }

            if ((plane & Axis.Y) == Axis.Y)
            {
                draggingAxes++;

                if (draggingAxes > 1)
                    drag.plane.SetNormalAndPosition(Vector3.Cross(drag.worldAxis, parentTransform.up).normalized, parentTransform.position);
                else
                    drag.plane.SetNormalAndPosition(parentTransform.up.normalized, parentTransform.position);

                drag.worldAxis += parentTransform.up.normalized;
                drag.localAxis += Vector3.up;
            }

            if ((plane & Axis.Z) == Axis.Z)
            {
                draggingAxes++;
                if (draggingAxes > 1)
                    drag.plane.SetNormalAndPosition(Vector3.Cross(drag.worldAxis, parentTransform.forward).normalized, parentTransform.position);
                else
                    drag.plane.SetNormalAndPosition(parentTransform.forward.normalized, parentTransform.position);

                drag.worldAxis += parentTransform.forward.normalized;
                drag.localAxis += Vector3.forward;
            }

            if (draggingAxes < 2)
            {
                if (HandleUtility.PointOnLine(new Ray(parentTransform.position, drag.worldAxis), ray, out a, out b))
                    drag.offset = a - parentTransform.position;
            }
            else
            {
                if (drag.plane.Raycast(ray, out float hit))
                    drag.offset = ray.GetPoint(hit) - parentTransform.position;
            }
        }

        private void OnFinishHandleMovement()
        {
            draggingHandle = false;
            //Reset the offset between the on-screen mouse position and its hypothetical unclamped position
            mouseOffest = Vector2.zero;
            //Save the scale before resetting it
            endScale = scale;
            scale = Vector3.one;

            //Notify listeners that the handle is no longer being dragged
            StartCoroutine(InvokeFinishHandleEvent());
        }

        private IEnumerator InvokeFinishHandleEvent()
        {
            //Wait until the end of the frame so that the OnHandleFinish event doesn't overlap with the OnMouseUp event
            yield return new WaitForEndOfFrame();

            //After dragging the handle, release the cursor
            EditorManager.Instance.ReleaseCursor();
            //Notify all listeners that the handle is no longer being interacted with
            OnHandleFinish?.Invoke();
        }
        #endregion

        #region Getters/Setters
        public TransformData GetTransform()
        {
            return new TransformData(parentTransform.position, parentTransform.rotation, scale);
        }

        public bool GetDragging()
        {
            return draggingHandle;
        }

        public void Hide()
        {
            hidden = true;
            draggingHandle = false;
        }

        public void Show()
        {
            hidden = false;
        }

        //Return the position displacement between the current and last frame
        public Vector3 GetPosDisplacement()
        {
            return parentTransform.position - prevPosition;
        }
        //Return the rotation displacement between the current and last frame
        public float GetRotDisplacement(out Vector3 rotationAxis)
        {
            rotationAxis = drag.worldAxis;
            return rotationDisplacement;
        }
        //Return the scale displacement between the current and last frame
        public Vector3 GetScaleDisplacement()
        {
            Vector3 scaleDisplacement = new Vector3();

            for (int axis = 0; axis < 3; axis++)
                scaleDisplacement[axis] = scale[axis] / prevScale[axis];

            return scaleDisplacement;
        }

        //Return the position of the handle when it was clicked
        public Vector3 GetStartPosition() { return originalPosition; }
        //Return the rotation of the handle when it was clicked
        public Quaternion GetStartRotation() { return originalRotation; }
        //Return the scale of the handle after it was released
        public Vector3 GetEndScale() { return endScale; }
        #endregion

        #region Render Methods
        void OnRenderObject()
        {
            //Don't render the handle if it is hidden or this is not the designated camera
            if (hidden || Camera.current != mainCamera)
                return;

            switch (Tool)
            {
                case Tool.Translate:
                case Tool.Scale:
                    handleOpaqueMaterial.SetPass(0);
                    Graphics.DrawMeshNow(handleLineMesh, handleMatrix);
                    Graphics.DrawMeshNow(handleTriangleMesh, handleMatrix, 1);  // Cones

                    handleTransparentMaterial.SetPass(0);
                    Graphics.DrawMeshNow(handleTriangleMesh, handleMatrix, 0);  // Box
                    break;

                case Tool.Rotate:
                    rotateLineMaterial.SetPass(0);
                    Graphics.DrawMeshNow(handleLineMesh, handleMatrix);
                    break;
            }
        }

        private void RebuildGizmoMatrix()
        {
            float handleScreenSize = HandleUtility.GetHandleSize(parentTransform.position, mainCamera.transform.position, handleSize);
            Matrix4x4 scale = Matrix4x4.Scale(Vector3.one * handleScreenSize);

            handleMatrix = parentTransform.localToWorldMatrix * scale;
        }

        private void RebuildGizmoMesh(Vector3 scale)
        {
            CreateHandleLineMesh(ref handleLineMesh, scale);
            CreateHandleTriangleMesh(ref handleTriangleMesh, scale);
        }
        #endregion

        #region Mesh Generation Methods
        private void CreateHandleLineMesh(ref Mesh mesh, Vector3 scale)
        {
            switch (Tool)
            {
                case Tool.Translate:
                case Tool.Scale:
                    HandleMesh.CreatePositionLineMesh(ref mesh, parentTransform, scale, viewOctant, mainCamera, handleBoxSize);
                    break;
                
                case Tool.Rotate:
                    HandleMesh.CreateRotateMesh(ref mesh, 48, 1f);
                    break;

                default:
                    return;
            }
        }

        private void CreateHandleTriangleMesh(ref Mesh mesh, Vector3 scale)
        {
            if (Tool == Tool.Translate)
                HandleMesh.CreateTriangleMesh(ref mesh, parentTransform, scale, viewOctant, mainCamera, coneMesh, handleBoxSize, capSize);
            else if (Tool == Tool.Scale)
                HandleMesh.CreateTriangleMesh(ref mesh, parentTransform, scale, viewOctant, mainCamera, cubeMesh, handleBoxSize, capSize);
        }
        #endregion
    }
}