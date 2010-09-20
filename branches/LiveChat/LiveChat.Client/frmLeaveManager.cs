using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using LiveChat.Entity;

namespace LiveChat.Client
{
    public partial class frmLeaveManager : Form
    {
        private ISeatService service;
        private string companyID;
        private const int PAGESIZE = 50;
        private int pageCount = 1;
        private int currentPage = 1;
        private DateTime startTime, endTime;

        public frmLeaveManager(ISeatService service, string companyID)
        {
            this.service = service;
            this.companyID = companyID;

            InitializeComponent();
        }

        private void frmLeaveManager_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = DateTime.Today.AddDays(-7);
            dtpEndTime.Value = DateTime.Today.AddHours(23).AddMinutes(59);

            DataView<IList<Leave>> view = service.GetLeaves(companyID, dtpStartTime.Value, dtpEndTime.Value, 1, PAGESIZE);
            pageCount = view.PageCount;
            foreach (Leave info in view.DataSource)
            {
                string content = info.Body != null && info.Body.Length > 30 ? info.Body.Substring(0, 30) : info.Body;
                info.Body = content;
            }
            dgvLeaves.AutoGenerateColumns = false;
            dgvLeaves.DataSource = view.DataSource;

            if (view.IsFirstPage) toolStripButton2.Enabled = false;
            if (view.IsLastPage) toolStripButton4.Enabled = false;

            this.Text = string.Format("留言管理 【当前{0}/{1} 每页{2}条】", currentPage, pageCount, PAGESIZE);
            toolStripStatusLabel1.Text = string.Format("共有{0}条留言信息......", view.RowCount);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = true;
            toolStripButton4.Enabled = true;

            if (currentPage > 1)
            {
                currentPage--;
                DataView<IList<Leave>> view = service.GetLeaves(companyID, startTime, endTime, currentPage, PAGESIZE);
                pageCount = view.PageCount;
                foreach (Leave info in view.DataSource)
                {
                    string content = info.Body != null && info.Body.Length > 30 ? info.Body.Substring(0, 30) : info.Body;
                    info.Body = content;
                }
                dgvLeaves.DataSource = view.DataSource;

                this.Text = string.Format("留言管理 【当前{0}/{1} 每页{2}条】", currentPage, pageCount, PAGESIZE);
                toolStripStatusLabel1.Text = string.Format("共有{0}条留言信息......", view.RowCount);

                if (view.IsFirstPage) toolStripButton2.Enabled = false;
                if (view.IsLastPage) toolStripButton4.Enabled = false;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = true;
            toolStripButton4.Enabled = true;

            if (currentPage < pageCount)
            {
                currentPage++;
                DataView<IList<Leave>> view = service.GetLeaves(companyID, startTime, endTime, currentPage, PAGESIZE);
                pageCount = view.PageCount;
                foreach (Leave info in view.DataSource)
                {
                    string content = info.Body != null && info.Body.Length > 30 ? info.Body.Substring(0, 30) : info.Body;
                    info.Body = content;
                }
                dgvLeaves.DataSource = view.DataSource;

                this.Text = string.Format("留言管理 【当前{0}/{1} 每页{2}条】", currentPage, pageCount, PAGESIZE);
                toolStripStatusLabel1.Text = string.Format("共有{0}条留言信息......", view.RowCount);

                if (view.IsFirstPage) toolStripButton2.Enabled = false;
                if (view.IsLastPage) toolStripButton4.Enabled = false;
            }
        }

        /// <summary>
        /// 查询留言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            startTime = dtpStartTime.Value;
            endTime = dtpEndTime.Value;
            DataView<IList<Leave>> view = service.GetLeaves(companyID, dtpStartTime.Value, dtpEndTime.Value, 1, PAGESIZE);
            pageCount = view.PageCount;
            dgvLeaves.AutoGenerateColumns = false;
            dgvLeaves.DataSource = view.DataSource;

            if (view.IsFirstPage) toolStripButton2.Enabled = false;
            if (view.IsLastPage) toolStripButton4.Enabled = false;

            this.Text = string.Format("留言管理 【当前{0}/{1} 每页{2}条】", currentPage, pageCount, PAGESIZE);
            toolStripStatusLabel1.Text = string.Format("共有{0}条留言信息......", view.RowCount);
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvLeaves_Click(object sender, EventArgs e)
        {
            if (dgvLeaves.SelectedRows.Count == 0) return;

            Leave leave = dgvLeaves.SelectedRows[0].DataBoundItem as Leave;
            //label3.Text = leave.Title;
            textBox1.Text = leave.Body;
        }

        private void frmLeaveManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
