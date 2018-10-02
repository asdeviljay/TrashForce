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
	int score = 0;
	int life = 3;
	public Trashes t;
	public Text timeUpText;
	public Text comboTimeText;
	public Text comboTimeText2;
	public Text scoreText;
	public float addComboTime = 1.0f;
	public GameObject[] lifeImage;

	// Use this for initialization
	void Start () {
		StartCoroutine (StartCountdown ());
	}
	
	// Update is called once per frame
	void Update () {
		if (isTimeUp || life == 0) {
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
			lifeImage [0].SetActive (false);
		else if (life == 1)
			lifeImage [1].SetActive (false);
		else if (life == 0)
			lifeImage [2].SetActive (false);
		else 
			for (int i = 0 ; i < lifeImage.Length ; i++)
				lifeImage [i].SetActive (true);
	}

	IEnumerator StartCountdown (float timeUp = 60.0f) {
		curTimeUp = timeUp;
		while (curTimeUp > 0 && !isTimeUp) {
			timeUpText.text = curTimeUp.ToString();
			yield return new WaitForSecondsRealtime (1.0f);
			curTimeUp--;
		}
		timeUpText.text = curTimeUp.ToString();
		isTimeUp = true;
	}

	IEnumerator StartCombo (float addComboTime) {
		curComboTime += addComboTime;
		while (curComboTime > 0) {
			comboTimeText.text = curComboTime.ToString();
			yield return new WaitForSecondsRealtime (0.1f);
			curComboTime -= 0.1f;
		}
		countToMul = 0;
		scoreMul = 1;
		curComboTime = 2.0f;
		isCombo = false;
	}

	void AddAndCheckToMul () {
		score += scoreMul;
		countToMul++;
		curComboTime += addComboTime;
		if (isCombo) {
			if (countToMul < 2)
				scoreMul = 1;
			else if (countToMul < 4)
				scoreMul = 2;
			else if (countToMul < 8)
				scoreMul = 3;
			else if (countToMul < 12)
				scoreMul = 4;
			else if (countToMul < 16)
				scoreMul = 5;
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
	}

	public void Incorrect () {
		isCombo = false;
		curComboTime = 0.0f;
		countToMul = 0;
		scoreMul = 1;
		life--;
	}

	public void NewGame () {
		BroadcastMessage ("Restart");
		score = 0;
		scoreText.text = score.ToString();
		life = 3;
		curComboTime = 2.0f;
		countToMul = 0;
		scoreMul = 1;
		isTimeUp = false;
		isCombo = false;
		StartCoroutine (StartCountdown ());
	}
}
