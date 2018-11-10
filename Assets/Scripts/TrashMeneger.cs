using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashMeneger : MonoBehaviour {

	public GameObject[] canvasUI;
	float curTimeUp;
	float curComboTime = 2.0f;
	float tempComboTime;
	int countToMul = 0;
	int scoreMul = 1;
	bool isTimeUp = false;
	bool isCombo = false;
	bool isPause = false;
	bool isSoundGameOver = false;
	bool isUltimateUsed = false;
	bool isSubmit = false;
	int score = 0;
	int life = 3;
	int countToUltimate = 0;
	public int countUltimate = 10;
	public Trashes t;
	public Text timeUpText;
	public GameObject comboTimeBarParent;
	public Image comboTimeBar;
	public Image UltimateBar;
	public Text scoreText;
	public Text highScoreText;
	public InputField LeaderText;
	public GameObject Transparent;
	public float addComboTime = 1.0f;
	public GameObject[] lifeImage;
	public Sprite normalLifeImage;
	public Sprite lifeBreakImage;
	public SpriteRenderer showCombo;
	public Sprite[] comboMultiplier;

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll ();
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
			if(!isSubmit)
				foreach (GameObject cui in canvasUI)
					cui.SetActive (true);
			Component[] c = FindObjectsOfType<SpawnWorm> ();
			foreach (SpawnWorm b in c)
				b.End ();
			BroadcastMessage ("End");

		} else {
			foreach (GameObject cui in canvasUI)
				cui.SetActive (false);
		}
		if (!isCombo)
			comboTimeBarParent.SetActive (false);
		else 
			comboTimeBarParent.SetActive (true);
		if (countToUltimate >= countUltimate && isUltimateUsed)
			StartCoroutine (StartUltimate ());
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
			if (isPause || isUltimateUsed) {
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
		tempComboTime = curComboTime;
		while (curComboTime > 0) {
			if (isPause) {
				yield return null;
			} else {
				comboTimeBar.fillAmount = curComboTime / tempComboTime;
				float deltaTime = Time.deltaTime;
				// yield return new WaitForSecondsRealtime(deltaTime);
				yield return null;
				curComboTime -= deltaTime;
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

	IEnumerator StartUltimate(float ultimateTime = 5.0f) {
		countToUltimate = 0;
		float curUltimateTime = ultimateTime;
		Transparent.SetActive (true);
		while (curUltimateTime > 0) {
			if (isPause) {
				yield return null;
			} else {
				UltimateBar.fillAmount = curUltimateTime / ultimateTime;
				yield return new WaitForSecondsRealtime (0.1f);
				curUltimateTime -= 0.1f;
			}
		}
		countToUltimate = 0;
		isUltimateUsed = false;
		Transparent.SetActive (false);
	}

	public void UseUltimate () {
		if (countToUltimate >= countUltimate) {
			isUltimateUsed = true;
			CheckToMul ();
		}
	}

	void CheckToMul () {
		if (isCombo) {
			if (countToMul < 1) {
				scoreMul = 1;
				if(isUltimateUsed)
					showCombo.sprite = comboMultiplier [4];
				else
					showCombo.sprite = null;
			}
			else if (countToMul < 3) {
				if (scoreMul == 1)
					FindObjectOfType<AudioManager>().Play("Combo2");
				scoreMul = 2;
				if(isUltimateUsed)
					showCombo.sprite = comboMultiplier [5];
				else
					showCombo.sprite = comboMultiplier [0];
			} else if (countToMul < 7) {
				if (scoreMul == 2)
					FindObjectOfType<AudioManager>().Play("Combo3");
				scoreMul = 3;
				if(isUltimateUsed)
					showCombo.sprite = comboMultiplier [6];
				else
					showCombo.sprite = comboMultiplier [1];
			} else if (countToMul < 11) {
				if (scoreMul == 3)
					FindObjectOfType<AudioManager>().Play("Combo4");
				scoreMul = 4;
				if(isUltimateUsed)
					showCombo.sprite = comboMultiplier [7];
				else
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
				if(isUltimateUsed)
					showCombo.sprite = comboMultiplier [8];
				else
					showCombo.sprite = comboMultiplier [3];
			}
		}
	}

	void AddAndCheckToMul () {
		if (isUltimateUsed)
			score += scoreMul * 2;
		else
			score += scoreMul;
		countToMul++;
		curComboTime += addComboTime;
		tempComboTime = curComboTime;
		CheckToMul ();
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
			if (isUltimateUsed) {
				score += 2;
				showCombo.sprite = comboMultiplier [4];
			} else
				score++;
			StartCoroutine (StartCombo (addComboTime));
		}
		if (isUltimateUsed)
			countToUltimate = 0;
		else
			countToUltimate++;
		UltimateBar.fillAmount = (float)countToUltimate / countUltimate;
		scoreText.text = score.ToString();
		isCombo = true;
		FindObjectOfType<AudioManager>().Play("Correct");
	}

	public void Incorrect () {
		isCombo = false;
		curComboTime = 0.0f;
		comboTimeBar.fillAmount = curComboTime / tempComboTime;
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
		Component[] c = FindObjectsOfType<SpawnWorm> ();
		foreach (SpawnWorm b in c)
			b.NewGame ();
		BroadcastMessage ("Restart");
		score = 0;
		scoreText.text = score.ToString();
		life = 3;
		curComboTime = 2.0f;
		countToMul = 0;
		scoreMul = 1;
		countToUltimate = 0;
		isTimeUp = false;
		isCombo = false;
		isSoundGameOver = false;
		isUltimateUsed = false;
		isSubmit = false;
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
		Component[] c = FindObjectsOfType<SpawnWorm> ();
		foreach (SpawnWorm b in c)
			b.Pause ();
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
		Component[] c = FindObjectsOfType<SpawnWorm> ();
		foreach (SpawnWorm b in c)
			b.UnPause ();
		isPause = false;
		BroadcastMessage ("Restart");
	}

	public void GetLeaderName () {
		if (LeaderText.text != "") {
			PlayerPrefs.SetString ("HighScore" + PlayerPrefs.GetInt ("PlayerCount"), LeaderText.text + " " + score);
			PlayerPrefs.SetInt ("PlayerCount", PlayerPrefs.GetInt ("PlayerCount") + 1);
			LeaderText.text = "";
			isSubmit = true;
		}
	}
}
