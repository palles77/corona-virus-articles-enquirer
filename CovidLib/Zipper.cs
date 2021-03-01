using System.IO;
using System.IO.Compression;
using System.Text;

namespace CovidLib
{
    public class Zipper
    {
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            byte[] result;

            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionLevel.Optimal))
                    {
                        CopyTo(msi, gs);
                    }

                    result =  mso.ToArray();
                }
            }

            return result;
        }

        public static string Unzip(byte[] bytes)
        {
            string result;
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        CopyTo(gs, mso);
                    }

                    result = Encoding.UTF8.GetString(mso.ToArray());
                }
            }

            return result;
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}
