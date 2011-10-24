using System;
using System.Collections.Generic;
using System.Linq;
using System.Json;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Commons.VocaloidApi
{
	static class Extensions
	{
		public static T GetCustomAttribute<T> (this MemberInfo mi) where T : Attribute
		{
			foreach (var a in mi.GetCustomAttributes (typeof (T), false))
				return (T) a;
			return null;
		}
	}

	class JsonRequestProcessor
	{
		static readonly IDictionary<string,object> empty = new Dictionary<string,object> ();
		static readonly IDictionary<string,object> result_true = new object [] {true}.ToDictionary (v => "result");
		static readonly IDictionary<string,object> result_false = new object [] {false}.ToDictionary (v => "result");

		Func<string,string> json_request_handler;

		public JsonRequestProcessor (Func<string,string> jsonRequestHandler)
		{
			json_request_handler = jsonRequestHandler;
		}
		
		public void ProcessEmpty (string functionName, params object [] args)
		{
			ProcessJson (ToJsonRequest (functionName, args));
		}
		
		// returns untyped result
		public IDictionary<string,object> ProcessUntyped (string functionName, params object [] args)
		{
			var jobj = ProcessJson (ToJsonRequest (functionName, args));
			if (jobj == null)
				return null;
			var ret = new Dictionary<string,object> ();
			foreach (var p in (JsonObject) jobj)
				ret [p.Key] = ((JsonPrimitive) p.Value).Value;
			return ret;
		}
		
		public T Process<T> (string functionName, params object [] args)
		{
			var ret = ProcessJson (ToJsonRequest (functionName, args));
			T v = UnJson<T> (ret);
			return v;
		}
		
		public Result<T> ProcessWithResult<T> (string functionName, params object [] args)
		{
			var ret = (JsonObject) ProcessJson (ToJsonRequest (functionName, args));
			bool result = (bool) ret ["result"];
			var val = ret.First (p => p.Key != "result").Value;
			T v = UnJson<T> (ret);
			return new Result<T> (result, v);
		}
		
		// non-public code
		
		JsonValue ProcessJson (JsonArray args)
		{
			return JsonValue.Parse (json_request_handler (args.ToString ()));
		}
		
		JsonArray ToJsonRequest (string functionName, object [] args)
		{
			var jargs = from a in new object [] {functionName}.Concat (args) select
				a == null ? null :
				Type.GetTypeCode (a.GetType ()) == TypeCode.Object ? ToMappedJsonObject (a) :
				JsonValue.ToJsonValue (a);
			return new JsonArray (jargs);
		}

		T UnJson<T> (JsonValue ret)
		{
			object v;
			if (ret is JsonObject)
				v = UnJsonTypedObject<T> ((JsonObject) ret);
			else if (ret is JsonPrimitive)
				v = ((JsonPrimitive) ret).Value;
			else
				throw new ArgumentException ();
			return (T) v;
		}
		
		T UnJsonTypedObject<T> (JsonObject obj)
		{
			var ret = FormatterServices.GetUninitializedObject (typeof (T));
			foreach (var fi in typeof (T).GetFields ()) {
				var ra = fi.GetCustomAttribute<RegisterAttribute> ();
				if (ra == null)
					continue;
				var jv = obj [ra.Name];
				fi.SetValue (ret, ((JsonPrimitive) jv).Value);
			}
			return (T) ret;
		}
		
		JsonObject ToMappedJsonObject (object value)
		{
			var dic = new Dictionary<string,JsonValue> ();
			var t = value.GetType ();
			foreach (var fi in t.GetFields ()) {
				var reg = fi.GetCustomAttribute<RegisterAttribute> ();
				if (reg == null)
					continue;
				dic [reg.Name] = JsonValue.ToJsonValue (fi.GetValue (value));
			}
			return new JsonObject (dic);
		}
	}
}
