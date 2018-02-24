using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerType { Worker, Traitor };

    [Header("Settings")]
    [Range(1, 4)]
    public int m_player = 1;
    public PlayerIndex m_pIndex = PlayerIndex.One;
    public GamePadState m_curState;
    public GamePadState m_prevState;
    public PlayerType m_playerType = PlayerType.Worker;
    [Space(10)]
    public float m_speed;
    public float m_dashSpeed;
    public float m_dashLength;
    float m_dashEnd;
    public float m_dashDelay;
    float m_nextDash;

    Vector3 m_lastDirection;
    bool m_dashing;
    bool m_inMenu;
    int m_menuSelection;
    int m_maxMenuSelection;
    Button[] m_usedButtons;
    Vector3 m_targetVelocity;

    //ability variables
    enum E_Ability { e_Stun, e_Sabotage, e_Shove, e_SwitchMachines };
    E_Ability m_currentAbility = E_Ability.e_Stun;
    bool m_stuned = false;
    float m_stunedTimer = 0;
    float m_stunCooldown = 0;
    bool m_stunOtherPlayer = false;
    float m_ShoveCooldown = 0;
    bool m_shoveOtherPlayer = false;
    bool m_shoved = false;
    float m_shovedTimer;
    Vector3 m_shoveVelocity;


    [Header("Components")]
    public Camera m_camera;
    public Rigidbody m_rigidBody;
    public Transform m_graphicTransform;
    public ParticleSystem m_dashParticles;
    public Button[] m_workerMenuButtons;
    public Button[] m_traitorMenuButtons;

    [Header("Objects")]
    public MeshRenderer m_graphicRenderer;

    [Header("UI")]
    public GameObject m_traitorMenu;
    public GameObject m_workerMenu;
    public Text m_playerText;
    public Text m_mMapText;
    public Text m_role;
    public Text m_interact;

    // Machine and Item Integration variables
    GameObject m_console;
    bool m_isInteracting;
    int m_timer;

    void Start()
    {
        m_playerText.text = "Player " + m_player.ToString();
        m_role.text = "Role: " + ((m_playerType == PlayerType.Worker) ? "Worker" : "Traitor");
        m_role.color = ((m_playerType == PlayerType.Worker) ? new Color(150.0f / 255, 255.0f / 255, 150.0f / 255, 255.0f / 255) : new Color(255.0f / 255, 150.0f / 255, 150.0f / 255, 255.0f / 255));
        m_mMapText.text = "P" + m_player.ToString();

        switch(m_player)
        {
            case 1:
                m_mMapText.color = new Color(0 / 255, 255.0f / 255, 0 / 255, 255.0f / 255);
                m_graphicRenderer.material.color = new Color(0 / 255, 255.0f / 255, 0 / 255, 255.0f / 255);
                break;
            case 2:
                m_mMapText.color = new Color(255.0f / 255, 0 / 255, 0 / 255, 255.0f / 255);
                m_graphicRenderer.material.color = new Color(255.0f / 255, 0 / 255, 0 / 255, 255.0f / 255);
                break;
            case 3:
                m_mMapText.color = new Color(255.0f / 255, 0 / 255, 255.0f / 255, 255.0f / 255);
                m_graphicRenderer.material.color = new Color(255.0f / 255, 0 / 255, 255.0f / 255, 255.0f / 255);
                break;
            case 4:
                m_mMapText.color = new Color(0 / 255, 0 / 255, 255.0f / 255, 255.0f / 255);
                m_graphicRenderer.material.color = new Color(0 / 255, 0 / 255, 255.0f / 255, 255.0f / 255);
                break;
        }

        if (m_playerType == PlayerType.Traitor)
        {
            m_maxMenuSelection = m_traitorMenuButtons.Length - 1;
            Debug.Log(m_maxMenuSelection);
            m_usedButtons = m_traitorMenuButtons;
        }
        else
        {
            m_maxMenuSelection = m_workerMenuButtons.Length - 1;
            Debug.Log(m_maxMenuSelection);
            m_usedButtons = m_workerMenuButtons;
        }
    }

    void FixedUpdate()
    {
        m_prevState = m_curState;
        m_curState = GamePad.GetState(m_pIndex);
        
        if (!m_inMenu&&!m_stuned&&!m_shoved)
        {
            m_targetVelocity = new Vector3(m_curState.ThumbSticks.Left.X, 0, m_curState.ThumbSticks.Left.Y);
        }

        if (m_stuned)
        {
            if (Time.time > m_stunedTimer)
            {
                m_stuned = false;
            }
        }
        if (m_shoved)
        {
            m_targetVelocity = m_shoveVelocity;
            if (Time.time > m_shovedTimer)
            {
                m_shoved = !m_shoved;
            }
        }

        m_targetVelocity = transform.TransformDirection(m_targetVelocity);

        if (m_dashing)
        {
            m_targetVelocity = m_lastDirection;
            m_targetVelocity = transform.TransformDirection(m_targetVelocity);
            m_targetVelocity *= m_dashSpeed;
        }
        else
        {
            m_lastDirection = m_targetVelocity;
            m_targetVelocity *= m_speed;

            if (m_targetVelocity != new Vector3(0, 0, 0))
                m_dashParticles.Emit((Random.Range(0, 10) == 0) ? 1 : 0);
        }

        Vector3 velocity = m_rigidBody.velocity;
        Vector3 velocityChange = m_targetVelocity - velocity;
        velocity.x = Mathf.Clamp(velocityChange.x, -1, 1);
        velocity.z = Mathf.Clamp(velocityChange.z, -1, 1);

        m_rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);

        if (!m_inMenu)
        {
            if (m_curState.ThumbSticks.Left.X != 0 || m_curState.ThumbSticks.Left.Y != 0)
            {
                Vector3 localForward = Vector3.Normalize(new Vector3(m_curState.ThumbSticks.Left.X, 0, m_curState.ThumbSticks.Left.Y));
                m_graphicTransform.forward = transform.localToWorldMatrix.MultiplyVector(localForward);
                m_graphicTransform.position = transform.position;
            }

            if (m_curState.Buttons.B == ButtonState.Pressed && Time.time > m_nextDash && !m_dashing)
            {
                m_dashing = true;
                m_dashEnd = Time.time + m_dashLength;
                m_dashParticles.Emit(15);
            }

            if (m_curState.Buttons.X == ButtonState.Pressed)
            {
                Debug.Log("UseAblilty");
                UseAbility();
            }
        }

        if (m_curState.Buttons.Start == ButtonState.Pressed && m_prevState.Buttons.Start != ButtonState.Pressed)
        {
            m_inMenu = !m_inMenu;
            if (m_playerType == PlayerType.Traitor)
            {
                m_traitorMenu.SetActive(m_inMenu);
            }
            else
            {
                m_workerMenu.SetActive(m_inMenu);
            }
        }
        if (m_inMenu)
        {
            MenuSelection();
        }
        if (Time.time > m_dashEnd && m_dashing)
        {
            m_nextDash = Time.time + m_dashDelay;
            m_dashing = false;
        }

        // Machine and Item integration
        m_isInteracting = m_curState.Buttons.A == 0;

        if (m_timer > 0) { m_timer--; }
        else if (m_timer == 0) { m_interact.text = ""; m_timer = -1; }

        // Item carrying
        if (m_console != null)
        {
            float distance = 0.5f;

            m_console.GetComponent<Transform>().SetPositionAndRotation(gameObject.GetComponent<Transform>().position, Quaternion.Euler(m_graphicTransform.eulerAngles));
            m_console.GetComponent<Transform>().Translate(new Vector3(0, 0, distance));

            if (m_curState.Buttons.X == 0) // Drop carried item
            {
                m_console.GetComponent<Rigidbody>().useGravity = true;
                m_console = null;
            }
        }

    }

    // Machine and item integration
    public bool IsInteracting() { return m_isInteracting; }
    
    public void SetItem(GameObject _console) { m_console = _console; }
    public void SetItem() { m_console = null; } // Remove item from player
    public GameObject GetItem() { return m_console; }

    public void CanInteract(string _message)
    {
        m_timer = 2;
        m_interact.enabled = true;
        m_interact.text = "A - " + _message;
    }

    public int GetNextStep()
    {
        if (m_console != null)
        {
            return m_console.GetComponent<Item>().GetStep();
        }

        return -1;
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

        m_usedButtons[m_menuSelection].Select();

        if (m_curState.Buttons.A == ButtonState.Pressed)
        {
            m_usedButtons[m_menuSelection].onClick.Invoke();
            m_inMenu = !m_inMenu;
            if (m_playerType == PlayerType.Traitor)
            {
                m_traitorMenu.SetActive(m_inMenu);
            }
            else
            {
                m_workerMenu.SetActive(m_inMenu);
            }
        }
    }

    void UseAbility()
    {
        switch (m_currentAbility)
        {
            case E_Ability.e_Stun:
                if (Time.time > m_stunCooldown)
                {
                    m_stunOtherPlayer = true;
                    m_stunCooldown = Time.time + 3;
                }
                break;
            case E_Ability.e_Shove:
                if (Time.time > m_ShoveCooldown)
                {
                    m_shoveOtherPlayer = true;
                    m_ShoveCooldown = Time.time + 3;
                }
                break;
            case E_Ability.e_Sabotage:

                break;
            case E_Ability.e_SwitchMachines:

                break;
        }
    }

    public int GetType() { return (int)m_playerType; }
    public void TriggerStunned() { m_stuned = true; m_stunedTimer = Time.time + 3; }
    public void TriggerShove(Vector3 _TraitorFacingDir) { m_shoveVelocity = new Vector3(_TraitorFacingDir.x * 2, 0, _TraitorFacingDir.z * 2); m_shoved = true; m_shovedTimer = Time.time + 0.2f; Debug.Log("Has Been shoved"); }

    // Swiping
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (m_playerType == 0 && other.GetComponent<PlayerController>().GetType() == 1 && m_console == null && other.GetComponent<PlayerController>().GetItem() != null) // You are an empty handed worker, they are a stealing fuckface
            {
                CanInteract("Steal!");

                if (m_isInteracting && m_console == null)
                {
                    m_console = other.GetComponent<PlayerController>().GetItem();
                    other.GetComponent<PlayerController>().SetItem();
                }
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (m_stunOtherPlayer)
            {
                Debug.Log("triggered stun");
                other.GetComponent<PlayerController>().TriggerStunned();
                m_stunOtherPlayer = false;
            }
            if (m_shoveOtherPlayer)
            {
                other.GetComponent<PlayerController>().TriggerShove(m_graphicTransform.forward);
                m_shoveOtherPlayer = false;
            }
        }
    }

    public void SwitchToStun()
    {
        m_currentAbility = E_Ability.e_Stun;
        Debug.Log("Switched to Stun");
    }

    public void SwitchToSabotage()
    {
        m_currentAbility = E_Ability.e_Sabotage;
        Debug.Log("Switched to saba");
    }

    public void SwitchToShove()
    {
        Debug.Log("Switched to Shove");
        m_currentAbility = E_Ability.e_Shove;
    }

    public void SwitchToMachineLocations()
    {
        m_currentAbility = E_Ability.e_SwitchMachines;
        Debug.Log("Switched to switch machines");
    }
}
