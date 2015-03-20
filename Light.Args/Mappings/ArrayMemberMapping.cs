using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Light.Args.Mappings
{
	class ArrayMemberMapping : MemberMapping
	{
		MemberMapping _innerMapping = null;

		public ArrayMemberMapping (string argName, Type objectType, MemberMapping innerMapping)
			: base (argName, objectType)
		{
			_innerMapping = innerMapping;
			_innerMapping.IsNullable = true;
		}

		public override object GetData (ArgsReader reader)
		{
			ArrayList list = new ArrayList (); 
			while (true) {
				object value = _innerMapping.GetData (reader);
				if (Object.Equals (value, null)) {
					break;
				}
				else {
					list.Add (value);
				}
			}
			if (list.Count == 0) {
				if (IsNullable) {
					return null;
				}
				else {
					throw new Exception (RE.UnableNull);
				}
			}
			else {
				return list.ToArray (_innerMapping.ObjectType);
			}
		}
	}
}
