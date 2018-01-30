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
using System.Reflection.Emit;
using System.Collections.Generic;

namespace Nitro{
	
	/// <summary>
	/// Represents a delegate call operation.
	/// </summary>
	
	public class DelegateOperation:Operation{
	
		/// <summary>The type which is a delegate type.</summary>
		public Type DelegateType;
		/// <summary>The method being used as a delegate.</summary>
		public MethodInfo ToDelegate;
		
		
		public DelegateOperation(CompiledMethod method,MethodInfo methodToDelegate,Type delegateType):base(method){
			ToDelegate=methodToDelegate;
			DelegateType=delegateType;
		}
		
		public override Type OutputType(out CompiledFragment newOperation){
			newOperation=this;
			
			// No need to call ToDelegate.OutputType(out ToDelegate) here; it's certainly already been done.
			
			return DelegateType;
		}
		
		public override void OutputIL(NitroIL into){
			
			// Two args for the create call:
			// - Note that the first is the scope/ variable set
			into.Emit(OpCodes.Ldnull);
			into.Emit(OpCodes.Ldftn,ToDelegate);
			
			// Get the constructor:
			ConstructorInfo ctr=DelegateType.GetConstructors()[0];
			
			// Create the delegate:
			into.Emit(OpCodes.Newobj,ctr);
			
		}
		
	}
	
}