﻿using System.Collections.Generic;

namespace Native.Csharp.App.Config
{
    /// <summary>
    /// 群管理配置类
    /// </summary>
    public class SysConfig
    {
        /// <summary>
        /// 如果指定群，输入群id
        /// </summary>
        public List<long> groupIds { get; set; }
       
        /// <summary>
        /// 数据库连接
        /// </summary>
        public string dbConnectionString { get; set; }
    }
}