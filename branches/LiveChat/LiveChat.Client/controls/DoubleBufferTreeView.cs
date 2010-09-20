using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LiveChat.Client
{
    /// <summary>
    /// 双缓冲TreeView
    /// </summary>
    public class DoubleBufferTreeView : TreeView
    {
        public DoubleBufferTreeView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
