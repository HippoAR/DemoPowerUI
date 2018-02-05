using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControlScript : MonoBehaviour {

	bool isMoving = false;
	bool isRotating = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (isRotating) {
			gameObject.transform.RotateAround (gameObject.transform.position, gameObject.transform.up, 100f * Time.deltaTime);
		}
	}

	public void startMoveTo(Vector3 target, float duration) {
		if (isMoving)
			return;
		StartCoroutine (moveTo (target, duration));
	}

	public void startRotate() {
		isRotating = !isRotating;
		//StartCoroutine(rotateTo(eulerAngle, duration));
	}

	IEnumerator moveTo(Vector3 target, float duration) {
		isMoving = true;
		float speed = Vector3.Distance (gameObject.transform.position, target) / duration;
		while (Vector3.Distance(gameObject.transform.position, target) > 0.1f) {
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, target, speed * Time.deltaTime);
			yield return null;
		}
		isMoving = false;
		yield return null;
	}

	IEnumerator rotateTo(Vector3 eulerAngle, float duration) {
		isRotating = true;
		float speed = (eulerAngle.y - gameObject.transform.eulerAngles.y) / duration;
		Debug.Log ("rotate speed: " + speed.ToString());
		while (Mathf.Abs(gameObject.transform.eulerAngles.y - eulerAngle.y) > 0.1f) {
			Debug.Log ("to Y: " + eulerAngle.y.ToString() + "  now Y: " + gameObject.transform.eulerAngles.y.ToString());
			gameObject.transform.eulerAngles = Vector3.Lerp (gameObject.transform.eulerAngles, eulerAngle, 5f * Time.deltaTime);
			yield return null;
		}
		isRotating = false;
		yield return null;
	}
}
