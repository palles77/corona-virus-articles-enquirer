using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    /// <summary>
    /// Specifies that RtfWriter must ignore a member of a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal class RtfIgnoreAttribute : Attribute
    {
    }
}