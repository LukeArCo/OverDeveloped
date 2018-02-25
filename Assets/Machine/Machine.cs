using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

// Base class for machine

public class Machine : MonoBehaviour {
    enum E_Status {e_working, e_broken };

    public E_Type m_currType;
    public E_Step m_step;
    public float m_processingTime;

    float m_timer;
    GameObject m_console;
    E_Status m_currentStatus;
    bool m_canInteract = false;
    bool m_processing = false;
    PlayerController m_controller;
    bool m_skipFrame;

    [Header("Objects")]
    public GameObject m_product;
    public Transform m_displayPos;

    [Header("Interface")]
    public Image m_progressBar;
    public Image m_progressBarBack;

    [Header("Colour Machine")]
    public Color m_processingColour;
    public Color m_green;
    public Color m_red;
    public Color m_blue;
    public Color m_yellow;
    public GameObject m_colourSelect;

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
        if (m_colourSelect != null && m_colourSelect.activeSelf && !m_skipFrame)
        {
            if (m_controller.m_curState.Buttons.X == ButtonState.Pressed && m_controller.m_prevState.Buttons.X == ButtonState.Released)
            {
                m_processingColour = m_blue;
                m_controller.m_inMenu = false;
                m_colourSelect.SetActive(false);
                m_console.GetComponent<Item>().SetIsProcessing(true);
                m_processing = true;
                m_progressBar.enabled = true;
                m_progressBarBack.enabled = true;
            }
            else if (m_controller.m_curState.Buttons.Y == ButtonState.Pressed && m_controller.m_prevState.Buttons.Y == ButtonState.Released)
            {
                m_processingColour = m_yellow;
                m_controller.m_inMenu = false;
                m_colourSelect.SetActive(false);
                m_console.GetComponent<Item>().SetIsProcessing(true);
                m_processing = true;
                m_progressBar.enabled = true;
                m_progressBarBack.enabled = true;
            }
            else if (m_controller.m_curState.Buttons.A == ButtonState.Pressed && m_controller.m_prevState.Buttons.A == ButtonState.Released)
            {
                m_processingColour = m_green;
                m_controller.m_inMenu = false;
                m_colourSelect.SetActive(false);
                m_console.GetComponent<Item>().SetIsProcessing(true);
                m_processing = true;
                m_progressBar.enabled = true;
                m_progressBarBack.enabled = true;
            }
            else if (m_controller.m_curState.Buttons.B == ButtonState.Pressed && m_controller.m_prevState.Buttons.B == ButtonState.Released)
            {
                m_processingColour = m_red;
                m_controller.m_inMenu = false;
                m_colourSelect.SetActive(false);
                m_console.GetComponent<Item>().SetIsProcessing(true);
                m_processing = true;
                m_progressBar.enabled = true;
                m_progressBarBack.enabled = true;
            }
        }

        // Do working stuff
        if (m_console != null && m_processing)
        {
            m_console.GetComponent<Transform>().Rotate(new Vector3(0, Time.deltaTime * 50, 0));

            m_timer -= Time.deltaTime;

            m_progressBar.fillAmount = 1 - (m_timer / m_processingTime);

            if(m_timer <= 0)
            {

                BoxCollider[] boxes = m_console.GetComponents<BoxCollider>();

                if (m_step == E_Step.s_processor)
                {
                    Destroy(m_console);
                    m_console = Instantiate(m_product);
                    m_console.transform.position = m_displayPos.position;
                }

                if (m_step == E_Step.s_case)
                {
                    Destroy(m_console);
                    m_console = Instantiate(m_product);
                    m_console.transform.position = m_displayPos.position;
                }

                if (m_step == E_Step.s_colour)
                {
                    m_console.GetComponent<MeshRenderer>().material.color = m_processingColour;
                }

                //m_console.GetComponent<Item>().AdvanceStep();
                m_console.GetComponent<Item>().SetIsProcessing(false);
            }
        }

        if (m_skipFrame)
            m_skipFrame = false;
    }

    void Broken()
    {
        // Do broken shit
    }

    private void OnTriggerStay(Collider other)
    {

    }

    public bool AddItem(GameObject _object, PlayerController _player)
    {
        if (m_console != null)
        {
            return false;
        }

        if(_object.GetComponent<Item>().GetType() != m_currType && m_currType != E_Type.t_all)
        {
            return false;
        }

        switch (m_step)
        {
            case E_Step.s_processor:
                if (_object.GetComponent<Item>().GetStep() != E_Step.s_raw) return false;
                break;
            case E_Step.s_case:
                if (_object.GetComponent<Item>().GetStep() != E_Step.s_processor) return false;
                break;
            case E_Step.s_colour:
                if (_object.GetComponent<Item>().GetStep() != E_Step.s_case) return false;
                break;
        }

        m_console = _object;

        m_console.transform.position = m_displayPos.position;
        m_console.transform.SetParent(null);
        m_timer = m_processingTime;
        m_processing = false;

        if (m_step == E_Step.s_colour)
        {
            m_skipFrame = true;
            m_controller = _player;
            m_controller.m_inMenu = true;
            m_colourSelect.SetActive(true);
        }
        else
        {
            m_console.GetComponent<Item>().SetIsProcessing(true);
            m_processing = true;
            m_progressBar.enabled = true;
            m_progressBarBack.enabled = true;

        }

        return true;
    }

    public GameObject TakeObject()
    {
        if(m_console && m_timer <= 0)
        {
            m_progressBar.enabled = false;
            m_progressBarBack.enabled = false;

            GameObject temp = m_console;
            m_console = null;
            return temp;
        }
        else
        {
            return null;
        }
    }

    void Interact(GameObject _player, GameObject _item)
    {

    }


    
}


