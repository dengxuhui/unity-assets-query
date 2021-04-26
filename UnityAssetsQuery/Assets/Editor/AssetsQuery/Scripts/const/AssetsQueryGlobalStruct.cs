using System;
using AssetsQuery.Scripts.tools;

namespace AssetsQuery.Scripts
{
    /// <summary>
    /// 相对路径数据
    /// </summary>
    [Serializable]
    internal struct RelativePathData
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 相对路径类型
        /// </summary>
        public RelativeType Type;
    }

    [Serializable]
    internal struct IMGRefQueryData
    {
        public RelativePathData PathData;
        public QueryWhat QueryWhat;
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
        public RelativePathData ImgPathData;
        /// <summary>
        /// 查询数据
        /// </summary>
        public IMGRefQueryData[] QueryDatas;
    }
}