#if !NETFX_CORE

//--------------------------------------
//         Nitro Script Engine
//          Dom Framework
//
//        For documentation or 
//    if you have any issues, visit
//         nitro.kulestar.com
//
//    Copyright © 2013 Kulestar Ltd
//          www.kulestar.com
//--------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace PowerUI{

	/// <summary>
	/// Maps PowerUI specific aliases in the Nitro alias map.
	/// E.g. Event to UIEvent.
	/// </summary>

	public static class TypeAliases{
		
		/// <summary>Adds an alias to the global map.</summary>
		/// <param name="alias">The alias string to use.</param>
		/// <param name="forType">The system type it maps to.</param>
		public static void Add(string alias,Type forType){
			Nitro.TypeAliases.Add(alias,forType);
		}
		
		/// <summary>Called when the system is starting to setup the default alias set.</summary>
		public static void Setup(){
			Add("event",typeof(UIEvent));
			Add("htmlnode",typeof(HtmlElement));
			Add("textnode",typeof(HtmlTextNode));
			Add("animation",typeof(UIAnimation));
			
			// Blocks it from trying to use System.Console:
			Add("console",typeof(PowerUI.Console));
			
		}
		
	}
	
}

#endif