using AssetsQuery.Scripts.func;
using AssetsQuery.Scripts.MultiLanguage;
using UnityEditor;
using UnityEngine;

namespace AssetsQuery.Scripts.window
{
    /// <summary>
    /// 工具窗口
    /// </summary>
    public class AssetsQueryWindow : EditorWindow
    {
        /// <summary>
        /// 打开查找工具
        /// </summary>
        [MenuItem("Window/AssetsQuery", false, 0)]
        static void Init()
        {
            var window = GetWindow(typeof(AssetsQueryWindow));
            window.titleContent = new GUIContent(LanguageMgr.Read("main_window_title"));
            window.Focus();
        }


        #region ui逻辑

        private void OnGUI()
        {
            #region 图片工具

            if (GUILayout.Button(LanguageMgr.Read("btn_text_clear_cache")))
            {
                EditorPrefs.DeleteKey(AssetsQueryGlobalConst.ImageUseMonitorSaveKey);
                EditorUtility.DisplayDialog("清除缓存", "清除缓存成功", "OK");
            }
            if (GUILayout.Button("查询未使用的图片"))
            {
                FuncFindUnusedIMG.Start();
            }

            #endregion
        }

        #endregion
    }
}