using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Light.Args.Mappings
{
	class DateTimeMemberMapping : MemberMapping
	{
		static string[] formats = new string[] {
			"yyyyMMdd",
			"yyyy-MM-dd",
			"yyyy/MM/dd",
			"yyyyMMddHHmmss",
			"yyyy-MM-ddTHH:mm:ss"
		};

		public DateTimeMemberMapping (string argName)
			: base (argName, typeof(DateTime))
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
			DateTime dt = ParseDateTime (strValue);
			return dt;
		}

		DateTime ParseDateTime (string value)
		{
			DateTime dt;
			if (DateTime.TryParseExact (value, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dt)) {
				return dt;
			}
			else {
				throw new Exception (string.Format (RE.ParseDateTimeError, value));
			}
		}
	}
}
