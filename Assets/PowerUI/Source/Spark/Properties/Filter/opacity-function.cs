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

using System;


namespace Css.Functions{
	
	/// <summary>
	/// Represents the opacity() css function.
	/// </summary>
	
	public class OpacityFunction:FilterFunction{
		
		public OpacityFunction(){
			
			Name="opacity";
			
		}
		
		public override string[] GetNames(){
			return new string[]{"opacity"};
		}
		
		protected override Css.Value Clone(){
			OpacityFunction result=new OpacityFunction();
			result.Values=CopyInnerValues();
			return result;
		}
		
		public override void OnValueReady(CssLexer lexer){
			
		}
		
	}
	
}



