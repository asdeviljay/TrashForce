using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slideTrash : MonoBehaviour {

	//public GameObject gameMeneger;

	Vector3 mouseDown;
	Vector3 mouseUp;
	string trashToBin;
	public float speed = 10.0f;
	public TrashMeneger tm;
	Rigidbody2D rb2d;
	SpriteRenderer sr;
	BoxCollider2D bc2d;
	Vector3 spawnPos;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		bc2d = GetComponent<BoxCollider2D> ();
		spawnPos = transform.position;
		//tm = GetComponentInParent<TrashMeneger> ();
		//t = tm.GetTrashes ();
		int ran = Random.Range (0, tm.GetTrashesLenght ());
		trashToBin = tm.GetBinName (ran);
		sr.sprite = tm.GetTrashImage (ran);
		bc2d.size = sr.size;
		//Debug.Log (trashToBin);
	}

	void Update () {
		//rb2d.AddForce (new Vector2 (Camera.main.ScreenToWorldPoint (mouseUp).x - Camera.main.ScreenToWorldPoint (mouseDown).x, Camera.main.ScreenToWorldPoint (mouseUp).y- Camera.main.ScreenToWorldPoint (mouseDown).y));
	}

	void OnMouseDown () {
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
	}

	void OnTriggerEnter2D (Collider2D collision) {
		//Debug.Log ("Hit Me");
		if (collision.gameObject.name == "Bin1" && trashToBin == "Recycle") {
			tm.Correct ();
			ResetTrash ();
		} else if (collision.gameObject.name == "Bin2" && trashToBin == "Danger") {
			tm.Correct ();
			ResetTrash ();
		} else if (collision.gameObject.name == "Bin3" && trashToBin == "Organic") {
			tm.Correct ();
			ResetTrash ();
		} else if (collision.gameObject.name == "Edge") {
			tm.Incorrect ();
			ResetTrash ();
		}
	}

	void ResetTrash () {
		int ran = Random.Range (0, tm.GetTrashesLenght ());
		trashToBin = tm.GetBinName (ran);
		sr.sprite = tm.GetTrashImage (ran);
		bc2d.size = sr.size;
		//transform.position = Vector3.zero + Vector3.up * 2;
		spawnPos += Vector3.forward * 0.03f;
		transform.position = Vector3.zero + spawnPos;
		rb2d.velocity = Vector2.zero;
	}

	void End () {
		//gameObject.SetActive (false);
		//rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
		rb2d.bodyType = RigidbodyType2D.Static;
	}

	void Restart () {
		//gameObject.SetActive (true);
		//rb2d.constraints = RigidbodyConstraints2D.None;
		rb2d.bodyType = RigidbodyType2D.Kinematic;
		transform.position = Vector3.zero + spawnPos;
	}
}
