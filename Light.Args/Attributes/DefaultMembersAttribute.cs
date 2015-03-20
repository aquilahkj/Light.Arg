using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Args.Attributes
{
	[AttributeUsage (AttributeTargets.Property, Inherited = false)]
	public class DefaultMembersAttribute : Attribute
	{
		public DefaultMembersAttribute ()
		{

		}
	}
}
