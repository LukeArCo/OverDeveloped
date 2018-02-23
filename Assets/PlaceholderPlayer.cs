using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public class PlaceholderPlayer : MonoBehaviour {
    bool indexSet = false;
    PlayerIndex pIndex;

    public int index;

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
    }

    // Update is called once per frame
    void Update () {
		
	}

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        movement.z = GamePad.GetState(pIndex).ThumbSticks.Left.Y;
        movement.x = GamePad.GetState(pIndex).ThumbSticks.Left.X;
        Debug.Log(GamePad.GetState(pIndex).ThumbSticks.Left.Y);

        gameObject.GetComponent<Transform>().Translate(movement * 0.1f);
        gameObject.GetComponent<Transform>().Rotate(new Vector3(0, GamePad.GetState(pIndex).ThumbSticks.Right.X * 4.0f, 0));

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Test");
            // GamePad.SetVibration(pIndex, 0.1f, 0.1f);
        
        }
    }
}
