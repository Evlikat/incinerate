using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace IncinerateService.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class OptionAttribute : Attribute
    {
        object m_OptValue;
        string m_OptName;
        string m_Description;

        public string Short
        {
            get
            {
                return m_OptName;
            }

            set
            {
                m_OptName = value;
            }
        }

        public object Value
        {
            get
            {
                return m_OptValue;
            }

            set
            {
                m_OptValue = value;
            }
        }

        public string Description
        {
            get
            {
                return m_Description;
            }

            set
            {
                m_Description = value;
            }
        }
    }

    public class CommandLineOptions
    {
        private int m_OptionCount;
        private bool m_AllowForwardSlash;
        private List<string> m_InvalidArguments = new List<string>();
        private List<string> m_Parameters = new List<string>();

        public CommandLineOptions(string[] args)
            : this(System.IO.Path.DirectorySeparatorChar != '/', args)
        {
        }

        public CommandLineOptions(bool allowForwardSlash, string[] args)
        {
            this.m_AllowForwardSlash = allowForwardSlash;
            m_OptionCount = Init(args);
        }

        public IList<string> InvalidArguments
        {
            get
            {
                return m_InvalidArguments;
            }
        }

        public bool NoArgs
        {
            get
            {
                return ParameterCount == 0 && m_OptionCount == 0;
            }
        }

        protected int OptionCount
        {
            get
            {
                return m_OptionCount;
            }
        }

        public bool AllowForwardSlash
        {
            get
            {
                return m_AllowForwardSlash;
            }
        }

        protected int Init(params string[] args)
        {
            int count = 0;
            int n = 0;
            while (n < args.Length)
            {
                int pos = OptionPosition(args[n]);
                if (pos > 0)
                {
                    if (GetOption(args, ref n, pos))
                        count++;
                    else
                        InvalidateOption(args[Math.Min(n, args.Length - 1)]);
                }
                else
                {
                    m_Parameters.Add(args[n]);
                    if (!IsValidParameter(args[n]))
                        InvalidateOption(args[n]);
                }
                n++;
            }
            return count;
        }

        protected virtual int OptionPosition(string opt)
        {
            if ((opt[0] == '-' || (opt[0] == '/' && AllowForwardSlash)) && IsOptionNameChar(opt[1]))
                return 1;
            else if (opt.Length > 2 && opt[0] == '-' && opt[1] == '-' && IsOptionNameChar(opt[2]))
                return 2;

            return 0;
        }

        protected virtual bool IsOptionNameChar(char c)
        {
            return Char.IsLetterOrDigit(c) || c == '?';
        }

        protected virtual void InvalidateOption(string name)
        {
            m_InvalidArguments.Add(name);
        }

        protected virtual bool IsValidParameter(string param)
        {
            return true;
        }

        protected virtual bool MatchShortName(FieldInfo field, string name)
        {
            object[] atts = field.GetCustomAttributes(typeof(OptionAttribute), true);
            foreach (OptionAttribute att in atts)
            {
                if (String.Compare(att.Short, name, StringComparison.Ordinal) == 0)
                    return true;
            }
            return false;
        }

        protected virtual FieldInfo GetMemberField(string name)
        {
            Type t = this.GetType();
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                if (string.Compare(field.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                    return field;

                if (MatchShortName(field, name))
                    return field;
            }
            return null;
        }

        protected virtual object GetOptionValue(FieldInfo field)
        {
            object[] atts = field.GetCustomAttributes(typeof(OptionAttribute), true);
            if (atts.Length > 0)
            {
                OptionAttribute att = (OptionAttribute)atts[0];
                return att.Value;
            }
            return null;
        }

        protected virtual bool GetOption(string[] args, ref int index, int pos)
        {
            try
            {
                string opt = args[index].Substring(pos, args[index].Length - pos);
                string cmdLineVal = SplitOptionAndValue(ref opt);

                FieldInfo field = GetMemberField(opt);
                if (field != null)
                {
                    object value = GetOptionValue(field);
                    if (value == null)
                    {
                        if (field.FieldType == typeof(bool))
                        {
                            //default for bool values is true
                            value = true;
                        }
                        else if (field.FieldType == typeof(string))
                        {
                            value = cmdLineVal != null ? cmdLineVal : args[++index];
                            field.SetValue(this, Convert.ChangeType(value, field.FieldType, CultureInfo.InvariantCulture));
                            string stringValue = (string)value;
                            if (String.IsNullOrEmpty(stringValue))
                                return false;

                            return true;
                        }
                        else if (field.FieldType.IsEnum)
                        {
                            value = Enum.Parse(field.FieldType, cmdLineVal, true);
                        }
                        else
                        {
                            value = cmdLineVal != null ? cmdLineVal : args[++index];
                        }
                    }

                    field.SetValue(this, Convert.ChangeType(value, field.FieldType, CultureInfo.InvariantCulture));
                    return true;
                }
            }
            catch (Exception)
            {
                // Ignore exceptions like type conversion errors.
            }
            return false;
        }

        protected virtual string SplitOptionAndValue(ref string opt)
        {
            // Look for ":" or "=" separator in the option:
            int pos = opt.IndexOfAny(new char[] { ':', '=' });
            if (pos < 1)
                return null;

            string val = opt.Substring(pos + 1);
            opt = opt.Substring(0, pos);
            return val;
        }

        public string this[int index]
        {
            get
            {
                if (m_Parameters != null) return (string)m_Parameters[index];
                return null;
            }
        }

        public IList<string> Parameters
        {
            get
            {
                return m_Parameters;
            }
        }

        public int ParameterCount
        {
            get
            {
                return m_Parameters == null ? 0 : m_Parameters.Count;
            }
        }

        public virtual void PrintUsage()
        {
            Console.WriteLine(GetUsageText());
        }

        public virtual string GetUsageText()
        {
            StringBuilder helpText = new StringBuilder();

            Type t = this.GetType();
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
            char optChar = m_AllowForwardSlash ? '/' : '-';
            foreach (FieldInfo field in fields)
            {
                object[] atts = field.GetCustomAttributes(typeof(OptionAttribute), true);
                if (atts.Length > 0)
                {
                    OptionAttribute att = (OptionAttribute)atts[0];
                    if (att.Description != null)
                    {
                        string valType = "";
                        if (att.Value == null)
                        {
                            if (field.FieldType == typeof(float)) valType = "=FLOAT";
                            else if (field.FieldType == typeof(string)) valType = "=STR";
                            else if (field.FieldType != typeof(bool)) valType = "=X";
                        }

                        helpText.AppendFormat("{0}{1,-20}\t{2}", optChar, field.Name + valType, att.Description);
                        if (!String.IsNullOrEmpty(att.Short)) helpText.AppendFormat(" (Short format: {0}{1}{2})", optChar, att.Short, valType);
                        helpText.Append(Environment.NewLine);
                    }
                }
            }
            return helpText.ToString();
        }
    }

    internal class CommandLineArgs : CommandLineOptions
    {
        public CommandLineArgs(bool allowForwardSlash, string[] args) : base(allowForwardSlash, args) { }

        [Option(Short = "i", Description = "Install Service as system service")]
        public bool Install = false;

        [Option(Short = "u", Description = "Uninstall Service service")]
        public bool Uninstall = false;

        [Option(Short = "s", Description = "Run as system service")]
        public bool Service = false;

        [Option(Short = "h", Description = "Display this help")]
        public bool Help = false;
    }
}
