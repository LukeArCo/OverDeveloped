using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {
    E_Status currentStatus;
    enum E_Status {e_working, e_broken };
	// Use this for initialization
	void Start () {
        currentStatus = E_Status.e_working;
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentStatus)
        {
            case E_Status.e_working:

                break;
            case E_Status.e_broken:

                break;
        }
	}

    void Working()
    {

    }

    void Broken()
    {

    }

}


