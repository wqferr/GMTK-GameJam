using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public Text killsText, comboText;

	public class Score {
		public int maxCombo;
		public int enemiesKilled;
		public float dist;
		public string name;

		public Score() {}

		public Score(float dist, int enemies, int maxCombo, string name) {
			this.maxCombo = maxCombo;
			this.enemiesKilled = enemies;
			this.dist = dist;
			this.name = name;
		}
	}

	public static int MAX_N = 10;

	public int combo;
	public int maxCombo;
	public int enemiesKilled;
	public float distance;

	private float timer = 0.0f;

	public void Start() {
		combo = 0;
		maxCombo = 0;
		enemiesKilled = 0;
		distance = 0;
	}

	public void Hit()
	{
		enemiesKilled++;
		combo++;

		killsText.text = "Kills: " + enemiesKilled;
		comboText.text = "Combo: " + combo;
	}

	public void EndCombo()
	{
		if (combo > maxCombo)
			SetMaxCombo (combo);
		combo = 0;
	}

	public void SetMaxCombo(int cmb) {
		maxCombo = cmb;
	}

	public void AddScore(string name) {
		Score s = new Score (distance, enemiesKilled, maxCombo, name);
		Score[] scores;

		if (PlayerPrefs.HasKey ("n")) {
			int n = PlayerPrefs.GetInt("n");
			int i;

			Score[] oldScores = GetHighScores (n);

			n++;
			scores = new Score[n];
			for (i = 0; i < oldScores.Length; i++)
				scores [i] = oldScores[i];
			
			// Sorted insert
			i = oldScores.Length-1;
			while (i >= 0 && scores [i].dist < s.dist) {
				scores [i + 1] = scores [i];
				i--;
			}
			if (i < n - 1)
				scores [i + 1] = s;
		} else {
			scores = new Score[] { s };
		}

		SetScores (scores);
	}

	private void SetScores(Score[] scores) {
		int n = Mathf.Min (scores.Length, MAX_N);
		PlayerPrefs.SetInt ("n", n);
		for (int i = 0; i < n; i++) {
			PlayerPrefs.SetString ("HS_" + i + "_name", scores [i].name);
			PlayerPrefs.SetFloat ("HS_" + i + "_dist", scores[i].dist);
			PlayerPrefs.SetInt ("HS_" + i + "_ekill", scores[i].enemiesKilled);
			PlayerPrefs.SetInt ("HS_" + i + "_combo", scores[i].maxCombo);
		}
	}

	public Score[] GetHighScores(int n) {
		if (!PlayerPrefs.HasKey ("n"))
			return new Score[] { };
		
		n = Mathf.Min (n, PlayerPrefs.GetInt ("n"));
		
		Score[] scores = new Score[n];
		for (int i = 0; i < n; i++) {
			var s = new Score ();
			s.name = PlayerPrefs.GetString ("HS_" + i + "_name");
			s.dist = PlayerPrefs.GetFloat ("HS_" + i + "_dist");
			s.enemiesKilled = PlayerPrefs.GetInt ("HS_" + i + "_ekill");
			s.maxCombo = PlayerPrefs.GetInt ("HS_" + i + "_combo");

			scores [i] = s;
		}

		return scores;
	}

}
