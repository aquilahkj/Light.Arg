using System;
using System.Collections.Generic;
using System.Text;
using Light.Args.Mappings;

namespace Light.Args
{
    public static class ArgsParser
    {
        public static T GetArgObject<T>(string[] args) where T : class
        {
            return GetArgObject<T>(args, 0);
        }

        public static T GetArgObject<T>(string[] args, int index) where T : class
        {
            PacketMapping packetMapping = PacketMapping.GetMapping(typeof(T));
            T obj = Activator.CreateInstance<T>();
            ArgsReader reader = new ArgsReader(args, index);
            while (true)
            {
                string arg = reader.ReadArgument();
                if (string.IsNullOrEmpty(arg))
                {
                    break;
                }
                if (arg.StartsWith("--"))
                {
                    string name = arg.Substring(2);
                    try
                    {
                        MemberMapping mapping = packetMapping.FindByName(name);
                        object value = mapping.GetData(reader);
                        if (!Object.Equals(value, null))
                        {
                            mapping.Handler.Set(obj, value);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format(RE.ParseArgumentError, name, e.Message));
                    }
                }
                else if (arg.StartsWith("-"))
                {
                    char[] flags = arg.Substring(1).ToCharArray();
                    if (flags.Length == 1)
                    {
                        try
                        {
                            MemberMapping mapping = packetMapping.FindByFlag(flags[0]);
                            if (mapping is BooleanMemberMapping)
                            {
                                mapping.Handler.Set(obj, true);
                            }
                            else
                            {
                                object value = mapping.GetData(reader);
                                if (!Object.Equals(value, null))
                                {
                                    mapping.Handler.Set(obj, value);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception(string.Format(RE.ParseArgumentError, flags[0], e.Message));
                        }
                    }
                    else
                    {
                        foreach (char flag in flags)
                        {
                            try
                            {
                                MemberMapping mapping = packetMapping.FindByFlag(flag);
                                if (mapping is BooleanMemberMapping)
                                {
                                    mapping.Handler.Set(obj, true);
                                }
                                else
                                {
                                    throw new Exception(string.Format(RE.ArgumentIsNotBoolType, flag));
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception(string.Format(RE.ParseArgumentError, flag, e.Message));
                            }
                        }
                    }
                    
                }
            }
            DefaultMembersMapping defaultMapping = packetMapping.GetDefault();
            if (defaultMapping != null)
            {
                object value = defaultMapping.GetData(reader);
                if (!Object.Equals(value, null))
                {
                    defaultMapping.Handler.Set(obj, value);
                }
            }
            return obj;
        }
    }
}
