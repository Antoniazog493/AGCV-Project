# HidHide Integration

BetterJoy now supports **HidHide**, the modern successor to HidGuardian!

## What is HidHide?

HidHide is a modern, actively maintained driver that hides game controllers from other applications (like Steam) to prevent conflicts and double-input issues. It's the recommended replacement for the deprecated HidGuardian.

## Features

? **Auto-Detection**: BetterJoy automatically detects and uses HidHide if installed  
? **Fallback Support**: Falls back to HidGuardian if HidHide is not available  
? **Better Performance**: More efficient and reliable than HidGuardian  
? **Active Development**: Maintained by Nefarius, creator of ViGEm  
? **Steam Compatibility**: Prevents Steam from seeing Nintendo controllers  

## Installation

1. **Download HidHide** from the official repository:
   - https://github.com/nefarius/HidHide/releases
   - Download the latest `HidHide_X.X.X_x64.exe` installer

2. **Install HidHide**:
   - Run the installer as Administrator
   - Restart your computer if prompted

3. **Configure BetterJoy** (optional):
   - Open `App.config` in BetterJoy folder
   - Ensure `<add key="PreferHidHide" value="true"/>` is set (default)
   - Set `<add key="UseHIDG" value="false"/>` to disable legacy HidGuardian

4. **Run BetterJoy**:
   - BetterJoy will automatically detect HidHide
   - You'll see "HidHide detected and will be used (modern driver)" in the console

## How It Works

When a Nintendo controller connects:
1. BetterJoy adds itself to HidHide's **whitelist** (allowed apps)
2. BetterJoy adds the controller to HidHide's **blacklist** (hidden devices)
3. Only whitelisted apps (like BetterJoy, games using ViGEm) can see the controller
4. Other apps (Steam, JoyToKey, etc.) won't see the Nintendo controller

This prevents:
- Double input in games
- Steam's Nintendo controller support from interfering
- Other programs from grabbing the controller

## Configuration

### In App.config:

```xml
<!-- Prefer HidHide over HidGuardian (recommended) -->
<add key="PreferHidHide" value="true"/>

<!-- Legacy HidGuardian (set to false when using HidHide) -->
<add key="UseHIDG" value="false"/>

<!-- Clean up hidden devices on exit -->
<add key="PurgeAffectedDevices" value="false"/>
```

### Settings Explained:

- **PreferHidHide**: When `true`, BetterJoy will use HidHide if detected
- **UseHIDG**: Legacy HidGuardian support (keep `false` for HidHide)
- **PurgeAffectedDevices**: When `true`, unhides all devices on exit

## Troubleshooting

### "HidHide not detected" but it's installed
- Restart your computer after installing HidHide
- Make sure HidHide service is running: `sc query HidHide`
- Run BetterJoy as Administrator

### Controller still visible in Steam
- Open HidHide Configuration Client (installed with HidHide)
- Verify the controller is in the blacklist
- Verify BetterJoy.exe is in the whitelist
- Restart Steam

### Want to use HidGuardian instead
- Set `<add key="PreferHidHide" value="false"/>`
- Set `<add key="UseHIDG" value="true"/>`

### Manually configure HidHide
You can use the **HidHide Configuration Client** app to:
- View/edit whitelisted applications
- View/edit blacklisted devices
- Enable/disable HidHide globally

## Comparison: HidHide vs HidGuardian

| Feature | HidHide | HidGuardian |
|---------|---------|-------------|
| Status | ? Active | ?? Deprecated |
| Performance | ? Fast | ?? Slower |
| Reliability | ? High | ?? Medium |
| Windows 11 | ? Full support | ?? Limited |
| Easy config | ? GUI app | ? Web API only |
| Installation | ? Simple | ?? Complex |

## Links

- **HidHide Repository**: https://github.com/nefarius/HidHide
- **HidHide Releases**: https://github.com/nefarius/HidHide/releases
- **Documentation**: https://docs.nefarius.at/projects/HidHide/
- **Support**: https://discord.vigem.org/

## Migration from HidGuardian

If you're currently using HidGuardian:

1. Install HidHide (see Installation above)
2. Stop HidCerberus Service: `net stop HidCerberus`
3. Disable HidGuardian in BetterJoy: `UseHIDG = false`
4. Enable HidHide preference: `PreferHidHide = true`
5. Restart BetterJoy

BetterJoy will handle the rest automatically!

## Credits

- **HidHide**: Created by [Nefarius](https://github.com/nefarius)
- **BetterJoy Integration**: Added in .NET 8 migration
