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
	/// Represents the var() css function.
	/// </summary>
	
	public class VarFunction:CssFunction{
		
		/// <summary>The link to the value.</summary>
		public Css.Value Value;
		
		
		public VarFunction(){
			
			Name="var";
			Type=ValueType.Text;
			
		}
		
		public override void OnValueReady(CssLexer lexer){
			
			// Get the var name:
			string name=this[0].Text;
			
			if(name!=null){
				Value=lexer.GetVariable(name.ToLower());
			}
			
		}
		
		public override string[] GetNames(){
			return new string[]{"var"};
		}
		
		protected override Css.Value Clone(){
			VarFunction result=new VarFunction();
			result.Values=CopyInnerValues();
			result.Value=Value;
			return result;
		}
		
		public override string GetText(RenderableData context,CssProperty property){
			if(Value==null){
				return null;
			}
			
			return Value.GetText(context,property);
		}
		
		public override float GetDecimal(RenderableData context,CssProperty property){
			if(Value==null){
				return 0f;
			}
			
			return Value.GetDecimal(context,property);
		}
		
		public override bool GetBoolean(RenderableData context,CssProperty property){
			if(Value==null){
				return false;
			}
			
			return Value.GetBoolean(context,property);
		}
		
	}
	
}



