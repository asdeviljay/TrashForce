using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePositioningandScaling : MonoBehaviour {

	public GameObject[] gameObj;


	// Use this for initialization
	void Start () {
		foreach (GameObject obj in gameObj) {
			/*float originalPositionXAt1280x720 = obj.transform.position.x;
			float originalPositionYAt1280x720 = obj.transform.position.y;
			float originalPositionZAt1280x720 = obj.transform.position.z;*/
			float originalScaleXAt1280x720 = obj.transform.localScale.x;
			//float originalScaleYAt1280x720 = obj.transform.localScale.y;

			//Debug.Log (Camera.main.aspect);
			//obj.transform.position = new Vector3 (originalPositionXAt1280x720 * Screen.width / 1280.0f, originalPositionYAt1280x720 * Screen.height / 720.0f, originalPositionZAt1280x720);
			//obj.transform.localScale = new Vector3 (originalScaleXAt1280x720 * Screen.width / 1280.0f, originalScaleYAt1280x720 * Screen.height / 720.0f, 1);
			/*float height = Camera.main.orthographicSize * 2;
			float width = height * Screen.width / Screen.height;
			obj.transform.position = Vector3.one * width / 5.0f;
			obj.transform.localScale = Vector3.one * width / 5.0f;*/
			float curAspect = Camera.main.aspect;
			float originalAspect = 1280.0f / 720.0f;
			//obj.transform.localScale = new Vector3 (originalScaleXAt1280x720 * curAspect / originalAspect, originalScaleYAt1280x720 * curAspect / originalAspect, 1);
			obj.transform.localScale = new Vector3 (originalScaleXAt1280x720 * curAspect / originalAspect, 1, 1);
		}
	}
}
