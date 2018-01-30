using System;
using UnityEngine;

namespace Loonim{

	public class Sepia : Std1InputNode{
		
		public Sepia(){}
		
		public Sepia(TextureNode src){
			SourceModule=src;
		}
		
		public override UnityEngine.Color GetColour(double x,double y){
			
			UnityEngine.Color col1=SourceModule.GetColour(x,y);
			
			return new Color(
				(col1.r * .393f) + (col1.g *.769f) + (col1.b * .189f),
				(col1.r * .349f) + (col1.g *.686f) + (col1.b * .168f),
				(col1.r * .272f) + (col1.g *.534f) + (col1.b * .131f),
				col1.a
			);
			
		}
		
		public override double GetWrapped(double x, double y, int wrap){
			if(SourceModule == null){
				return 0;
			}
			
			return SourceModule.GetWrapped(x,y,wrap);
			
		}
		
		public override double GetValue(double x, double y, double z){
			if(SourceModule == null){
				return 0;
			}
			
			return SourceModule.GetValue(x,y,z);
			
		}
		
		public override double GetValue(double x, double y)
		{
			if(SourceModule == null){
				return 0;
			}
			
			return SourceModule.GetValue(x,y);
			
		}	  
		
		public override int TypeID{
			get{
				return 112;
			}
		}
		
	}
	
}
