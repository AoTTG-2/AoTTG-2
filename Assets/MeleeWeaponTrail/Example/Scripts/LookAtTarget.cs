using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour
{
	[SerializeField]
	Transform _target = null;

	[SerializeField]
	float _speed = 0.5f;

	Vector3 _lookAtTarget;

	void Update()
	{
		_lookAtTarget = Vector3.Lerp(_lookAtTarget, _target.position, Time.deltaTime * _speed);
		transform.LookAt(_lookAtTarget);
	}
}
