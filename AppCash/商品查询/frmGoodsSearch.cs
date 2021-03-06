﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace AppCash
{
    public partial class frmGoodsSearch : Form
    {
        #region ------初始化变量------
        private string strSql = "";                 //查询条件
        private int intPage = 1;                    //当前页码
        private int intPageSize = 20;               //默认每页显示条数
        private int pageCounts = 0;                 //总计路数
        #endregion

        #region ------窗体实例化------
        public frmGoodsSearch()
        {
            InitializeComponent();
        }
        #endregion

        #region ------窗体加载------
        private void frmGoodsSearch_Load(object sender, EventArgs e)
        {
            //绑定商品类别

            Dong.BLL.Category catebll = new Dong.BLL.Category();

            DataTable dt = catebll.GetAllList().Tables[0];
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = "0";
            dr[2] = "全部";
            dt.Rows.InsertAt(dr, 0);
          
            ddlCate.DataSource = dt;
            ddlCate.DisplayMember = "Name";
            ddlCate.ValueMember = "Id";


            //设置DataGridView的显示样式
            gvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gvList.AutoGenerateColumns = false;
            fillGVList("", intPageSize, 1);         //填充数据

            //分页
            //初始页面显示条数选择框
            for (int i = 1; i < 6; i++)
            {
                int j = i * 10;
                cbPageSize.Items.Add(j.ToString());
            }
            cbPageSize.Text = "20";                 //页面条数显示框默认显示为20条

            //绑定页面显示条数设置控件的TextChanged事件
            cbPageSize.TextChanged += new EventHandler(cbPageSize_TextChanged);

        }
        #endregion



        #region ------搜索------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            strSql = "";
            if (tbKey.Text.Trim() != "")
            {
                string key = Maticsoft.Common.StringPlus.GetText(tbKey.Text);
                strSql += " and GoodName like '%" + key + "%'";


            }

            if (txtCode.Text.Trim() != "")
            {
                string code = Maticsoft.Common.StringPlus.GetText(txtCode.Text);
                strSql += " and  Code like '%" + code + "%'";
            }


            if (ddlCate.SelectedValue != null && ddlCate.SelectedValue.ToString() != "0")
            {
                string code = Maticsoft.Common.StringPlus.GetText(txtCode.Text);
                strSql += " and  Category =" + ddlCate.SelectedValue.ToString() + " ";
            }
            intPage = 1;
            fillGVList(strSql, intPageSize, 1);
        }
        #endregion

        #region ------共用刷新方法------
        public void refreshData()
        {
            fillGVList(strSql, intPageSize, intPage);
        }
        #endregion

        #region ------填出datagridview------
        private void fillGVList(string key, int pageSize, int page)
        {
            strSql = key;
            intPage = page;
            Dong.BLL.GoodsInfo bll = new Dong.BLL.GoodsInfo();
            DataSet ds = new DataSet();
            ds = bll.GetListPage(key, pageSize, page);

            gvList.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                intPages();
            }
            else
            {
                MessageBoxEx.Show("对不起，没有您搜索的记录!");
            }

        }
        #endregion

        #region ------显示所有数据------
        private void btnAll_Click(object sender, EventArgs e)
        {
            fillGVList("", intPageSize, intPage);
            intPage = 1;


        }
        #endregion



        #region ------分页代码------
        private void intPages()
        {
            Dong.BLL.GoodsInfo bVip = new Dong.BLL.GoodsInfo();
            int counts = bVip.GetRecordCount(strSql);
            lblCounts.Text = counts.ToString();
            if (counts % intPageSize == 0)
            {
                pageCounts = counts / intPageSize;

            }
            else
            {
                pageCounts = (counts / intPageSize) + 1;

            }

            cbPage.Items.Clear();
            for (int i = 1; i <= pageCounts; i++)
            {
                cbPage.Items.Add(i.ToString());
            }
            cbPage.SelectedIndex = intPage - 1;
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            //转移到第一页
            fillGVList(strSql, intPageSize, 1);

        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            intPage--;
            if (intPage >= 1)
            {
                fillGVList(strSql, intPageSize, intPage);
            }
            else
            {
                intPage++;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            intPage++;
            if (intPage <= pageCounts)
            {
                fillGVList(strSql, intPageSize, intPage);
            }
            else
            {
                intPage--;
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            fillGVList(strSql, intPageSize, pageCounts);
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            int cPage = int.Parse(cbPage.Text);
            fillGVList(strSql, intPageSize, cPage);
        }

        private void cbPageSize_TextChanged(object sender, EventArgs e)
        {
            if (cbPageSize.Text != "")
            {
                int cPageSize = int.Parse(cbPageSize.Text);
                intPageSize = cPageSize;
                fillGVList(strSql, cPageSize, 1);
            }
        }

        #endregion

    }
}
