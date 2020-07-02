using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour
{
	[SerializeField]
	float _sensitivity = 0.5f;

	Vector3 _originalPos;

	// Use this for initialization
	void Start () {
		_originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.x /= Screen.width;
		mousePos.y /= Screen.height;

		mousePos.x = mousePos.x - 0.5f;
		mousePos.y = mousePos.y - 0.5f;

		mousePos *= 2 * _sensitivity;

		//mousePos.z = 8.0f;
		//Debug.Log(mousePos);
		transform.position = _originalPos + mousePos;
		//transform.position = Camera.main.ViewportToWorldPoint(mousePos);
		//Debug.Log(mousePos);
		//transform.Translate(Input.GetAxis("Mouse X") * _speed, Input.GetAxis("Mouse Y") * _speed, 0);
	}
}
