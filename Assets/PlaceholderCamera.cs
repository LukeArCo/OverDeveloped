using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderCamera : MonoBehaviour {

    Transform self;
    Transform target;

    public GameObject followTarget;
    public Vector3 offset;
    public Vector3 angOffset;

	// Use this for initialization
	void Start () {
        self = GetComponent<Transform>();
        target = followTarget.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion ang = Quaternion.Euler(target.eulerAngles);

        self.SetPositionAndRotation(target.position, ang);
        self.Translate(offset);
        self.Rotate(angOffset);
	}
}
