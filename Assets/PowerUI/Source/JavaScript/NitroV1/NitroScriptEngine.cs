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
using UnityEngine;
using Dom;
using Nitro;


namespace PowerUI{
	
	/// <summary>
	/// The default script handler for Nitro.
	/// </summary>
	
	public class NitroScriptEngine : ScriptEngine{
		
		#if !NoNitroRuntime
		/// <summary>The nitro securty domain for this document.</summary>
		private NitroDomainManager SecurityDomain;
		#endif
		
		/// <summary>An instance of the compiled code on this page.
		/// May also be null of there is no script on the page.</summary>
		public UICode CodeInstance;
		
		/// <summary>The meta types that your engine will handle. E.g. "text/javascript".</summary>
		public override string[] GetTypes(){
			return new string[]{"text/nitro"};
		}
		
		public NitroScriptEngine(){
			
			#if !NoNitroRuntime
			// Get the default security domain:
			SecurityDomain=UI.DefaultSecurityDomain;
			#endif
			
		}
		
		/// <summary>Gets or sets script variable values.</summary>
		/// <param name="index">The name of the variable.</param>
		/// <returns>The variable value.</returns>
		public override object this[string global]{
			get{
				if(CodeInstance==null){
					return null;
				}
				
				return CodeInstance[global];
			}
			set{
				if(CodeInstance==null){
					return;
				}
				
				CodeInstance[global]=value;
			}
		}
		
		/// <summary>Runs a nitro function by name with a set of arguments only if the method exists.</summary>
		/// <param name="name">The name of the function in lowercase.</param>
		/// <param name="context">The context to use for the 'this' value.</param>
		/// <param name="args">The set of arguments to use when calling the function.</param>
		/// <param name="optional">True if the method call is optional. No exception is thrown if not found.</param>
		/// <returns>The value that the called function returned, if any.</returns>
		public override object RunLiteral(string name,object context,object[] args,bool optional){
			if(string.IsNullOrEmpty(name)||CodeInstance==null){
				return null;
			}
			CodeInstance.This=context as HtmlElement ;
			return CodeInstance.RunLiteral(name,args,optional);
		}
		
		public override ScriptEngine Instance(Document document){
			
			#if !NoNitroRuntime
			HtmlDocument doc=document as HtmlDocument;
			
			if(doc!=null){
				
				Location location=doc.location;
				Window window=doc.window;
				
				// Iframe security check - can code from this domain run at all?
				// We have the Nitro runtime so it could run unwanted code.
				if(window.parent!=null && location!=null && !location.fullAccess){
					
					// It's an iframe to some unsafe location. We must have a security domain for this to be allowed at all.
					if(SecurityDomain==null || !SecurityDomain.AllowAccess(location.Protocol,location.host,location.ToString())){
						Dom.Log.Add("Warning: blocked Nitro on a webpage - You must use a security domain to allow this. See http://help.kulestar.com/nitro-security/ for more.");
						return null;
					}
					
				}
				
			}
			
			#endif
			
			return new NitroScriptEngine();
		}
		
		protected override void Compile(string codeToCompile){
			
			if(!AotDocument){
				CodeInstance=NitroCache.TryGetScript(codeToCompile);
			}
			
			try{
				#if !NoNitroRuntime
				string aotFile=null;
				string aotAssemblyName=null;
				
				if(AotDocument){
					aotFile="";
					string[] pieces=htmlDocument.ScriptLocation.Split('.');
					for(int i=0;i<pieces.Length-1;i++){
						if(i!=0){
							aotFile+=".";
						}
						aotFile+=pieces[i];
					}
					aotFile+="-nitro-aot.dll";
					aotAssemblyName=NitroCache.GetCodeSeed(codeToCompile)+".ntro";
				}
			
				if(CodeInstance==null){
					
					NitroCode script=new NitroCode(codeToCompile,UI.BaseCodeType,SecurityDomain,aotFile,aotAssemblyName);
					if(AotDocument){
						// Internally the constructor will write it to the named file.
						return;
					}
					CodeInstance=(UICode)script.Instance();
					CodeInstance.BaseScript=script;
				}
				#endif
				
				if(CodeInstance!=null){
					CodeInstance.document=htmlDocument;
					CodeInstance.window=htmlDocument.window;
					
					// Trigger an event to say Nitro is about to start:
					Dom.Event e=new Dom.Event("scriptenginestart");
					htmlDocument.dispatchEvent(e);
					
					
					CodeInstance.OnWindowLoaded();
					CodeInstance.Start();
					// Standard method that must be called.
					// Any code outside of functions gets dropped in here:
					CodeInstance.OnScriptReady();
				}
			}catch(Exception e){
				string scriptLocation=htmlDocument.ScriptLocation;
				
				if(string.IsNullOrEmpty(scriptLocation)){
					// Use document.basepath instead:
					scriptLocation=Document.basepath.ToString();
				}
				
				if(!string.IsNullOrEmpty(scriptLocation)){
					scriptLocation=" (At "+scriptLocation+")";
				}
				
				Dom.Log.Add("Script error"+scriptLocation+": "+e);
			}
			
		}
		
	}
	
}