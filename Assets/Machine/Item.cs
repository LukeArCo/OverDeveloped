using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    enum E_Step { s_gbProc, s_gbCase, s_dsProc, s_dsCase, s_switchProc, s_switchCase, s_colour };
    enum E_Type { t_gb, t_ds, t_switch };

    E_Step nextStep;

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
            if (collider.GetComponent<PlaceholderPlayer>().GetItem() == null) // The player isn't carrying anything
            {
                collider.GetComponent<PlaceholderPlayer>().CanInteract("Pick Up");

                if(collider.GetComponent<PlaceholderPlayer>().IsInteracting())
                {
                    collider.GetComponent<PlaceholderPlayer>().SetItem(gameObject);
                }
            }
        }
    }
}
