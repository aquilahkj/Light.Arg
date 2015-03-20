using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Light.Args
{
    class ArgsReader
    {
        Regex nameRegex = new Regex(@"^--[a-zA-Z]\w*=.*$", RegexOptions.Compiled);

        Regex flagRegex = new Regex(@"^-[a-zA-Z]+$", RegexOptions.Compiled);

        List<string> _argList = new List<string>();

        List<string> _defaultList = new List<string>();

        int offset = 0;

        public ArgsReader(string[] args, int index)
        {
            for (int i = index; i < args.Length; i++)
            {
                _argList.Add(args[i]);
            }
        }

        public string ReadContent()
        {
            while (true)
            {
                if (offset >= _argList.Count)
                {
                    return null;
                }
                else
                {
                    string arg = _argList[offset];
                    if (string.IsNullOrEmpty(arg))
                    {
                        offset++;
                        continue;
                    }
                    else
                    {
                        if (nameRegex.IsMatch(arg) || flagRegex.IsMatch(arg))
                        {
                            return null;
                        }
                        else
                        {
                            offset++;
                            return arg;
                        }
                    }

                }
            }
        }

        public string ReadArgument()
        {
            while (true)
            {
                if (offset >= _argList.Count)
                {
                    return null;
                }
                else
                {
                    string arg = _argList[offset];
                    if (string.IsNullOrEmpty(arg))
                    {
                        offset++;
                        continue;
                    }
                    else
                    {
                        if (nameRegex.IsMatch(arg))
                        {
                            int index = arg.IndexOf('=');
                            if (index + 1 == arg.Length)
                            {
                                throw new Exception(string.Format(RE.ArgumentValueIsNull, arg));
                            }
                            string argName = null;
                            //if (index > 0)
                            //{
                            argName = arg.Substring(0, index);
                            _argList[offset] = arg.Substring(index + 1);
                            //}
                            //else
                            //{
                            //    argName = arg;
                            //    _argList[offset] = string.Empty;
                            //}
                            
                            return argName;
                        }
                        else if (flagRegex.IsMatch(arg))
                        {
                            offset++;
                            return arg;
                        }
                        else
                        {
                            offset++;
                            _defaultList.Add(arg);
                            continue;
                        }
                    }
                }
            }
            
        }

        //public bool SeekArgument()
        //{
        //    int i = offset;
        //    if (i >= _argList.Count)
        //    {
        //        return false;
        //    }


        //    //int i = offset;
        //    //while (true)
        //    //{
        //    //    if (i >= _argList.Count)
        //    //    {
        //    //        return false;
        //    //    }
        //    //    string arg = _argList[i];
        //    //    if (string.IsNullOrEmpty(arg))
        //    //    {
        //    //        i++;
        //    //        continue;
        //    //    }
        //    //    else
        //    //    {
        //    //        if (nameRegex.IsMatch(arg))
        //    //        {
        //    //            int index = arg.IndexOf('=');
        //    //            string argName = null;
        //    //            if (index > 0)
        //    //            {
        //    //                argName = arg.Substring(0, index);
        //    //                _argList[offset] = arg.Substring(index + 1);
        //    //            }
        //    //            else
        //    //            {
        //    //                argName = arg;
        //    //                _argList[offset] = string.Empty;
        //    //            }

        //    //            return argName;
        //    //        }
        //    //        else if (flagRegex.IsMatch(arg))
        //    //        {
        //    //            offset++;
        //    //            return arg;
        //    //        }
        //    //        else
        //    //        {
        //    //            offset++;
        //    //            _defaultList.Add(arg);
        //    //            continue;
        //    //        }
        //    //    }
        //    //}

        //}

        public string[] GetDefaultArguments()
        {
            return _defaultList.ToArray();
        }
    }
}
