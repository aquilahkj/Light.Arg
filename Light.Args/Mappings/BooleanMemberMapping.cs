using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Args.Mappings
{
	class BooleanMemberMapping : MemberMapping
	{
		static Dictionary<string, bool> formats = new Dictionary<string, bool> ();

		static BooleanMemberMapping ()
		{
			formats.Add ("true", true);
			formats.Add ("t", true);
			formats.Add ("1", true);
			formats.Add ("y", true);
			formats.Add ("yes", true);
			formats.Add ("false", false);
			formats.Add ("f", false);
			formats.Add ("0", false);
			formats.Add ("n", false);
			formats.Add ("no", false);
		}

		public BooleanMemberMapping (string argName)
			: base (argName, typeof(bool))
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
					return true;
				}
			}
			bool b = ParseBoolan (strValue);
			return b;
		}

		private bool ParseBoolan (string strValue)
		{
			string value = strValue.ToLower ().Trim ();
			if (formats.ContainsKey (value)) {
				return formats [value];
			}
			else {
				throw new Exception (string.Format (RE.ParseDateTimeError, value));
			}
		}
	}
}
