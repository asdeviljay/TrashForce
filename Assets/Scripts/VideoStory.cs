using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoStory : MonoBehaviour {

	VideoPlayer vp;

	void Start () {
		vp = GetComponent<VideoPlayer> ();
		vp.Play ();
		vp.loopPointReached += OnVideoEnd;
	}

	void OnVideoEnd (VideoPlayer v) {
		SceneManager.LoadScene ("StartMenu");
	}

	public void SkipVideo () {
		SceneManager.LoadScene ("StartMenu");
	}
}
