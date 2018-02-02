using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PowerUI;


public class MyTest2Script : MonoBehaviour {

	private GameObject go;

	private HtmlDocument htmlDocument;

	void Start () {

		go = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		go.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		go.transform.position = new Vector3 (0f, 0f, -0.96f);

		htmlDocument = GetComponent<WorldUIHelper> ().WorldUI.document;
		
//		var element1=GetComponent<WorldUIHelper> ().WorldUI.document.body.getElementsByClassName("button1");
//		foreach (var element in element1) {
//			element.ontouchmove = delegate(TouchEvent touchEvent) {
//				print("Button clicked ontouchmove");
//				//right
//				go.transform.position += new Vector3(0.1f, 0, 0);
//			};
//			element.onmousedown = delegate (MouseEvent mouseEvent) {
//				print ("button1 button1 button1");
//				go.transform.position += new Vector3(0.1f, 0, 0);
//			};
//		}

		var ele_left = htmlDocument.body.getElementById ("btn_left");
		ele_left.onmousedown = delegate (MouseEvent mouseEvent) {
			print ("btn_left btn_left btn_left");
			go.transform.position += new Vector3(-0.1f, 0, 0);
		};

		var ele_right = htmlDocument.body.getElementById ("btn_right");
		ele_right.onmousedown = delegate (MouseEvent mouseEvent) {
			print ("btn_right btn_right btn_right");
			go.transform.position += new Vector3(0.1f, 0, 0);
		};

		var ele_color = htmlDocument.body.getElementByName ("btn_color");
		ele_color.onmousedown = delegate (MouseEvent mouseEvent) {
			print ("btn_color btn_color btn_color");
			go.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 2), 
				Random.Range(0, 2), Random.Range(0, 2));
		};

		var ele_bigger = htmlDocument.body.getElementByName ("btn_bigger");
		ele_bigger.onmousedown = delegate (MouseEvent mouseEvent) {
			print ("btn_bigger btn_bigger btn_bigger");
			go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		};
		ele_bigger.onmouseup = OnElementMouseUp;
			
	}

	private void OnElementMouseUp(MouseEvent mouseEvent){

		// mouseEvent.target is the element that actually got clicked.
		// Note that it could be e.g an SVG element. So, htmlTarget
		// is it cast as a HtmlElement.
		mouseEvent.htmlTarget.innerHTML="You clicked it!";

	}

}
