using Alakajam4;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitalCamera : MonoBehaviour {

	public float vertSensivity = 10f;
	public float rotSensivity = 3f;
	[System.NonSerialized]
	public bool locked = false;

	private Camera cam;
	private Vector3 pointRotation;

	void Start() {
		cam = GetComponent<Camera>();
		pointRotation = new Vector3(Tower.I.transform.position.x + (Tower.I.towerSize.x * Tower.I.blockSize) / 2f - Tower.I.blockSize / 2f, transform.position.y - 5f, Tower.I.transform.position.z + (Tower.I.towerSize.z * Tower.I.blockSize) / 2f - Tower.I.blockSize / 2f);
		transform.LookAt(pointRotation);
	}

	void Update() {
		if (locked) return;

		if (Input.GetMouseButton(1)) {
			float xRange = transform.position.x - pointRotation.x;
			float yRange = transform.position.z - pointRotation.z;
			float newX = xRange * Mathf.Cos(-Input.GetAxis("Mouse X") * (rotSensivity * Time.deltaTime)) - yRange * Mathf.Sin(-Input.GetAxis("Mouse X") * (rotSensivity * Time.deltaTime)) + pointRotation.x;
			float newY = xRange * Mathf.Sin(-Input.GetAxis("Mouse X") * (rotSensivity * Time.deltaTime)) + yRange * Mathf.Cos(-Input.GetAxis("Mouse X") * (rotSensivity * Time.deltaTime)) + pointRotation.z;

			transform.position = new Vector3(newX, transform.position.y, newY);
			transform.LookAt(pointRotation);
		}

		float heightIncrease = Input.GetAxis("Mouse ScrollWheel") * vertSensivity * Time.deltaTime;
		pointRotation.y += heightIncrease;
		transform.position += Vector3.up * heightIncrease;

	}
}
