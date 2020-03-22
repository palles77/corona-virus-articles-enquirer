using System;
using System.Collections.Generic;
using System.Text;

namespace ESCommon.Rtf
{
    /// <summary>
    /// Can be used within an RtfDocument
    /// </summary>
    public abstract class RtfDocumentContentBase
    {
        protected RtfDocument _document;

        internal virtual RtfDocument DocumentInternal
        {
            get { return _document; }
            set { _document = value; }
        }
    }
}
