namespace FortyOne.AudioSwitcher.Configuration
{
    public interface ISettingsSource
    {
        string Path { get; set; }
        string Get(string key);
        bool Exists(string key);
        void Set(string key, string value);
    }
}