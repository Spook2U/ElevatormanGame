using UnityEngine;
using System.Collections;

public class DeviceCamera : MonoBehaviour {

	WebCamTexture cam;

	void Awake() 
	{
		cam = new WebCamTexture();
		GetComponent<Renderer>().material.mainTexture = cam;
		cam.Play();
	}
	
}
