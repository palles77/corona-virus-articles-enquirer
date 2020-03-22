using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    public abstract class RtfTextBase : RtfParagraphContentBase
    {
        protected StringBuilder sb;

        public RtfTextBase()
        {
            sb = new StringBuilder();
        }

        public RtfTextBase(string text)
        {
            sb = new StringBuilder(text);
        }

        /// <summary>
        /// Appends text to the current object.
        /// </summary>
        /// <param name="text">Text to append.</param>
        public void AppendText(string text)
        {
            sb.Append(text);
        }
    }
}