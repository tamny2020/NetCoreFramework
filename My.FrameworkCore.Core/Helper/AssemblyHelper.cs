using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace My.FrameworkCore.Core.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyHelper
    {
        /// <summary>
        /// 通过程序集的名称加载程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

    }
}
