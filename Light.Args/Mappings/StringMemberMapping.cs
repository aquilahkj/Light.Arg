using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Args.Mappings
{
	class StringMemberMapping : MemberMapping
	{
		public StringMemberMapping (string argName)
			: base (argName, typeof(string))
		{

		}

		public override object GetData (ArgsReader reader)
		{
			string strValue = reader.ReadContent ();
			if (string.IsNullOrEmpty (strValue)) {
				if (IsNullable) {
					return null;
				}
				else {
					throw new Exception (RE.UnableNull);
				}
			}
			return strValue;
		}
	}
}
