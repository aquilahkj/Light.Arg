using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Args.Attributes
{
	[AttributeUsage (AttributeTargets.Property, Inherited = false)]
	public class ArgsMemberAttribute : Attribute
	{
		public ArgsMemberAttribute (char argFlag)
		{
			_argFlag = argFlag;
		}

		public ArgsMemberAttribute ()
		{

		}

		string _argName = null;

		public string ArgName {
			get {
				return _argName;
			}
			set {
				_argName = value;
			}
		}

		char? _argFlag;

		internal char? ArgFlag {
			get {
				return _argFlag;
			}
			set {
				_argFlag = value;
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

		//object _defaultValue = null;
		//public object DefaultValue
		//{
		//    get
		//    {
		//        return _defaultValue;
		//    }
		//    set
		//    {
		//        _defaultValue = value;
		//    }
		//}
	}
}
