using System.IO;
using System.IO.Compression;

namespace MySoft.Remoting.CompressionSink
{
    /// <summary>
    /// ѹ������
    /// </summary>
    public enum ZipSinkType
    {
        /// <summary>
        /// ��ѹ��
        /// </summary>
        None,
        /// <summary>
        /// Deflateѹ��
        /// </summary>
        Deflate,
        /// <summary>
        /// GZipѹ��
        /// </summary>
        GZip
    }

    /// <summary>
    /// ѹ��������
    /// </summary>
    public class ZipHelper
    {
        /// <summary>
        /// refactor  by zendy
        /// </summary>
        /// <param name="inStream"></param>
        /// <returns></returns>
        public static Stream GetCompressedStreamCopy(Stream inStream, ZipSinkType type)
        {

            //using sharpziplib
            //			 MemoryStream outStream = new MemoryStream();
            //			 ICSharpCode.SharpZipLib.Zip.Compression.Deflater mydeflater = new ICSharpCode.SharpZipLib.Zip.Compression.Deflater(ICSharpCode.SharpZipLib.Zip.Compression.Deflater.DEFAULT_COMPRESSION);
            //			 
            //			 ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream  deflateStream =  new ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream(outStream,mydeflater);
            //			 CopyStream(inStream,deflateStream);
            //			deflateStream.Finish();
            //			outStream.Seek(0,SeekOrigin.Begin);


            //			MemoryStream outStream = new MemoryStream();
            //			Deflater mDeflater = new Deflater(Deflater.BEST_COMPRESSION);
            //			DeflaterOutputStream compressStream = new DeflaterOutputStream(outStream,mDeflater);
            //
            //			byte[] buf = new Byte[4096];
            //			int cnt = inStream.Read(buf,0,4096);
            //			while (cnt>0) 
            //			{
            //				compressStream.Write(buf,0,cnt);
            //				cnt = inStream.Read(buf,0,4096);
            //			}
            //			compressStream.Finish();

            //��¼16���Ƶ���
            //			System.Text.StringBuilder str=new System.Text.StringBuilder();
            //
            //			byte[] buf = new Byte[4096];
            //			int cnt = outStream.Read(buf,0,4096);
            //			while (cnt>0) {
            //							for(int i=0;i<cnt;i++)
            //							{
            //							
            //								str.Append( buf[i].ToString("X2"));
            //								//buf[i]+=1;
            //							}
            //				
            //				 
            //				cnt = outStream.Read(buf,0,4096);
            //			}

            //modify by zendy //������÷ǳ���Ҫ,����ᵼ�º���Sink�ڴ����streamʱʧ��,��ԭ����Դ���о�����Ϊû����������³�������ʧ��

            //outStream.Seek(0,SeekOrigin.Begin);
            //
            //			//inStream.Seek(0,SeekOrigin.Begin); 
            //System.Diagnostics.Debug.WriteLine("compress in mySink:" +outStream.Length.ToString() + " | "+ str.ToString());
            //			 
            //
            //if(inStream.Length==0) return inStream;

            MemoryStream outStream = new MemoryStream();
            Stream memoryStream = new MemoryStream();

            switch (type)
            {
                case ZipSinkType.Deflate:
                    {
                        memoryStream = new DeflateStream(inStream, CompressionMode.Compress);
                    }
                    break;
                case ZipSinkType.GZip:
                    {
                        memoryStream = new GZipStream(inStream, CompressionMode.Compress);
                    }
                    break;
            }

            CopyStream(memoryStream, outStream);

            // ���ܵ������·���
            // Crypto(inStream,outStream);

            //��¼16���Ƶ���
            //			System.Text.StringBuilder str=new System.Text.StringBuilder();
            //
            //			byte[] buf = new Byte[4096];
            //			int cnt = outStream.Read(buf,0,4096);
            //			while (cnt>0) {
            //							for(int i=0;i<cnt;i++)
            //							{
            //							
            //								str.Append( buf[i].ToString("X2"));
            //								//buf[i]+=1;
            //							}
            //				
            //				 
            //				cnt = outStream.Read(buf,0,4096);
            //			}
            //			System.Diagnostics.Debug.WriteLine("compress in mySink:" +outStream.Length.ToString() + " | "+ str.ToString());


            //ѹ����
            //			MemoryStream outStream1 = new MemoryStream();
            //			zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outStream1,zlib.zlibConst.Z_DEFAULT_COMPRESSION);
            //			
            //			if(outStream.Length==0) return outStream;
            //			CopyStream(outStream, outZStream);
            //			
            //			outStream1.Seek(0,SeekOrigin.Begin);

            System.Diagnostics.Debug.WriteLine("Compress in zipSink:" + outStream.Length.ToString());
            outStream.Seek(0, SeekOrigin.Begin);

            return outStream;
        }

        /// <summary>
        /// refactor  by zendy
        /// </summary>
        /// <param name="inStream"></param>
        /// <returns></returns>
        public static Stream GetUncompressedStreamCopy(Stream inStream, ZipSinkType type)
        {

            //			MemoryStream outStream = new MemoryStream();
            //			ICSharpCode.SharpZipLib.Zip.Compression.Inflater inflater = new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();
            //			 
            //			ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream umcompressStream = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream (inStream,inflater);
            //
            //			 
            //			 
            //			CopyStream(umcompressStream,outStream);
            //			outStream.Seek(0,SeekOrigin.Begin);

            //			InflaterInputStream unCompressStream = new InflaterInputStream(inStream); 
            //			MemoryStream outStream = new MemoryStream();
            //			int mSize;
            //			Byte[] mWriteData = new Byte[4096];
            //			while(true)
            //			{
            //				mSize = unCompressStream.Read(mWriteData, 0, mWriteData.Length);
            //				if (mSize > 0)
            //				{
            //					outStream.Write(mWriteData, 0, mSize);
            //				}
            //				else
            //				{
            //					break;
            //				}
            //			}
            //			unCompressStream.Close();
            //modify by zendy//������÷ǳ���Ҫ,����ᵼ�º���Sink�ڴ����streamʱʧ��,,��ԭ����Դ���о�����Ϊû����������³�������ʧ��
            //			outStream.Seek(0,SeekOrigin.Begin);
            //inStream.Seek(0,SeekOrigin.Begin);
            //if(inStream.==0) return inStream;

            MemoryStream outStream = new MemoryStream();
            Stream memoryStream = new MemoryStream();

            switch (type)
            {
                case ZipSinkType.Deflate:
                    {
                        memoryStream = new DeflateStream(inStream, CompressionMode.Decompress);
                    }
                    break;
                case ZipSinkType.GZip:
                    {
                        memoryStream = new GZipStream(inStream, CompressionMode.Decompress);
                    }
                    break;
            }

            CopyStream(memoryStream, outStream);

            // ���ܵ������·���
            // Decrypto(inStream, outStream);

            //��¼16���Ƶ���
            //			System.Text.StringBuilder str=new System.Text.StringBuilder();
            //
            //			byte[] buf = new Byte[4096];
            //			int cnt = outStream.Read(buf,0,4096);
            //			while (cnt>0) 
            //			{
            //				for(int i=0;i<cnt;i++)
            //				{
            //							
            //					str.Append( buf[i].ToString("X2"));
            //					//buf[i]+=1;
            //				}
            //				
            //				 
            //				cnt = outStream.Read(buf,0,4096);
            //			}
            //			System.Diagnostics.Debug.WriteLine("uncompress in mySink:" +outStream.Length.ToString() + " | "+ str.ToString());
            //			
            //			outStream.Seek(0,SeekOrigin.Begin); 
            //			if(outStream.Length==0) return outStream;
            //			MemoryStream outStream1 = new MemoryStream();

            System.Diagnostics.Debug.WriteLine("Uncompress in zipSink:" + outStream.Length.ToString());
            outStream.Seek(0, SeekOrigin.Begin);

            return outStream;
        }

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[4096];
            int len;
            while ((len = input.Read(buffer, 0, 4096)) > 0)
            {

                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static void Decrypto(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] mycode = System.Text.Encoding.Default.GetBytes("123456789");
            byte[] b1 = new byte[200];
            input.Read(b1, 0, mycode.Length);

            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static void Crypto(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] mycode = System.Text.Encoding.Default.GetBytes("123456789");
            output.Write(mycode, 0, mycode.Length);
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {

                output.Write(buffer, 0, len);
            }
            output.Flush();
        }
    }
}
