using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxElementController : MonoBehaviour
{
		[SerializeField] private float parallaxEffectMultiplier;
	
    	private Transform _cameraTransform;
    	private Vector3 _lastCameraPosition;
    	private float _textureUnitSizeX;
	    
    
    	private void Start()
    	{
    		_cameraTransform = Camera.main.transform;
    		_lastCameraPosition = _cameraTransform.position;
    		Sprite sprite = GetComponent<SpriteRenderer>().sprite;
    		Texture2D texture = sprite.texture;
    		_textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    	}
    
    	private void LateUpdate()
    	{
    		Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
    		transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y);
    		_lastCameraPosition = _cameraTransform.position;
    
    		if (_cameraTransform.position.x - transform.position.x >= _textureUnitSizeX)
    		{
    			float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
    			transform.position = new Vector3(_cameraTransform.position.x - offsetPositionX , transform.position.y);
    		}
    	}
}
