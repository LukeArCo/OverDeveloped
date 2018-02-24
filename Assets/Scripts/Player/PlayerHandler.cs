using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlayerHandler : MonoBehaviour
{

    [Header("Settings")]
    public GamePadState m_player1;
    [Space(10)]
    public GameObject m_player;
    public int m_playerCount;
    [Space(10)]
    public bool[] m_activePlayers = new bool[4];
    public Image[] m_activeLights = new Image[4];
    public PlayerController[] m_players = new PlayerController[4];
    [Space(10)]
    public Camera m_menuCamera;

    bool m_gameRunning = false;

	// Use this for initialization
	void Start () {
        m_player1 = GamePad.GetState(PlayerIndex.One);
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_gameRunning)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex index = (PlayerIndex)i;
                GamePadState state = GamePad.GetState(index);
                if (state.IsConnected)
                {
                    if (state.Buttons.A == ButtonState.Pressed && !m_activePlayers[i])
                    {
                        m_activePlayers[i] = true;
                        StartCoroutine(Vibrate(index, 0.5f, 0.5f, 0.15f));
                    }
                    else if (state.Buttons.B == ButtonState.Pressed)
                    {
                        m_activePlayers[i] = false;
                    }
                }
                else
                {
                    m_activePlayers[i] = false;
                }

                if (m_activePlayers[i])
                {
                    m_activeLights[i].color = new Color(150.0f / 255, 255.0f / 255, 150.0f / 255, 255.0f / 255);
                }
                else
                {
                    m_activeLights[i].color = new Color(255.0f / 255, 150.0f / 255, 150.0f / 255, 255.0f / 255);
                }
            }

            m_player1 = GamePad.GetState(PlayerIndex.One);
            if (m_player1.Buttons.Start == ButtonState.Pressed && !m_gameRunning)
            {
                m_gameRunning = true;
                m_menuCamera.enabled = false;
                StartGame();
            }
        }
    }

    void StartGame()
    {
        int traitor = Random.Range(0, m_playerCount + 1);

        for(int i = 0; i < m_playerCount; ++i)
        {
            if(!m_activePlayers[i])
            {
                continue;
            }

            m_players[i] = Instantiate(m_player, transform.position, Quaternion.identity).GetComponent<PlayerController>();
            m_players[i].m_player = i + 1;
            m_players[i].m_pIndex = (PlayerIndex)i;
            m_players[i].m_curState = GamePad.GetState((PlayerIndex)i);
            m_players[i].m_playerType = (i == traitor) ? PlayerController.PlayerType.Traitor : PlayerController.PlayerType.Worker;

            switch (i)
            {
                case 0:
                    m_players[i].m_camera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    break;
                case 1:
                    m_players[i].m_camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case 2:
                    m_players[i].m_camera.rect = new Rect(0f, 0, 0.5f, 0.5f);
                    break;
                case 3:
                    m_players[i].m_camera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
            }
        }
    }

    IEnumerator Vibrate(PlayerIndex _index, float _leftStrength, float _rightStrength, float _length)
    {
        GamePad.SetVibration(_index, _leftStrength, _rightStrength);
        yield return new WaitForSeconds(_length);
        GamePad.SetVibration(_index, 0, 0);
    }
}
