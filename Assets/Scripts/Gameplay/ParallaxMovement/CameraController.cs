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
		transform.position = new Vector3(_playerTransform.position.x + 4.5f, position.y, position.z);
	}
}

public static class CameraExtensions
{
	public static Bounds OrthographicBounds(this Camera camera)
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 100));
		return bounds;
	}
}