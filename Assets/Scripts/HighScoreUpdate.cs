using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUpdate : MonoBehaviour {

	Text highScore;

	List<string> info = new List<string> ();

	// Use this for initialization
	void Start () {
		highScore = GetComponent<Text> ();
		for(int i = 0 ; i < PlayerPrefs.GetInt ("PlayerCount") ; i++)
			if (PlayerPrefs.HasKey ("HighScore" + i))
				info.Add (PlayerPrefs.GetString ("HighScore" + i));
		info.Sort ((x, y) => int.Parse(y.Split(' ')[1]).CompareTo(int.Parse(x.Split(' ')[1])));
		highScore.text = info [0].Split(' ')[1];
	}

}
