# Little Rocket Lab Mods

A collection of MelonLoader mods for the game [Little Rocket Lab](https://store.steampowered.com/app/2451100/Little_Rocket_Lab/).

## Mods Included

### HigherStackSize
Increases the maximum stack size for all stackable items to 999.
- Automatically patches all inventory items when they're loaded or used
- Works with existing saves and new items
- **Warning:** If you remove this mod, loading a save will reset all item quantities above the default stack size to the default maximum (e.g., items with 50+ will be reduced to the vanilla stack limit)

### ScrollSplit
Split item stacks easily using the mouse scroll wheel.
- Scroll up on a stack to pick up items form hovered slot
- Scroll down to place items into empty slots or matching stacks

### RunFaster
Enables the run speed boost for faster player movement.
- Lightweight mod with no debug console or extra features
- Perfect for players who just want to move faster

### SaveAnyTime
Save your game at any time with a configurable hotkey (default: F5).
- Bypass normal save restrictions
- Configurable keybinding via MelonLoader preferences
- Useful for saving progress at critical moments

### SpeedControl
Adjust the game speed in real-time with configurable hotkeys.
- Increase speed: Numpad + (default)
- Decrease speed: Numpad - (default)
- Reset to normal: Numpad * (default)
- Speed range: 0.5x to 10x

### ToastyDestroysRocks
Enables the mech (Toasty) to destroy rocks with the hammer action.
- Destroy tier 1 rocks (stone, copper, iron, coal)
- Tier 2 and 3 rocks are protected from destruction

## Setup

### Prerequisites
- [Little Rocket Lab](https://store.steampowered.com/app/2451100/Little_Rocket_Lab/) (Steam)
- [MelonLoader](https://melonwiki.xyz/) v0.7.2-ci.2341 Open-Beta
  - This version is tested and confirmed working
  - Recommended for compatibility with these mods
  - Install in the game directory

### Building from Source

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd LittleRocketLabMods
   ```

2. **Configure game path**

   Copy `Directory.Build.props.user.example` to `Directory.Build.props.user`:
   ```bash
   cp Directory.Build.props.user.example Directory.Build.props.user
   ```

   Edit `Directory.Build.props.user` and set the `GamePath` to your game installation:
   ```xml
   <Project>
     <PropertyGroup>
       <GamePath>C:\Path\To\Your\Little Rocket Lab</GamePath>
     </PropertyGroup>
   </Project>
   ```

3. **Build the solution**
   ```bash
   dotnet build LittleRocketLabMods.sln
   ```

   The mods will automatically be copied to `<GamePath>\Mods\` after building.

### Manual Installation

If you just want to use pre-built mods:

1. Download the mod DLL files
2. Place them in `<GamePath>\Mods\` folder
3. Launch the game with MelonLoader

## Configuration

All mods with configurable hotkeys can be customized via the MelonLoader preferences file:

1. Run the game once with the mod(s) installed
2. Edit `<GamePath>\UserData\MelonPreferences.cfg`
3. Find the relevant section and modify the keybindings:

**SaveAnyTime**
- `[SaveAnyTime]` section
- `SaveKey` (default: F5)

**SpeedControl**
- `[SpeedControl]` section
- `IncreaseKey` (default: KeypadPlus)
- `DecreaseKey` (default: KeypadMinus)
- `ResetKey` (default: KeypadMultiply)

