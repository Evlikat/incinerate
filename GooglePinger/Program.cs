using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace GooglePinger
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; ; i++)
            {
                HttpWebRequest request = null;
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.CreateDefault(new Uri(@"http://ya.ru"));
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream resStream = response.GetResponseStream();
                    string tempString = null;
                    int count = 0;
                    int k = 0;
                    byte[] buf = new byte[1024];
                    do
                    {
                        count = resStream.Read(buf, 0, buf.Length);
                        if (count != 0)
                        {
                            tempString = Encoding.ASCII.GetString(buf, 0, count);
                        }
                    } while (count > 0);
                    Console.WriteLine("Requests made: {0}", i);
                }
                finally
                {
                }
            }
        }
    }
}
