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

	int[] m_scores = new int[4];

	int m_consoles;
	float m_timer;

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		m_timer = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;

		// Debug.Log (m_timer);

		if (m_timer <= 0) {
			m_goal += m_increment;
			Debug.LogWarning ("Goal is to make " + m_goal + " consoles!");

			// Debug.Log ("Times up!");
			m_timer = m_roundTime;


			SceneManager.LoadScene (Random.Range (1, m_mapCount + 1));
		}

	}

	public void AddPoint() {
		m_consoles++;
	}

	public void AddScore(int _player)
	{
		m_scores [_player] += 1;
	}
}
