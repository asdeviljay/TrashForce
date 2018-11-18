using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	public GameObject UltimateAura;
	public Image UltimateHand;
	public Sprite[] UltimateHandImage;
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
	public GameObject getReady;
	public GameObject getPause;
	Sound PlayingBGM;
	Sound PausingBGM;
	Sound PausingByButtonBGM;

	// Use this for initialization
	void Start () {
		PlayingBGM = null;
		PausingBGM = null;
		PausingByButtonBGM = null;
		LeaderText.text = "Player" + PlayerPrefs.GetInt ("PlayerCount");
		FindObjectOfType<AudioManagerTrans>().Stop("StartBGM");
		//PlayerPrefs.DeleteAll ();
		StartCoroutine (StartGetReady ());
	}
	
	// Update is called once per frame
	void Update () {
		//if (PausingBGM != null)
			//Debug.Log(PausingBGM.name);
		//Debug.Log(FindObjectOfType<AudioManager>().IsPause("MaxComboTheme"));
		PlayingBGM = FindObjectOfType<AudioManager>().IsPlayingAnyBGM();
		if (isTimeUp || life == 0) {
			if (!isSoundGameOver) {
				/*FindObjectOfType<AudioManager> ().Stop ("PlayingTheme");
				FindObjectOfType<AudioManager> ().Stop ("PlayingTheme2");
				FindObjectOfType<AudioManager> ().Stop ("PlayingTheme3");
				FindObjectOfType<AudioManager> ().Stop ("MaxComboTheme");*/
				if (PlayingBGM != null)
					FindObjectOfType<AudioManager>().Stop(PlayingBGM.name);
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
			FindObjectOfType<RatSpawn> ().End ();
			UltimateBar.fillAmount = 0.0f;
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

	IEnumerator StartGetReady (float TimeUp = 3.0f) {
		PlayingBGM = null;
		PausingBGM = null;
		PausingByButtonBGM = null;
		float curTimeToReady = TimeUp;
		getReady.SetActive (true);
		getPause.SetActive (false);
		FindObjectOfType<AudioManager>().Play("GetReady");
		while (curTimeToReady > 0) {
			if (isPause) {
				yield return null;
			} else {
				yield return new WaitForSecondsRealtime (0.5f);
				curTimeToReady-= 0.5f;
				if (curTimeToReady == 0.5f) {
					FindObjectOfType<AudioManager>().Play("Start");
				}
			}
		}
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
		getReady.SetActive (false);
		getPause.SetActive (true);
		StartCoroutine (StartCountdown ());
	}

	IEnumerator StartCountdown (float timeUp = 60.0f) {
		int curTimeupInt;
		curTimeUp = timeUp;
		while (curTimeUp > 0 && !isTimeUp) {
			curTimeupInt = (int) curTimeUp;
			if (isPause || isUltimateUsed) {
				yield return null;
			} else {
				timeUpText.text = curTimeUp.ToString ();
				if (curTimeUp <= 5)
				{
					switch (curTimeupInt)
					{
						case 1:
							FindObjectOfType<AudioManager>().Play("1");
							break;
						case 2:
							FindObjectOfType<AudioManager>().Play("2");
							break;
						case 3:
							FindObjectOfType<AudioManager>().Play("3");
							break;
						case 4:
							FindObjectOfType<AudioManager>().Play("4");
							break;
						case 5:
							FindObjectOfType<AudioManager>().Play("5");
							break;
					}
				}
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
		FindObjectOfType<AudioManager>().Play("ComboBreak");
		/*FindObjectOfType<AudioManager>().Stop("MaxComboTheme");
		FindObjectOfType<AudioManager>().UnPause("PlayingTheme");
		FindObjectOfType<AudioManager>().UnPause("PlayingTheme2");
		FindObjectOfType<AudioManager>().UnPause("PlayingTheme3");*/
		if (PlayingBGM != null) {
			if (!isTimeUp && life != 0) {
				FindObjectOfType<AudioManager>().Stop("MaxComboTheme");
				if (PlayingBGM.name != "UltimateTheme" && PausingBGM != null) {
					FindObjectOfType<AudioManager>().UnPause(PausingBGM.name);
					PausingBGM = null;
				}
			}
		}
		showCombo.sprite = null;
		countToMul = 0;
		scoreMul = 1;
		curComboTime = 2.0f;
		isCombo = false;
	}

	IEnumerator StartUltimate(float ultimateTime = 5.0f) {
		UltimateHand.sprite = UltimateHandImage [1];
		countToUltimate = 0;
		float curUltimateTime = ultimateTime;
		Transparent.SetActive (true);
		while (curUltimateTime > 0) {
			if (isPause) {
				yield return null;
			} else {
				UltimateBar.fillAmount = curUltimateTime / ultimateTime;
				float deltaTime = Time.deltaTime;
				//yield return new WaitForSecondsRealtime (0.1f);
				yield return null;
				curUltimateTime -= deltaTime;
			}
		}
		countToUltimate = 0;
		isUltimateUsed = false;
		Transparent.SetActive (false);
		CheckToMul ();
		UltimateHand.sprite = UltimateHandImage [0];
		FindObjectOfType<AudioManager>().Stop("UltimateTheme");
		if (PausingBGM != null && !isTimeUp && life != 0 && scoreMul != 5) {
			FindObjectOfType<AudioManager>().UnPause(PausingBGM.name);
			PausingBGM = null;
		} else if (scoreMul == 5) {
			FindObjectOfType<AudioManager>().Play("MaxComboTheme");
		}
	}

	public void UseUltimate () {
		if (countToUltimate >= countUltimate) {
			if (PlayingBGM.name != "MaxComboTheme" || PausingBGM == null)
				PausingBGM = PlayingBGM;
			FindObjectOfType<AudioManager>().Pause(PlayingBGM.name);
			//Debug.Log(PausingBGM.name);
			FindObjectOfType<AudioManager>().Play("UltimatePush");
			FindObjectOfType<AudioManager>().Play("UltimateTheme");
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
					//Debug.Log("Check");
					FindObjectOfType<AudioManager>().Play("Combo5");
					if (PlayingBGM.name != "UltimateTheme") {
						/*
						FindObjectOfType<AudioManager>().Pause("PlayingTheme");
						FindObjectOfType<AudioManager>().Pause("PlayingTheme2");
						FindObjectOfType<AudioManager>().Pause("PlayingTheme3");
						FindObjectOfType<AudioManager>().Play("MaxComboTheme");
						*/
						PausingBGM = PlayingBGM;
						FindObjectOfType<AudioManager>().Pause(PlayingBGM.name);
						FindObjectOfType<AudioManager>().Play("MaxComboTheme");
					}
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
		if (countToUltimate >= countUltimate)
			UltimateAura.SetActive (true);
		scoreText.text = score.ToString();
		isCombo = true;
		FindObjectOfType<AudioManager>().Play("Correct");
	}

	public void Incorrect () {
		if (PlayingBGM != null && isCombo)
		{
			if (PlayingBGM.name != "UltimateTheme" && !isTimeUp && life != 0)
			{
				if (scoreMul == 5)
					FindObjectOfType<AudioManager>().Stop("MaxComboTheme");
				if (PausingBGM != null)
				{
					FindObjectOfType<AudioManager>().UnPause(PausingBGM.name);
					PausingBGM = null;
				}
			}
		}
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
		/*int ran = Random.Range(1, 4);
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
		}*/
		PlayingBGM = null;
		PausingBGM = null;
		PausingByButtonBGM = null;
		if (int.Parse (highScoreText.text) < score)
			highScoreText.text = score.ToString ();
		Component[] c = FindObjectsOfType<SpawnWorm> ();
		foreach (SpawnWorm b in c) 
			b.NewGame ();
		FindObjectOfType<RatSpawn> ().NewGame ();
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
		isPause = false;
		StartCoroutine (StartGetReady ());
	}

	public void Pause() {
		/*if (FindObjectOfType<AudioManager> ().IsPlaying ("MaxComboTheme")) {
			FindObjectOfType<AudioManager> ().Pause ("MaxComboTheme");
		} else {
			FindObjectOfType<AudioManager> ().Pause ("PlayingTheme");
			FindObjectOfType<AudioManager> ().Pause ("PlayingTheme2");
			FindObjectOfType<AudioManager> ().Pause ("PlayingTheme3");
		}*/
		if (PlayingBGM != null)	{
			PausingByButtonBGM = PlayingBGM;
			FindObjectOfType<AudioManager>().Pause(PlayingBGM.name);
		}
		Component[] c = FindObjectsOfType<SpawnWorm> ();
		foreach (SpawnWorm b in c)
			b.Pause ();
		isPause = true;
		BroadcastMessage ("End");
	}

	public void UnPause() {
		/*if (FindObjectOfType<AudioManager> ().IsPause ("MaxComboTheme")) {
			FindObjectOfType<AudioManager> ().UnPause ("MaxComboTheme");
		} else {
			FindObjectOfType<AudioManager> ().UnPause ("PlayingTheme");
			FindObjectOfType<AudioManager> ().UnPause ("PlayingTheme2");
			FindObjectOfType<AudioManager> ().UnPause ("PlayingTheme3");
		}*/
		if (PausingByButtonBGM != null && !isTimeUp && life != 0) {
			FindObjectOfType<AudioManager>().UnPause(PausingByButtonBGM.name);
			PausingByButtonBGM = null;
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
			LeaderText.text = "Player" + PlayerPrefs.GetInt ("PlayerCount");
			isSubmit = true;
		}
	}

	public void ResetGame () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		PlayingBGM = null;
		PausingBGM = null;
		PausingByButtonBGM = null;
	}


	public void Exit () {
		SceneManager.LoadScene ("StartMenu");
	}
}
