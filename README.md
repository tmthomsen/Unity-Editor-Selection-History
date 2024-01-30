# Editor Selection History
This is a fork of the excellent [Editor-History](https://github.com/BedtimeDigitalGames/Unity-Editor-History) by Bedtime Digital Games.

*Navigate between previously selected gameobjects or assets with rebindable shortcuts.*

![Imgur](https://i.imgur.com/HcHr0GP.gif)

*Features a jump list to navigate to a specific selection history element (requires Unity-Toolbar-Extender).*

![Imgur](https://i.imgur.com/pIU6KtL.png)

 Differences:
- Unity-Toolbar-Extender is now an optional extension.
- Converted history back/forward keybinds to make use of Unity's editor shortcut system (default binds: Mouse3 / Mouse4)
- History is now saved between editor sessions.


## Installation
Add this URL to your Unity Package Manager as a Git package: 
```https://github.com/tmthomsen/Unity-Editor-Selection-History.git```

> [!NOTE]
> If you want to make use of the history UI window, simply add [Unity-Toolbar-Extender](https://github.com/marijnz/unity-toolbar-extender) to your project as well, and the UI button will appear in the upper right corner of the editor next to the "Undo History" button.
