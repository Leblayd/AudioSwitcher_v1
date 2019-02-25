using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace FortyOne.AudioSwitcher.Configuration
{
    public class ConfigurationSettings
    {
        public const string GUID_REGEX = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
        public readonly Regex GUID_REGEX_OBJ = new Regex(GUID_REGEX);
        private readonly ISettingsSource _configWriter;

        public ConfigurationSettings(ISettingsSource source)
        {
            _configWriter = source;
            CreateDefaults();
        }

        public Guid StartupRecordingDeviceID
        {
            get => new Guid(GUID_REGEX_OBJ.Match(_configWriter.Get(Settings.StartupRecordingDeviceID)).ToString());
            set => _configWriter.Set(Settings.StartupRecordingDeviceID, value);
        }

        public Guid StartupPlaybackDeviceID
        {
            get => new Guid(GUID_REGEX_OBJ.Match(_configWriter.Get(Settings.StartupPlaybackDeviceID)).ToString());
            set => _configWriter.Set(Settings.StartupPlaybackDeviceID, value);
        }

        public int PollForUpdates
        {
            get => Convert.ToInt32(_configWriter.Get(Settings.PollForUpdates));
            set => _configWriter.Set(Settings.PollForUpdates, value);
        }

        public bool CheckForUpdatesOnStartup
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.CheckForUpdatesOnStartup));
            set => _configWriter.Set(Settings.CheckForUpdatesOnStartup, value);
        }

        public bool DualSwitchMode
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.DualSwitchMode));
            set => _configWriter.Set(Settings.DualSwitchMode, value);
        }

        public bool ShowDisabledDevices
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.ShowDisabledDevices));
            set => _configWriter.Set(Settings.ShowDisabledDevices, value);
        }

        public bool ShowUnknownDevicesInHotkeyList
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.ShowUnknownDevicesInHotkeyList));
            set => _configWriter.Set(Settings.ShowUnknownDevicesInHotkeyList, value);
        }

        public bool ShowDisconnectedDevices
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.ShowDisconnectedDevices));
            set => _configWriter.Set(Settings.ShowDisconnectedDevices, value);
        }

        public bool ShowDPDeviceIconInTray
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.ShowDPDeviceIconInTray));
            set => _configWriter.Set(Settings.ShowDPDeviceIconInTray, value);
        }

        public int WindowWidth
        {
            get => Convert.ToInt32(_configWriter.Get(Settings.WindowWidth));
            set => _configWriter.Set(Settings.WindowWidth, value);
        }

        public int WindowHeight
        {
            get => Convert.ToInt32(_configWriter.Get(Settings.WindowHeight));
            set => _configWriter.Set(Settings.WindowHeight, value);
        }

        public string FavouriteDevices
        {
            get => _configWriter.Get(Settings.FavouriteDevices);
            set => _configWriter.Set(Settings.FavouriteDevices, value);
        }

        public string HotKeys
        {
            get => _configWriter.Get(Settings.HotKeys);
            set => _configWriter.Set(Settings.HotKeys, value);
        }

        public bool CloseToTray
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.CloseToTray));
            set => _configWriter.Set(Settings.CloseToTray, value);
        }

        public bool StartMinimized
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.StartMinimized));
            set => _configWriter.Set(Settings.StartMinimized, value);
        }

        public bool AutoStartWithWindows
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.AutoStartWithWindows));
            set
            {
                bool val = value;
                string name = Assembly.GetEntryAssembly().GetName().Name;
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
                    val = false;
                }
                _configWriter.Set(Settings.AutoStartWithWindows, val);
            }
        }

        public bool DisableHotKeys
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.DisableHotKeys));
            set => _configWriter.Set(Settings.DisableHotKeys, value);
        }

        public bool EnableQuickSwitch
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.EnableQuickSwitch));
            set => _configWriter.Set(Settings.EnableQuickSwitch, value);
        }

        public bool OpenControlPanelInstead
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.OpenControlPanelInstead));
            set => _configWriter.Set(Settings.OpenControlPanelInstead, value);
        }

        public bool UpdateNotificationsEnabled
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.UpdateNotificationsEnabled));
            set => _configWriter.Set(Settings.UpdateNotificationsEnabled, value);
        }

        public bool VolumeControlShow
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.VolumeControlShow));
            set => _configWriter.Set(Settings.VolumeControlShow, value);
        }
        
        public bool VolumeControlScrollInEntireMenu
        {
            get => Convert.ToBoolean(_configWriter.Get(Settings.VolumeControlScrollInEntireMenu));
            set => _configWriter.Set(Settings.VolumeControlScrollInEntireMenu, value);
        }
        
        public int VolumeControlDivisibleByNumber
        {
            get => Convert.ToInt32(_configWriter.Get(Settings.VolumeControlDivisibleByNumber));
            set => _configWriter.Set(Settings.VolumeControlDivisibleByNumber, value);
        }

        private void CreateDefaults()
        {
            if (!_configWriter.Exists(Settings.CloseToTray))
                _configWriter.Set(Settings.CloseToTray, false);
            if (!_configWriter.Exists(Settings.StartMinimized))
                _configWriter.Set(Settings.StartMinimized, false);
            if (!_configWriter.Exists(Settings.AutoStartWithWindows))
                _configWriter.Set(Settings.AutoStartWithWindows, false);
            if (!_configWriter.Exists(Settings.DisableHotKeys))
                _configWriter.Set(Settings.DisableHotKeys, false);
            if (!_configWriter.Exists(Settings.EnableQuickSwitch))
                _configWriter.Set(Settings.EnableQuickSwitch, false);
            if (!_configWriter.Exists(Settings.OpenControlPanelInstead))
                _configWriter.Set(Settings.OpenControlPanelInstead, false);
            if (!_configWriter.Exists(Settings.HotKeys))
                _configWriter.Set(Settings.HotKeys, "[]");
            if (!_configWriter.Exists(Settings.FavouriteDevices))
                _configWriter.Set(Settings.FavouriteDevices, "[]");
            if (!_configWriter.Exists(Settings.WindowHeight))
                _configWriter.Set(Settings.WindowHeight, 400);
            if (!_configWriter.Exists(Settings.WindowWidth))
                _configWriter.Set(Settings.WindowWidth, 300);
            if (!_configWriter.Exists(Settings.CheckForUpdatesOnStartup))
                _configWriter.Set(Settings.CheckForUpdatesOnStartup, false);
            if (!_configWriter.Exists(Settings.PollForUpdates))
                _configWriter.Set(Settings.PollForUpdates, CheckForUpdatesOnStartup ? 60 : 0);
            if (!_configWriter.Exists(Settings.StartupPlaybackDeviceID))
                _configWriter.Set(Settings.StartupPlaybackDeviceID, Guid.Empty);
            if (!_configWriter.Exists(Settings.StartupRecordingDeviceID))
                _configWriter.Set(Settings.StartupRecordingDeviceID, Guid.Empty);
            if (!_configWriter.Exists(Settings.DualSwitchMode))
                _configWriter.Set(Settings.DualSwitchMode, false);
            if (!_configWriter.Exists(Settings.ShowDisabledDevices))
                _configWriter.Set(Settings.ShowDisabledDevices, false);
            if (!_configWriter.Exists(Settings.ShowUnknownDevicesInHotkeyList))
                _configWriter.Set(Settings.ShowUnknownDevicesInHotkeyList, false);
            if (!_configWriter.Exists(Settings.ShowDisconnectedDevices))
                _configWriter.Set(Settings.ShowDisconnectedDevices, false);
            if (!_configWriter.Exists(Settings.ShowDPDeviceIconInTray))
                _configWriter.Set(Settings.ShowDPDeviceIconInTray, false);
            if (!_configWriter.Exists(Settings.UpdateNotificationsEnabled))
                _configWriter.Set(Settings.UpdateNotificationsEnabled, PollForUpdates > 0);
            if (!_configWriter.Exists(Settings.VolumeControlShow))
                _configWriter.Set(Settings.VolumeControlShow, true);
            if (!_configWriter.Exists(Settings.VolumeControlScrollInEntireMenu))
                _configWriter.Set(Settings.VolumeControlScrollInEntireMenu, false);
            if (!_configWriter.Exists(Settings.VolumeControlDivisibleByNumber))
                _configWriter.Set(Settings.VolumeControlDivisibleByNumber, 0);
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

    public static class Settings
    {
        public const string CloseToTray = "CloseToTray";
        public const string AutoStartWithWindows = "AutoStartWithWindows";
        public const string StartMinimized = "StartMinimized";
        public const string HotKeys = "HotKeys";
        public const string FavouriteDevices = "FavouriteDevices";
        public const string WindowWidth = "WindowWidth";
        public const string WindowHeight = "WindowHeight";
        public const string DisableHotKeys = "DisableHotKeys";
        public const string EnableQuickSwitch = "EnableQuickSwitch";
        public const string OpenControlPanelInstead = "OpenControlPanelInsteadOfPreferences";
        public const string CheckForUpdatesOnStartup = "CheckForUpdatesOnStartup";
        public const string PollForUpdates = "PollForUpdates";
        public const string StartupRecordingDeviceID = "StartupRecordingDeviceID";
        public const string StartupPlaybackDeviceID = "StartupPlaybackDeviceID";
        public const string DualSwitchMode = "DualSwitchMode";
        public const string ShowDisabledDevices = "ShowDisabledDevices";
        public const string ShowUnknownDevicesInHotkeyList = "ShowUnknownDevicesInHotkeyList";
        public const string ShowDisconnectedDevices = "ShowDisconnectedDevices";
        public const string ShowDPDeviceIconInTray = "ShowDPDeviceIconInTray";
        public const string UpdateNotificationsEnabled = "UpdateNotificationsEnabled";
        public const string VolumeControlShow = "VolumeControlShow";
        public const string VolumeControlScrollInEntireMenu = "VolumeControlScrollInEntireMenu";
        public const string VolumeControlDivisibleByNumber = "VolumeControlDivisibleByNumber";
    }
}