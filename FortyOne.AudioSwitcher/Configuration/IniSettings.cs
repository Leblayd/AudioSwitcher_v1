namespace FortyOne.AudioSwitcher.Configuration
{
    public class IniSettings : ISettingsSource
    {
        private const string SECTION_NAME = "Settings";
        private readonly ConfigurationWriter _writer = new ConfigurationWriter();
        public string Path
        {
            get => _writer.Path;
            set => _writer.Path = value;
        }

        public string Get(string key) =>
            _writer.IniReadValue(SECTION_NAME, key);

        public void Set(string key, object value) =>
            _writer.IniWriteValue(SECTION_NAME, key, value.ToString());

        public bool Exists(string key) =>
            _writer.IniValueExists(SECTION_NAME, key);
    }
}