using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenBehaviour : MonoBehaviour {
	public void OnPlayClick() {
		SceneManager.LoadScene("MainScene");
	}

	public void OnCreditsClick() {
		SceneManager.LoadScene ("Credits");
	}

	public void OnClearScoreClick() {
		PlayerPrefs.DeleteAll ();
	}
}
