using System;
using System.Diagnostics;

namespace DllProps.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            var info = FileVersionInfo.GetVersionInfo(args[0]);
            Console.WriteLine($"Company: {info.CompanyName}");
            Console.WriteLine($"Copyright: {info.LegalCopyright}");
            Console.WriteLine($"FileVersion: {info.FileVersion}");
            Console.WriteLine($"ProductVersion: {info.ProductVersion}");
            Console.WriteLine($"Description: {info.FileDescription}");
            Console.WriteLine($"Product: {info.}");
        }
    }
}
