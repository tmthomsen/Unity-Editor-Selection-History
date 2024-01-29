# Editor Selection History
This is a fork of the excellent [Editor-History](https://github.com/BedtimeDigitalGames/Unity-Editor-History) by Bedtime Digital Games.

Differences:
- Unity-Toolbar-Extender is now an optional extension (handled via a asmdef Version Define).
- Converted history back/forward keybinds to make use of Unity's editor shortcut system (defaults: Mouse3 / Mouse4)
- Fixed history not saving between editor sessions.

## Installation
Add this URL to your Unity Package manager as a git package
```https://github.com/tmthomsen/Unity-Editor-Selection-History.git```

> [!NOTE]
> If you want to make use of the history UI window, simply add [Unity-Toolbar-Extender](https://github.com/marijnz/unity-toolbar-extender) to your project, and the button will appear in the upper right corner of the editor.
