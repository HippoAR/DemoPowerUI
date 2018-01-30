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

namespace PowerUI{
	
	/// <summary>
	/// Represents a HTML abbr(eviation) element.
	/// </summary>
	
	[Dom.TagName("a_model")]
	public class HtmlA1Element:HtmlElement{

		public override void OnTagLoaded(){
			string innterName =htmlDocument.innerText;

			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.name = htmlDocument.innerHTML;
			go.transform.position = new Vector3(2f, 0f, 0f);

			Debug.Log ("HtmlA1Element Loaded");
			go.transform.parent = GameObject.Find ("Container").transform;
		}
		
	}
	
}