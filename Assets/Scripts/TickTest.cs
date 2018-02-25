using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickTest : MonoBehaviour {

	public float bpm;
	public float offset;
	float delay;
	float elapsed;

	// Use this for initialization
	void Start () {
		delay = 60 / bpm;
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;

		if (elapsed > delay) {
			elapsed -= delay;
			GetComponent<AudioSource> ().Play ();
		}
	}
}
