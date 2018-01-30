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
using System.Reflection;
using System.Collections;
using System.Reflection.Emit;
using System.Collections.Generic;
using Dom;

namespace Nitro{
	
	/// <summary>
	/// Represents a new object construct. {..}.
	/// </summary>
	
	public class ObjectFragment:CodeFragment{
	
		/// <summary>The content of the object.</summary>
		public BracketFragment Contents;
		
		
		public ObjectFragment(BracketFragment contents){
			Contents=contents;
			
			// Add them as children such that code tree iterators can visit them:
			AddChild(Contents);
			
		}
		
		public override CompiledFragment Compile(CompiledMethod parent){
			
			// For each value in the brackets..
			
			return null;//return new ObjectOperation(parent,null,CompilationServices.CompileParameters(Defaults,parent));
		}
		
		public override string ToString(){
			return Contents.ToString();
		}
		
	}
	
}