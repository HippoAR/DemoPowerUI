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

namespace Nitro{
	
	/// <summary>
	/// Represents the modulo into (A%=B) operator.
	/// </summary>
	
	public class OperatorModuloInTo:Operator{
		
		public OperatorModuloInTo():base("%=",26){}
		
		protected override Operation Compile(CompiledFragment left,CompiledFragment right,CompiledMethod method){
			return new ModuloOperation(method,left,new MultiplyOperation(method,left,right));
		}
		
	}
	
}