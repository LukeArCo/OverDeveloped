using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for machine

public class Machine : MonoBehaviour {
    enum E_Status {e_working, e_broken };
    public enum E_Type { t_gbProc, t_gbCase, t_dsProc, t_dsCase, t_switchProc, t_switchCase, t_colour };

    E_Status currentStatus;
    public E_Type currType;
    
    private bool canInteract = false;



	// Use this for initialization
	void Start () {
        currentStatus = E_Status.e_working;
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentStatus)
        {
            case E_Status.e_working:
                Working();
                break;
            case E_Status.e_broken:
                Broken();
                break;
        }
        
	}

    void Working()
    {
        // Do working stuff
    }

    void Broken()
    {
        // Do broken shit
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject collider = other.gameObject;

        if (collider.GetComponent<PlaceholderPlayer>() != null)
        {
            // Debug.Log("Player can interact!");
            canInteract = true;

            if (collider.GetComponent<PlaceholderPlayer>().GetNextStep() == (int)currType)
            {
                collider.GetComponent<PlaceholderPlayer>().CanInteract("Interact");
            }
        }
        else
        {
            canInteract = false;
        }
    }

    void Interact(GameObject _player, GameObject _item)
    {

    }


    
}


