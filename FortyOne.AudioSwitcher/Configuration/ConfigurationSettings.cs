using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Linq;

namespace FortyOne.AudioSwitcher.Configuration
{
    public class ConfigurationSettings
    {
        public const string GUID_REGEX = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
        public readonly Regex GUID_REGEX_OBJ = new Regex(GUID_REGEX);
        public const string SETTING_CLOSETOTRAY = "CloseToTray";
        public const string SETTING_AUTOSTARTWITHWINDOWS = "AutoStartWithWindows";
        public const string SETTING_STARTMINIMIZED = "StartMinimized";
        public const string SETTING_HOTKEYS = "HotKeys";
        public const string SETTING_FAVOURITEDEVICES = "FavouriteDevices";
        public const string SETTING_WINDOWWIDTH = "WindowWidth";
        public const string SETTING_WINDOWHEIGHT = "WindowHeight";
        public const string SETTING_DISABLEHOTKEYS = "DisableHotKeys";
        public const string SETTING_ENABLEQUICKSWITCH = "EnableQuickSwitch";
        public const string SETTING_OPENCONTROLPANELINSTEAD = "OpenControlPanelInsteadOfPreferences";
        public const string SETTING_CHECKFORUPDATESONSTARTUP = "CheckForUpdatesOnStartup";
        public const string SETTING_POLLFORUPDATES = "PollForUpdates";
        public const string SETTING_STARTUPRECORDINGDEVICE = "StartupRecordingDeviceID";
        public const string SETTING_STARTUPPLAYBACKDEVICE = "StartupPlaybackDeviceID";
        public const string SETTING_DUALSWITCHMODE = "DualSwitchMode";
        public const string SETTING_SHOWDISABLEDDEVICES = "ShowDisabledDevices";
        public const string SETTING_SHOWUNKNOWNDEVICESINHOTKEYLIST = "ShowUnknownDevicesInHotkeyList";
        public const string SETTING_SHOWDISCONNECTEDDDEVICES = "ShowDisconnectedDevices";
        public const string SETTING_SHOWDPDEVICEIICONINTRAY = "ShowDPDeviceIconInTray";
        public const string SETTING_UPDATE_NOTIFICATIONS_ENABLED = "UpdateNotificationsEnabled";
        private readonly ISettingsSource _configWriter;

        public ConfigurationSettings(ISettingsSource source)
        {
            _configWriter = source;
            CreateDefaults();
        }

        public Guid StartupRecordingDeviceID
        {
            get => new Guid(GUID_REGEX_OBJ.Match(_configWriter.Get(SETTING_STARTUPRECORDINGDEVICE)).ToString());
            set => _configWriter.Set(SETTING_STARTUPRECORDINGDEVICE, value.ToString());
        }

        public Guid StartupPlaybackDeviceID
        {
            get=> new Guid(GUID_REGEX_OBJ.Match(_configWriter.Get(SETTING_STARTUPPLAYBACKDEVICE)).ToString());
            set => _configWriter.Set(SETTING_STARTUPPLAYBACKDEVICE, value.ToString());
        }

        public int PollForUpdates
        {
            get => Convert.ToInt32(_configWriter.Get(SETTING_POLLFORUPDATES));
            set => _configWriter.Set(SETTING_POLLFORUPDATES, value.ToString());
        }

        public bool CheckForUpdatesOnStartup
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_CHECKFORUPDATESONSTARTUP));
            set => _configWriter.Set(SETTING_CHECKFORUPDATESONSTARTUP, value.ToString());
        }

        public bool DualSwitchMode
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_DUALSWITCHMODE));
            set => _configWriter.Set(SETTING_DUALSWITCHMODE, value.ToString());
        }

        public bool ShowDisabledDevices
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_SHOWDISABLEDDEVICES));
            set => _configWriter.Set(SETTING_SHOWDISABLEDDEVICES, value.ToString());
        }

        public bool ShowUnknownDevicesInHotkeyList
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_SHOWUNKNOWNDEVICESINHOTKEYLIST));
            set => _configWriter.Set(SETTING_SHOWUNKNOWNDEVICESINHOTKEYLIST, value.ToString());
        }

        public bool ShowDisconnectedDevices
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_SHOWDISCONNECTEDDDEVICES));
            set => _configWriter.Set(SETTING_SHOWDISCONNECTEDDDEVICES, value.ToString());
        }

        public bool ShowDPDeviceIconInTray
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_SHOWDPDEVICEIICONINTRAY));
            set => _configWriter.Set(SETTING_SHOWDPDEVICEIICONINTRAY, value.ToString());
        }

        public int WindowWidth
        {
            get => Convert.ToInt32(_configWriter.Get(SETTING_WINDOWWIDTH));
            set => _configWriter.Set(SETTING_WINDOWWIDTH, value.ToString());
        }

        public int WindowHeight
        {
            get => Convert.ToInt32(_configWriter.Get(SETTING_WINDOWHEIGHT));
            set => _configWriter.Set(SETTING_WINDOWHEIGHT, value.ToString());
        }

        public string FavouriteDevices
        {
            get => _configWriter.Get(SETTING_FAVOURITEDEVICES);
            set => _configWriter.Set(SETTING_FAVOURITEDEVICES, value);
        }

        public string HotKeys
        {
            get => _configWriter.Get(SETTING_HOTKEYS);
            set => _configWriter.Set(SETTING_HOTKEYS, value);
        }

        public bool CloseToTray
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_CLOSETOTRAY));
            set => _configWriter.Set(SETTING_CLOSETOTRAY, value.ToString());
        }

        public bool StartMinimized
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_STARTMINIMIZED));
            set => _configWriter.Set(SETTING_STARTMINIMIZED, value.ToString());
        }

        public bool AutoStartWithWindows
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_AUTOSTARTWITHWINDOWS));
            set
            {
                string val = value.ToString(), name = Assembly.GetEntryAssembly().GetName().Name;
                try
                {
                    var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (value)
                        key.SetValue(name, $@"""{Assembly.GetEntryAssembly().Location}""");
                    else if (key != null && key.GetValue(name) != null)
                        key.DeleteValue(name);
                }
                catch
                {
                    val = false.ToString();
                }
                _configWriter.Set(SETTING_AUTOSTARTWITHWINDOWS, val);
            }
        }

        public bool DisableHotKeys
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_DISABLEHOTKEYS));
            set => _configWriter.Set(SETTING_DISABLEHOTKEYS, value.ToString());
        }

        public bool EnableQuickSwitch
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_ENABLEQUICKSWITCH));
            set => _configWriter.Set(SETTING_ENABLEQUICKSWITCH, value.ToString());
        }

        public bool OpenControlPanelInstead
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_OPENCONTROLPANELINSTEAD));
            set => _configWriter.Set(SETTING_OPENCONTROLPANELINSTEAD, value.ToString());
        }

        public bool UpdateNotificationsEnabled
        {
            get => Convert.ToBoolean(_configWriter.Get(SETTING_UPDATE_NOTIFICATIONS_ENABLED));
            set => _configWriter.Set(SETTING_UPDATE_NOTIFICATIONS_ENABLED, value.ToString());
        }

        private void CreateDefaults()
        {
            if (!_configWriter.Exists(SETTING_CLOSETOTRAY))
                CloseToTray = false;

            if (!_configWriter.Exists(SETTING_STARTMINIMIZED))
                StartMinimized = false;

            if (!_configWriter.Exists(SETTING_AUTOSTARTWITHWINDOWS))
                AutoStartWithWindows = false;

            if (!_configWriter.Exists(SETTING_DISABLEHOTKEYS))
                DisableHotKeys = false;

            if (!_configWriter.Exists(SETTING_ENABLEQUICKSWITCH))
                EnableQuickSwitch = false;

            if (!_configWriter.Exists(SETTING_OPENCONTROLPANELINSTEAD))
                OpenControlPanelInstead = false;

            if (!_configWriter.Exists(SETTING_HOTKEYS))
                HotKeys = "[]";

            if (!_configWriter.Exists(SETTING_FAVOURITEDEVICES))
                FavouriteDevices = "[]";

            if (!_configWriter.Exists(SETTING_WINDOWHEIGHT))
                WindowHeight = 400;

            if (!_configWriter.Exists(SETTING_WINDOWWIDTH))
                WindowWidth = 300;

            if (!_configWriter.Exists(SETTING_CHECKFORUPDATESONSTARTUP))
                CheckForUpdatesOnStartup = false;
            
            if (!_configWriter.Exists(SETTING_POLLFORUPDATES))
                PollForUpdates = CheckForUpdatesOnStartup ? 60 : 0;

            if (!_configWriter.Exists(SETTING_STARTUPPLAYBACKDEVICE))
                StartupPlaybackDeviceID = Guid.Empty;

            if (!_configWriter.Exists(SETTING_STARTUPRECORDINGDEVICE))
                StartupRecordingDeviceID = Guid.Empty;

            if (!_configWriter.Exists(SETTING_DUALSWITCHMODE))
                DualSwitchMode = false;

            if (!_configWriter.Exists(SETTING_SHOWDISABLEDDEVICES))
                ShowDisabledDevices = false;

            if (!_configWriter.Exists(SETTING_SHOWUNKNOWNDEVICESINHOTKEYLIST))
                ShowUnknownDevicesInHotkeyList = false;

            if (!_configWriter.Exists(SETTING_SHOWDISCONNECTEDDDEVICES))
                ShowDisconnectedDevices = false;

            if (!_configWriter.Exists(SETTING_SHOWDPDEVICEIICONINTRAY))
                ShowDPDeviceIconInTray = false;

            if (!_configWriter.Exists(SETTING_UPDATE_NOTIFICATIONS_ENABLED))
                UpdateNotificationsEnabled = PollForUpdates > 0;
        }

        public void LoadFrom(ConfigurationSettings otherSettings)
        {
            AutoStartWithWindows = otherSettings.AutoStartWithWindows;
            CheckForUpdatesOnStartup = otherSettings.CheckForUpdatesOnStartup;
            CloseToTray = otherSettings.CloseToTray;
            DisableHotKeys = otherSettings.DisableHotKeys;
            DualSwitchMode = otherSettings.DualSwitchMode;
            EnableQuickSwitch = otherSettings.EnableQuickSwitch;
            FavouriteDevices = otherSettings.FavouriteDevices;
            HotKeys = otherSettings.HotKeys;
            PollForUpdates = otherSettings.PollForUpdates;
            ShowDisabledDevices = otherSettings.ShowDisabledDevices;
            ShowUnknownDevicesInHotkeyList = otherSettings.ShowUnknownDevicesInHotkeyList;
            ShowDisconnectedDevices = otherSettings.ShowDisconnectedDevices;
            StartMinimized = otherSettings.StartMinimized;
            StartupPlaybackDeviceID = otherSettings.StartupPlaybackDeviceID;
            StartupRecordingDeviceID = otherSettings.StartupRecordingDeviceID;
            WindowHeight = otherSettings.WindowHeight;
            WindowWidth = otherSettings.WindowWidth;
        }
    }
}