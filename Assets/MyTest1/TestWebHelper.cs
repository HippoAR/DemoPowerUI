using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUI;

public class TestWebHelper : MonoBehaviour {

	[SerializeField]
	private WorldUIHelper hepler;

	// Use this for initialization
	void Start () {
		hepler.Url = "http://powerui.kulestar.com/";
		hepler.OnEnable ();
	}
	
	// Update is called once per frame
	void Update () {
		if (UnityEngine.Input.GetKeyDown (KeyCode.B)) {
			StartCoroutine (ReloadUrlCoroutine());
		}
	}


	private IEnumerator ReloadUrlCoroutine() {
		yield return null;
		hepler.Reload ("http://heran.me/research-of-seo/something-of-textlinks/");
		yield return new WaitForSeconds (2f); // 加载延迟两秒后才能再次点击

	}
}
