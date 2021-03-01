using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    /// <summary>
    /// Can be used within a paragraph
    /// </summary>
    public abstract class RtfParagraphContentBase
    {
        internal RtfParagraphBase ParagraphInternal;

        public RtfParagraphBase Paragraph
        {
            get { return ParagraphInternal; }
        }
    }
}
