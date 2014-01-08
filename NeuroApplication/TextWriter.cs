using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NeuroApplication
{
    class ConsoleWriter : TextWriter
    {
        public int LineIndex { get; set; }

        public ConsoleWriter()
        {
            LineIndex = 0;
        }

        public override void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteAt(string value, int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.Write("                                                                                 ");
            Console.SetCursorPosition(left, top);
            Console.Write(value);
        }

        public override void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
