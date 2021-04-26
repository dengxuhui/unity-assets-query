using AssetsQuery.Scripts.func;
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
            GetWindow(typeof(AssetsQueryWindow));
        }


        #region ui逻辑

        private void OnGUI()
        {
            #region 图片工具

            if (GUILayout.Button("查询未使用的图片"))
            {
                FuncFindUnusedIMG.Start();
            }
            

            #endregion
        }
 
        #endregion
    }
}