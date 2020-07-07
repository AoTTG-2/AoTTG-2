using UnityEngine;
using System.Collections;

public class SwooshTest : MonoBehaviour
{


	[SerializeField]
	int _start = 0;
	
	[SerializeField]
	int _end = 0;
	
	float _startN = 0.0f;
	float _endN = 0.0f;
	
	float _time = 0.0f;
	float _prevTime = 0.0f;
	float _prevAnimTime = 0.0f;
		
	[SerializeField]
	MeleeWeaponTrail _trail;
	
	bool _firstFrame = true;
	
	void Start()
	{
        _trail = gameObject.GetComponentInChildren<MeleeWeaponTrail>();
		_trail.Emit = false;
	}
	
	void Update()
	{
		if (_time > 1.0f || _firstFrame)
		{
			if (!_firstFrame)
			{
				_time -= 1.0f;
			}
			_firstFrame = false;
		}
		
		if (_prevTime < _startN && _time >= _startN)
		{
			_trail.Emit = true;
		}
		else if (_prevTime < _endN && _time >= _endN)
		{
			_trail.Emit = false;
		}
		
		_prevTime = _time;
	}
}
