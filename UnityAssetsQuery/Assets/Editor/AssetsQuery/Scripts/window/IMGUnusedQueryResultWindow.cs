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
        public static void ShowWindow(List<string> unusedGuidList)
        {
            if (unusedGuidList == null || unusedGuidList.Count <= 0)
            {
                return;
            }

            m_guidList = unusedGuidList;
            m_guidDeleteFlag = new List<bool>(m_guidList.Count);
            for (var i = 0; i < m_guidList.Count; i++)
            {
                m_guidDeleteFlag.Add(true);
            }

            var window = GetWindow<IMGUnusedQueryResultWindow>();
            window.titleContent = new GUIContent(LanguageMgr.Read("img_query_result"));
            window.Focus();
        }

        #region window字段

        private static List<string> m_guidList;

        /// <summary>
        /// 删除标志位
        /// </summary>
        private static List<bool> m_guidDeleteFlag;

        private Vector2 m_scrollPos = Vector2.zero;

        #endregion

        private void OnGUI()
        {
            if (m_guidList == null || m_guidDeleteFlag == null || m_guidList.Count != m_guidDeleteFlag.Count)
            {
                Close();
                return;
            }

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
                m_guidDeleteFlag[i] = EditorGUILayout.ToggleLeft(LanguageMgr.Read("toggle_delete"), m_guidDeleteFlag[i],
                    GUILayout.Width(80f));
                EditorGUILayout.ObjectField("", asset, typeof(TextAsset), false);
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button(LanguageMgr.Read("button_sure_delete")))
            {
                DoDeleteOperate();
            }
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        private void DoDeleteOperate()
        {
            for (var i = 0; i < m_guidList.Count; i++)
            {
                var guid = m_guidList[i];
                var delete = m_guidDeleteFlag[i];
                if (delete)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        AssetDatabase.DeleteAsset(path);
                        m_guidList.RemoveAt(i);
                        m_guidDeleteFlag.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (m_guidList.Count <= 0)
            {
                Close();
            }

            EditorUtility.DisplayDialog(LanguageMgr.Read("delete_over_title"),
                LanguageMgr.Read("delete_over_content"), "OK");
        }
    }
}