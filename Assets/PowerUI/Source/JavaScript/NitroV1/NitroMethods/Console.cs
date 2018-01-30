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

using Nitro;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace PowerUI{

	/// <summary>
	/// Wraps around the UnityEngine.Debug class to provide the JS friendly methods.
	/// </summary>

	public static class Console{
		
		private static Dictionary<string,System.Diagnostics.Stopwatch> Watches;
		
		public static void Log(object value){
			Debug.Log(value.ToString());
		}
		
		public static void Assert(bool condition){
			if(!condition){
				throw new Exception("Assertion Failure");
			}
		}
		
		public static void Clear(){
			Debug.ClearDeveloperConsole();
		}
		
		public static void Count(){}
		
		public static void Error(object value){
			Debug.LogError(value.ToString());
		}
		
		public static void Info(object value){
			Debug.Log(value.ToString());
		}
		
		public static void Trace(){
			Debug.Log(Environment.StackTrace);
		}
		
		public static void Warn(object value){
			Debug.LogWarning(value.ToString());
		}
		
		public static void Time(){
			
			if(Watches==null){
				Watches=new Dictionary<string,System.Diagnostics.Stopwatch>();
			}
			
			System.Diagnostics.Stopwatch sw=new System.Diagnostics.Stopwatch();
			
			Watches["std"]=sw;
			
		}
		
		public static void Time(string name){
			
			if(Watches==null){
				Watches=new Dictionary<string,System.Diagnostics.Stopwatch>();
			}
			
			System.Diagnostics.Stopwatch sw=new System.Diagnostics.Stopwatch();
			
			Watches[name]=sw;
			
			sw.Start();
			
		}
		
		public static void TimeEnd(){
			
			System.Diagnostics.Stopwatch sw=Watches["std"];
			sw.Stop();
			
			if(Watches.Count==1){
				Watches=null;
			}else{
				Watches.Remove("std");
			}
			
			Log(sw.ElapsedMilliseconds+"ms");
			
		}
		
		public static void TimeEnd(string name){
			
			System.Diagnostics.Stopwatch sw=Watches[name];
			sw.Stop();
			
			if(Watches.Count==1){
				Watches=null;
			}else{
				Watches.Remove(name);
			}
			
			Log(sw.ElapsedMilliseconds+"ms");
			
		}
		
	}
	
}