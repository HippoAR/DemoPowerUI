using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControlScript : MonoBehaviour {

	bool isMoving = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startMoveTo(Vector3 target, float duration) {
		if (isMoving) {
			return;
		}
		StartCoroutine (moveTo (target, duration));
	}

	IEnumerator moveTo(Vector3 target, float duration) {
		isMoving = true;
		float speed = Vector3.Distance (gameObject.transform.position, target) / duration;
		while (Vector3.Distance(gameObject.transform.position, target) > 0.1f) {
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, target, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
		isMoving = false;
		yield return null;
	}
}
