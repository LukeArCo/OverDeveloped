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

    [Header("Components")]
    public Camera m_camera;
    public Rigidbody m_rigidBody;
    public Transform m_graphicTransform;
    public ParticleSystem m_dashParticles;

    [Header("UI")]
    public GameObject m_traitorMenu;
    public GameObject m_workerMenu;
    public Text m_playerText;
    public Text m_role;

    void Start()
    {
        m_playerText.text = "Player " + m_player.ToString();
        m_role.text = "Role: " + ((m_playerType == PlayerType.Worker) ? "Worker" : "Traitor");
        m_role.color = ((m_playerType == PlayerType.Worker) ? new Color(150.0f / 255, 255.0f / 255, 150.0f / 255, 255.0f / 255) : new Color(255.0f / 255, 150.0f / 255, 150.0f / 255, 255.0f / 255));
    }

    void FixedUpdate()
    {
        m_prevState = m_curState;
        m_curState = GamePad.GetState(m_pIndex);

        Vector3 targetVelocity = new Vector3(0, 0, 0);

        if (!m_inMenu)
        {
            targetVelocity = new Vector3(m_curState.ThumbSticks.Left.X, 0, m_curState.ThumbSticks.Left.Y);
        }

        targetVelocity = transform.TransformDirection(targetVelocity);

        if (m_dashing)
        {
            targetVelocity = m_lastDirection;
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= m_dashSpeed;
        }
        else
        {
            m_lastDirection = targetVelocity;
            targetVelocity *= m_speed;

            if (targetVelocity != new Vector3(0, 0, 0))
                m_dashParticles.Emit((Random.Range(0, 10) == 0) ? 1 : 0);
        }

        Vector3 velocity = m_rigidBody.velocity;
        Vector3 velocityChange = targetVelocity - velocity;
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

        if (Time.time > m_dashEnd && m_dashing)
        {
            m_nextDash = Time.time + m_dashDelay;
            m_dashing = false;
        }
    }
}
