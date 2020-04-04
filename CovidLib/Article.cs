using System;

namespace CovidLib
{
    [Serializable]
    public struct Article
    {
        public byte[] ZippedArchiveSetFileName { get; set; }
        public byte[] ZippedTitle { get; set; }
        public byte[] ZippedAbstract { get; set; }
    }
}
