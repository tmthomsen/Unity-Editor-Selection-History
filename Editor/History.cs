using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TMT.EditorSelectionHistory
{
    [FilePath("EditorSelectionHistory/History.log", FilePathAttribute.Location.PreferencesFolder)]
    internal class History : ScriptableSingleton<History>
    {
        private const int HISTORY_MAX = 1024;

        [SerializeField]
        private int _location;

        [SerializeField]
        private List<SelectionHistoryObject> _historyObjects = new List<SelectionHistoryObject>(32);

        public int Location
        {
            get => _location;
            set => _location = value;
        }

        public void Save()
        {
            if (_historyObjects.Count > HISTORY_MAX)
            {
                var toRemove = _historyObjects.Count - HISTORY_MAX;
                _historyObjects.RemoveRange(0, toRemove);
            }
            Save(true);
        }

        public List<SelectionHistoryObject> HistoryObjects => _historyObjects;
    }
}