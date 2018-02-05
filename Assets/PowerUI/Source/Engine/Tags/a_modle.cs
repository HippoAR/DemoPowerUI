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
using System.Collections.Generic;

namespace PowerUI{
	
	/// <summary>
	/// Represents a HTML abbr(eviation) element.
	/// </summary>
	
	[Dom.TagName("a_model")]
	public class HtmlA1Element:HtmlElement{

		GameObject go;

		string goName = "micro_knight"; // "phantom";

		ModelControlScript modelControl;

		string[] animArray = {"Attack", "Block", "Die", "GetHitFront", "GetHitLeft", "GetHitRight", "IdleCombat", "Laugh", "Panic", "Talk", "Walk"};

		public override void OnTagLoaded(){
			string innterName =htmlDocument.innerText;

			//go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go = GameObject.Instantiate (Resources.Load (goName)) as GameObject;
			go.name = goName;
			ChangeLayer (go, "PowerUI");
			modelControl = go.AddComponent<ModelControlScript> ();
			go.transform.parent = GameObject.Find ("ModelContainer").transform;

		}

		public override void move(float x, float y, float z, float duration) {
			if (go == null)
				return;
			Vector3 targetPos = new Vector3(x, y, z);
			modelControl.startMoveTo (targetPos, duration);
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

		private void ChangeLayer(GameObject obj, string layerName) {
			for (int i = 0; i < obj.transform.childCount; i++) {
				obj.transform.GetChild (i).gameObject.layer = LayerMask.NameToLayer (layerName);
			}
		}

		public void playAnimation(string name) {
			if (name == "") {
				name = animArray[Random.Range (0, animArray.Length)];
			}
			go.GetComponent<Animator> ().Play (name, -1, 0f);
		}

		public void rotate() {
			Debug.Log ("start rotate");
			modelControl.startRotate ();
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