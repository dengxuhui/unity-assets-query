using System;
using System.Collections.Generic;
using AssetsQuery.Scripts.MultiLanguage;
using UnityEditor;
using UnityEngine;

// ReSharper disable All

namespace AssetsQuery.Scripts.window
{
    public class IMGUnusedQueryResultWindow : EditorWindow
    {
        private static IMGUnusedQueryResultWindow _showingWindow;

        public static void ShowWindow(List<string> unusedGuidList)
        {
            if (unusedGuidList == null || unusedGuidList.Count <= 0)
            {
                if (_showingWindow)
                {
                    _showingWindow.Close();
                    _showingWindow = null;
                }

                return;
            }

            m_guidList = unusedGuidList;
            var window = GetWindow<IMGUnusedQueryResultWindow>();
            window.titleContent = new GUIContent(LanguageMgr.Read("img_query_result"));
            window.Focus();
            _showingWindow = window;
        }

        #region window字段

        private static List<string> m_guidList;
        private Vector2 m_scrollPos = Vector2.zero;

        #endregion

        private void OnGUI()
        {
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
            for (var i = 0; i < m_guidList.Count; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(m_guidList[i]);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }

                var asset = AssetDatabase.LoadAssetAtPath<Texture>(path);
                if (asset == null) continue;
                GUILayout.BeginHorizontal();
                EditorGUILayout.ToggleLeft(LanguageMgr.Read("toggle_delete"), true, GUILayout.Width(80f));
                EditorGUILayout.ObjectField("", asset, typeof(TextAsset), false);
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button(LanguageMgr.Read("button_sure_delete")))
            {
            }
        }
    }
}