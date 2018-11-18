using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardName : MonoBehaviour {

	public Text leaderName;
	public Text leaderScore;
	public Text highScore;

	List<string> info = new List<string> ();

	public void ShowName () {
		int count = 1;
		leaderName.text = "";
		leaderScore.text = "";
		if(PlayerPrefs.GetInt ("PlayerCount") >= info.Count) {
			info.RemoveRange (0, info.Count);
			for(int i = 0 ; i < PlayerPrefs.GetInt ("PlayerCount") ; i++)
				info.Add (PlayerPrefs.GetString ("HighScore" + i));
		}
		info.Sort ((x, y) => int.Parse(y.Split(' ')[1]).CompareTo(int.Parse(x.Split(' ')[1])));
		foreach (string str in info) {
			string[] strSplit = str.Split (' ');
			leaderName.text += count + ". " + strSplit [0] + "\n";
			leaderScore.text += strSplit [1] + "\n";
			count++;
		}
	}
}
