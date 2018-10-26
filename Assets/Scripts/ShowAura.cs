using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAura : MonoBehaviour {

	public slideTrash CompoTrash;
	Sprite storeImage;
	SpriteRenderer curSprite;
	public ParticleSystem ps;

	// Use this for initialization
	void Start () {
		//CompoTrash = GetComponentInParent<slideTrash> ();
		curSprite = GetComponent<SpriteRenderer> ();
		storeImage = curSprite.sprite;
	}
	
	// Update is called once per frame
	void Update () {
		if (CompoTrash.IsClick () && !CompoTrash.OnCollision()) {
			Vector2 cts = CompoTrash.GetSpriteSize ();
			curSprite.sprite = storeImage;
			curSprite.size = new Vector2 (cts.x * 1.05f, cts.y * 1.05f);
			if (ps.isStopped)
				ps.Play ();
		} else {
			curSprite.sprite = null;
			ps.Stop ();
		}
	}
}
