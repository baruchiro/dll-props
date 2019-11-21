using DocoptNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DllValidator
{
    class Program
    {
        private const string usage = @"dll-validator

    Usage:
      dll-validator <validation> <dir> [-r] [<white-list>]

    Options:
      -h --help     Show this screen.
      --version     Show version.
    ";

        static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(usage, args, version: "dll-validator alpha", exit: true);

            var validationFile = arguments["<validation>"].Value as string;
            var whiteList = arguments["<white-list>"]?.Value as string;
            var dir = Path.GetFullPath(arguments["<dir>"].Value as string);
            var recursive = arguments["-r"].IsTrue ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var validations = File.ReadAllLines(validationFile)
                .Select(l => l.Split(','))
                .Select(a => new { Key = a[0], Value = a[1] });
            var dlls = Directory.EnumerateFiles(dir, "*.dll", recursive);

            if (whiteList != null)
            {
                foreach(var line in File.ReadAllLines(whiteList))
                {
                    var startWith = line[0] != '*';
                    var endWith = line.Last() != '*';
                    var contain = !startWith && !endWith;
                    var pattern = line.Replace("*", "");
                    if (contain)
                    {
                        dlls = dlls.Where(d => !Path.GetFileNameWithoutExtension(d).Contains(pattern));
                    }
                    else
                    {
                        if (startWith)
                        {
                            dlls = dlls.Where(d => !Path.GetFileNameWithoutExtension(d).StartsWith(pattern));
                        }
                        if (endWith)
                        {
                            dlls = dlls.Where(d => !Path.GetFileNameWithoutExtension(d).EndsWith(pattern));
                        }
                    }
                }
            }

            foreach(var dll in dlls)
            {
                var fileProperties = new FileProperties(dll);
                foreach(var validate in validations)
                {
                    var value = fileProperties[validate.Key];
                    if(value?.Equals(validate.Value, StringComparison.OrdinalIgnoreCase) != true)
                    {
                        Console.WriteLine($"{dll}->{validate.Key}- Expected:'{validate.Value}' Actual:'{value}'");
                    }
                }
            }
        }
    }
}
