/*
 * Created by: Umidjon Vakhidov. 
 * Date: 02.02.2015
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Management;
using SMSapplication.Classes;
using SMSapplication;


namespace SMSapplication
{
    public partial class SMSapplication : Form
    {

        #region Constructor
        public SMSapplication()
        {

            
            //AppDomain.CurrentDomain.SetData("DataDirectory", @"H:\docs\SMSapplication\SMSapplication\");

            InitializeComponent();
            //MessageBox.Show(Environment.CurrentDirectory);
            this.tabSMSapplication.TabPages.Remove(tbReadSMS);
            this.tabSMSapplication.TabPages.Remove(tbDeleteSMS);

            
            this.AppSettings = new Settings();
            //if (!this.AppSettings.LoadSettings())
            //{ this.isexit = true; Application.Exit(); }
            try
            {
                #region Database load place (not active)
                //string path = Path.GetDirectoryName(Application.ExecutablePath);
                
                //this.clientsTableAdapter.Connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + "\\localDB.mdb";
                //this.eeventLogTableAdapter.Connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + "\\localDB.mdb";


                //this.clientsTableAdapter.Fill(this.localDBDataSet.Clients);
                //this.eeventLogTableAdapter.Fill(this.localDBDataSet.EeventLog);
                //foreach (localDBDataSet.EeventLogRow row in localDBDataSet.EeventLog.Select("Status='Ошибка'"))
                //{
                //    this.dgvEvent.Rows.Add(new object[] { row.ClBdg, row.PhoneNumber, row.ClientName, row.EventTime, row.Status, row.EventText });
                //    rtbEvents.Text += (row.EventlogID + "=>   " + row.ClientName + "=>   Текст(" + row.EventText + ")=>   " + row.EventTime + "=>   " + row.PhoneNumber + "=>   " + row.Status).Replace("\r\n", "") + "\r\n";
                //}
                
                //System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter("select * from EeventLog where EventTime>CDate(\"" + DateTime.Now.AddHours((double)-tbPrevEvents.Value).ToString() + "\") ", this.eeventLogTableAdapter.Connection);

                //adapter.Fill((DataTable)this.localDBDataSet.EeventLog);
                #endregion
                //load settings

                this.senderSMS.dgv = dgvEvent;
                
                this.senderSMS.rtb = richTextBox1;
                this.senderSMS.beginDate = this.AppSettings.beginDate;

                this.linkClientBase.Text = this.AppSettings.clientDB;
                this.tbxOrganization.Text = this.AppSettings.organization;
                this.tbIntervalEvent.Value = this.AppSettings.CheckInterval;
                this.tbErrorSendInterval.Value = this.AppSettings.ErrorInterval;
                this.tbPrevEvents.Value = this.AppSettings.PrevEventsInterval;
                this.cbxOtmetkaProxod.Text = this.AppSettings.OtmetkaProxod;

                this.Show();
                ///Begin work with EventLog
                ///

                //MessageBox.Show("Showed");

                #region Display all available COM Ports
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity WHERE Caption like '%COM%'");

                //foreach(ManagementObject searchObj in searcher.Get())
                //{
                //    listBox2.Items.Add(searchObj["Caption"]);
                //}

                string[] ports = SerialPort.GetPortNames();
                this.cboPortName.Items.Clear();
                // Add all port names to the combo box:
                foreach (string port in ports)
                {

                    this.cboPortName.Items.Add(port);
                }
                if (this.cboPortName.Items.Count != 0)
                    this.cboPortName.SelectedIndex = 0;
                #endregion
                this.btnDisconnect.Enabled = false;
                //this.tabSMSapplication.TabPages.Remove(tbEvents);
                
                
                if (this.CheckPort())
                {
                    
                    this.senderSMS.objectSMS = this.objclsSMS;
                    //SMSapplication_FormClosing(this, new FormClosingEventArgs(CloseReason.FormOwnerClosing, false));
                }

                //Remove tab pages
                //this.tabSMSapplication.TabPages.Remove(tbSendSMS);


                //this.LoadDGVEmployee();





            }
            
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
            

        }
        #endregion

        #region Private Variables
        private bool hide = false;
        public bool isexit = false;
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        Sender senderSMS = new Sender();
        public Settings AppSettings;

        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion

        #region Classes
        

        #endregion

        #region Private Methods

        #region Write StatusBar
        //private void WriteStatusBar(string status)
        //{
        //    try
        //    {
        //        statusBar1.Text = "Сообщение: " + status;
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}
        //delegate void RefreshControl();
        
        

        
        //delegate void listItems(string[] items);
        //delegate object itemsGet();

        
        //delegate string NextRowValue();
        
        

       
        
        
        
        //string hour = "14";
        
        //string yesterday = "";
        //private string MakeDate()
        //{
        //    string month = (DateTime.Now.Month < 10) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
        //    string day = (DateTime.Now.Day < 10) ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
        //    string str = DateTime.Now.Year + "-" + month +"-" + day;
        //    return str;
        //}

        //private bool CheckDay()
        //{

        //    string date = MakeDate();
            
        //    if (date != yesterday)
        //    {
        //        if (DateTime.Now.Hour.ToString() == hour)
        //        {
        //            return true; 
        //        }
        //        return false;
        //    }

        //    return false;
 
        //}
        #endregion
        
        #endregion

        #region Private Events

        

        

        private bool CheckPort()
        {
            int i = 0;
                
            
            foreach (object port in this.cboPortName.Items)
            {
                try
                {

                    if (!string.IsNullOrEmpty(this.AppSettings.port))
                    {
                        if((string)port!=this.AppSettings.port)
                        continue;
                        cboPortName.SelectedItem = this.AppSettings.port;
 
                    }
                    this.port = objclsSMS.OpenPort((string)port, Convert.ToInt32(this.cboBaudRate.Text), Convert.ToInt32(this.cboDataBits.Text), Convert.ToInt32(this.txtReadTimeOut.Text), Convert.ToInt32(this.txtWriteTimeOut.Text));

                    string receive = objclsSMS.ExecCommand(this.port, "AT+CGMI", clsSMS.command_timeout, "Not connected to port");

                    if (!string.IsNullOrEmpty(receive))
                    {

                        objclsSMS.SetTextMode(this.port);
                        receive = objclsSMS.ExecCommand(this.port, "AT+CMEE=1", clsSMS.command_timeout, "Error codes not enabled");
                        receive = objclsSMS.ExecCommand(this.port, "ATV1", clsSMS.command_timeout, "Error codes not enabled");
                        receive = objclsSMS.ExecCommand(this.port, "ATS3=013", clsSMS.command_timeout, "Error not set carriage return");
                        receive = objclsSMS.ExecCommand(this.port, "ATS4=010", clsSMS.command_timeout, "Error not set line feed");

                        btnStartEvent_Click(btnStartEvent, new EventArgs());
  
                        //MessageBox.Show("Message has sent successfully");
                        this.statusBar1.Text = "Модем подключен удачно";
                        
                        //this.tabSMSapplication.TabPages.Add(tbEvents);
                        cboPortName.SelectedIndex = i;
                        gboPortSettings.Enabled = false;
                    }

                    if (this.port != null)
                    {
                        //this.gboPortSettings.Enabled = false;

                        //MessageBox.Show("Modem is connected at PORT " + this.cboPortName.Text);
                        this.statusBar1.Text = "Модем подключен к порту: " + (string)port;

                        //Add tab pages
                        //this.tabSMSapplication.TabPages.Add(tbSendSMS);
                        //this.tabSMSapplication.TabPages.Add(tbReadSMS);
                        //this.tabSMSapplication.TabPages.Add(tbDeleteSMS);

                        this.lblConnectionStatus.Text = "Подключен к " + this.cboPortName.Text;
                        this.btnDisconnect.Enabled = true;

                    }

                    else
                    {
                        //MessageBox.Show("Invalid port settings");
                        this.statusBar1.Text = "Не подходящий порт";
                    }
                    
                    return true;
                    
                }
                catch (Exception ex)
                {
                    this.statusBar1.Text = ex.Message;
                    objclsSMS.ClosePort(this.port);
                }
                finally 
                {
                    i++;
                                                        
                }
 
            }
            
            return false;

            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {

                //Open communication port 
                this.port = objclsSMS.OpenPort(this.cboPortName.Text, Convert.ToInt32(this.cboBaudRate.Text), Convert.ToInt32(this.cboDataBits.Text), Convert.ToInt32(this.txtReadTimeOut.Text), Convert.ToInt32(this.txtWriteTimeOut.Text));
                
                if (this.port != null)
                {
                    this.gboPortSettings.Enabled = false;

                    //MessageBox.Show("Modem is connected at PORT " + this.cboPortName.Text);
                    this.statusBar1.Text = "Модем подключен к порту: " + this.cboPortName.Text;
                    
                    //Add tab pages
                    //this.tabSMSapplication.TabPages.Add(tbEvents);
                    //this.tabSMSapplication.TabPages.Add(tbReadSMS);
                    //this.tabSMSapplication.TabPages.Add(tbDeleteSMS);

                    this.lblConnectionStatus.Text = "Connected at " + this.cboPortName.Text;
                    this.btnDisconnect.Enabled = true;
                    
                }

                else
                {
                    //MessageBox.Show("Invalid port settings");
                    this.statusBar1.Text = "Invalid port settings";
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }

        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnStopEvent_Click(btnStopEvent, null);
                this.btnStopEvent_Click(null, null);
                this.gboPortSettings.Enabled = true;
                objclsSMS.ClosePort(this.port);
                
                //Remove tab pages
               
                //this.tabSMSapplication.TabPages.Remove(tbReadSMS);
                //this.tabSMSapplication.TabPages.Remove(tbDeleteSMS);

                this.lblConnectionStatus.Text = "Not Connected";
                this.btnDisconnect.Enabled = false;

            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {

            //.............................................. Send SMS ....................................................
            try
            {

                if (objclsSMS.sendMsg(this.port, this.txtSIM.Text, this.txtMessage.Text))
                {
                    //MessageBox.Show("Message has sent successfully");
                    this.statusBar1.Text = "Message has sent successfully";
                }
                else
                {
                    //MessageBox.Show("Failed to send message");
                    this.statusBar1.Text = "Failed to send message";
                }
                
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }
        private void btnReadSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //count SMS 
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                if (uCountSMS > 0)
                {

                    #region Command
                    string strCommand = "AT+CMGL=\"ALL\"";

                    if (this.rbReadAll.Checked)
                    {
                        strCommand = "AT+CMGL=\"ALL\"";
                    }
                    else if (this.rbReadUnRead.Checked)
                    {
                        strCommand = "AT+CMGL=\"REC UNREAD\"";
                    }
                    else if (this.rbReadStoreSent.Checked)
                    {
                        strCommand = "AT+CMGL=\"STO SENT\"";
                    }
                    else if (this.rbReadStoreUnSent.Checked)
                    {
                        strCommand = "AT+CMGL=\"STO UNSENT\"";
                    }
                    #endregion

                    // If SMS exist then read SMS
                    #region Read SMS
                    //.............................................. Read all SMS ....................................................
                    objShortMessageCollection = objclsSMS.ReadSMS(this.port, strCommand);
                    foreach (ShortMessage msg in objShortMessageCollection)
                    {

                        ListViewItem item = new ListViewItem(new string[] { msg.Index, msg.Sent, msg.Sender, msg.Message });
                        item.Tag = msg;
                        lvwMessages.Items.Add(item);

                    }
                    #endregion

                }
                else
                {
                    lvwMessages.Clear();
                    //MessageBox.Show("There is no message in SIM");
                    this.statusBar1.Text = "There is no message in SIM";
                    
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }
        private void btnDeleteSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //Count SMS 
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                if (uCountSMS > 0)
                {
                    DialogResult dr = MessageBox.Show("Are u sure u want to delete the SMS?", "Delete confirmation", MessageBoxButtons.YesNo);

                    if (dr.ToString() == "Yes")
                    {
                        #region Delete SMS

                        if (this.rbDeleteAllSMS.Checked)
                        {                           
                            //...............................................Delete all SMS ....................................................

                            #region Delete all SMS
                            string strCommand = "AT+CMGD=1,4";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                //MessageBox.Show("Messages has deleted successfuly ");
                                this.statusBar1.Text = "Messages has deleted successfuly";
                            }
                            else
                            {
                                //MessageBox.Show("Failed to delete messages ");
                                this.statusBar1.Text = "Failed to delete messages";
                            }
                            #endregion
                            
                        }
                        else if (this.rbDeleteReadSMS.Checked)
                        {                          
                            //...............................................Delete Read SMS ....................................................

                            #region Delete Read SMS
                            string strCommand = "AT+CMGD=1,3";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                //MessageBox.Show("Messages has deleted successfuly");
                                this.statusBar1.Text = "Messages has deleted successfuly";
                            }
                            else
                            {
                                //MessageBox.Show("Failed to delete messages ");
                                this.statusBar1.Text = "Failed to delete messages";
                            }
                            #endregion

                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }

        }
        private void btnCountSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //Count SMS
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                this.txtCountSMS.Text = uCountSMS.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        #endregion

        #region Error Log
        public void ErrorLog(string Message)
        {
            StreamWriter sw = null;

            try
            {
                //WriteStatusBar(Message);

                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                //string sPathName = @"E:\";
                string sPathName = @"SMSapplicationErrorLog_";

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                sw = new StreamWriter(sPathName + sErrorTime + ".txt", true);

                sw.WriteLine(sLogFormat + Message);
                sw.Flush();

            }
            catch (Exception ex)
            {
                //ErrorLog(ex.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
            }
            
        }
        #endregion 

        private void linkClientBase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.AppSettings.SetClientDB();
            linkClientBase.Text = this.AppSettings.clientDB;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            this.AppSettings.organization = this.tbxOrganization.Text;
            this.AppSettings.CheckInterval = (int)tbIntervalEvent.Value;
            this.AppSettings.ErrorInterval = (int)tbErrorSendInterval.Value;
            this.AppSettings.PrevEventsInterval = (int)tbPrevEvents.Value;
            this.AppSettings.OtmetkaProxod = cbxOtmetkaProxod.Text;
            this.AppSettings.port = (string)cboPortName.SelectedItem;
            this.AppSettings.SaveSettings();
            statusBar1.Text= "Сохранено!";
            gpbAppSettings.Enabled = false;
        }

        private void btnChangeSettings_Click(object sender, EventArgs e)
        {
            gpbAppSettings.Enabled = true;
        }
        
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (hide)
            {
                this.Hide();
                this.hide = false;
            }
            else
            {
                this.Show();
                this.hide = true;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.senderSMS.th.Abort();
            
            if (this.senderSMS.worker.IsBusy)
            {
                if (MessageBox.Show("Вы прерываете не завершенную опреацию, продолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {

                    this.isexit = true;
                    this.senderSMS.IsExit = true;
                    senderSMS.worker.CancelAsync();
                    Application.Exit();
                }
                else
                {
                    this.isexit = false;
                }
            }
            else { this.isexit = true; Application.Exit(); }

            
                
            
            this.AppSettings.beginDate = senderSMS.beginDate;
            this.AppSettings.SaveSettings();
            
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.hide = true;
        }

        private void SMSapplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isexit)
            {
                e.Cancel = true;
                this.Hide();
                this.hide = false;
            }
            //else 
            //{
            //    try
            //    {
                    
            //            if(!this.isexit)
            //                e.Cancel = true;
                        
                        
                            
            //                //YesorNo = true;  
                            
                             
                        
                    
                    
            //    }
            //    catch (Exception ex) { MessageBox.Show(ex.Message); }
            //}
            localDBDataSetTableAdapters.EeventLogTableAdapter da = new localDBDataSetTableAdapters.EeventLogTableAdapter();
            da.Update(localDBDataSet.EeventLog);
            this.AppSettings.beginDate = senderSMS.beginDate;
            this.AppSettings.SaveSettings();
            
        }


        #region Employees
        public void LoadDGVEmployee()
        {
            if (dgvEmployees.DataSource != null)
            {
                //dgvEmployees.Columns["TemplateID"].Visible = false;
                //dgvEmployees.Columns["BdgNum"].HeaderText = "Номер бэйджа";
                //dgvEmployees.Columns["BdgNum"].Width = 50;
                //dgvEmployees.Columns["IsSend"].HeaderText = "Отправка СМС";
                //dgvEmployees.Columns["ClientName"].HeaderText = "Имя влдаельца";
                //dgvEmployees.Columns["PhoneNumber"].HeaderText = "Мобильный";
                
                //dgvEmployees.Columns["ClientName"].Width = 100;
                //dgvEmployees.Columns["BdgNum"].HeaderText = "Номер бэйджа";

                dgvEmployees.Columns["PhoneNumber"].CellTemplate.Style.Format = "(998)93-000-00000" ;
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            dgvEmployees.ScrollBars = ScrollBars.None;
            if (!string.IsNullOrEmpty(this.AppSettings.clientDB))
            {
                ClientsData clientdata = new ClientsData();
                clientdata.OtmetkaProxod = cbxOtmetkaProxod.Text;
                if (clientdata.ConnectingBase(this.AppSettings.clientDB))
                {
                    if (clientdata.SyncroniseTables("clients"))
                    {
                        //clientdata.CopyTable(clientdata.ds.Tables["clients"], (DataTable)localDBDataSet.Clients, "BdgNum");
                        BackgroundWorker bgworker = new BackgroundWorker();
                        bgworker.WorkerReportsProgress = true;

                        bgworker.ProgressChanged += bgworker_ProgressChanged;
                        bgworker.DoWork += bgworker_DoWork;
                        bgworker.RunWorkerAsync(new object[] { clientdata.ds.Tables["clients"], (DataTable)localDBDataSet.Clients });
                        bgworker.RunWorkerCompleted += bgworker_RunWorkerCompleted;

                    }
                    else
                    {
                        statusBar1.Text = clientdata.errorMessage; 
                    }

                }
                else
                {
                    statusBar1.Text = clientdata.errorMessage;
                }

            }
        }

        void bgworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvEmployees.Refresh();
            dgvEmployees.ScrollBars = ScrollBars.Both;
            if (MessageBox.Show("Сохранить загруженные данные?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                localDBDataSetTableAdapters.ClientsTableAdapter clientda = new localDBDataSetTableAdapters.ClientsTableAdapter();
                clientda.Update(localDBDataSet.Clients);

            }
            else {
                localDBDataSet.Clients.RejectChanges();
                
            }
            
        }

        void bgworker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = ((BackgroundWorker)sender);
            worker.ReportProgress(0);
            object[] arguments = (object[])e.Argument;
            DataTable clientSRC = (DataTable)arguments[0];
            DataTable clientDST = (DataTable)arguments[1];
            float onepercent = 100f/clientSRC.Rows.Count;
            float percent = 0;
            foreach (DataRow dr in clientSRC.Rows)
            {
                
                DataRow[] rows = clientDST.Select("BdgNum=" + dr["BdgNum"]);
                if (rows.Length != 0)
                {
                    rows[0]["ClientName"] = dr["ClientName"];
                }
                else
                {
                    clientDST.Rows.Add(dr.ItemArray);

                    
                }
                percent += onepercent;
                worker.ReportProgress((int)percent);
            }
            worker.ReportProgress(100);
            percent = 100;
        }

        void bgworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void btnSaveClients_Click(object sender, EventArgs e)
        {
            /*
            foreach (DataRow row in localDBDataSet.Clients.Rows)
            {
                if (row.RowState == DataRowState.Added)
                {
                    
                }
            }*/
            int count = clientsTableAdapter.Adapter.Update(localDBDataSet.Clients);
            //clientsTableAdapter.Update(localDBDataSet.Clients);
            //localDBDataSet.Clients.AcceptChanges();
            MessageBox.Show("Сохранено:"+count.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            localDBDataSetTableAdapters.ClientsTableAdapter adapter = new localDBDataSetTableAdapters.ClientsTableAdapter();
            //adapter.Connection = new System.Data.OleDb.OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=H:\docs\SMSapplication\SMSapplication\localDB.mdb;Jet OLEDB:Database Password=DthbCjabcnbrt");
            int count = adapter.Update(localDBDataSet.Clients);
            MessageBox.Show(count.ToString());
        }

        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvEmployees_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            clientsBindingSource.Position = e.RowIndex;
        }

        private void chbIsSend_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dgvEmployees.Columns[3].Name);
            dgvEmployees.Rows[clientsBindingSource.Position].Cells[3].Value = chbIsSend.Checked;
            dgvEmployees.Refresh();
        }
        #endregion


        #region Eventlog
        private void btnStartEvent_Click(object sender, EventArgs e)
        {
            if (btnStopEvent.Enabled == false&&gboPortSettings.Enabled==false)
            {
                this.senderSMS.btnStart = this.btnStartEvent;
                this.senderSMS.btnStop = this.btnStopEvent;
                this.senderSMS.IsExit = false;
                this.senderSMS.interval = (int)tbIntervalEvent.Value;
                this.senderSMS.HoursPrevEvents = (int)tbPrevEvents.Value;
                this.senderSMS.intervalCheckErr = (int)tbErrorSendInterval.Value;



                this.senderSMS.objectSMS = this.objclsSMS;

                this.senderSMS.port = this.port;
                this.senderSMS.StartWork(localDBDataSet.Clients, localDBDataSet.EeventLog, DateTime.Now, this.AppSettings.clientDB, tbxOrganization.Text, cbxOtmetkaProxod.Text);
            }

        }
        #endregion

        
        

        private void btnStopEvent_Click(object sender, EventArgs e)
        {
            this.senderSMS.IsExit = true;
            this.statusBar1.Text = "Ожидание остановки рассылки";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //string receive = this.objclsSMS.ExecCommand(this.port, "AT+CSCS=\"UCS2\"", 300, "No phone connected");
                //MessageBox.Show(receive);
                
                //receive = this.objclsSMS.ExecCommand(this.port, "AT+CSMP=?", 300, "No phone connected");
                //MessageBox.Show(receive);
                //receive = this.objclsSMS.ExecCommand(this.port, "AT+CSMP?", 300, "No phone connected");
                //MessageBox.Show(receive);
                //receive = this.objclsSMS.ExecCommand(this.port, "AT+CSMP=,,0,8", 300, "No phone connected");
                //MessageBox.Show(receive);
                
                string strHEX = ConvertStringToHex("Ахтунг!", Encoding.Unicode);
                string strPhone = ConvertStringToHex("998939505202", Encoding.Unicode);
                strPhone = "+998939505202";
                string text = "Hello world!";

                //AT+CGMI        #Запрос идентификатора производителя
                //AT+CGMM        #Запрос идентификатора модели
                //AT+CGMR        #Запрос ревизии устройства
                //AT+CGSN  

                string message = objclsSMS.ExecCommand(this.port, "AT+CSMP=?", 300, "Not connected to port");
                MessageBox.Show(message);
                objclsSMS.sendMsg(this.port, strPhone, text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        private void lnkDelAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("Действительно удалить все данные с таблицы!", "Предупреждение...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                
                foreach (DataRow row in localDBDataSet.Clients.Rows)
                {
                    row.Delete(); 
                }
                localDBDataSetTableAdapters.ClientsTableAdapter clientda = new localDBDataSetTableAdapters.ClientsTableAdapter();
                int count = clientda.Update(localDBDataSet.Clients);
                
            }
        }

        private void dgvEmployees_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvEmployees_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                if (dgvEmployees.Rows[e.RowIndex].Cells[2].Value.ToString() == "00-000-0000")
                {
                    dgvEmployees.Rows[e.RowIndex].Cells[2].Value = "";
                }
                //if ((dgvEmployees.CurrentCell.Value!=DBNull.Value ((bool)dgvEmployees.Rows[e.RowIndex].Cells[3].Value) == true) && dgvEmployees.Rows[e.RowIndex].Cells[2].Value.ToString().Replace("-","").Trim()=="")
                //{ MessageBox.Show("Укажите номер телефона"); }
                
            }
        }
        BackgroundWorker bgsend;
        private void btnSendOwn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(mtbOwnText.Text) && mtbOwnText.Text != "00-000-0000")
            {
                bgsend = new BackgroundWorker();
                bgsend.DoWork+=bgsend_DoWork;
                bgsend.RunWorkerCompleted += bgsend_RunWorkerCompleted;
                string[] sendarg = { "+998" + mtbOwnText.Text.Replace("-", "").Replace(" ", ""), tbOwnText.Text };
                bgsend.RunWorkerAsync(sendarg);
                btnSendOwn.Enabled = false;
            }
        }

        void bgsend_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if(e.Error!=null)
                richTextBox1.Text += e.Error.Message+"\r\n";
            btnSendOwn.Enabled = true;
        }

        void bgsend_DoWork(object sender, DoWorkEventArgs e)
        {
            //try{
                string phone = ((string[])e.Argument)[0];
                string owntext = ((string[])e.Argument)[1];
                    clsSMS.AT_CMGF = false;
                    bool sended = this.objclsSMS.sendMsg(this.port, phone, owntext);
                    if (sended)
                    {
                        MessageBox.Show("Сообщение отпрвлено!");
                    }
                    else
                    {
                        MessageBox.Show("Сообщение не отпрвлено!");
                    }
                //}
                //catch (Exception ex) {
                //    e.Result = ex.Message;
                //}
        }

        private void btnStartEvent_EnabledChanged(object sender, EventArgs e)
        {
            if(((Button)sender).Enabled==true)
                statusBar1.Text = "";
        }
        bool isFirstVisible = true;
        private void SMSapplication_VisibleChanged(object sender, EventArgs e)
        {
            

        }

        private void lnkSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SearchForm searchForm = new SearchForm(dgvEmployees);
            searchForm.Show();
            
        }

        private void SMSapplication_Load(object sender, EventArgs e)
        {

        }

        private void at_timeout_ValueChanged(object sender, EventArgs e)
        {
            clsSMS.command_timeout = (int)At_timeout.Value * 1000;
        }
        

        
        



    }
}