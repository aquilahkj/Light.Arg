using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Light.Args.Attributes;
using Light.Args.Handler;

namespace Light.Args.Mappings
{
	class PacketMapping
	{
		#region static

		static object _synobj = new object ();
		static Dictionary<Type, PacketMapping> _mappings = new Dictionary<Type, PacketMapping> ();

		public static PacketMapping GetMapping (Type type)
		{
			if (!_mappings.ContainsKey (type)) {
				lock (_synobj) {
					if (!_mappings.ContainsKey (type)) {
						PacketMapping mapping = new PacketMapping (type);
						_mappings [type] = mapping;
					}
				}
			}
			return _mappings [type];
		}

		#endregion

		protected Dictionary<string, MemberMapping> _fieldNameMappingDictionary = new Dictionary<string, MemberMapping> ();

		protected Dictionary<char, MemberMapping> _fieldFlagMappingDictionary = new Dictionary<char, MemberMapping> ();

		protected DefaultMembersMapping _defaultMapping = null;

		internal Type ObjectType {
			get;
			set;
		}

		public PacketMapping (Type type)
		{
			ObjectType = type;
			InitialDataField ();
		}

		private void InitialDataField ()
		{
			PropertyInfo[] propertys = ObjectType.GetProperties ();

			foreach (PropertyInfo pi in propertys) {
				//字段属性
				ArgsMemberAttribute[] spattributes = AttributeCore.GetPropertyAttributes<ArgsMemberAttribute> (pi, true);
				DefaultMembersAttribute[] dfattributes = AttributeCore.GetPropertyAttributes<DefaultMembersAttribute> (pi, true);
				if (spattributes.Length > 0) {
					MemberMapping mapping = MemberMapping.CreateParameterMapping (pi, spattributes [0]);
					_fieldNameMappingDictionary.Add (mapping.ArgName, mapping);
					if (mapping.ArgFlag.HasValue) {
						_fieldFlagMappingDictionary.Add (mapping.ArgFlag.Value, mapping);
					}
				}
				else if (dfattributes.Length > 0) {
					_defaultMapping = MemberMapping.CreateDefaultMapping (pi, dfattributes [0]);
				}

			}
		}

		public MemberMapping FindByName (string name)
		{
			if (_fieldNameMappingDictionary.ContainsKey (name)) {
				return _fieldNameMappingDictionary [name];
			}
			else {
				throw new Exception (string.Format (RE.CanNotFindArgumentName, name));
			}
		}

		public MemberMapping FindByFlag (char flag)
		{
			if (_fieldFlagMappingDictionary.ContainsKey (flag)) {
				return _fieldFlagMappingDictionary [flag];
			}
			else {
				throw new Exception (string.Format (RE.CanNotFindArgumentName, flag));
			}
		}

		public DefaultMembersMapping GetDefault ()
		{
			return _defaultMapping;
		}
	}
}
