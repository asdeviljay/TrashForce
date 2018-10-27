using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashMeneger : MonoBehaviour {

	public GameObject[] canvasUI;
	float curTimeUp;
	float curComboTime = 2.0f;
	int countToMul = 0;
	int scoreMul = 1;
	bool isTimeUp = false;
	bool isCombo = false;
	bool isPause = false;
	bool isSoundGameOver = false;
	int score = 0;
	int life = 3;
	public Trashes t;
	public Text timeUpText;
	public Text comboTimeText;
	public Text comboTimeText2;
	public Text scoreText;
	public Text highScoreText;
	public float addComboTime = 1.0f;
	public GameObject[] lifeImage;
	public Sprite normalLifeImage;
	public Sprite lifeBreakImage;
	public SpriteRenderer showCombo;
	public Sprite[] comboMultiplier;

	// Use this for initialization
	void Start () {
		int ran = Random.Range(1, 4);
		switch (ran) {
		case 1:
			FindObjectOfType<AudioManager>().Play("PlayingTheme");
			break;
		case 2:
			FindObjectOfType<AudioManager>().Play("PlayingTheme2");
			break;
		case 3:
			FindObjectOfType<AudioManager>().Play("PlayingTheme3");
			break;
		}
		StartCoroutine (StartCountdown ());
	}
	
	// Update is called once per frame
	void Update () {
		if (isTimeUp || life == 0) {
			if (!isSoundGameOver) {
				FindObjectOfType<AudioManager> ().Stop ("PlayingTheme");
				FindObjectOfType<AudioManager> ().Stop ("PlayingTheme2");
				FindObjectOfType<AudioManager> ().Stop ("PlayingTheme3");
				FindObjectOfType<AudioManager> ().Stop ("MaxComboTheme");
				FindObjectOfType<AudioManager> ().PlayOnce ("GameOver");
				isSoundGameOver = true;
			}
			timeUpText.text = "0";
			curTimeUp = 1.0f;
			isTimeUp = true;
			isCombo = false;
			foreach (GameObject cui in canvasUI)
				cui.SetActive (true);
			BroadcastMessage ("End");

		} else {
			foreach (GameObject cui in canvasUI)
				cui.SetActive (false);
		}
		if (!isCombo) {
			comboTimeText.gameObject.SetActive (false);
			comboTimeText2.gameObject.SetActive (false);
		} else {
			comboTimeText.gameObject.SetActive (true);
			comboTimeText2.gameObject.SetActive (true);
		}
		if (life == 2)
			lifeImage [0].GetComponent<SpriteRenderer> ().sprite = lifeBreakImage;
		else if (life == 1)
			lifeImage [1].GetComponent<SpriteRenderer> ().sprite = lifeBreakImage;
		else if (life == 0)
			lifeImage [2].GetComponent<SpriteRenderer> ().sprite = lifeBreakImage;
		else 
			for (int i = 0 ; i < lifeImage.Length ; i++)
				lifeImage [i].GetComponent<SpriteRenderer> ().sprite = normalLifeImage;
		if (isPause)
			BroadcastMessage("End");
	}

	IEnumerator StartCountdown (float timeUp = 60.0f) {
		curTimeUp = timeUp;
		while (curTimeUp > 0 && !isTimeUp) {
			if (isPause) {
				yield return null;
			} else {
				timeUpText.text = curTimeUp.ToString ();
				yield return new WaitForSecondsRealtime (1.0f);
				curTimeUp--;
			}
		}
		timeUpText.text = curTimeUp.ToString();
		isTimeUp = true;
	}

	IEnumerator StartCombo (float addComboTime) {
		curComboTime += addComboTime;
		while (curComboTime > 0) {
			if (isPause) {
				yield return null;
			} else {
				comboTimeText.text = curComboTime.ToString ();
				yield return new WaitForSecondsRealtime (0.1f);
				curComboTime -= 0.1f;
			}
		}
		showCombo.sprite = null;
		countToMul = 0;
		scoreMul = 1;
		curComboTime = 2.0f;
		isCombo = false;
		FindObjectOfType<AudioManager>().Play("ComboBreak");
		FindObjectOfType<AudioManager>().Stop("MaxComboTheme");
		FindObjectOfType<AudioManager>().UnPause("PlayingTheme");
		FindObjectOfType<AudioManager>().UnPause("PlayingTheme2");
		FindObjectOfType<AudioManager>().UnPause("PlayingTheme3");
	}

	void AddAndCheckToMul () {
		score += scoreMul;
		countToMul++;
		curComboTime += addComboTime;
		if (isCombo) {
			if (countToMul < 1) {
				scoreMul = 1;
				showCombo.sprite = null;
			}
			else if (countToMul < 3) {
				if (scoreMul == 1)
					FindObjectOfType<AudioManager>().Play("Combo2");
				scoreMul = 2;
				showCombo.sprite = comboMultiplier [0];
			} else if (countToMul < 7) {
				if (scoreMul == 2)
					FindObjectOfType<AudioManager>().Play("Combo3");
				scoreMul = 3;
				showCombo.sprite = comboMultiplier [1];
			} else if (countToMul < 11) {
				if (scoreMul == 3)
					FindObjectOfType<AudioManager>().Play("Combo4");
				scoreMul = 4;
				showCombo.sprite = comboMultiplier [2];
			} else if (countToMul < 15) {
				if (scoreMul == 4) {
					FindObjectOfType<AudioManager> ().Play ("Combo5");
					FindObjectOfType<AudioManager> ().Pause ("PlayingTheme");
					FindObjectOfType<AudioManager> ().Pause ("PlayingTheme2");
					FindObjectOfType<AudioManager> ().Pause ("PlayingTheme3");
					FindObjectOfType<AudioManager> ().Play ("MaxComboTheme");
				}
				scoreMul = 5;
				showCombo.sprite = comboMultiplier [3];
			}
		}
	}

	public int GetTrashesLenght () {
		return t.GetLenght ();
	}

	public string GetBinName (int ran) {
		return t.GetBinToDump (ran);
	}

	public Sprite GetTrashImage (int ran) {
		return t.GetTrashImage (ran);
	}

	public void Correct () {
		if (isCombo) {
			AddAndCheckToMul ();
		} else {
			score++;
			StartCoroutine (StartCombo (addComboTime));
		}
		scoreText.text = score.ToString();
		isCombo = true;
		FindObjectOfType<AudioManager>().Play("Correct");
	}

	public void Incorrect () {
		isCombo = false;
		curComboTime = 0.0f;
		countToMul = 0;
		scoreMul = 1;
		life--;
		showCombo.sprite = null;
		FindObjectOfType<AudioManager>().Play("Incorrect");
	}

	public void NewGame () {
		int ran = Random.Range(1, 4);
		switch (ran) {
		case 1:
			FindObjectOfType<AudioManager> ().Play ("PlayingTheme");
			break;
		case 2:
			FindObjectOfType<AudioManager> ().Play ("PlayingTheme2");
			break;
		case 3:
			FindObjectOfType<AudioManager> ().Play ("PlayingTheme3");
			break;
		}
		if (int.Parse (highScoreText.text) < score)
			highScoreText.text = score.ToString ();
		BroadcastMessage ("Restart");
		score = 0;
		scoreText.text = score.ToString();
		life = 3;
		curComboTime = 2.0f;
		countToMul = 0;
		scoreMul = 1;
		isTimeUp = false;
		isCombo = false;
		isSoundGameOver = false;
		StartCoroutine (StartCountdown ());
	}

	public void Pause() {
		if (FindObjectOfType<AudioManager> ().IsPlaying ("MaxComboTheme")) {
			FindObjectOfType<AudioManager> ().Pause ("MaxComboTheme");
		} else {
			FindObjectOfType<AudioManager> ().Pause ("PlayingTheme");
			FindObjectOfType<AudioManager> ().Pause ("PlayingTheme2");
			FindObjectOfType<AudioManager> ().Pause ("PlayingTheme3");
		}
		isPause = true;
		BroadcastMessage ("End");
	}

	public void UnPause() {
		if (FindObjectOfType<AudioManager> ().IsPause ("MaxComboTheme")) {
			FindObjectOfType<AudioManager> ().UnPause ("MaxComboTheme");
		} else {
			FindObjectOfType<AudioManager> ().UnPause ("PlayingTheme");
			FindObjectOfType<AudioManager> ().UnPause ("PlayingTheme2");
			FindObjectOfType<AudioManager> ().UnPause ("PlayingTheme3");
		}
		isPause = false;
		BroadcastMessage ("Restart");
	}
}
