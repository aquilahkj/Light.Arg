using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Light.Args.Attributes;
using Light.Args.Handler;

namespace Light.Args.Mappings
{
	abstract class MemberMapping
	{
		char? _argFlag;

		public char? ArgFlag {
			get {
				return _argFlag;
			}
			set {
				_argFlag = value;
			}
		}

		string _argName = null;

		public string ArgName {
			get {
				return _argName;
			}
		}

		bool _isNullable;

		public bool IsNullable {
			get {
				return _isNullable;
			}
			set {
				_isNullable = value;
			}
		}

		Type _objectType = null;

		public Type ObjectType {
			get {
				return _objectType;
			}
		}


		public PropertyHandler Handler {
			get;
			protected set;
		}


		protected MemberMapping (string argName, Type objectType)
		{
			_argName = argName;
			_objectType = objectType;
		}

		public abstract object GetData (ArgsReader reader);

		internal static MemberMapping CreateParameterMapping (PropertyInfo property, ArgsMemberAttribute attribute)
		{
			MemberMapping mapping = null;
			string paramName = null;
			if (!string.IsNullOrEmpty (attribute.ArgName)) {
				paramName = attribute.ArgName;
			}
			else {
				paramName = property.Name;
			}
			Type type = property.PropertyType;

			if (type == typeof(string)) {
				mapping = CreatePrimitiveMapping (paramName, type);
			}
			else if (type.IsGenericType) {
				Type frameType = type.GetGenericTypeDefinition ();
				if (frameType.FullName == "System.Nullable`1") {
					Type[] arguments = type.GetGenericArguments ();
					Type atype = arguments [0];
					mapping = CreatePrimitiveMapping (paramName, atype);

				}
				else {
					throw new Exception (string.Format (RE.UnsupportType, type));
				}
			}
			else if (type.IsArray) {
				if (type.FullName == "System.Byte[]") {
					throw new Exception (string.Format (RE.UnsupportType, type));
				}
				Type aType = type.GetElementType ();
				MemberMapping pmapping = CreatePrimitiveMapping (paramName, aType);
				mapping = new ArrayMemberMapping (paramName, type, pmapping);
			}
			else {
				throw new Exception (string.Format (RE.UnsupportType, type));
			}
			mapping.Handler = new PropertyHandler (property);
			mapping.IsNullable = attribute.IsNullable;
			mapping.ArgFlag = attribute.ArgFlag;
			return mapping;
		}

		internal static DefaultMembersMapping CreateDefaultMapping (PropertyInfo property, DefaultMembersAttribute attribute)
		{
			Type type = property.PropertyType;
			if (type != typeof(string[])) {
				throw new Exception (RE.DefaultMemberMustStringArray);
			}
			DefaultMembersMapping mapping = new DefaultMembersMapping ("default");
			mapping.Handler = new PropertyHandler (property);
			return mapping;
		}

		internal static void CheckPrimitiveType (Type type)
		{
			TypeCode code = Type.GetTypeCode (type);
			if (code == TypeCode.Byte || code == TypeCode.SByte || code == TypeCode.Char || code == TypeCode.DBNull || code == TypeCode.Empty || code == TypeCode.Object) {
				throw new Exception (string.Format (RE.UnsupportType, type));
			}
		}

		private static MemberMapping CreatePrimitiveMapping (string name, Type type)
		{
			TypeCode code = Type.GetTypeCode (type);
			if (code == TypeCode.Boolean) {
				return new BooleanMemberMapping (name);
			}
			else if (code == TypeCode.String) {
				return new StringMemberMapping (name);
			}
			else if (code == TypeCode.DateTime) {
				return new DateTimeMemberMapping (name);
			}
			else if (code == TypeCode.Decimal || code == TypeCode.Double || code == TypeCode.Single
			                  || code == TypeCode.Int16 || code == TypeCode.Int32 || code == TypeCode.Int64
			                  || code == TypeCode.UInt16 || code == TypeCode.UInt32 || code == TypeCode.UInt64) {
				return new NumberMemberMapping (name, type);
			}
			else {
				throw new Exception (string.Format (RE.UnsupportType, type));
			}

		}


	}
}
