using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    enum E_Step { s_gbProc, s_gbCase, s_dsProc, s_dsCase, s_switchProc, s_switchCase, s_colour, s_complete };
    enum E_Type { t_gb, t_ds, t_switch };

    E_Step nextStep;

    bool isProcessing = false;

    void Awake()
    {
        nextStep = E_Step.s_gbProc;
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
        return (int)nextStep;
    }

    public void AdvanceStep()
    {
        if((int)nextStep % 2 == 0 && (int)nextStep < 6)
        {
            nextStep++;
        }
        else if ((int)nextStep < 6) { nextStep = E_Step.s_colour; }
        else { nextStep = E_Step.s_complete; }
    }

    public void SetType(int _type)
    {
        switch (_type)
        {
            case (int)E_Type.t_gb:
                nextStep = E_Step.s_gbProc;
                break;
            case (int)E_Type.t_ds:
                nextStep = E_Step.s_dsProc;
                break;
            case (int)E_Type.t_switch:
                nextStep = E_Step.s_switchProc;
                break;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        GameObject collider = other.gameObject;

        if (collider.GetComponent<PlaceholderPlayer>() != null)
        {
            if (collider.GetComponent<PlaceholderPlayer>().GetItem() == null && !isProcessing) // The player isn't carrying anything
            {
                collider.GetComponent<PlaceholderPlayer>().CanInteract("Pick Up"); // Pickup indicator

                if(collider.GetComponent<PlaceholderPlayer>().IsInteracting()) // Player picks it up
                {
                    collider.GetComponent<PlaceholderPlayer>().SetItem(gameObject);
                }
            }
        }
    }

    public bool IsProcessing() { return isProcessing; }
    public void SetIsProcessing(bool _isProcessing) { isProcessing = _isProcessing; }
}
