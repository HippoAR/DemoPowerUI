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
	/// Represents the a property get/set (Of.property) operation.
	/// </summary>
	
	public class PropertyOperation:Operation,ISettable{
		
		/// <summary>The name of the property. Always lowercase.</summary>
		public string Name;
		/// <summary>True if this property is a static one.</summary>
		public bool IsStatic;
		/// <summary>If this is a field, the FieldInfo that represents it.</summary>
		public FieldInfo Field;
		/// <summary>The fragment/object this is a property of. "Of.property".</summary>
		public CompiledFragment Of;
		/// <summary>If this is a property, the PropertyInfo that represents it.</summary>
		public PropertyInfo Property;
		/// <summary>If this is a method, the type it returns.</summary>
		public Type MethodReturnType;
		/// <summary>If this is an extension method, the overloaded set of methods.</summary>
		public List<MethodInfo> Methods;
		
		
		public PropertyOperation(CompiledMethod method,string name):this(method,new ThisOperation(method),name){}
		
		public PropertyOperation(CompiledMethod method,CompiledFragment of,string name):base(method){
			if(of!=null){
				of.ParentFragment=this;
			}
			Of=(of!=null)?of:new ThisOperation(method);
			Name=name;	
		}
		
		public override bool IsMemberAccessor(){
			return true;
		}
		
		/// <summary>Gets the type of the object that this is a property of.</summary>
		public Type OfType(){
			Type type=Of.OutputType(out Of);
			Of.ParentFragment=this;
			// Static if the object this is a property of is a type.
			IsStatic=(Of.GetType()==typeof(TypeOperation));
			
			if(type==null){
				Error("Unable to determine type of something. This is required to access the property '"+Name+"' on it.");
			}
			
			if(IsStatic){
				return ((TypeOperation)Of).TypeObject;
			}
			
			return type;
		}
		
		/// <summary>Finds extension methods for this method and the given type. Their output to the 'Methods' set 
		// if more than one overload is found.</summary>
		public MethodInfo FindExtensionMethod(Type type,string alternative){
			
			// E.g. GetSomething or SetSomething
			string getSetName=alternative+Name;
			
			// Try the extension type(s) instead:
			List<Type> extensionTypes=Method.Script.GetTypes(type.Name+"Extensions");
			
			List<MethodInfo> results=null;
			
			if(extensionTypes!=null){
				
				foreach(Type extensionType in extensionTypes){
					
					// Get the methods:
					MethodInfo[] methods=extensionType.GetMethods();
					
					// Looking for  either be Name or [Alternative]Name.
					
					for(int i=0;i<methods.Length;i++){
						
						// Get the current method:
						MethodInfo current=methods[i];
						
						// LC name:
						string lowerName=current.Name.ToLower();
						
						// Match?
						if(lowerName==Name || lowerName==getSetName){
							
							// Yep! e.g. 'Something' or 'Set/GetSomething'
							
							// Set of overloads.
							if(results==null){
								
								results=new List<MethodInfo>();
								
							}
							
							results.Add(current);
							
						}
						
					}
					
				}
				
			}
			
			// Apply the methods set:
			Methods=results;
			
			if(results==null){
				
				// It doesn't exist!
				return null;
				
			}
			
			// It's static:
			IsStatic=true;
			
			return results[0];
			
		}
		
		public override Type OutputType(out CompiledFragment v){
			v=this;
			
			Type type=OfType();
			
			// Map to functionality:
			CompiledClass Class=null;
			bool isDynamic=Types.IsDynamic(type);
			
			// (Constant) binding flags:
			BindingFlags flags=BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
			
			if(Name=="length" && type.IsGenericType && !isDynamic){
				// Does length actually exist as a field/ property?
				
				Field=type.GetField(Name,flags);
				
				if(Field==null){
					Property=type.GetProperty(Name,flags);
					
					if(Property==null){
						// Assume we meant count instead:
						Name="Count";
					}
				}
			}
			
			if(isDynamic){
				Class=Method.Script.GetClass(type);
			}else if(!Method.Script.AllowUse(type)){
				Error("Unable to access properties of type "+type+" as it has not been made accessible.");
			}
			
			if(isDynamic){
				Field=Class.GetField(Name);
			}else{
				Field=type.GetField(Name,flags);
			}
			
			if(Field!=null){
				if(IsStatic&&!Field.IsStatic){
					Error("Property "+Name+" is not static. You must use an object reference to access it.");
				}
				
				return Field.FieldType;
			}
			
			if(isDynamic){
				Property=Class.GetProperty(Name);
			}else{
				Property=type.GetProperty(Name,flags);
			}
			
			if(Property!=null){
				if(IsStatic){
					MethodInfo staticTest=Property.GetGetMethod();
					if(staticTest==null){
						staticTest=Property.GetSetMethod();
					}
					if(!staticTest.IsStatic){
						Error("Property "+Name+" is not static. You must use an object reference to access it.");
					}
				}
				return Property.PropertyType;
			}
			
			if(isDynamic){
				MethodReturnType=Class.MethodReturnType(Name);
			}else{
				MethodReturnType=Types.MethodReturnType(type,Name);
			}
			
			if(MethodReturnType!=null){
				if(Types.IsVoid(MethodReturnType)){
					MethodReturnType=typeof(Void);
				}
				return DynamicMethodCompiler.TypeFor(MethodReturnType);
			}
			
			if(Of.GetType()==typeof(ThisOperation)){
				// This was the first property - it can potentially be a static type name too.
				Type staticType=Method.Script.GetType(Name);
				if(staticType!=null){
					// It's a static type! Generate a new type operation to replace this one and return the type.
					v=new TypeOperation(Method,staticType);
					return v.OutputType(out v);
				}
			}
			
			if(Name=="this"){
				// This is handled here as it allows variables called "This". Use case: PowerUI.
				v=new ThisOperation(Method);
				return v.OutputType(out v);
			}
			
			// Does it support indexing? If so, Do Parent["property"] instead.
			
			MethodOperation mOp=null;
			
			if(Input0!=null){
				// This is a set. Input0 is the object we're setting.
				
				Type setType=Input0.OutputType(out Input0);
				
				// Get the set method:
				MethodInfo mInfo;
				
				if(isDynamic){
					mInfo=Class.FindMethodOverload("set_Item",new Type[]{typeof(string),setType});
				}else{
					// Grab all the methods of the type:
					MethodInfo[] allMethods=type.GetMethods();
					mInfo=Types.GetOverload(allMethods,"set_Item",new Type[]{typeof(string),setType});
				}
				
				if(mInfo==null){
					
					// Try finding the extension method:
					mInfo=FindExtensionMethod(type,"set");
					
					if(mInfo==null){
						
						// It doesn't exist!
						// Error("Property '"+ToString()+"' is not a property or extension of "+type.ToString()+".");
						
						// Create as a global:
						Field=Method.Script.MainClass.DefineField(Name,true,setType);
						
						return setType;
					}
					
					// Extension property or method.
					// -> We only know which based on MethodOperation later calling GetOverload.
					//    Or OutputSet/ OutputIL being called.
					
					return mInfo.ReturnType;
					
				}
				
				// It exists - create the method operation now.
				mOp=new MethodOperation(Method,mInfo,new CompiledFragment(Name),Input0);
				v=mOp;
				mOp.CalledOn=Of;
				return setType;
			}else{
				// Get.
				
				// Get the get method:
				MethodInfo mInfo;
				
				if(isDynamic){
					mInfo=Class.FindMethodOverload("get_Item",new Type[]{typeof(string)});
				}else{
					// Grab all the methods of the type:
					MethodInfo[] allMethods=type.GetMethods();
					mInfo=Types.GetOverload(allMethods,"get_Item",new Type[]{typeof(string)});
				}
				
				
				if(mInfo==null){
					
					// Try finding the extension method:
					mInfo=FindExtensionMethod(type,"get");
					
					if(mInfo==null){
						
						// It doesn't exist!
						// Error("Property '"+ToString()+"' is not a property or extension of "+type.ToString()+".");
					
						// Create as a global:
						Field=Method.Script.MainClass.DefineField(Name,true,typeof(object));
						
						return typeof(object);
					}
					
					// Extension property or method.
					// -> We only know which based on MethodOperation later calling GetOverload.
					//    Or OutputSet/ OutputIL being called.
					
					return mInfo.ReturnType;
					
				}
				
				// It exists - create the method operation now:
				mOp=new MethodOperation(Method,mInfo,new CompiledFragment(Name));
				v=mOp;
				mOp.CalledOn=Of;
				return mInfo.ReturnType;
			}
			
		}
		
		/// <summary>Does this property operation equal the given one? Compares value names.</summary>
		public bool Equals(PropertyOperation prop){
			
			if(Name==prop.Name){
				
				// So far so good! Match for Of's too?
				if(Of==null && prop.Of==null){
					return true;
				}
				
				if(Of!=null && prop.Of!=null){
					
					// Compare those:
					if(Of.GetType()==prop.Of.GetType()){
						
						return ( Of.ToString()==prop.Of.ToString() );
						
					}
					
				}
				
			}
			
			return false;
			
		}
		
		/// <summary>If this is a method, this attempts to find the correct overload
		/// by using the set of arguments given in the method call.</summary>
		/// <param name="arguments">The set of arguments given in the method call.</param>
		/// <returns>The MethodInfo if found; null otherwise.</returns>
		public MethodInfo GetOverload(CompiledFragment[] arguments){
			Type fragType=OfType();
			
			if(Methods!=null){
				
				// Extension methods.
				
				// Get types:
				Type[] argTypes=Types.GetTypes(fragType,arguments);
				
				// Get the overload:
				return Types.GetOverload(Methods,argTypes);
				
			}
			
			if(Types.IsDynamic(fragType)){
			
				CompiledClass cc=Method.Script.GetClass(fragType);
				
				return (cc==null)?null:cc.FindMethodOverload(Name,arguments);
				
			}else if(Name=="gettype"&&(arguments==null||arguments.Length==0)){
				
				return fragType.GetMethod("GetType",new Type[0]);
				
			}
			
			if(!Method.Script.AllowUse(fragType)){
				Error("Unable to call methods on type "+fragType.Name+" as it is restricted.");
			}
			
			Type[] paramTypes=Types.GetTypes(arguments);
			
			MethodInfo result=Types.GetOverload(fragType.GetMethods(),Name,paramTypes,true);
			
			if(IsStatic && result!=null && !result.IsStatic){
				// Special case! This is where we're calling e.g. ToString on a straight type, for example int.ToString();
				// Another example is actually the call below! We're getting a type, then calling a method on the type - not a static method of it.
				// The method is not static yet we're 'expecting' one.
				// So, look for the same method on System.Type and return that instead.
				return Types.GetOverload(typeof(System.Type).GetMethods(),Name,paramTypes,true);
			}
			
			return result;
			
		}
		
		/// <summary>If this is a method, this attempts to find the correct overload
		/// by using the set of arguments given in the method call.</summary>
		/// <param name="arguments">The set of arguments given in the method call.</param>
		/// <returns>The MethodInfo if found; null otherwise.</returns>
		public MethodInfo GetOverload(Type[] paramTypes){
			
			// Get the parent type:
			Type fragType=OfType();
			
			if(Methods!=null){
				
				// Extension methods.
				
				int count=0;
				
				if(paramTypes!=null){
					count=paramTypes.Length;
				}
				
				// Get types:
				Type[] argTypes=new Type[count+1];
				
				if(paramTypes!=null){
				
					Array.Copy(paramTypes,0,argTypes,1,count);
				
				}
				
				// Get the overload:
				return Types.GetOverload(Methods,argTypes);
				
			}
			
			if(Types.IsDynamic(fragType)){
				
				CompiledClass cc=Method.Script.GetClass(fragType);
				
				return (cc==null)?null:cc.FindMethodOverload(Name,paramTypes);
				
			}else if(Name=="gettype"&&(paramTypes==null || paramTypes.Length==0)){
				
				return fragType.GetMethod("GetType",new Type[0]);
				
			}
			
			if(!Method.Script.AllowUse(fragType)){
				Error("Unable to call methods on type "+fragType.Name+" as it is restricted.");
			}
			
			MethodInfo result=Types.GetOverload(fragType.GetMethods(),Name,paramTypes,true);
			
			if(IsStatic && result!=null && !result.IsStatic){
				// Special case! This is where we're calling e.g. ToString on a straight type, for example int.ToString();
				// Another example is actually the call below! We're getting a type, then calling a method on the type - not a static method of it.
				// The method is not static yet we're 'expecting' one.
				// So, look for the same method on System.Type and return that instead.
				return Types.GetOverload(typeof(System.Type).GetMethods(),Name,paramTypes,true);
			}
			
			return result;
			
		}
		
		public bool OutputTarget(NitroIL into){
			
			if(IsStatic && (Field!=null || Property!=null) && Methods==null){
				return true;
			}
			
			if(Of==null||(Field==null && Property==null && MethodReturnType==null && Methods==null)){
				// Error("Unused or invalid property.");
				// Essentially declaring a property but we can just ignore this.
				// I.e. something like "hello;" which is valid in JS.
				return false;
			}
			
			if(MethodReturnType==null){
				Of.OutputIL(into);
			}
			
			return true;
			
		}
		
		public void OutputSet(NitroIL into,Type setting){
			
			if(Field!=null){
				
				if(IsStatic){
					into.Emit(OpCodes.Stsfld,Field);
				}else{
					into.Emit(OpCodes.Stfld,Field);
				}
				
			}else if(Property!=null){
				bool useVirtual=!IsStatic && !Property.PropertyType.IsValueType;
				MethodInfo setMethod=Property.GetSetMethod();
				
				if(setMethod==null){
					Error(Name+" is a readonly property.");
				}
				
				into.Emit(useVirtual?OpCodes.Callvirt:OpCodes.Call,setMethod);
			}else if(Methods!=null){
				
				// Extension property (hopefully!). Look for a set method lookalike.
				
				// Get types:
				Type[] argTypes=new Type[]{OfType(),setting};
				
				// Get the overload:
				MethodInfo setMethod=Types.GetOverload(Methods,argTypes);
				
				if(setMethod==null){
					Error(Name+" is a 'readonly' extension property.");
				}
				
				// Call the set method:
				into.Emit(OpCodes.Call,setMethod);
				
			}else{
				Error(Name+" is a function! Unable to set it's value.");
			}
			
		}
		
		public override void OutputIL(NitroIL into){
			
			if(!OutputTarget(into)){
				return;
			}
			
			if(Field!=null){
				if(Field.IsLiteral){
					// Special case - this field isn't actually a field at all!
					// Load it's literal value:
					object literalFieldValue=Field.GetValue(null);
					
					// Get ready to write out the literal value:
					CompiledFragment literalValue=new CompiledFragment(literalFieldValue);
					
					// It might even be from an enum - let's check:
					if(Field.FieldType.IsEnum){
						// Ok great it's from an enum. What type is it?
						Type literalValueType=Enum.GetUnderlyingType(Field.FieldType);
						
						// Use that type to emit the value:
						literalValue.EmitValue(literalValueType,into);
					}else{
						literalValue.OutputIL(into);
					}
				}else if(ParentFragment!=null && ParentFragment.IsMemberAccessor() && Field.FieldType.IsValueType){
					// Are we followed by another PropertyOperation?
					// A following operation in this sitation ends up being the parent.
					// If we are, and we output a valuetype, Ldflda must be used.
					
					if(IsStatic){
						into.Emit(OpCodes.Ldsflda,Field);
					}else{
						into.Emit(OpCodes.Ldflda,Field);
					}
					
				}else if(IsStatic){
					into.Emit(OpCodes.Ldsfld,Field);
				}else{
					into.Emit(OpCodes.Ldfld,Field);
				}
			}else if(Property!=null){
				
				bool useVirtual=!IsStatic && !Property.PropertyType.IsValueType;
				
				into.Emit(useVirtual?OpCodes.Callvirt:OpCodes.Call,Property.GetGetMethod());
				
			}else if(Methods!=null){
				
				// Extension property (hopefully!). Look for a get method lookalike.
				
				// Get types:
				Type[] argTypes=new Type[]{OfType()};
				
				// Get the overload:
				MethodInfo getMethod=Types.GetOverload(Methods,argTypes);
				
				if(getMethod==null){
					Error(Name+" is a 'write only' extension property.");
				}
				
				// Call the set method:
				into.Emit(OpCodes.Call,getMethod);
				
			}else{
				DynamicMethodCompiler.Compile(Method,Name,MethodReturnType,Of).OutputIL(into);
			}
		}
		
		public override bool EmitsAddress{
			get{
				return (Field!=null && !Field.IsLiteral);
			}
		}
		
		public override string ToString(){
			string result="";
			if(Of!=null){
				result=Of.ToString();
			}
			if(result!=""){
				result+=".";
			}
			result+=Name;
			return result;
		}
		
	}
	
}