using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using LiveChat.Entity;

namespace LiveChat.Service
{
    /// <summary>
    /// 公共操作类
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// 产生一个随便数Key
        /// </summary>
        /// <param name="length"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string MakeUniqueKey(int length, string prefix)
        {
            int prefixLength = prefix == null ? 0 : prefix.Length;
            //Check.Require(length >= 8, "length must be greater than 0!");
            //Check.Require(length > prefixLength, "length must be greater than prefix length!");

            string chars = "1234567890";//abcdefghijklmnopqrstuvwxyz
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            if (prefixLength > 0)
            {
                sb.Append(prefix);
            }
            int dupCount = 0;
            int preIndex = 0;
            for (int i = 0; i < length - prefixLength; ++i)
            {
                int index = rnd.Next(0, chars.Length);
                if (index == preIndex)
                {
                    ++dupCount;
                }
                sb.Append(chars[index]);
                preIndex = index;
            }
            if (dupCount >= length - prefixLength - 2)
            {
                rnd = new Random();
                return MakeUniqueKey(length, prefix);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取图片类型
        /// </summary>
        /// <param name="extention"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormat(string extention)
        {
            extention = extention.Trim('.').ToLower();
            switch (extention)
            {
                case "jpg":
                    return ImageFormat.Jpeg;
                case "gif":
                    return ImageFormat.Gif;
                case "png":
                    return ImageFormat.Png;
                default:
                    return ImageFormat.Bmp;
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(Image originalImage, string thumbnailPath, int width, int height, string mode)
        {
            //System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode.ToUpper())
            {
                case "HW"://指定高宽缩放（可能变形）
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        #region 保存文件

        /// <summary>
        /// 用户端上传图片
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileContent"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string[] UploadData(UploadType type, FileType fileType, byte[] fileContent, string extension)
        {
            extension = extension.Trim('.').ToLower();

            string root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
            string rpath = type == UploadType.User ? Path.Combine(root, "User") : Path.Combine(root, "Seat");
            string dir = Path.Combine(rpath, DateTime.Now.ToString("yyyy-MM-dd"));

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (fileType == FileType.Image)
            {
                string dir1 = Path.Combine(dir, "Thumbnails");
                if (!Directory.Exists(dir1))
                {
                    Directory.CreateDirectory(dir1);
                }

                string imageName = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                string path = Path.Combine(dir, imageName);
                string path1 = Path.Combine(dir1, imageName);

                using (MemoryStream ms = new MemoryStream(fileContent))
                {
                    ImageFormat fmt = CommonUtils.GetImageFormat(extension);
                    Image img = Image.FromStream(ms);
                    img.Save(path, fmt);

                    if (img.Height <= 50)
                    {
                        img.Save(path1, fmt);
                    }
                    else
                    {
                        CommonUtils.MakeThumbnail(img, path1, 50, 50, "h");
                    }
                }

                string imageUrl = string.Format("{0}{1}", ServiceConfig.FileUploadUrl, path.Replace(root, "").Replace("\\", "/"));
                string thumbnailImageUrl = string.Format("{0}{1}", ServiceConfig.FileUploadUrl, path1.Replace(root, "").Replace("\\", "/"));

                return new string[] { thumbnailImageUrl, imageUrl };
            }
            else
            {
                string fileName = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                string path = Path.Combine(dir, fileName);

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    fs.SetLength(0);
                    fs.Write(fileContent, 0, fileContent.Length);
                }

                string fileUrl = string.Format("{0}{1}", ServiceConfig.FileUploadUrl, path.Replace(root, "").Replace("\\", "/"));

                return new string[] { fileUrl };
            }
        }

        #endregion

    }
}
