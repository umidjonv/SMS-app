using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SMSapplication
{
    public partial class SearchForm : Form
    {
        public SearchForm(DataGridView dgv)
        {
            currentDgv = dgv;
            InitializeComponent();
            
        }
        DataGridView currentDgv;

        

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;
            if (rbSender.Checked)
            {
                switch (rbSender.Name)
                {
                    case "rbBadge":
                        gbxBadge.Enabled = true;
                        gbxFIO.Enabled = false;
                        gbxPhone.Enabled = false;

                        break;
                    case "rbPhone":
                        gbxBadge.Enabled = false;
                        gbxFIO.Enabled = false;
                        gbxPhone.Enabled = true;
                        break;
                    case "rbFIO":
                        gbxBadge.Enabled = false;
                        gbxFIO.Enabled = true;
                        gbxPhone.Enabled = false;
                        break;
 
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
            BindingSource source = (BindingSource)currentDgv.DataSource;
            if (rbBadge.Checked && !string.IsNullOrEmpty(tbxBadge.Text.Trim()))
            {
                int num;
                if(int.TryParse(tbxBadge.Text, out num))
                {
                    source.Filter = "BdgNum="+tbxBadge.Text;
                }else
                {
                    tbxBadge.Text = "";
                }
                
                
            }
            else if(rbFIO.Checked)
            {
                source.Filter = "ClientName like '%" + tbxFIO.Text+"%'";
            }
            else if (rbPhone.Checked&&!string.IsNullOrEmpty(tbxPhone.Text.Replace("-","").Trim()))
            {
                source.Filter =  "PhoneNumber like '%" + tbxPhone.Text.Replace("-","").Trim()+"%'";
 
            }
        }

        private void SearchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            BindingSource source = (BindingSource)currentDgv.DataSource;
            source.RemoveFilter();
        }

        private void tbxBadge_Click(object sender, EventArgs e)
        {
            tbxBadge.SelectAll();
        }
    }
}
