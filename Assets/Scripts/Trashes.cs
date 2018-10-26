using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashes : MonoBehaviour {

	[System.Serializable]
	public class TrashInfo {
		public string trashItem;
		public string trashToBin;
		public Sprite trashImage;
	}
	public TrashInfo[] trashInfo;
	List<TrashInfo> trashList;

	void Start () {
		trashList = new List<TrashInfo> ();
		for (int i = 0; i < trashInfo.Length; i++) {
			trashList.Add (trashInfo[i]);
		}
	}

	public int GetLenght () {
		return trashInfo.Length;
	}

	public string GetBinToDump (int index) {
		return trashList[index].trashToBin;
	}

	public Sprite GetTrashImage (int index) {
		return trashList[index].trashImage;
	}
}
