using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PowerUI;


public class MyTest2Script : MonoBehaviour {

	private GameObject go;

	void Start () {

		go = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		go.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		go.transform.position = new Vector3 (0f, 0f, -0.96f);
		
		var element1=GetComponent<WorldUIHelper> ().WorldUI.document.body.getElementsByClassName("button1");
		foreach (var element in element1) {
			element.ontouchmove = delegate(TouchEvent touchEvent) {
				print("Button clicked ontouchmove");
				//right
				go.transform.position += new Vector3(0.1f, 0, 0);
			};
			element.onmousedown = delegate (MouseEvent mouseEvent) {
				print ("button1 button1 button1");
				go.transform.position += new Vector3(0.1f, 0, 0);
			};
		}

		var element2=GetComponent<WorldUIHelper> ().WorldUI.document.body.getElementsByClassName("dog4");
		foreach (var element in element2) {
			element.ontouchmove = delegate(TouchEvent touchEvent) {
				print("Button clicked ontouchmove");
				// bigger
				go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
			};

			element.onmousedown = delegate (MouseEvent mouseEvent) {
				print ("dog4 dog4 dog4");
				go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

			};
		}

		var element3=GetComponent<WorldUIHelper> ().WorldUI.document.body.getElementsByClassName("bat2");
		foreach (var element in element3) {
			element.ontouchmove = delegate(TouchEvent touchEvent) {
				print("Button clicked ontouchmove");
				// left
				go.transform.position += new Vector3(-0.1f, 0, 0);
			};

			element.onmousedown = delegate (MouseEvent mouseEvent) {
				print ("bat2 bat2 bat2");
				go.transform.position += new Vector3(-0.1f, 0, 0);
			};
		}

		var element4=GetComponent<WorldUIHelper> ().WorldUI.document.body.getElementsByClassName("mouse3");
		foreach (var element in element4) {
			element.ontouchmove = delegate(TouchEvent touchEvent) {
				print("Button clicked ontouchmove");
			};

			element.onmousedown = delegate (MouseEvent mouseEvent) {
				print ("mouse3 mouse3 mouse3");
				// color
				go.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 2), 
					Random.Range(0, 2), Random.Range(0, 2));
			};
		}
	
	}
	

	
}
