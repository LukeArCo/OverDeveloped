using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float m_roundTime;
	public int m_scoreLimit;
	public int m_goal;
	public int m_increment;
	public int m_mapCount;

	AudioSource m_sound;

	public AudioClip[] m_bgm;

	int[] m_scores = new int[4];

	int m_consoles;
	float m_timer;
	int m_traitor;

	void Awake() {
		m_traitor = -1; // Set traitor to player 1 (0) by default

		m_sound = GetComponent<AudioSource>();
		m_sound.ignoreListenerVolume = true;

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		m_timer = m_roundTime;
	}
	
	// Update is called once per frame
	void Update () {

		// Debug.Log (m_timer);
		if (m_traitor == -1) {
			m_traitor = 0;

			m_sound.Stop();
			m_sound.clip = m_bgm[Random.Range (0, m_bgm.Length)];
			m_sound.Play();

			SceneManager.LoadScene (Random.Range (1, m_mapCount + 1));
			return;
		}

		m_timer -= Time.deltaTime;

		if (m_timer <= 0) {
			// Debug.Log ("Times up!");
			m_timer = m_roundTime;


			if (m_consoles < m_goal) {
				m_scores [m_traitor]++;
				Debug.Log ("Player " + (m_traitor + 1) + " has won the round!");
			} else {
				Debug.Log ("Player " + (m_traitor + 1) + " has lost the round!");
			}

			// Is traitor winner?
			if (m_scores [m_traitor] == m_scoreLimit) {
				Debug.Log ("Player " + (m_traitor + 1) + " Wins!");
			}

			m_goal += m_increment;
			Debug.Log ("Goal is to make " + m_goal + " consoles!");

			m_sound.Stop();
			m_sound.clip = m_bgm[Random.Range (0, m_bgm.Length)];
			m_sound.Play();

			SceneManager.LoadScene (Random.Range (1, m_mapCount + 1));
		}

	}

	public void AddPoint() {
		m_consoles++;
	}

	// May not be needed but putting this here just in case...
	public void AddScore(int _player)
	{
		m_scores [_player] += 1;
	}

	public void SetTraitor(int _player) { // Have the player manager call this giving the playerID on level start
		m_traitor = _player;
	}
}
