using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsBehaviour : MonoBehaviour
{
	public ScoreManager scoreManager;
	public Text textPrefab; 
	public RectTransform panel; 

	public float moveDuration;
	public float activatedHeight;
	private float targetHeight;

	private bool move;
	private float timeMoving;
	private float initialHeight;

	void FixedUpdate()
	{
		if(move)
		{
			timeMoving += Time.fixedDeltaTime;
			if (Mathf.Abs (transform.position.y - targetHeight) < 0.01f)
			{
				transform.position = new Vector3 (transform.position.x, targetHeight, transform.position.z);
				move = false;
			}
			else
			{
				float t = timeMoving / moveDuration - 1;
				float dh = targetHeight - initialHeight;
				transform.position = new Vector3 (transform.position.x, dh * (t*t*t*t*t + 1) + initialHeight, transform.position.z);
			}
		}
	}

	public void Activate()
	{
		targetHeight = activatedHeight;
		initialHeight = transform.position.y;
		timeMoving = 0.0f;

		move = true;

		//populate the screen
		List<ScoreManager.Score> highScores = new List<ScoreManager.Score>(scoreManager.GetHighScores(9));
		foreach(ScoreManager.Score highScore in highScores)
		{
			Text nameText = Instantiate (textPrefab) as Text;
			nameText.text = "bananinha";
			nameText.transform.SetParent (panel);
			nameText.gameObject.SetActive (true);
		}
	}

	public void Deactivate()
	{
		targetHeight = initialHeight;
		initialHeight = transform.position.y;
		timeMoving = 0.0f;

		move = true;
	}
}
