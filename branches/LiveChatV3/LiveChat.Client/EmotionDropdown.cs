﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CSharpWin;
using MySoft.Core;
using System.IO;

namespace LiveChat.Client
{
    public partial class EmotionDropdown : UserControl
    {
        private Popup _popup;
        private static Image[] _images;

        public EmotionDropdown()
        {
            InitializeComponent();
            _popup = new Popup(this);

            EmotionContainer.Row = 15;
            EmotionContainer.Columns = 9;
            EmotionContainer.ItemClick +=
                new EmotionItemMouseEventHandler(EmotionContainerItemClick);
        }

        void EmotionContainerItemClick(
            object sender, EmotionItemMouseClickEventArgs e)
        {
            _popup.Close();
        }

        public EmotionContainer EmotionContainer
        {
            get { return emotionContainer1; }
        }

        public void Show(Control owner)
        {
            _popup.Show(owner, true);
        }

        public string Root
        {
            get;
            set;
        }

        private void EmotionDropdown_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Root))
            {
                if (_images == null)
                {
                    _images = new Image[136];
                    for (int i = 0; i < 135; i++)
                    {
                        var image = Image.FromFile(Path.Combine(Root, string.Format("images/face/{0}.gif", i)));
                        _images[i] = image;
                        var item = new EmotionItem(i.ToString(), image);
                        EmotionContainer.Items.Add(item);
                    }
                }
                else
                {
                    for (int i = 0; i < 135; i++)
                    {
                        var item = new EmotionItem(i.ToString(), _images[i]);
                        EmotionContainer.Items.Add(item);
                    }
                }
            }
        }
    }
}
