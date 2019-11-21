using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DllValidator
{
    class FileProperties
    {
        private readonly FileVersionInfo fileVersionInfo;
        private readonly Type fileVersionInfoType = typeof(FileVersionInfo);

        public FileProperties(string filePath)
        {
            fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
        }

        public string this[string prop] =>
            fileVersionInfoType.GetProperty(prop).GetValue(fileVersionInfo) as string;
    }
}
