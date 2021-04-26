using UnityEngine;

namespace AssetsQuery.Scripts.func
{
    /// <summary>
    /// 搜索未使用的图片
    /// </summary>
    internal static class FuncFindUnusedIMG
    {
        /// <summary>
        /// 搜寻没有使用的图片
        /// </summary>
        public static void Collect()
        {
            Debug.Log("Start collect");
            var rules = AssetsQueryAssetManager.GetRules();
        }
    }
}