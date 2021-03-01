using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    /// <summary>
    /// Specifies that a member is an index and must be ignored if value is negative.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal class RtfIndexAttribute : Attribute
    {
    }
}