using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Light.Args.Attributes;
using Light.Args.Handler;

namespace Light.Args.Mappings
{
	class DefaultMembersMapping : MemberMapping
	{

		public DefaultMembersMapping (string argName)
			: base (argName, typeof(string[]))
		{

		}

		public override object GetData (ArgsReader reader)
		{
			return reader.GetDefaultArguments ();
		}
	}
}
