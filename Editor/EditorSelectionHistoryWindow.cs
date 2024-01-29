#if EDITOR_SELECTION_HISTORY_TOOLBAR_EXTENDER_PRESENT
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

namespace TMT.EditorSelectionHistory
{
    internal class EditorSelectionHistoryWindow : EditorWindow
    {
        [SerializeField]
        private Vector2 _scroll;
        private bool _clickedHistoryEntry;
        private bool _isModal;

        private GUIStyle _listStyle;
        private GUIContent _popoutButtonContent;

        private const int ENTRY_HEIGHT = 16;
        private const int TOOLBAR_BUTTON_SPACE = 2;
        private const string TOOLBAR_BUTTON_ICON = "d_UnityEditor.AnimationWindow";
        private const string POPOUT_ICON = "d_ScaleTool On";
        private const string TOOLBAR_BUTTON_TITLE = "Selection History";

        private static Rect _toolbarButtonScreenRect;

        [InitializeOnLoadMethod]
        static void InitializeOnLoad() => ToolbarExtender.RightToolbarGUI.Add(DrawToolbarButton);

        private void OnEnable()
        {
            _listStyle = null;
            _popoutButtonContent = EditorGUIUtility.TrTextContentWithIcon(string.Empty, "Pop Out", POPOUT_ICON);
            titleContent = EditorGUIUtility.TrTextContentWithIcon(TOOLBAR_BUTTON_TITLE, TOOLBAR_BUTTON_ICON);
            EditorSelectionHistory.OnHistoryUpdated += OnHistoryChanged;
            Selection.selectionChanged += Repaint;
        }

        private void OnDisable()
        {
            EditorSelectionHistory.OnHistoryUpdated -= OnHistoryChanged;
            Selection.selectionChanged -= Repaint;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            DrawModalToolbar();
            DrawList();
            EditorGUILayout.EndVertical();
        }

        private static void DrawToolbarButton()
        {
            var icon = EditorGUIUtility.IconContent(TOOLBAR_BUTTON_ICON);
            GUIContent buttonText = new GUIContent(icon.image, TOOLBAR_BUTTON_TITLE);
            GUIStyle buttonStyle = EditorStyles.toolbarButton;

            GUILayout.FlexibleSpace();
            var rect = GUILayoutUtility.GetRect(buttonText, buttonStyle, GUILayout.Width(32));
            var clicked = GUI.Button(rect, buttonText, buttonStyle);
            _toolbarButtonScreenRect = GUIUtility.GUIToScreenRect(rect);

            if (clicked)
                ShowModalWindow();

            GUILayout.Space(TOOLBAR_BUTTON_SPACE);
        }

        public static void ShowModalWindow()
        {
            var window = CreateInstance<EditorSelectionHistoryWindow>();
            var size = new Vector2(250, 300);
            var pos = _toolbarButtonScreenRect;
            pos.x -= size.x - pos.width - TOOLBAR_BUTTON_SPACE;
            window._scroll = Vector2.up * int.MaxValue;
            window._isModal = true;
            window.ShowAsDropDown(pos, size);
        }

        private void ShowPopoutWindow()
        {
            var window = CreateInstance<EditorSelectionHistoryWindow>();
            var rect = position;
            rect.size = rect.size * 1.2f;
            window.Show();
            window.position = rect;
        }

        private void DrawModalToolbar()
        {
            if (!_isModal)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(TOOLBAR_BUTTON_TITLE, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(_popoutButtonContent, EditorStyles.toolbarButton))
            {
                ShowPopoutWindow();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnHistoryChanged(int location)
        {
            if (!_clickedHistoryEntry)
            {
                _scroll.y = (location * ENTRY_HEIGHT) - ENTRY_HEIGHT;
            }
            Repaint();
        }

        private void DrawList()
        {
            var selectedColor = new Color(.6f, .6f, .6f, 1f);
            var orgBGColor = GUI.backgroundColor;

            _scroll = GUILayout.BeginScrollView(_scroll, GUIStyle.none, GUI.skin.verticalScrollbar);
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < EditorSelectionHistory.HistoryObjects.Count; i++)
            {
                var entry = EditorSelectionHistory.HistoryObjects[i];
                if (!entry.Exists)
                {
                    GUI.enabled = false;
                }

                if (i == EditorSelectionHistory.Location)
                {
                    GUI.backgroundColor = selectedColor;
                }

                if (GUILayout.Button(entry.GUIContent, ListStyle))
                {
                    _clickedHistoryEntry = true;
                    EditorSelectionHistory.SetSelection(i);
                    _clickedHistoryEntry = false;
                }
                GUI.backgroundColor = orgBGColor;
                GUI.enabled = true;
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private GUIStyle ListStyle
        {
            get
            {
                if (_listStyle != null)
                {
                    return _listStyle;
                }

                _listStyle = new GUIStyle(EditorStyles.toolbarButton)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fixedHeight = ENTRY_HEIGHT,
                };
                return _listStyle;
            }
        }
    }
}
#endif