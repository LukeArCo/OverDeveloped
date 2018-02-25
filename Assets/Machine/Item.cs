using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Step { s_raw, s_processor, s_case, s_colour, s_complete };
public enum E_Type { t_all, t_raw, t_gb, t_ds, t_switch };

public class Item : MonoBehaviour
{

    public E_Step nextStep;
    public E_Type m_type;

    bool isProcessing = false;

    void Awake()
    {
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

    public E_Step GetStep()
    {
        return nextStep;
    }

    public void AdvanceStep()
    {
        switch(nextStep)
        {
            case E_Step.s_raw:
                nextStep = E_Step.s_processor;
                break;
            case E_Step.s_processor:
                nextStep = E_Step.s_case;
                break;
            case E_Step.s_case:
                nextStep = E_Step.s_colour;
                break;
            case E_Step.s_colour:
                nextStep = E_Step.s_complete;
                break;
        }
    }

    public E_Type GetType()
    {
        return m_type;
    }

    public void SetType(E_Type _type)
    {
        m_type = _type;
    }

    private void OnTriggerStay(Collider other)
    {

    }

    public bool IsProcessing() { return isProcessing; }
    public void SetIsProcessing(bool _isProcessing) { isProcessing = _isProcessing; }
}
