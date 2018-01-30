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
using Nitro;

namespace PowerUI{

	/// <summary>
	/// A delegate used by UI.OnGetSecurityDomain. Set to provide your own default domain.
	/// </summary>
	public delegate NitroDomainManager GetSecurityDomain();
	
	/// <summary>
	/// The default nitro script security domain manager.
	/// <see cref="UI.Start(UIScriptDomainManager)"/> for passing in a custom domain manager to extend or override this one.
	/// </summary>

	public class UIScriptDomainManager:NitroDomainManager{
		
		public UIScriptDomainManager(){
			// Anything in the UI is ok:
			AddReference(".PowerUI");
			// Any http is also ok:
			AddReference(".PowerUI.Http");
			// First dot tells it to use 'this' assembly and the PowerUI namespace.
			// Any Css is ok:
			AddReference(".Css");
			// JSON too:
			AddReference(".Json");
			// Dom:
			AddReference(".Dom");
			// Windows and PowerSlide:
			AddReference(".Windows");
			AddReference(".PowerSlide");
			// Include the Unity classes by default:
			AddReference("UnityEngine.UnityEngine");
			// Include collections by default:
			AddReference("mscorlib.System.Collections");
			// Include generic classes by default:
			AddReference("mscorlib.System.Collections.Generic");
			
			AllowEverything();
		}
		
	}

}