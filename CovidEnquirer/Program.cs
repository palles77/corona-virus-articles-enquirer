using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.IO;

namespace CovidEnquirer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new MainForm().ShowDialog();
        }
    }
}
