using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slideTrash : MonoBehaviour {

	//public GameObject gameMeneger;

	//Vector3 mouseDown;
	//Vector3 mouseUp;

	Vector2 dist;
	float posX;
	float posY;
	Vector2 oldPos = Vector2.zero;
	bool onCollision = false;
	bool isClick = false;

	string trashToBin;
	//public float speed = 10.0f;
	public TrashMeneger tm;
	Rigidbody2D rb2d;
	SpriteRenderer sr;
	BoxCollider2D bc2d;
	Vector3 spawnPos;

	void Start () {
		//rb2d = GetComponent<Rigidbody2D> ();	//For SlideTrash Code
		sr = GetComponent<SpriteRenderer> ();
		bc2d = GetComponent<BoxCollider2D> ();
		spawnPos = transform.position;
		RandomTrashPosition ();
		//tm = GetComponentInParent<TrashMeneger> ();
		//t = tm.GetTrashes ();
		int ran = Random.Range (0, tm.GetTrashesLenght ());
		trashToBin = tm.GetBinName (ran);
		sr.sprite = tm.GetTrashImage (ran);
		bc2d.size = sr.size;
		//Debug.Log (trashToBin);
	}

	void Update () {
		if (isClick) {
			bc2d.enabled = false;
			bc2d.isTrigger = false;
		} else {
			bc2d.enabled = true;
			bc2d.isTrigger = true;
		}
		//rb2d.AddForce (new Vector2 (Camera.main.ScreenToWorldPoint (mouseUp).x - Camera.main.ScreenToWorldPoint (mouseDown).x, Camera.main.ScreenToWorldPoint (mouseUp).y- Camera.main.ScreenToWorldPoint (mouseDown).y));
	}

	// DragTrash Code
	void OnMouseDown () {
		isClick = true;
		dist = Camera.main.WorldToScreenPoint (transform.position);
		posX = Input.mousePosition.x - dist.x;
		posY = Input.mousePosition.y - dist.y;
		oldPos = new Vector2(dist.x, dist.y);
		if (!onCollision)
			FindObjectOfType<AudioManager>().PlayOnce("Grabing");
	}

	void OnMouseDrag () {
		Vector2 curPos = new Vector2 (Input.mousePosition.x - posX, Input.mousePosition.y - posY);
		Vector2 worldPos = Camera.main.ScreenToWorldPoint (curPos);
		if (!onCollision)
		{
			transform.position = worldPos;
			if (Vector2.Distance(oldPos, curPos) > 10)
			{
				//Debug.Log("oldPos : " + oldPos);
				//Debug.Log("curPos : " + curPos);
				FindObjectOfType<AudioManager>().PlayOneShot("TrashDrag",0.35f);
				//FindObjectOfType<AudioManager>().PlayOnce("TrashDrag");
				//FindObjectOfType<AudioManager>().Stop("Grabing");
			}
			//FindObjectOfType<AudioManager>().PlayOnce("Grabing");
		}
		oldPos = curPos;
	}

	void OnMouseUp () {
		isClick = false;
		onCollision = false;
		//transform.position = Vector3.zero + spawnPos;
		//RandomTrashPosition ();
		FindObjectOfType<AudioManager>().Stop("Grabing");
	}

	// SlideTrash Code
	/*void OnMouseDown () {
		//Debug.Log ("Click Me");
		mouseDown = Input.mousePosition;
	}

	void OnMouseUp (){
		//Debug.Log ("Out Me");
		mouseUp = Input.mousePosition;
		//Debug.DrawLine (Camera.main.ScreenToWorldPoint (mouseDown), Camera.main.ScreenToWorldPoint (mouseUp), Color.green, 1.0f);
		//transform.Translate (Camera.main.ScreenToWorldPoint (mouseUp) - Camera.main.ScreenToWorldPoint (mouseDown));
		rb2d.velocity += new Vector2 (mouseUp.x - mouseDown.x, mouseUp.y - mouseDown.y).normalized * speed;
		//rb2d.AddTorque (speed);
		//rb2d.MovePosition (new Vector2 (mouseUp.x - mouseDown.x, mouseUp.y - mouseDown.y));
		//rb2d.AddForce (new Vector2 (Camera.main.ScreenToWorldPoint (mouseUp).x - Camera.main.ScreenToWorldPoint (mouseDown).x, Camera.main.ScreenToWorldPoint (mouseUp).y- Camera.main.ScreenToWorldPoint (mouseDown).y));
	}*/

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.layer == 14 && !isClick) {
			//Debug.Log ("Hit Me");
			//onCollision = false;
			//isClick = false;
			if (collision.gameObject.name == "Bin1" && trashToBin == "Danger") {
				Animator c = collision.GetComponentInChildren (typeof(Animator)) as Animator;
				c.SetTrigger ("CorrectTrigger");
				tm.Correct ();
			} else if (collision.gameObject.name == "Bin2" && trashToBin == "Recycle") {
				Animator c = collision.GetComponentInChildren (typeof(Animator)) as Animator;
				c.SetTrigger ("CorrectTrigger");
				tm.Correct ();
			} else if (collision.gameObject.name == "Bin3" && trashToBin == "Organic") {
				Animator c = collision.GetComponentInChildren (typeof(Animator)) as Animator;
				c.SetTrigger ("CorrectTrigger");
				tm.Correct ();
			} else {
				tm.Incorrect ();
				if (collision.gameObject.name == "Bin1") {
					Component[] c = collision.GetComponentsInChildren (typeof(SpriteRenderer));
					StartCoroutine (StartCountdown (2.0f, c [1] as SpriteRenderer));
				} else if (collision.gameObject.name == "Bin2") {
					Component[] c = collision.GetComponentsInChildren (typeof(SpriteRenderer));
					StartCoroutine (StartCountdown (2.0f, c [1] as SpriteRenderer));
				} else if (collision.gameObject.name == "Bin3") {
					Component[] c = collision.GetComponentsInChildren (typeof(SpriteRenderer));
					StartCoroutine (StartCountdown (2.0f, c [1] as SpriteRenderer));
				}
			}
			ResetTrash ();
		} else if (collision.gameObject.layer == 15) {
			RandomTrashPosition ();
		}
	}

	public void ResetTrash () {
		int ran = Random.Range (0, tm.GetTrashesLenght ());
		trashToBin = tm.GetBinName (ran);
		sr.sprite = null;
		sr.sprite = tm.GetTrashImage (ran);
		bc2d.size = sr.size;
		//transform.position = Vector3.zero + Vector3.up * 2;
		spawnPos += Vector3.forward * 0.03f;
		//transform.position = Vector3.zero + spawnPos;
		RandomTrashPosition ();
		//onCollision = false;
		//rb2d.velocity = Vector2.zero;	//For SlideTrash Code
	}

	void End () {
		onCollision = true;
		//rb2d.bodyType = RigidbodyType2D.Static;	//For SlideTrash Code
	}

	void Restart () {
		onCollision = false;
		ResetTrash ();
		//rb2d.bodyType = RigidbodyType2D.Kinematic;	//For SlideTrash Code
		//transform.position = Vector3.zero + spawnPos;
		RandomTrashPosition ();
	}

	public Vector3 GetSpriteSize() {
		return sr.size;
	}

	public bool IsClick() {
		return isClick;
	}

	public bool OnCollision() {
		return onCollision;
	}

	IEnumerator StartCountdown (float timeUp = 2.0f, SpriteRenderer sr = null) {
		float curTimeUp = timeUp;
		sr.color = new Color (1, 1, 1, 1);
		while (curTimeUp > 0) {
			yield return new WaitForSecondsRealtime (1.0f);
			curTimeUp--;
		}
		sr.color = new Color (1, 1, 1, 0);
	}

	public void RandomTrashPosition () {
		transform.position = (Random.onUnitSphere * 1.5f) + spawnPos;
	}

	public Vector3 GetPosition () {
		return transform.position;
	}
}