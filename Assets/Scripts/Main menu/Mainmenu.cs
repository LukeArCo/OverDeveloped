using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour {
    public PlayerIndex m_pIndex = PlayerIndex.One;
    public GamePadState m_curState;
    public GamePadState m_prevState;
    public Button[] m_menuButtons;

    int m_menuSelection = 0;
    int m_maxMenuSelection = 0;

    // Use this for initialization
    void Start () {
        m_maxMenuSelection = m_menuButtons.Length - 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        m_prevState = m_curState;
        m_curState = GamePad.GetState(m_pIndex);

        MenuSelection();
    }

    void MenuSelection()
    {
        //input
        if (m_curState.ThumbSticks.Left.Y < -0.1 && m_prevState.ThumbSticks.Left.Y == 0)
        {
            m_menuSelection++;
        }
        else if (m_curState.ThumbSticks.Left.Y > 0.1 && m_prevState.ThumbSticks.Left.Y == 0)
        {
            m_menuSelection--;
        }
        //looping
        if (m_menuSelection < 0)
        {
            m_menuSelection = m_maxMenuSelection;
        }
        if (m_menuSelection > m_maxMenuSelection)
        {
            m_menuSelection = 0;
        }

        m_menuButtons[m_menuSelection].Select();

        if (m_curState.Buttons.A == ButtonState.Pressed)
        {
            m_menuButtons[m_menuSelection].onClick.Invoke();
        }
    }

    public void SwitchToLevel1()
    {
        SceneManager.LoadScene(1);
    }

     public void SwitchToLevel2()
    {
        SceneManager.LoadScene(2);
    }

    public void SwitchToLevel3()
    {
        SceneManager.LoadScene(3);
    }
}
