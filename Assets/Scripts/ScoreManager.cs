using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

	public class Score {
		public int maxCombo;
		public int enemiesKilled;
		public float dist;
	}

	public static int MAX_N = 10;

	public int combo;
	public int maxCombo;
	public int enemiesKilled;
	public float distance;

	public void Start() {
		combo = 0;
		maxCombo = 0;
		enemiesKilled = 0;
		distance = 0;
	}

	public void Hit() {
		enemiesKilled++;
		combo++;
	}

	public void EndCombo() {
		// TODO add score
		combo = 0;
		if (combo > maxCombo)
			SetMaxCombo (combo);
	}

	public void SetMaxCombo(int cmb) {
		maxCombo = cmb;
	}

	public void AddScore() {
		Score s = new Score ();
		Score[] scores;

		s.dist = distance;
		s.enemiesKilled = enemiesKilled;
		s.maxCombo = maxCombo;

		if (PlayerPrefs.HasKey ("n")) {
			int n = PlayerPrefs.GetInt("n");
			Score[] oldScores = GetHighScores (n);

			if (n < MAX_N)
				n++;
			scores = new Score[n];

			// Sorted insert
			int i = n;
			while (i >= 0 && scores [i].dist < s.dist)
				i--;
			if (i < n - 1)
				scores [i + 1] = s;
		} else {
			scores = new Score[] { s };
		}

		SetScores (scores);
	}

	private void SetScores(Score[] scores) {
		PlayerPrefs.SetInt ("n", scores.Length);
		for (int i = 0; i < scores.Length; i++) {
			PlayerPrefs.SetFloat ("HS_" + i + "_dist", scores[i].dist);
			PlayerPrefs.SetInt ("HS_" + i + "_ekill", scores[i].enemiesKilled);
			PlayerPrefs.SetInt ("HS_" + i + "_combo", scores[i].maxCombo);
		}
	}

	public Score[] GetHighScores(int n) {
		if (PlayerPrefs.HasKey ("n"))
			n = Mathf.Min (n, PlayerPrefs.GetInt ("n"));
		
		Score[] scores = new Score[n];
		for (int i = 0; i < n; i++) {
			var s = new Score ();
			s.dist = PlayerPrefs.GetFloat ("HS_" + i + "_dist");
			s.enemiesKilled = PlayerPrefs.GetInt ("HS_" + i + "_ekill");
			s.maxCombo = PlayerPrefs.GetInt ("HS_" + i + "_combo");

			scores [i] = s;
		}

		return scores;
	}

}
