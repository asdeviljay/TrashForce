using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToStart : MonoBehaviour {

	public void ClickToStart () {
		FindObjectOfType<AudioManagerTrans>().Play("StartButton");
		SceneManager.LoadScene ("Story");
	}
}
