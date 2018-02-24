using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for machine

public class Machine : MonoBehaviour {
    enum E_Status {e_working, e_broken };
    public enum E_Type { t_gbProc, t_gbCase, t_dsProc, t_dsCase, t_switchProc, t_switchCase, t_colour };

    public Mesh m_swapMesh;
    public Material m_paintMat;
    public E_Type m_currType;
    public float m_processingTime;
    public Vector3 m_partOffset;

    float m_timer;
    GameObject m_console;
    E_Status m_currentStatus;
    bool m_canInteract = true;


    // Use this for initialization
    void Start () {
        m_currentStatus = E_Status.e_working;
	}
	
	// Update is called once per frame
	void Update () {
		switch(m_currentStatus)
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
        if(m_console != null)
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            Quaternion ang = Quaternion.Euler(gameObject.GetComponent<Transform>().eulerAngles);

            m_console.GetComponent<Transform>().SetPositionAndRotation(pos, ang);
            m_console.GetComponent<Transform>().Translate(m_partOffset);
            m_console.GetComponent<Transform>().Rotate(new Vector3(0, m_timer * 500.0f, 0));

            m_timer -= Time.deltaTime;

            if(m_timer <= 0)
            {
                m_console.GetComponent<Rigidbody>().useGravity = true;

                if(m_currType != E_Type.t_colour) { m_console.GetComponent<MeshFilter>().mesh = m_swapMesh; } // Don't swap mesh for colouring

                BoxCollider[] boxes = m_console.GetComponents<BoxCollider>();

                if (m_console.GetComponent<Item>().GetStep() % 2 == 0 && m_console.GetComponent<Item>().GetStep() < 6)
                {
                    boxes[0].size = new Vector3(0.02f, 0.02f, 0.02f);
                    boxes[1].size = new Vector3(0.12f, 0.12f, 0.12f);
                }

                if(m_console.GetComponent<Item>().GetStep() == (int)E_Type.t_colour || m_console.GetComponent<Item>().GetStep() == 7)
                {
                    m_console.GetComponent<Renderer>().material = m_paintMat;
                    m_console.GetComponent<Item>().SetPaintTime(1.0f);
                    m_console.GetComponent<Item>().SetIsProcessing(false);
                }
                

                m_console.GetComponent<Transform>().localScale = new Vector3(14, 14, 14);

                m_console.GetComponent<Item>().AdvanceStep();
                m_console.GetComponent<Item>().SetIsProcessing(false);
                Debug.Log("Processing is done...");

                m_canInteract = true;

                m_console = null;
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

        // Worker interactions
        if (collider.GetComponent<PlayerController>() != null && m_currentStatus == E_Status.e_working && collider.GetComponent<PlayerController>().GetType() == 0 && m_canInteract)
        {
            // Debug.Log("Player can interact!");
            m_canInteract = true;

            if (collider.GetComponent<PlayerController>().GetNextStep() == (int)m_currType)
            {

                collider.GetComponent<PlayerController>().CanInteract("Interact");

                if(collider.GetComponent<PlayerController>().IsInteracting())
                {
                    m_console = collider.GetComponent<PlayerController>().GetItem();
                    collider.GetComponent<PlayerController>().SetItem(); // Set players item to null

                    m_console.GetComponent<Item>().SetIsProcessing(true);
                    m_console.GetComponent<Rigidbody>().useGravity = false;

                    m_timer = m_processingTime;

                    m_canInteract = false;
                }
            }
            else if(collider.GetComponent<PlayerController>().GetItem().GetComponent<Item>().GetPaintTime() < 0 &&   // Allow the player to reuse painting machines
                collider.GetComponent<PlayerController>().GetNextStep() == 7 && m_currType == E_Type.t_colour) // WHAT THE FUCK HAVE I DONE?
            {
                collider.GetComponent<PlayerController>().CanInteract("Interact");

                if (collider.GetComponent<PlayerController>().IsInteracting())
                {
                    m_console = collider.GetComponent<PlayerController>().GetItem();
                    collider.GetComponent<PlayerController>().SetItem(); // Set players item to null

                    m_console.GetComponent<Item>().SetIsProcessing(true);
                    m_console.GetComponent<Rigidbody>().useGravity = false;

                    m_timer = m_processingTime;

                    m_canInteract = false;
                }
            }
        }
        else if (collider.GetComponent<PlayerController>() != null && m_currentStatus == E_Status.e_working && collider.GetComponent<PlayerController>().GetType() == 1) // Traitor Interactions
        {
            collider.GetComponent<PlayerController>().CanInteract("Sabotage");
        }
    }

    void Interact(GameObject _player, GameObject _item)
    {

    }


    
}


