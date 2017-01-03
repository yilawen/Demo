using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wood.Utilities
{
    public enum Role : int
    {
        /// <summary>
        /// 游客
        /// </summary>
        Guest = 0,

        /// <summary>
        /// 普通用户
        /// </summary>
        General = 1,

        /// <summary>
        /// 系统管理员
        /// </summary>
        Admin = 9,

        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdmin = 10,
    }
}