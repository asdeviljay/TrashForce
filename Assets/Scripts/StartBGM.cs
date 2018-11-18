using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBGM : MonoBehaviour {

	void Start () {
		if(!FindObjectOfType<AudioManagerTrans>().IsPlaying("StartBGM"))
		FindObjectOfType<AudioManagerTrans>().Play("StartBGM");
	}

}
