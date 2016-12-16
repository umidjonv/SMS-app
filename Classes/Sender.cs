using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.IO.Ports;

namespace SMSapplication.Classes
{
    class Sender
    {
        public BackgroundWorker worker;
        public bool IsExit = false;
        private bool worked = false;
        public clsSMS objectSMS;
        public int interval = 15;
        public int intervalCheckErr = 15;
        public SerialPort port;
        public Button btnStart;
        public Button btnStop;
        System.Timers.Timer timer;
        public Thread th;
        private bool checkErrors = true;
        public int HoursPrevEvents = 0;
        public DateTime beginDate = DateTime.Now;
        localDBDataSetTableAdapters.EeventLogTableAdapter da;

        public Sender()
        {
            
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            //th = new Thread(new ThreadStart(DoTimer));
            //th.Start();
            timer = new System.Timers.Timer((double)intervalCheckErr * 60 * 1000);
            
            timer.Elapsed += timer_Elapsed;
            
            //timer = new System.Threading.Timer(new TimerCallback(DoTimer),null, 0, intervalCheckErr*60*1000);
            //timer.
            da = new localDBDataSetTableAdapters.EeventLogTableAdapter();
            da.Adapter.AcceptChangesDuringUpdate = false;
            //da.Adapter.
            
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           int cur_int = intervalCheckErr * 60 * 1000;
            
            checkErrors = true;
        }

        void DoTimer(object arg)
        { 
        }

        void DoTimer()
        {
            int cur_int = intervalCheckErr * 60 * 1000;
            //timer.Change(0, cur_int);
            //Thread.Sleep(cur_int);
            
            checkErrors = true;
 
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                localDBDataSet.EeventLogDataTable eventTable = (localDBDataSet.EeventLogDataTable)e.Result;
                //da = new localDBDataSetTableAdapters.EeventLogTableAdapter();
                da.Update(eventTable);
                
                SMSapplication f = (SMSapplication)btnStart.FindForm();
                f.AppSettings.beginDate = this.beginDate;
                f.AppSettings.SaveSettings();

                f.statusBar1.Text = "Рассылка остановлена";
                if (btnStart != null)
                {
                    btnStart.Enabled = true;
                }
                if (btnStop != null)
                {
                    btnStop.Enabled = false;
                }
                worked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool Worked
        {
            get { return worked; }
        }

        public void StartWork(localDBDataSet.ClientsDataTable clientTable, localDBDataSet.EeventLogDataTable eventTable, DateTime startTime, string dbfile, string organization, string proxod)
        {
            Dictionary<string, object> argument = new Dictionary<string, object>();
            argument["clientTable"] = clientTable;
            argument["eventTable"] = eventTable;
            argument["startTime"] = startTime;
            argument["dbfile"] = dbfile;
            argument["organization"] = organization;
            argument["otmetka_proxod"] = proxod;
            if (btnStart != null)
            {
                btnStart.Enabled = false;
            }
            if (btnStop != null)
            {
                btnStop.Enabled = true;
            }
            worker.RunWorkerAsync(argument); 
        }

        void CheckErrors(localDBDataSet.EeventLogDataTable events_tbl)
        {
            try
            {
                da.Fill(events_tbl);
                localDBDataSet.EeventLogRow[] logs = (localDBDataSet.EeventLogRow[])events_tbl.Select("Status='Ошибка'");
                
                //da = new localDBDataSetTableAdapters.EeventLogTableAdapter();
                foreach (localDBDataSet.EeventLogRow row in logs)
                {
                    bool sended = this.SendSMS(row.PhoneNumber, Transliteration.Transliteration.Front(row.EventText));
                    if (sended)
                    {

                        row.Status = "Отправлено";

                        
                        da.Update(events_tbl);
                        this.dgv.FindForm().Invoke(new UpdateDgv(UpdateDatagridview), new object[] { row });
                    }

                }
                this.checkErrors = false;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            da.Fill(events_tbl);
        }
        #region work with DB SMS for school
        //void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    worked = true;
        //    Form f = this.dgv.FindForm();
        //    timer.Interval = intervalCheckErr * 60 * 1000;
        //    timer.Enabled = true;
        //    //if(!th.IsAlive)th.Start();
        //    timer.Start();
        //    Dictionary<string, object> curargument = e.Argument as Dictionary<string, object>;
        //    if (curargument != null && objectSMS != null)
        //    {
        //        //get params
        //        localDBDataSet.ClientsDataTable clientTable = (localDBDataSet.ClientsDataTable)curargument["clientTable"];
        //        localDBDataSet.EeventLogDataTable eventTable = (localDBDataSet.EeventLogDataTable)curargument["eventTable"];
        //        DateTime startTime = (DateTime)curargument["startTime"];
        //        string dbfile = (string)curargument["dbfile"];
        //        string organization = (string)curargument["organization"];
        //        string proxod = (string)curargument["otmetka_proxod"];
        //        ClientsData clientdata = new ClientsData();
        //        clientdata.OtmetkaProxod = proxod;
        //        DateTime curDate = DateTime.Now.AddHours(-HoursPrevEvents);
        //        if (DateTime.Now.Month >= 6 && DateTime.Now.Day >= 31)
        //        {
        //            MessageBox.Show("Истек период использования ПО!");
        //            return;
        //        }

        //        if (beginDate > curDate)
        //        { clientdata.selectDate = beginDate.ToString(); }
        //        else
        //        {
        //            clientdata.selectDate = curDate.ToString();
        //            beginDate = curDate;
        //        }
        //        beginDate = beginDate.AddSeconds(this.interval);
        //        Dictionary<string, bool> sendlist = new Dictionary<string, bool>();
        //        if (clientdata.ConnectingBase(dbfile))
        //        {
        //            try
        //            {
        //                while (!IsExit)
        //                {
        //                    //Check the error events and send again
        //                    if (checkErrors)
        //                        CheckErrors(eventTable);


        //                    if (clientdata.SyncroniseTables("eventlog"))
        //                    {
        //                        localDBDataSet.EeventLogRow eventRow;
        //                        Dictionary<string, string> mess_parts;
        //                        bool eventSend = false;
        //                        DataRow[] availableRows = clientdata.ds.Tables["eventlog"].Select("(EventTime>'" + beginDate.AddSeconds((-interval)).ToString() + "' and EventTime<'" + beginDate.ToString() + "' )");
        //                        for (int i = 0; i < availableRows.Length; i++) //(System.Data.DataRow row in clientdata.ds.Tables["eventlog"].Rows)
        //                        {
        //                            int bdgnum = (int)availableRows[i]["ClBdg"];
        //                            DataRow[] eventRows = clientdata.ds.Tables["eventlog"].Select("ClBdg=" + bdgnum + " and (EventTime>'" + beginDate.AddSeconds((-interval)).ToString() + "' and EventTime<'" + beginDate.ToString() + "' )");


        //                            string cur_message = "";
        //                            string cur_event = "";
        //                            for (int j = 0; j < eventRows.Length; j++)
        //                            {
        //                                System.Data.DataRow rowEvent = eventRows[j];

        //                                //System.Data.DataRow rowEvent = clientdata.ds.Tables["eventlog"].Rows[i];
        //                                if (!sendlist.ContainsKey(rowEvent["IDNum"].ToString()) || sendlist[rowEvent["IDNum"].ToString()] == false)
        //                                {
        //                                    if (!sendlist.ContainsKey(rowEvent["IDNum"].ToString()))
        //                                    {
        //                                        sendlist.Add(rowEvent["IDNum"].ToString(), false);
        //                                    }
        //                                    //
        //                                    DataRow[] srowClient = clientTable.Select("(BdgNum=" + rowEvent["ClBdg"] + " and PhoneNumber Is not Null) and IsSend='true'");
        //                                    if (srowClient.Length > 0)
        //                                    {
        //                                        mess_parts = this.MessageParts(rowEvent, srowClient[0]);

        //                                        if (!eventSend)
        //                                        {
        //                                            string check_sms_str = organization + "\r\n" + (string)srowClient[0]["ClientName"] + "-" + DateTime.Parse(mess_parts["EventTime"]).ToShortDateString() + "\r\n" + cur_message + mess_parts["Event"] + "-" + DateTime.Parse(mess_parts["EventTime"]).ToLongTimeString();
        //                                            if (check_sms_str.Length < 160)
        //                                            {
        //                                                cur_message += mess_parts["Event"] + "-" + DateTime.Parse(mess_parts["EventTime"]).ToLongTimeString() + "\r\n";
        //                                                cur_event += (string)rowEvent["Event"] + "\r\n";
        //                                                sendlist[rowEvent["IDNum"].ToString()] = true;
        //                                                if ((j + 1) == eventRows.Length)
        //                                                {
        //                                                    sendlist[rowEvent["IDNum"].ToString()] = true;
        //                                                    eventSend = true;
        //                                                }

        //                                            }
        //                                            else
        //                                            {
        //                                                eventSend = true;
        //                                                sendlist[rowEvent["IDNum"].ToString()] = false;
        //                                                j--;
        //                                            }

        //                                        }
        //                                        if (eventSend)
        //                                        {
        //                                            eventSend = false;
        //                                            eventRow = eventTable.NewEeventLogRow();
        //                                            eventRow.ClBdg = (int)rowEvent["ClBdg"];
        //                                            eventRow.ClientName = mess_parts["ClientName"];
        //                                            eventRow.PhoneNumber = mess_parts["PhoneNumber"];
        //                                            eventRow.EventTime = DateTime.Now;
        //                                            eventRow.EventText = organization + "\r\n" + (string)srowClient[0]["ClientName"] + " " + DateTime.Parse(mess_parts["EventTime"]).ToShortDateString() + "\r\n" + cur_message;
        //                                            eventRow.EventType = cur_event;
        //                                            eventRow.Status = "Отправляется";

        //                                            eventTable.AddEeventLogRow(eventRow);

        //                                            //f.Invoke(new UpdateDgv(UpdateDatagridview), new object[] { eventRow });


        //                                            bool sended = this.SendSMS(mess_parts["PhoneNumber"], Transliteration.Transliteration.Front(eventRow.EventText));//organization + "\r\n" + mess_parts["ClientName"] + " " + mess_parts["Event"] + " v " + mess_parts["EventTime"])); //+ " " + mess_parts["EventTime"] + " " + mess_parts["Zone"]);


        //                                            //int randomInt = err.Next(1000);
        //                                            //if (randomInt % 2 == 1)
        //                                            //{ eventRow.Status = "Отправлено"; }
        //                                            //else
        //                                            //{ eventRow.Status = "Ошибка"; }
        //                                            if (sended)
        //                                            {
        //                                                eventRow.Status = "Отправлено";



        //                                            }
        //                                            else
        //                                            {
        //                                                eventRow.Status = "Ошибка";


        //                                            }

        //                                            //f.Invoke(new UpdateDgv(UpdateDatagridview), new object[] { null });
        //                                            da.Update(eventRow);


        //                                            f.Invoke(new UpdateDgv(UpdateDatagridview), new object[] { eventRow });
        //                                            cur_event = "";
        //                                            cur_message = "";
        //                                            da.Fill(eventTable);
        //                                        }




        //                                        /*// send of same rows part
        //                                        ///
        //                                        ///
        //                                        if (rowEvent["BdgNum"].ToString() == clientdata.ds.Tables["eventlog"].Rows[i + 1]["BdgNum"].ToString())
        //                                        {
        //                                            if (!perekluchatel)
        //                                            { beginIndex = i; perekluchatel = true; }

                                                
        //                                        }
        //                                        else if(perekluchatel)
        //                                        {
        //                                            rowEvent = clientdata.ds.Tables["eventlog"].Rows[beginIndex];
        //                                            mess_parts = this.MessageParts(rowEvent, srowClient[0]);
        //                                            eventRow = eventTable.NewEeventLogRow();
        //                                            eventRow.ClBdg =        (int)rowEvent["ClBdg"];
        //                                            eventRow.ClientName =   mess_parts["ClientName"];
        //                                            eventRow.PhoneNumber =  mess_parts["PhoneNumber"];
        //                                            eventRow.EventTime =    DateTime.Now;
        //                                            eventRow.EventText =    organization + "\r\n" + (string)srowClient[0]["ClientName"] + " " + DateTime.Parse(mess_parts["EventTime"]).ToShortDateString()+"\r\n";
        //                                            eventRow.EventType = "";
        //                                            eventRow.Status = "Отправляется";
                                                
                                                
        //                                            for (; beginIndex <= i; beginIndex++)
        //                                            {
        //                                                rowEvent = clientdata.ds.Tables["eventlog"].Rows[beginIndex];
        //                                                mess_parts = this.MessageParts(rowEvent, srowClient[0]);

        //                                                eventRow.EventText +=  mess_parts["Event"]+"-"+DateTime.Parse(mess_parts["EventTime"]).ToLongTimeString();
        //                                                eventRow.EventType += (string)rowEvent["Event"] + "\r\n";
                                                      
        //                                            }
        //                                            eventTable.AddEeventLogRow(eventRow);
        //                                            perekluchatel = false;
        //                                        }
        //                                        */
        //                                        /// 
        //                                        //mess_parts = this.MessageParts(rowEvent, srowClient[0]);
        //                                        //sendlist.Add(rowEvent["IDNum"].ToString(), false);

        //                                        //eventRow = eventTable.NewEeventLogRow();
        //                                        //eventRow.ClBdg = (int)rowEvent["ClBdg"];
        //                                        //eventRow.ClientName = mess_parts["ClientName"];
        //                                        //eventRow.PhoneNumber = mess_parts["PhoneNumber"];
        //                                        //eventRow.EventTime = DateTime.Now;
        //                                        //eventRow.EventText = organization + "\r\n" + mess_parts["ClientName"] + " " + mess_parts["EventTime"] + " " + mess_parts["Event"];
        //                                        //eventRow.EventType = mess_parts["Event"];
        //                                        //eventRow.Status = "Отправляется";

        //                                    }

        //                                    //}
        //                                    ///Send section
        //                                    ///
        //                                    ///

        //                                }
        //                            }




        //                        }

        //                        if (IsExit)
        //                            break;
        //                        if (beginDate > DateTime.Now.AddSeconds(-this.interval))
        //                        {
        //                            int j = 0;
        //                            int sekund5 = this.interval / 5;
        //                            while (!(sekund5 <= j))
        //                            {
        //                                if (IsExit)
        //                                    break;
        //                                Thread.Sleep(5000);
        //                                j++;
        //                            }
        //                        }

        //                        beginDate = beginDate.AddSeconds(this.interval);
        //                        if (IsExit)
        //                        { break; }
        //                    }
        //                    else
        //                    {
        //                        //MessageBox.Show(clientdata.errorMessage);
        //                        this.SendSMS("error", clientdata.errorMessage);
        //                    }
        //                    //f = this.dgv.FindForm();
        //                    //f.Invoke(new UpdateDgv(UpdateDatagridview));
        //                    //del = new UpdateDgv(UpdateDatagridview);
        //                    //del.Invoke();


        //                }
        //                //th.Abort();
        //                timer.Stop();
        //            }
        //            catch (Exception ex)
        //            {
        //                this.SendSMS("error", ex.Message);
        //            }

        //        }
        //        else
        //        {
        //            //MessageBox.Show(clientdata.errorMessage);
        //            this.SendSMS("error", clientdata.errorMessage);
        //        }
        //        e.Result = eventTable;
        //    }



        //    //throw new NotImplementedException();
        //}
        
        #endregion
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            worked = true;
            Form f = this.dgv.FindForm();
            timer.Interval = intervalCheckErr * 60 * 1000;
            timer.Enabled = true;
            //if(!th.IsAlive)th.Start();
            timer.Start();
            Dictionary<string, object> curargument = e.Argument as Dictionary<string, object>;
            if (curargument != null && objectSMS != null)
            {
                //get params
                localDBDataSet.ClientsDataTable clientTable = (localDBDataSet.ClientsDataTable)curargument["clientTable"];
                localDBDataSet.EeventLogDataTable eventTable = (localDBDataSet.EeventLogDataTable)curargument["eventTable"];
                DateTime startTime = (DateTime)curargument["startTime"];
                string dbfile = (string)curargument["dbfile"];
                string organization = (string)curargument["organization"];
                string proxod = (string)curargument["otmetka_proxod"];
                ClientsData clientdata = new ClientsData();
                clientdata.OtmetkaProxod = proxod;
                DateTime curDate = DateTime.Now.AddHours(-HoursPrevEvents);
                if (DateTime.Now.Month >= 6 && DateTime.Now.Day >= 31)
                {
                    MessageBox.Show("Истек период использования ПО!");
                    return;
                }

                if (beginDate > curDate)
                { clientdata.selectDate = beginDate.ToString(); }
                else
                {
                    clientdata.selectDate = curDate.ToString();
                    beginDate = curDate;
                }
                beginDate = beginDate.AddSeconds(this.interval);
                Dictionary<string, bool> sendlist = new Dictionary<string, bool>();
                if (clientdata.ConnectingBase(dbfile))
                {
                    try
                    {
                        while (!IsExit)
                        {
                            //Check the error events and send again
                            if (checkErrors)
                                CheckErrors(eventTable);


                            if (clientdata.SyncroniseTables("eventlog"))
                            {
                                localDBDataSet.EeventLogRow eventRow;
                                Dictionary<string, string> mess_parts;
                                bool eventSend = false;
                                DataRow[] availableRows = clientdata.ds.Tables["eventlog"].Select("(EventTime>'" + beginDate.AddSeconds((-interval)).ToString() + "' and EventTime<'" + beginDate.ToString() + "' )");
                                for (int i = 0; i < availableRows.Length; i++) //(System.Data.DataRow row in clientdata.ds.Tables["eventlog"].Rows)
                                {
                                    int bdgnum = (int)availableRows[i]["ClBdg"];
                                    DataRow[] eventRows = clientdata.ds.Tables["eventlog"].Select("ClBdg=" + bdgnum + " and (EventTime>'" + beginDate.AddSeconds((-interval)).ToString() + "' and EventTime<'" + beginDate.ToString() + "' )");


                                    string cur_message = "";
                                    string cur_event = "";
                                    for (int j = 0; j < eventRows.Length; j++)
                                    {
                                        System.Data.DataRow rowEvent = eventRows[j];

                                        //System.Data.DataRow rowEvent = clientdata.ds.Tables["eventlog"].Rows[i];
                                        if (!sendlist.ContainsKey(rowEvent["IDNum"].ToString()) || sendlist[rowEvent["IDNum"].ToString()] == false)
                                        {
                                            if (!sendlist.ContainsKey(rowEvent["IDNum"].ToString()))
                                            {
                                                sendlist.Add(rowEvent["IDNum"].ToString(), false);
                                            }
                                            //
                                            DataRow[] srowClient = clientTable.Select("(BdgNum=" + rowEvent["ClBdg"] + " and PhoneNumber Is not Null) and IsSend='true'");
                                            if (srowClient.Length > 0)
                                            {
                                                mess_parts = this.MessageParts(rowEvent, srowClient[0]);

                                                if (!eventSend)
                                                {
                                                    string check_sms_str = organization + "\r\n" + (string)srowClient[0]["ClientName"] + "-" + DateTime.Parse(mess_parts["EventTime"]).ToShortDateString() + "\r\n" + cur_message + mess_parts["Event"] + "-" + DateTime.Parse(mess_parts["EventTime"]).ToLongTimeString();
                                                    if (check_sms_str.Length < 160)
                                                    {
                                                        cur_message += mess_parts["Event"] + "-" + DateTime.Parse(mess_parts["EventTime"]).ToLongTimeString() + "\r\n";
                                                        cur_event += (string)rowEvent["Event"] + "\r\n";
                                                        sendlist[rowEvent["IDNum"].ToString()] = true;
                                                        if ((j + 1) == eventRows.Length)
                                                        {
                                                            sendlist[rowEvent["IDNum"].ToString()] = true;
                                                            eventSend = true;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        eventSend = true;
                                                        sendlist[rowEvent["IDNum"].ToString()] = false;
                                                        j--;
                                                    }

                                                }
                                                if (eventSend)
                                                {
                                                    eventSend = false;
                                                    eventRow = eventTable.NewEeventLogRow();
                                                    eventRow.ClBdg = (int)rowEvent["ClBdg"];
                                                    eventRow.ClientName = mess_parts["ClientName"];
                                                    eventRow.PhoneNumber = mess_parts["PhoneNumber"];
                                                    eventRow.EventTime = DateTime.Now;
                                                    eventRow.EventText = organization + "\r\n" + (string)srowClient[0]["ClientName"] + " " + DateTime.Parse(mess_parts["EventTime"]).ToShortDateString() + "\r\n" + cur_message;
                                                    eventRow.EventType = cur_event;
                                                    eventRow.Status = "Отправляется";

                                                    eventTable.AddEeventLogRow(eventRow);

                                                    //f.Invoke(new UpdateDgv(UpdateDatagridview), new object[] { eventRow });


                                                    bool sended = this.SendSMS(mess_parts["PhoneNumber"], Transliteration.Transliteration.Front(eventRow.EventText));//organization + "\r\n" + mess_parts["ClientName"] + " " + mess_parts["Event"] + " v " + mess_parts["EventTime"])); //+ " " + mess_parts["EventTime"] + " " + mess_parts["Zone"]);



                                                    if (sended)
                                                    {
                                                        eventRow.Status = "Отправлено";



                                                    }
                                                    else
                                                    {
                                                        eventRow.Status = "Ошибка";


                                                    }

                                                    //f.Invoke(new UpdateDgv(UpdateDatagridview), new object[] { null });
                                                    da.Update(eventRow);


                                                    f.Invoke(new UpdateDgv(UpdateDatagridview), new object[] { eventRow });
                                                    cur_event = "";
                                                    cur_message = "";
                                                    da.Fill(eventTable);
                                                }





                                            }


                                        }
                                    }




                                }

                                if (IsExit)
                                    break;
                                if (beginDate > DateTime.Now.AddSeconds(-this.interval))
                                {
                                    int j = 0;
                                    int sekund5 = this.interval / 5;
                                    while (!(sekund5 <= j))
                                    {
                                        if (IsExit)
                                            break;
                                        Thread.Sleep(5000);
                                        j++;
                                    }
                                }

                                beginDate = beginDate.AddSeconds(this.interval);
                                if (IsExit)
                                { break; }
                            }
                            else
                            {
                                //MessageBox.Show(clientdata.errorMessage);
                                this.SendSMS("error", clientdata.errorMessage);
                            }
                            //f = this.dgv.FindForm();
                            //f.Invoke(new UpdateDgv(UpdateDatagridview));
                            //del = new UpdateDgv(UpdateDatagridview);
                            //del.Invoke();


                        }
                        //th.Abort();
                        timer.Stop();
                    }
                    catch (Exception ex)
                    {
                        this.SendSMS("error", ex.Message);
                    }

                }
                else
                {
                    //MessageBox.Show(clientdata.errorMessage);
                    this.SendSMS("error", clientdata.errorMessage);
                }
                e.Result = eventTable;
            }



            //throw new NotImplementedException();
        }
        Random err = new Random(10);
        //string errorMessage;
        delegate void UpdateDgv(localDBDataSet.EeventLogRow row);
        delegate void UpdateLv(string str);

        public DataGridView dgv;
        public RichTextBox rtb;
        private void UpdateDatagridview(localDBDataSet.EeventLogRow row)
        {

            object lockobj = new object();
            lock (lockobj)
            {
                if (this.dgv != null)
                {
                    //if(row==null)
                    //{
                        this.dgv.Rows.Add(new object[] { row.EventlogID, row.PhoneNumber, row.ClientName, row.EventTime, row.Status, row.EventText });
                        this.dgv.Refresh();

                    //}
                    //else
                    //{
                    //    this.dgv.DataSource = table.DefaultView;
                    //}
                    
                    //this.dgv.ScrollBars = ScrollBars.None;
                    //this.dgv.Refresh();
                    //this.dgv.ScrollBars = ScrollBars.Both;
                    
                }
            }
 
        }
        private void UpdateErrorList(string str)
        {
            rtb.Text += str + "\r\n";
 
        }
 

        private bool SendSMS(string phone, string txt)
        {
              bool sended ;
                if (phone != "error")
                {
                    try
                    {
                        //MessageBox.Show(phone + ":\r\n" + txt);
                        sended = this.objectSMS.sendMsg(this.port, ("+998" + phone).Replace("-", ""), txt);
                        if (objectSMS.r_message != null)
                        {
                            Form f = this.rtb.FindForm();
                            if(rtb.InvokeRequired)
                            f.Invoke(new UpdateLv(UpdateErrorList), new object[] { "Modem received:"+objectSMS.r_message });
                        }
                    }
                    catch (Exception ex)
                    {
                        Form f = this.rtb.FindForm();
                        if(rtb.InvokeRequired)
                        f.Invoke(new UpdateLv(UpdateErrorList), new object[] {"Modem:"+ex.Message});
                        //System.IO.StreamWriter writer = new System.IO.StreamWriter("templog", true);
                        //writer.WriteLine("Modem Error:" + phone + " Message-" + ex.Message);
                        //writer.Close();
                        return false;

                    }
                }
                else
                {
                        Form f = this.rtb.FindForm();
                        f.Invoke(new UpdateLv(UpdateErrorList), new object[] { "Code:"+phone + ":Message-" + txt });
                        return false;
                    
                }
                
                return sended;
            
        }

        private Dictionary<string, string> MessageParts(DataRow row, DataRow srow)
        {
            Dictionary<string, string> messageparts = new Dictionary<string, string>(4);
            messageparts["PhoneNumber"] = srow["PhoneNumber"].ToString();
            messageparts["ClientName"] = srow["ClientName"].ToString();
            string ev;
            if((row["Event"].ToString().ToUpper().Contains("ВЫХОД")))
            {
                ev = "ch";
            }else if((row["Event"].ToString().ToUpper().Contains("ВХОД")))
            {
                ev = "k";
            }else
            {
                ev = row["Event"].ToString();
            }
            messageparts["Event"] = ev;
            messageparts["EventTime"] = row["EventTime"].ToString();
           
           
            return messageparts;

        }

        

    }
}
