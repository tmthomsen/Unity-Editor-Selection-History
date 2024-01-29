using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;

namespace TMT.EditorSelectionHistory
{
    [InitializeOnLoad]
	internal class EditorSelectionHistory
	{
		static EditorSelectionHistory()
		{
			Selection.selectionChanged += OnSelectionChanged;
			EditorSceneManager.sceneOpened += OnSceneChanged;
			EditorSceneManager.sceneSaved += OnSceneSaved;
			AddHistoryObject(Selection.activeObject);
		}

		public static event Action<int> OnHistoryUpdated;

		public static bool IsNavigating { get; private set; }

		public static List<SelectionHistoryObject> HistoryObjects => History.HistoryObjects;

		public static int Location
		{
			get => IsNavigating ? History.Location : HistoryObjects.Count - 1;

			private set
			{
				History.Location = ClampLocation(value);
				OnHistoryUpdated?.Invoke(Location);
			}
		}

        private static History History => History.instance;

        private static readonly string _gameViewTypeName = "UnityEditor.GameView";
        private static bool _selectionWasSet;


        public static void SetSelection(int location, bool setActive = true)
		{
			EditorWindow focus = EditorWindow.focusedWindow;
			if (Application.isPlaying && focus != null)
			{
				var focusType = focus.GetType();
				if (focusType.FullName == _gameViewTypeName)
				{
					return;
				}
			}

			if (HistoryObjects.Count > 0)
			{
				IsNavigating = setActive;
				_selectionWasSet = setActive;
				Selection.activeObject = HistoryObjects[ClampLocation(location)].Selection;
				Location = location;
			}
		}

        public static void Navigate(NavigationDirection direction, int amount = 1)
		{
			var totalAmount = amount * (direction == NavigationDirection.Forward ? 1 : -1);
			if (!IsNavigating)
			{
				SetSelection(HistoryObjects.Count - 1);
			}
			else if (Location - ClampLocation(Location + totalAmount) != 0)
			{
				var target = HistoryObjects[ClampLocation(Location + totalAmount)];
				if (!target.Exists)
				{
					Navigate(direction, amount + 1);
				}
				else
				{
					SetSelection(Location + totalAmount);
				}
			}
		}

		private static int ClampLocation(int location)
		{
			location = location >= HistoryObjects.Count - 1 ? HistoryObjects.Count - 1 : location;
			location = location < 0 ? 0 : location;
			return location;
		}

		private static void ClearInFront()
		{
			for (int i = HistoryObjects.Count - 1; i > Location; i--)
			{
				HistoryObjects.RemoveAt(i);
			}
		}

		private static void AddHistoryObject(UnityObject obj)
		{
			if (obj == null)
			{
				SetSelection(HistoryObjects.Count - 1, false);
				return;
			}

			var entry = new SelectionHistoryObject(obj);
			ClearInFront();
			if (HistoryObjects.Count > 0 && HistoryObjects[ClampLocation(Location)].Selection == entry.Selection)
			{
				return;
			}

			HistoryObjects.Add(entry);
			Location = HistoryObjects.Count - 1;
			History.Save();
		}

		private static void OnSelectionChanged()
		{
			UnityObject selection = Selection.activeObject;
			IsNavigating = selection != null;

			if (_selectionWasSet)
			{
				_selectionWasSet = false;
				return;
			}

			AddHistoryObject(selection);
		}

		private static void OnSceneChanged(Scene scene, OpenSceneMode mode)
		{
			for (var i = 0; i < HistoryObjects.Count; i++)
			{
				HistoryObjects[i] = HistoryObjects[i].UpdateSelection(scene.name);
			}
		}

		private static void OnSceneSaved(Scene scene)
		{
			for (var i = 0; i < HistoryObjects.Count; i++)
			{
				HistoryObjects[i] =	HistoryObjects[i].UpdateName();
			}
		}
    }
}