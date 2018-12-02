using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonSound : MonoBehaviour {

	public void ButtonSound () {
		FindObjectOfType<AudioManagerTrans>().Play("MenuButton");
	}

}
