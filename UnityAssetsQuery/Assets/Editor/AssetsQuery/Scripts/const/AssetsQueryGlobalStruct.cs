using System;
using AssetsQuery.Scripts.tools;

namespace AssetsQuery.Scripts
{
    /// <summary>
    /// 相对路径数据
    /// </summary>
    [Serializable]
    internal struct RelativeDirectoryData
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string directoryPath;
        /// <summary>
        /// 相对路径类型
        /// </summary>
        public RelativeType relativeType;
    }

    /// <summary>
    /// 白名单数据
    /// </summary>
    [Serializable]
    internal struct WhiteListData
    {
        /// <summary>
        /// 白名单类型
        /// </summary>
        public WhiteListType type;
        /// <summary>
        /// 白名单路径
        /// </summary>
        public string path;
    }
}