using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawn : MonoBehaviour {

	Animator ratAnimation;

	float curSpawnTime;
	bool isPause = false;
	bool isEnd = false;

	// Use this for initialization
	void Start () {
		ratAnimation = GetComponent<Animator> ();
		StartCoroutine (StartSpawn (Random.Range (40.0f, 50.0f)));
	}

	IEnumerator StartSpawn (float timeToSapwn) {
		curSpawnTime = timeToSapwn;
		while (curSpawnTime > 0) {
			if (isPause) {
				yield return null;
			} else {
				yield return new WaitForSecondsRealtime (0.1f);
				curSpawnTime -= 0.1f;
			}
		}
		if (!isEnd)
		{
			ratAnimation.SetTrigger("RatSpawn");
			FindObjectOfType<AudioManager>().Play("Rat");
		}
	}

	public void Pause () {
		GetComponent<BoxCollider2D> ().enabled = false;
		isPause = true;
	}

	public void UnPause () {
		GetComponent<BoxCollider2D> ().enabled = true;
		isPause = false;
	}

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.layer == 15) 
			collision.GetComponent<slideTrash> ().ResetTrash ();
	}

	public void End () {
		isEnd = true;
		curSpawnTime = 0;
	}

	public void NewGame () {
		isEnd = false;
		StartCoroutine (StartSpawn (Random.Range (40.0f, 50.0f)));
	}
}
