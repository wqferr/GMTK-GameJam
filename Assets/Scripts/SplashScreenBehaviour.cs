﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenBehaviour : MonoBehaviour {
	public void OnPlayClick() {
		SceneManager.LoadScene("MainScene");
	}
}
