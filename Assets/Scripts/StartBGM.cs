using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBGM : MonoBehaviour {

	void Start () {
		if(!FindObjectOfType<AudioManager>().IsPlaying("StartBGM"))
		FindObjectOfType<AudioManager>().Play("StartBGM");
	}

}
