using UnityEngine;

namespace MapEditor
{
    public class Billboard : MonoBehaviour
    {
        //The main camera in the scene to face
        private Camera mainCamera;
        //The game object the billboard is positioned above
        [SerializeField] private GameObject targetObject;
        //The child 3D text object of this object
        private TextMesh text3D;
        //How high above the object the 3D text will sit
        [SerializeField] private float verticalOffset = 5f;
        //The ratio between the billboard scale and the target object scale
        [SerializeField] private float scaleFactor = 1f;
        //Scale the billboard with the scale target unless it outside of these bounds
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 10f;

        void Awake()
        {
            //Get the TextMesh component from the billboard object
            text3D = gameObject.GetComponent<TextMesh>();
            //Get the main camera in the scene
            mainCamera = Camera.main;
        }

        //Update the billboard to follow the parent object and face the camera
        private void LateUpdate()
        {
            if (targetObject)
            {
                //Get the local scale and position of the target object
                Vector3 targetScale = targetObject.transform.localScale;
                Vector3 targetPos = targetObject.transform.localPosition;

                //Calculate how high above the target the billboard should be and transform it
                float height = Mathf.Abs(targetScale.y / 2f)  + (transform.localScale.y * 5f) + verticalOffset;
                text3D.transform.localPosition = new Vector3(targetPos.x, targetPos.y + height, targetPos.z);

                //Calculate the scaled length and width of the target object
                float scaleWidth = Mathf.Abs(targetScale.x / scaleFactor);
                float scaleLength = Mathf.Abs(targetScale.z / scaleFactor);
                //Set the max scale to the minimum allowed scale for the comparison
                float newScale = minScale;

                //Find the maximum between the max scale, scaled width, and scaled length of the target object
                if (scaleWidth > newScale)
                    newScale = scaleWidth;
                if (scaleLength > newScale)
                    newScale = scaleLength;

                //If the new scale is larger than the maximum scale, clamp it
                if (newScale > maxScale)
                    newScale = maxScale;

                //Scale the billboard to match the scale target
                text3D.transform.localScale = new Vector3(-newScale, newScale, newScale);
            }

            //Rotate the billboard to face the camera
            transform.LookAt(mainCamera.transform.position);
        }
    }
}