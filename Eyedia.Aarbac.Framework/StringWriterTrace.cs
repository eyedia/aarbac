using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class StringWriterTrace : IDisposable
    {
        StringWriter Log { get; }
        public int IndentLevel { get; private set; }
        public string CustomPrefix { get; }
        public string Prefix
        {
            get
            {
                return string.Format("{0}{1}:", new string(' ', IndentLevel), CustomPrefix);
            }
        }
        public StringWriterTrace(string prefix = null)
        {
            CustomPrefix = string.IsNullOrEmpty(prefix) ? string.Empty : prefix;
            this.Log = new StringWriter();
        }

        public void Indent()
        {
            IndentLevel += 4;
        }

        public void UnIndent()
        {
            IndentLevel -= 4;
        }

        public void Write(string message)
        {            
            Log.Write(Prefix + message);            
        }
        public void Write(string format, params object[] arg)
        {
            Log.Write(Prefix + format, arg);
        }
        public void WriteLine()
        {
            Log.WriteLine();
        }
        public void WriteLine(string message)
        {
            Log.WriteLine(Prefix + message);
        }

        public void WriteLine(string format, params object[] arg)
        {
            Log.WriteLine(Prefix + format, arg);
        }

        public void Close()
        {
            Log.Close();
        }
        public void Dispose()
        {
            if (this.Log != null)
            {
                this.Log.Close();
                this.Log.Dispose();
            }
        }

        public override string ToString()
        {
            return Log.ToString();
        }
    }
}
