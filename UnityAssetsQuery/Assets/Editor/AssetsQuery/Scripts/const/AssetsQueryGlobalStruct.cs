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

    [Serializable]
    internal struct RefQueryData
    {
        public RelativeDirectoryData directoryData;
        public QueryWhat queryWhat;
    }
    
    /// <summary>
    /// 图片查询数据
    /// </summary>
    [Serializable]
    internal struct IMGQueryData
    {
        /// <summary>
        /// 图片根目录数据
        /// </summary>
        public RelativeDirectoryData imageRootDirectory;
        /// <summary>
        /// 查询数据
        /// </summary>
        public RefQueryData[] queryDatas;
    }
}