//--------------------------------------
//               PowerUI
//
//        For documentation or 
//    if you have any issues, visit
//        powerUI.kulestar.com
//
//    Copyright © 2013 Kulestar Ltd
//          www.kulestar.com
//--------------------------------------
using Dom;
using UnityEngine;
using System.Collections;

namespace PowerUI{
	
	/// <summary>
	/// Represents a HTML abbr(eviation) element.
	/// </summary>
	
	[Dom.TagName("a_model")]
	public class HtmlA1Element:HtmlElement{

		GameObject go;

		string goName = "phantom";

		ModelControlScript modelControl;

		public override void OnTagLoaded(){
			string innterName =htmlDocument.innerText;

			//go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go = GameObject.Instantiate (Resources.Load (goName)) as GameObject;
			go.name = goName;
			go.layer = LayerMask.NameToLayer ("PowerUI");
			modelControl = go.AddComponent<ModelControlScript> ();
			go.transform.parent = GameObject.Find ("ModelContainer").transform;
			Debug.Log ("HtmlA1Element Loaded");

		}

		public override void move(float x, float y, float z, float duration) {
			if (go == null)
				return;
			Vector3 targetPos = new Vector3(x, y, z);
			modelControl.startMoveTo (targetPos, duration);
			Debug.Log ("a_model move");
		}

		public override void changeColor(float r, float g, float b, float a) {
			if (go == null)
				return;
			// ????0~1
			Color color = new Color (r, g, b, a);
			go.GetComponent<MeshRenderer> ().material.color = color;
		}

		public override void scale(float factor) {
			if (go == null)
				return;
			go.transform.localScale = go.transform.localScale * factor;
		}
	}


	public partial class HtmlElement{

		public virtual void move(float x, float y, float z, float duration) {

		}

		public virtual void changeColor(float r, float g, float b, float a) {
			
		}

		public virtual void scale(float factor) {

		}

		public float getRandomValue() {
			return Random.value;
		}
	}
	
}