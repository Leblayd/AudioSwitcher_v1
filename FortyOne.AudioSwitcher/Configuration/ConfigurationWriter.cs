using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FortyOne.AudioSwitcher.Configuration
{
    public class ConfigurationWriter
    {
        private readonly object _mutex = new object();
        private string _path;
        public string Path
        {
            get => _path;
            set { 
                if (!File.Exists(value))
                    File.Create(value).Close();
                _path = value;
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        //[DllImport("kernel32")]
        //private static extern int GetPrivateProfileString(string section,
        //    string key, string def, StringBuilder retVal,
        //    int size, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        /// <summary>
        ///     Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Value"></PARAM>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            lock (_mutex)
            {
                if (!File.Exists(Path))
                    File.Create(Path).Close();

                WritePrivateProfileString(Section, Key, Value, Path);
            }
        }

        /// <summary>
        ///     Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <returns>Value of the key</returns>
        public string IniReadValue(string Section, string Key)
        {
            lock (_mutex)
            {
                var sb = new StringBuilder(500);
                GetPrivateProfileString(Section, Key, "", sb, (uint) sb.Capacity, Path);

                if (string.IsNullOrEmpty(sb.ToString()))
                    throw new KeyNotFoundException(Section + " - " + Key);

                return sb.ToString();
            }
        }

        /// <summary>
        ///     Check whether the key-value pair exists in the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <returns>True if the key exists, false otherwise</returns>
        public bool IniValueExists(string Section, string Key)
        {
            lock (_mutex)
            {
                var sb = new StringBuilder(500);
                GetPrivateProfileString(Section, Key, "", sb, (uint)sb.Capacity, Path);

                return string.IsNullOrEmpty(sb.ToString());
            }
        }
    }
}