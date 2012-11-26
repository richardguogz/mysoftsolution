using System.IO;
using System.IO.Compression;
using SevenZip.Compression.LZMA;
using SharpZip.Zip.Compression;
using SharpZip.Zip.Compression.Streams;

namespace MySoft
{
    /// <summary>
    /// Compression Manager
    /// </summary>
    public abstract class CompressionManager
    {
        #region SharpZip

        /// <summary>
        /// SharpZipѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] CompressSharpZip(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            using (MemoryStream outStream = new MemoryStream())
            using (DeflaterOutputStream compressStream = new DeflaterOutputStream(outStream, new Deflater(Deflater.BEST_COMPRESSION)))
            {
                CompressStream(new MemoryStream(buffer), compressStream);
                return outStream.ToArray();
            }
        }

        /// <summary>
        /// SharpZip��ѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] DecompressSharpZip(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            using (MemoryStream inStream = new MemoryStream(buffer))
            using (InflaterInputStream unCompressStream = new InflaterInputStream(inStream))
            using (MemoryStream outStream = new MemoryStream())
            {
                DecompressStream(unCompressStream, outStream);
                return outStream.ToArray();
            }
        }

        #endregion

        #region GZip

        /// <summary>
        /// GZipѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] CompressGZip(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            using (MemoryStream outStream = new MemoryStream())
            using (GZipStream compressStream = new GZipStream(outStream, CompressionMode.Compress, true))
            {
                CompressStream(new MemoryStream(buffer), compressStream);
                return outStream.ToArray();
            }
        }

        /// <summary>
        /// GZip��ѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] DecompressGZip(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            using (MemoryStream inStream = new MemoryStream(buffer))
            using (GZipStream unCompressStream = new GZipStream(inStream, CompressionMode.Decompress, true))
            using (MemoryStream outStream = new MemoryStream())
            {
                DecompressStream(unCompressStream, outStream);
                return outStream.ToArray();
            }
        }

        #endregion

        #region Deflate

        /// <summary>
        /// Deflateѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] CompressDeflate(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            using (MemoryStream outStream = new MemoryStream())
            using (DeflateStream compressStream = new DeflateStream(outStream, CompressionMode.Compress, true))
            {
                CompressStream(new MemoryStream(buffer), compressStream);
                return outStream.ToArray();
            }
        }

        /// <summary>
        /// Deflate��ѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] DecompressDeflate(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            using (MemoryStream inStream = new MemoryStream(buffer))
            using (DeflateStream unCompressStream = new DeflateStream(inStream, CompressionMode.Decompress, true))
            using (MemoryStream outStream = new MemoryStream())
            {
                DecompressStream(unCompressStream, outStream);
                return outStream.ToArray();
            }
        }

        #endregion

        #region 7Zip

        /// <summary>
        /// 7Zipѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Compress7Zip(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            return SevenZipHelper.Compress(buffer);
        }

        /// <summary>
        /// 7Zip��ѹ��
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Decompress7Zip(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }

            return SevenZipHelper.Decompress(buffer);
        }

        #endregion

        /// <summary>
        /// ѹ����
        /// </summary>
        /// <param name="originalStream"></param>
        /// <param name="compressStream"></param>
        private static void CompressStream(Stream originalStream, Stream compressStream)
        {
            BinaryWriter writer = new BinaryWriter(compressStream);
            BinaryReader reader = new BinaryReader(originalStream);
            while (true)
            {
                byte[] buffer = reader.ReadBytes(1024);
                if (buffer == null || buffer.Length < 1)
                    break;
                writer.Write(buffer);
            }
            writer.Close();
        }

        /// <summary>
        /// ��ѹ����
        /// </summary>
        /// <param name="compressStream"></param>
        /// <param name="originalStream"></param>
        private static void DecompressStream(Stream compressStream, Stream originalStream)
        {
            BinaryReader reader = new BinaryReader(compressStream);
            BinaryWriter writer = new BinaryWriter(originalStream);
            while (true)
            {
                byte[] buffer = reader.ReadBytes(1024);
                if (buffer == null || buffer.Length < 1)
                    break;
                writer.Write(buffer);
            }
            writer.Close();
        }
    }
}
