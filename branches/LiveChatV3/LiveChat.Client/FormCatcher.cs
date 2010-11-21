/*********************************************
 * 
 * ���ڲ����ƶ�����Ĵ���
 * ������Ϊ����8��С����ľ���
 * 8��С����
 * 0����1����2
 * 3         4
 * 5����6����7
 * һ���϶��ñ�λ�úʹ�С
 * �������������ڵ�ͼ����ȷ�����������ͼ��
 * -----------------------------------------
 *                     by zhouyinhui 2006_6_26
 *********************************************/

/*
 * δ��������⣺
 * 
 * �϶�С�����Ըı��С
 * 
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace LiveChat.Client
{
    public partial class FormCatcher : Form
    {
        public FormCatcher()
        {
            InitializeComponent();
        }

        public bool isMouseLeftDown = false;
        /// <summary>
        /// ��갴��ʱ����봰��λ�ú������
        /// </summary>
        public int dxCursorToWindow = 0;
        /// <summary>
        /// ��갴��ʱ����봰��λ�ú������
        /// </summary>
        public int dyCursorToWindow = 0;


        private void FormCatcher_Load(object sender, EventArgs e)
        {
            this.Resize += new EventHandler(FormCatcher_Resize);
        }

        void FormCatcher_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        //���ƾ��α߿���8��С����
        private void FormCatcher_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                Rectangle rect = new Rectangle(1, 1, this.ClientRectangle.Width - 3, this.ClientRectangle.Height - 3);
                g.DrawRectangle(Pens.Red, rect);

                //����С����
                for (int i = 0; i <= 7; i++)
                {
                    g.FillRectangle(Brushes.Red, this.GetBoundsOfSmallBlock(i));
                }
            }
        }

        /// <summary>
        /// ��ȡָ����ŵ�С�����λ��
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Rectangle GetBoundsOfSmallBlock(int index)
        {
            int x =0;
            int y=0;
            int w=4;
            int h=4;

            switch (index)
            {
                case 0:
                case 3:
                case 5:
                    x = 0;
                    break;
                case 1:
                case 6:
                    x = this.ClientRectangle.Width / 2-3;
                    break;
                case 2:
                case 4:
                case 7:
                    x = this.ClientRectangle.Width - 4;
                    break;
                default:
                    throw new IndexOutOfRangeException("С������Ŵ���Ӧ���ڣ�0��7��");
            }

            switch (index)
            {
                case 0:
                case 1:
                case 2:
                    y = 0;
                    break;
                case 3:
                case 4:
                    y = this.ClientRectangle.Height / 2-3;
                    break;
                case 5:
                case 6:
                case 7:
                    y = this.ClientRectangle.Height - 4;
                    break;
                default:
                    throw new IndexOutOfRangeException("С������Ŵ���Ӧ���ڣ�0��7��");
            }

            return new Rectangle(x, y, w, h);
        }

        private void FormCatcher_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.isMouseLeftDown = true;
                this.dxCursorToWindow = Cursor.Position.X - this.Location.X;
                this.dyCursorToWindow = Cursor.Position.Y - this.Location.Y;
            }
        }

        private void FormCatcher_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.isMouseLeftDown = false;
                this.dxCursorToWindow = this.dyCursorToWindow = 0;
            }

        }

        private void FormCatcher_MouseLeave(object sender, EventArgs e)
        {
            this.isMouseLeftDown = false;
            this.dxCursorToWindow = this.dyCursorToWindow = 0;
        }

        private void FormCatcher_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        
    }
}