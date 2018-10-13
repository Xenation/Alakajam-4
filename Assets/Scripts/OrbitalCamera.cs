using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour {

    public Camera cam;
    public float speed = 10f;
    public float rotSpeed = 3f;
    public Transform target;

    private Vector2 input;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButton(1))
        {
            Vector3 pointRotation = new Vector3(0, transform.position.y, 0);
            float xRange = transform.position.x - pointRotation.x;
            float yRange = transform.position.z - pointRotation.z;
            float newX = xRange * Mathf.Cos(Input.GetAxis("Mouse X") * (rotSpeed*Time.deltaTime)) - yRange * Mathf.Sin(Input.GetAxis("Mouse X") * (rotSpeed * Time.deltaTime)) + pointRotation.x;
            float newY = xRange * Mathf.Sin(Input.GetAxis("Mouse X") * (rotSpeed * Time.deltaTime)) + yRange * Mathf.Cos(Input.GetAxis("Mouse X") * (rotSpeed * Time.deltaTime)) + pointRotation.z;

            transform.position = new Vector3(newX,transform.position.y,newY);
            transform.LookAt(pointRotation);
            
        }
    
        transform.position = new Vector3(transform.position.x, transform.position.y + Input.GetAxis("Mouse ScrollWheel") * speed * Time.deltaTime, transform.position.z);

        Vector3 pivot = new Vector3(0, transform.position.y, 0);
        float dist = Vector3.Distance(transform.position, pivot);

        //transform.position = new Vector3(transform.position.x+ Input.GetAxis("Mouse ScrollWheel") * speed, transform.position.y, transform.position.z + Input.GetAxis("Mouse ScrollWheel") * speed);

    }
}
