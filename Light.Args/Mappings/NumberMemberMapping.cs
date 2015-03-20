using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Args.Mappings
{
    class NumberMemberMapping : MemberMapping
    {
        public NumberMemberMapping(string argName, Type objectType)
            : base(argName, objectType)
        {

        }

        public override object GetData(ArgsReader reader)
        {
            string strValue = reader.ReadContent();
            if (string.IsNullOrEmpty(strValue))
            {
                if (IsNullable)
                {
                    return null;
                }
                else
                {
                    throw new Exception(RE.UnableNull);
                }
            }
            object obj = ParseNumber(strValue);
            return obj;
        }

        object ParseNumber(string value)
        {
            try
            {
                object obj = Convert.ChangeType(value, ObjectType);
                return obj;
            }
            catch
            {
                throw new Exception(string.Format(RE.ParseDateTimeError, value, ObjectType.Name));
            }
        }
    }
}
