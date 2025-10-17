# Installation Guide

## Prerequisites
- Little Rocket Lab (Steam version)
- Windows PC
- MelonLoader v0.7.2-ci.2341 Open-Beta

## Step 1: Install MelonLoader

1. Download MelonLoader v0.7.2-ci.2341 Open-Beta from https://melonwiki.xyz/
2. Run the MelonLoader installer
3. Point the installer to your Little Rocket Lab game directory
   - Default location: `C:\Program Files (x86)\Steam\steamapps\common\Little Rocket Lab`
4. Launch the game once to generate the `Mods` folder
   - You should see a MelonLoader console window appear when the game starts

## Step 2: Install Mods

1. Download the mod DLL files from the [latest release](https://github.com/OGMods/LittleRocketLabMods/releases/latest)
2. Locate your game's `Mods` folder:
   - `<GamePath>\Mods\`
3. Place the desired mod files into the `Mods` folder
   - You can install individual mods or all of them together
   - **Important**: Place DLL files directly in the `Mods` folder, not in a subfolder
4. Launch the game to enjoy!

## Step 3: Configuration (Optional)

After running the game once with the mods installed, you can customize settings:

1. Close the game
2. Navigate to `<GamePath>\UserData\MelonPreferences.cfg`
3. Open the file with a text editor (Notepad, VS Code, etc.)
4. Find the section for the mod you want to configure:
   - `[SaveAnyTime]` - Change the save hotkey
   - `[SpeedControl]` - Change speed control hotkeys
   - `[HigherStackSize]` - Configure stack size behavior
5. Edit the values as desired
6. Save the file and launch the game

### Example Configuration

```ini
[SaveAnyTime]
# Key to press for saving the game.
SaveKey = "F5"

[SpeedControl]
# Key to increase Speed Control.
IncreaseKey = "KeypadPlus"
# Key to decrease Speed Control.
DecreaseKey = "KeypadMinus"
# Key to reset Speed Control to 1.0.
ResetKey = "KeypadMultiply"

[HigherStackSize]
# WARNING: Lowering stack size or multiplier may cause losing items with existing save files that have larger stacks!
WARNING = "Lowering stack size or multiplier may cause losing items with existing save files that have larger stacks!"
# If true, multiply the original stack size. If false, set to a fixed value.
UseMultiplier = false
# The fixed maximum stack size for stackable items when UseMultiplier is false. Default is 999.
StackSize = 999
# Multiply the original stack size by this value when UseMultiplier is true. Default is 10x.
Multiplier = 10.0
```

## Compatibility

- **Game**: Little Rocket Lab (Steam)
- **Tested with**: MelonLoader v0.7.2-ci.2341 Open-Beta
- **Platform**: Windows

## Troubleshooting

### Mods don't load

1. **Verify MelonLoader is installed correctly**
   - You should see a console window when launching the game
   - The console should show "MelonLoader" and list loaded mods

2. **Check DLL file location**
   - Files must be in `<GamePath>\Mods\`, not in a subfolder
   - Example correct path: `C:\...\Little Rocket Lab\Mods\HigherStackSize.dll`
   - Example wrong path: `C:\...\Little Rocket Lab\Mods\MyMods\HigherStackSize.dll`

3. **Check the MelonLoader console for errors**
   - Red text indicates errors
   - Look for messages about specific mods failing to load

4. **Ensure you're using the correct MelonLoader version**
   - These mods are tested with v0.7.2-ci.2341 Open-Beta
   - Other versions may not work correctly

### Mods load but don't work

1. **Check keybindings**
   - Default keys might conflict with other software
   - Edit `MelonPreferences.cfg` to change hotkeys

2. **Verify mod compatibility**
   - Some mods might conflict with each other
   - Try loading mods one at a time to isolate issues

3. **Check for game updates**
   - Game updates may break mod compatibility
   - Check the [releases page](https://github.com/OGMods/LittleRocketLabMods/releases) for updates

### Still having issues?

- Check existing [GitHub Issues](https://github.com/OGMods/LittleRocketLabMods/issues)
- Create a new issue with:
  - Your MelonLoader version
  - Your game version
  - Which mods you're using
  - The error message from the MelonLoader console

## Updating Mods

To update to a newer version:

1. Download the latest release
2. Replace the old DLL files in your `Mods` folder with the new ones
3. Launch the game

Your configuration in `MelonPreferences.cfg` will be preserved.