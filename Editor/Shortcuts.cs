using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace TMT.EditorSelectionHistory
{
    public class Shortcuts
    {
        [ClutchShortcut("Selection History/Backward", KeyCode.Mouse3)]
        private static void Backward(ShortcutArguments args)
        {
            if (args.stage == ShortcutStage.Begin)
                EditorSelectionHistory.Navigate(NavigationDirection.Backward);
        }

        [ClutchShortcut("Selection History/Forward", KeyCode.Mouse4)]
        private static void Forward(ShortcutArguments args)
        {
            if (args.stage == ShortcutStage.Begin)
                EditorSelectionHistory.Navigate(NavigationDirection.Forward);
        }

#if EDITOR_SELECTION_HISTORY_TOOLBAR_EXTENDER_PRESENT
        [ClutchShortcut("Selection History/Open History Modal")]
        private static void ShowModal(ShortcutArguments args)
        {
            if (args.stage == ShortcutStage.Begin)
                EditorSelectionHistoryWindow.ShowModalWindow();
        }
#endif
    }
}
