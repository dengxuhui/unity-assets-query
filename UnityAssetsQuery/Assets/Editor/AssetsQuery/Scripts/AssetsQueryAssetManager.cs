using UnityEditor;
using UnityEngine;

namespace AssetsQuery.Scripts
{
    /// <summary>
    /// 资源查找工具资源管理器
    /// </summary>
    public static class AssetsQueryAssetManager
    {
        /// <summary>
        /// 资源查找工具规则
        /// </summary>
        private static readonly AssetsQueryRules Rules;
        
        static AssetsQueryAssetManager()
        {
            Rules = GetAsset<AssetsQueryRules>("Assets/Editor/AssetsQuery/AssetsQueryRules.asset");
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        internal static AssetsQueryRules GetRules()
        {
            return Rules;
        }
        
        private static T GetAsset<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            //没有就创建一个资源出来
            if (asset != null) return asset;
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            return asset;
        }
    }
}