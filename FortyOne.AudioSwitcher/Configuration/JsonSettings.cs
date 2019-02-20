using System.Collections.Generic;
using System.IO;
using fastJSON;

namespace FortyOne.AudioSwitcher.Configuration
{
    public class JsonSettings : ISettingsSource
    {
        private readonly object _mutex = new object();
        private IDictionary<string, string> _settingsObject;
        public string Path { get; set; }

        public JsonSettings(string path)
        {
            Path = path;
            Load();
        }

        private void Load()
        {
            lock (_mutex)
            {
                try
                {
                    if (File.Exists(Path))
                        _settingsObject = JSON.ToObject<Dictionary<string, string>>(File.ReadAllText(Path));
                }
                catch
                {
                    _settingsObject = new Dictionary<string, string>();
                }
            }
        }

        private void Save()
        {
            try
            {
                //Write the result to file
                File.WriteAllText(Path, JSON.Beautify(JSON.ToJSON(_settingsObject)));
            }
            catch
            {
                //Too bad if we can't save, not like there's anything vitally important in settings
            }
        }

        public string Get(string key)
        {
            lock (_mutex)
            {
                return _settingsObject[key];
            }
        }

        public void Set(string key, object value)
        {
            lock (_mutex)
            {
                _settingsObject[key] = value.ToString();
                Save();
            }
        }

        public bool Exists(string key)
        {
            lock (_mutex)
            {
                return _settingsObject.ContainsKey(key);
            }
        }
    }
}