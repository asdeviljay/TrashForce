using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoTutorial : MonoBehaviour {

	VideoPlayer vp;

	void Start () {
		vp = GetComponent<VideoPlayer> ();
		vp.Play ();
		vp.loopPointReached += OnVideoEnd;
	}

	void OnVideoEnd (VideoPlayer v) {
		SceneManager.LoadScene ("MainGame");
	}

	public void SkipVideo () {
		SceneManager.LoadScene ("MainGame");
	}
}
