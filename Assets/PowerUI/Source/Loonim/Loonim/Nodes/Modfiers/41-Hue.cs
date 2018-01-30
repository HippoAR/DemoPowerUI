using System;
using UnityEngine;

namespace Loonim{

	public class Hue : Std2InputNode{
		
		public Hue(){}
		
		public Hue(TextureNode src,TextureNode hueChange){
			Sources[0]=src;
			Sources[1]=hueChange;
		}
		
		public TextureNode HueModule{
			get{
				return Sources[1];
			}
			set{
				Sources[1]=value;
			}
		}
		
		public override UnityEngine.Color GetColour(double x,double y){
			
			UnityEngine.Color col1=SourceModule.GetColour(x,y);
			float hueChange=(float)HueModule.GetValue(x,y);
			
			// Read hsl:
			float h=col1.r;
			float s=col1.g;
			float l=col1.b;
			HslRgb.ToHsl(ref h,ref s,ref l);
			
			// Boost hue:
			h*=(1f+hueChange);
			
			// Back to colour:
			HslRgb.ToRgb(ref h,ref s,ref l);
			
			// Now RGB:
			col1.r=h;
			col1.g=s;
			col1.b=l;
			
			return col1;
			
		}
		
		public override double GetWrapped(double x, double y, int wrap){
			if(SourceModule == null){
				return 0;
			}
			
			double baseValue=SourceModule.GetWrapped(x,y,wrap);
			double hue=1f+HueModule.GetWrapped(x,y,wrap);
			
			return baseValue * hue;
			
		}
		
		public override double GetValue(double x, double y, double z){
			if(SourceModule == null){
				return 0;
			}
			
			double baseValue=SourceModule.GetValue(x,y,z);
			double hue=1f+HueModule.GetValue(x,y,z);
			
			return baseValue * hue;
			
		}
		
		public override double GetValue(double x, double y)
		{
			if(SourceModule == null){
				return 0;
			}
			
			double baseValue=SourceModule.GetValue(x,y);
			double hue=1f+HueModule.GetValue(x,y);
			
			return baseValue * hue;
			
		}	  
		
		public override int TypeID{
			get{
				return 41;
			}
		}
		
	}
	
}
