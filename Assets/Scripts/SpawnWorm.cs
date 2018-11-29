using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWorm : MonoBehaviour {

	public BoxCollider2D bc2d;
	public SpriteRenderer sr;

	Animator wormAnimation;
	bool isWormSpawn = false;
	bool isPause = false;
	float curSpawnTime = 0;

	// Use this for initialization
	void Start () {
		wormAnimation = GetComponent (typeof(Animator)) as Animator;
		//bc2d = GetComponentInParent<BoxCollider2D> ();
		StartCoroutine (StartSpawn (Random.Range (10.0f, 20.0f)));
	}

	// Update is called once per frame
	void Update () {
		WormSpawn ();
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
		bc2d.enabled = false;
		bc2d.isTrigger = false;
		isWormSpawn = true;
	}

	void OnMouseDown () {
		if (isWormSpawn)
			FindObjectOfType<AudioManager>().Play("Worm");
		bc2d.enabled = true;
		bc2d.isTrigger = true;
		isWormSpawn = false;
		wormAnimation.SetBool ("IsSpawn", false);
		sr.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		StartCoroutine (StartSpawn (Random.Range (10.0f, 30.0f)));
	}

	void WormSpawn () {
		if (isWormSpawn) {
			wormAnimation.SetBool ("IsSpawn", true);
			sr.color = new Color (0.25f, 0.25f, 0.25f, 1.0f);
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

	public void End () {
		curSpawnTime = 0;
		isWormSpawn = false;
		bc2d.enabled = true;
		bc2d.isTrigger = true;
		wormAnimation.SetBool ("IsSpawn", false);
		sr.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	}

	public void NewGame () {
		StartCoroutine (StartSpawn (Random.Range (10.0f, 20.0f)));
	}
}
