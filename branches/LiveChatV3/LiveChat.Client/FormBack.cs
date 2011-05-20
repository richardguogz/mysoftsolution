
/****************************************
 * 背景窗口
 * 该窗口将以整个屏幕为backGroundImage
 * 其大小为屏幕大小
 * -------------------------------------
 *             by zhouyinhui 2006_6_26
 ****************************************/


using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LiveChat.Client
{
    public partial class FormBack : Form
    {
        public event CallbackEventHandler Callback;

        /// <summary>
        /// 保存整个屏幕的图像
        /// </summary>
        private Bitmap screenImage = null;

        /// <summary>
        /// 用于确定最终捕获图像的窗口
        /// </summary>
        private FormCatcher catcher = new FormCatcher();

        /// <summary>
        /// 用于记录catcher的位置
        /// </summary>
        private Point catcherLoc = Point.Empty;

        /// <summary>
        /// 标记鼠标左键是否按下
        /// </summary>
        private bool isMouseLeftDown = false;



        public FormBack()
        {
            InitializeComponent();
        }

        private void FormBack_Load(object sender, EventArgs e)
        {
            string path = ClientUtils.GetFullPath("cur.cur");
            if (File.Exists(path))
                this.Cursor = new Cursor(new FileStream(path, FileMode.Open));

            //设置位置和背景
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.screenImage = this.GetScreenImage();
            this.BackgroundImage = this.screenImage;

            //加载捕获窗口
            this.catcher.Opacity = 0.4;
            this.catcher.Visible = false;
            this.catcher.MouseDown += new MouseEventHandler(catcher_MouseDown);
            this.catcher.MouseLeave += new EventHandler(catcher_MouseLeave);
            this.catcher.DoubleClick += new EventHandler(catcher_DoubleClick);
            this.catcher.SizeChanged += new EventHandler(catcher_SizeChanged);
            this.catcher.MouseMove += new MouseEventHandler(catcher_MouseMove);
            this.catcher.VisibleChanged += new EventHandler(catcher_VisibleChanged);
            this.catcher.LocationChanged += new EventHandler(catcher_LocationChanged);

        }

        void catcher_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.catcher.Visible)
            {
                this.label_CatcherWidth.Text = "宽度     0";
                this.label_CatcherHeight.Text = "高度     0";
            }
        }

        void catcher_MouseMove(object sender, MouseEventArgs e)
        {

            int d = 7;
            if (e.X < d)
            {

                if (e.Y < d)
                {
                    this.catcher.Cursor = Cursors.SizeNWSE;//左上

                }
                else if (Math.Abs(e.Y - this.catcher.Height) < d)
                {
                    this.catcher.Cursor = Cursors.SizeNESW;//左下
                }
                else
                {
                    this.catcher.Cursor = Cursors.SizeWE;//左
                }
            }
            else if (Math.Abs(e.X - this.catcher.Width) < d)
            {

                if (e.Y < d)
                {
                    this.catcher.Cursor = Cursors.SizeNESW;//右上
                }
                else if (Math.Abs(e.Y - this.catcher.Height) < d)
                {
                    this.catcher.Cursor = Cursors.SizeNWSE;//右下
                }
                else
                {
                    this.catcher.Cursor = Cursors.SizeWE;//右
                }
            }
            else
            {
                if (e.Y < d || Math.Abs(e.Y - this.catcher.Height) < d)
                {
                    this.catcher.Cursor = Cursors.SizeNS;//中上. 中下
                }
                else
                {
                    this.catcher.Cursor = Cursors.SizeAll; //非边缘区域

                    if (this.catcher.isMouseLeftDown)
                    {//按下鼠标左键并拖动窗口,改变窗口位置
                        this.catcher.Location = new Point(Cursor.Position.X - this.catcher.dxCursorToWindow,
                                                  Cursor.Position.Y - this.catcher.dyCursorToWindow);
                    }
                }
            }

        }

        void catcher_LocationChanged(object sender, EventArgs e)
        {
            this.label_CatcherLoc.Text = "位置  X: " + this.catcher.Location.X + ", Y: " + this.catcher.Location.Y;
        }

        void catcher_SizeChanged(object sender, EventArgs e)
        {
            this.label_CatcherWidth.Text = "宽度     " + this.catcher.Width;
            this.label_CatcherHeight.Text = "高度     " + this.catcher.Height;
        }

        void catcher_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.catcherLoc = Point.Empty;
                this.isMouseLeftDown = false;

                this.catcher.Visible = false;
            }
        }

        void catcher_DoubleClick(object sender, EventArgs e)
        {
            Bitmap bmpCatched = new Bitmap(this.catcher.Width, this.catcher.Height);
            Graphics g = Graphics.FromImage(bmpCatched);
            g.CopyFromScreen(this.catcher.Location, new Point(0, 0), this.catcher.ClientRectangle.Size);
            //Clipboard.SetImage(bmpCatched);

            //if (MessageBox.Show("已经复制导剪切板") == DialogResult.OK)
            //{
            //    Application.Exit();
            //}

            this.Close();
            if (Callback != null) Callback(bmpCatched);
        }

        void catcher_MouseLeave(object sender, EventArgs e)
        {
            if (!this.isMouseLeftDown)
            {
                this.catcherLoc = Point.Empty;
                this.isMouseLeftDown = false;
            }
        }

        /// <summary>
        /// 获取屏幕图像
        /// </summary>
        private Bitmap GetScreenImage()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                    Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            g.Dispose();

            return bmp;
        }

        private void FormBack_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!this.catcher.Visible)
                {
                    //Application.Exit();

                    this.Close();
                    if (Callback != null) Callback(null);
                }
                else
                {
                    this.catcher.Visible = false;
                    this.catcherLoc = Point.Empty;
                    this.isMouseLeftDown = false;
                }
            }
            else if (e.Button == MouseButtons.Left && !this.catcher.Visible)
            {
                this.catcherLoc = e.Location;
                this.isMouseLeftDown = true;
            }
        }


        private void FormBack_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.catcherLoc = Point.Empty;
                this.isMouseLeftDown = false;
            }
        }

        private void FormBack_MouseMove(object sender, MouseEventArgs e)
        {


            if (!this.catcher.Visible)
            {
                this.label_CatcherLoc.Text = "位置  X: " + e.X + ", Y: " + e.Y;
            }


            if (this.isMouseLeftDown)
            {
                Rectangle rect = new Rectangle();
                //改进这里
                rect.X = e.X > this.catcherLoc.X ? this.catcherLoc.X : e.X;
                rect.Y = e.Y > this.catcherLoc.Y ? this.catcherLoc.Y : e.Y;
                rect.Width = Math.Abs(e.X - this.catcherLoc.X);
                rect.Height = Math.Abs(e.Y - this.catcherLoc.Y);

                this.catcher.Bounds = rect;
                if (!this.catcher.Visible)
                {
                    this.catcher.Show(this);
                }

            }
        }

        private void FormBack_MouseClick(object sender, MouseEventArgs e)
        {

        }



        private void panel_Info_MouseEnter(object sender, EventArgs e)
        {
            Panel pl = (Panel)sender;
            if (pl.Right < this.Width / 2)
            {
                pl.Location = new Point(this.Width - pl.Width - 20, pl.Location.Y);
            }
            else
            {
                pl.Location = new Point(10, 10);
            }
        }


    }


}