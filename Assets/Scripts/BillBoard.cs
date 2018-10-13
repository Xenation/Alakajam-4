using UnityEngine;

public class BillBoard : MonoBehaviour {

	private Camera cam;

	private void Awake() {
		cam = FindObjectOfType<Camera>();
	}
	
	private void Update() {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}
