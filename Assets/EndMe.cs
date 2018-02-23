using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; 

public class EndMe : MonoBehaviour {

    bool indexSet = false;
    PlayerIndex pIndex;

	// Use this for initialization
	void Start () {
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
	void FixedUpdate () {
	    if(Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Test");
            GamePad.SetVibration(pIndex, 0.1f, 0.1f);
        }
	}
}
