using System;

namespace CovidLib
{
    public struct Article
    {
        public byte[] ZippedTitle { get; set; }
        public byte[] ZippedAbstract { get; set; }
        public byte[] ZippedContent { get; set; }
        public byte[] ZippedJson { get; set; }
    }
}
