using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;


public class PlaceholderPlayer : MonoBehaviour {
    bool indexSet = false;
    PlayerIndex pIndex;

    GameObject ui;
    GameObject console;

    public int index;

    int timer = 5;
    bool isInteracting = false;
    

    // Use this for initialization
    void Start()
    {

        if (!indexSet)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    pIndex = testPlayerIndex;
                    indexSet = true;
                }
            }
        }

        ui = GameObject.Find("UI" + (int)pIndex);

    }

    // Update is called once per frame
    void Update () {
		
	}

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        movement.z = GamePad.GetState(pIndex).ThumbSticks.Left.Y;
        movement.x = GamePad.GetState(pIndex).ThumbSticks.Left.X;

        gameObject.GetComponent<Transform>().Translate(movement * 0.1f);
        gameObject.GetComponent<Transform>().Rotate(new Vector3(0, GamePad.GetState(pIndex).ThumbSticks.Right.X * 4.0f, 0));

        if (Input.GetButtonDown("Fire1"))
        {

            // GamePad.SetVibration(pIndex, 0.1f, 0.1f);
        
        }

        if(timer > 0)
        {
            timer--;
            ui.SetActive(!(timer == 0));
        }

        

        isInteracting = GamePad.GetState(pIndex).Buttons.A == 0;

        // MOVE ITEM
        if(console != null)
        {
            float distance = (gameObject.GetComponent<Transform>().localScale.x * 0.5f)+ console.GetComponent<Transform>().localScale.x;

            console.GetComponent<Transform>().SetPositionAndRotation(gameObject.GetComponent<Transform>().position, Quaternion.Euler(gameObject.GetComponent<Transform>().eulerAngles));
            console.GetComponent<Transform>().Translate(new Vector3(0, 0, distance));

            if(GamePad.GetState(pIndex).Buttons.X == 0)
            {
                console.GetComponent<Rigidbody>().useGravity = true;
                console = null;
            }
        }
    }

    public void CanInteract(string _message)
    {
        timer = 3;
        ui.SetActive(true);
        ui.GetComponent<Text>().text = "A - " + _message;
    }

    public bool IsInteracting() { return isInteracting; }

    public int GetNextStep()
    {
        if(console != null)
        {
            return console.GetComponent<Item>().GetStep();
        }

        return -1;
    }

    public void SetItem(GameObject _console)
    {
        _console.GetComponent<Rigidbody>().useGravity = false;
        console = _console;
    }

    public void SetItem()
    {
        console = null;
    }

    public GameObject GetItem() { return console; }

}
