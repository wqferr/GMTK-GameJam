using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPopupBehaviour : MonoBehaviour
{
	public LeaderboardsBehaviour leaderBoard; 
	public float targetHeight;
	public float moveDuration;

	private bool move;
	private float timeMoving;
	private float initialHeight;

	public void OnTryAgainClick()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnLeaderBoardsClick()
	{
		leaderBoard.gameObject.SetActive(true);
		leaderBoard.Activate();
	}

	public void OnMainClick()
	{
		SceneManager.LoadScene(0);
	}

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
		timeMoving = 0.0f;
		initialHeight = transform.position.y;
		move = true;
	}
}
