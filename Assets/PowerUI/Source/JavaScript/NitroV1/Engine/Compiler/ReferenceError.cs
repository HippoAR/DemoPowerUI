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

namespace Nitro{

	/// <summary>
	/// Represents a reference exception during the compilation of some nitro code.
	/// </summary>

	public class ReferenceError:CompilationException{
		
		/// <summary>Creates a new compilation exception with the given line number the error occured on and a message to show.</summary>
		public ReferenceError(string varName):base("ReferenceError: "+varName+" is not defined."){	
		}
		
	}
	
}