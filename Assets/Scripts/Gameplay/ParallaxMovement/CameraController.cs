using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform _playerTransform;
	public void LateUpdate()
	{
		var position = transform.position;
		transform.position = new Vector3(_playerTransform.position.x + 1.5f, position.y, position.z);
	}
}