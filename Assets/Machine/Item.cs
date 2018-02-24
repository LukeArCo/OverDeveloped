using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum E_Step { s_gbProc, s_gbCase, s_dsProc, s_dsCase, s_switchProc, s_switchCase, s_colour, s_complete };
    enum E_Type { t_gb, t_ds, t_switch };

    public E_Step m_nextStep;

    bool m_isProcessing = false;

    void Awake()
    {
        //nextStep = E_Step.s_gbProc;
    }

    // Use this for initialization
    void Start()
    {
        // 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetStep()
    {
        return (int)m_nextStep;
    }

    public void AdvanceStep()
    {
        if((int)m_nextStep % 2 == 0 && (int)m_nextStep < 6)
        {
            m_nextStep++;
        }
        else if ((int)m_nextStep < 6) { m_nextStep = E_Step.s_colour; }
        else { m_nextStep = E_Step.s_complete; }
    }

    public void SetType(int _type)
    {
        switch (_type)
        {
            case (int)E_Type.t_gb:
                m_nextStep = E_Step.s_gbProc;
                break;
            case (int)E_Type.t_ds:
                m_nextStep = E_Step.s_dsProc;
                break;
            case (int)E_Type.t_switch:
                m_nextStep = E_Step.s_switchProc;
                break;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        GameObject collider = other.gameObject;

        if (collider.GetComponent<PlayerController>() != null)
        {

            if (collider.GetComponent<PlayerController>().GetItem() == null && !m_isProcessing) // The player isn't carrying anything
            {
                collider.GetComponent<PlayerController>().CanInteract("Pick Up"); // Pickup indicator

                if(collider.GetComponent<PlayerController>().IsInteracting()) // Player picks it up
                {
                    gameObject.GetComponent<Rigidbody>().useGravity = false; // Because fuck gravity, it messes carrying up
                    collider.GetComponent<PlayerController>().SetItem(gameObject);

                    m_isProcessing = true;
                }
            }
        }
    }

    public bool IsProcessing() { return m_isProcessing; }
    public void SetIsProcessing(bool _isProcessing) { m_isProcessing = _isProcessing; }

    
}
