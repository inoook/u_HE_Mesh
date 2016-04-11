using UnityEngine;
using System.Collections;

public class RotateMousePos : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	
	float bufferMouseX = 0;
	float bufferMouseY = 0;
	// Update is called once per frame
	void Update () {
		Vector3 mouseInput = Input.mousePosition;
		
		float mouseX = mouseInput.x;
		float mouseY = Screen.height - mouseInput.y;
		
		float width = Screen.width;
		float height = Screen.height;
		
		bufferMouseX = 0.95f*bufferMouseX+0.05f*mouseX;
		bufferMouseY = 0.95f*bufferMouseY+0.05f*mouseY;
		
		float rX = (bufferMouseX/(float)width*Mathf.PI*2);
		float rY = (bufferMouseY/(float)height*Mathf.PI*2);
		float rZ = 0;
		
		this.transform.localEulerAngles = new Vector3(rY, -rX, rZ) * Mathf.Rad2Deg;
	}
}
