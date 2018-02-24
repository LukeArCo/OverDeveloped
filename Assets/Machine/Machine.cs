using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for machine

public class Machine : MonoBehaviour {
    enum E_Status {e_working, e_broken };
    public enum E_Type { t_gbProc, t_gbCase, t_dsProc, t_dsCase, t_switchProc, t_switchCase, t_colour };

    public Mesh swapMesh;
    public E_Type currType;
    public float processingTime;

    float timer;
    GameObject console;
    E_Status currentStatus;
    bool canInteract = false;

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
        if(console != null)
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Quaternion ang = Quaternion.Euler(gameObject.GetComponent<Transform>().eulerAngles);

            console.GetComponent<Transform>().SetPositionAndRotation(pos, ang);
            console.GetComponent<Transform>().Translate(new Vector3(0, 0.25f, 0));

            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                //Debug.Log("DING! Cookings done...");
                console.GetComponent<Rigidbody>().useGravity = true;

                console.GetComponent<MeshFilter>().mesh = swapMesh; // Resources.Load<Mesh>("cube");
                console.GetComponent<Item>().AdvanceStep();
                console.GetComponent<Item>().SetIsProcessing(false);

                console = null;
            }
        }
    }

    void Broken()
    {
        // Do broken shit
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject collider = other.gameObject;

        if (collider.GetComponent<PlaceholderPlayer>() != null && currentStatus == E_Status.e_working)
        {
            // Debug.Log("Player can interact!");
            canInteract = true;

            if (collider.GetComponent<PlaceholderPlayer>().GetNextStep() == (int)currType)
            {
                collider.GetComponent<PlaceholderPlayer>().CanInteract("Interact");

                if(collider.GetComponent<PlaceholderPlayer>().IsInteracting())
                {
                    console = collider.GetComponent<PlaceholderPlayer>().GetItem();
                    collider.GetComponent<PlaceholderPlayer>().SetItem(); // Set players item to null

                    console.GetComponent<Item>().SetIsProcessing(true);
                    console.GetComponent<Rigidbody>().useGravity = false;

                    timer = processingTime;
                }
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


