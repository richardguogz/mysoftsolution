using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CSharpWin;

namespace LiveChat.Client
{
    public partial class EmotionDropdown : UserControl
    {
        private Popup _popup;

        public EmotionDropdown()
        {
            InitializeComponent();
            _popup = new Popup(this);

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

        private void EmotionDropdown_Load(object sender, EventArgs e)
        {
            var items = new EmotionItem[24];
            items[0] = new EmotionItem("1", Properties.Resources.face1);
            items[1] = new EmotionItem("2", Properties.Resources.face2);
            items[2] = new EmotionItem("3", Properties.Resources.face3);
            items[3] = new EmotionItem("4", Properties.Resources.face4);
            items[4] = new EmotionItem("5", Properties.Resources.face5);
            items[5] = new EmotionItem("6", Properties.Resources.face6);
            items[6] = new EmotionItem("7", Properties.Resources.face7);
            items[7] = new EmotionItem("8", Properties.Resources.face8);
            items[8] = new EmotionItem("9", Properties.Resources.face9);
            items[9] = new EmotionItem("10", Properties.Resources.face10);
            items[10] = new EmotionItem("11", Properties.Resources.face11);
            items[11] = new EmotionItem("12", Properties.Resources.face12);
            items[12] = new EmotionItem("13", Properties.Resources.face13);
            items[13] = new EmotionItem("14", Properties.Resources.face14);
            items[14] = new EmotionItem("15", Properties.Resources.face15);
            items[15] = new EmotionItem("16", Properties.Resources.face16);
            items[16] = new EmotionItem("17", Properties.Resources.face17);
            items[17] = new EmotionItem("18", Properties.Resources.face18);
            items[18] = new EmotionItem("19", Properties.Resources.face19);
            items[19] = new EmotionItem("20", Properties.Resources.face20);
            items[20] = new EmotionItem("21", Properties.Resources.face21);
            items[21] = new EmotionItem("22", Properties.Resources.face22);
            items[22] = new EmotionItem("23", Properties.Resources.face23);
            items[23] = new EmotionItem("24", Properties.Resources.face24);

            foreach (var item in items)
                EmotionContainer.Items.Add(item);
        }
    }
}
