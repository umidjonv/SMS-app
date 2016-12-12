using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace SMSapplication.Classes
{
    public class Settings
    {

        public string clientDB = "";
        public int CheckInterval = 20;
        public string organization = "";
        public int ErrorInterval = 10;
        public int PrevEventsInterval = 1;
        public DateTime beginDate = DateTime.Now;
        public string OtmetkaProxod = "";
        public string port;
        public bool SetClientDB()
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Access file(*.mdb)|*.mdb";
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                this.clientDB = fileDlg.FileName;
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool SaveSettings()
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath);
                StreamWriter sw = new StreamWriter(path+"\\settings.db");
                sw.WriteLine("ClientDB:" + this.clientDB);
                sw.WriteLine("organization:" + this.organization);
                sw.WriteLine("CheckInterval:" + this.CheckInterval);
                sw.WriteLine("ErrorInterval:" + this.ErrorInterval);
                sw.WriteLine("beginDate:" + this.beginDate);
                sw.WriteLine("PrevEventsInterval:" + this.PrevEventsInterval);
                sw.WriteLine("OtmetkaProxod:" + this.OtmetkaProxod);
                if(this.port!=null)
                    sw.WriteLine("port:" + this.port);
                else sw.WriteLine("port:");
                sw.Close();
                return true;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message); 
            }
            return false;
        }

        public bool LoadSettings()
        {
            try
            {
                if (File.Exists("settings.db"))
                {
                    string path = Path.GetDirectoryName(Application.ExecutablePath);
                    StreamReader sr = new StreamReader(path+"\\settings.db");
                    this.clientDB = sr.ReadLine().Replace("ClientDB:", "");
                    this.organization = sr.ReadLine().Replace("organization:", "");
                    this.CheckInterval = int.Parse(sr.ReadLine().Replace("CheckInterval:",""));
                    this.ErrorInterval = int.Parse(sr.ReadLine().Replace("ErrorInterval:",""));
                    this.beginDate = DateTime.Parse(sr.ReadLine().Replace("beginDate:", ""));
                    this.PrevEventsInterval = int.Parse(sr.ReadLine().Replace("PrevEventsInterval:", ""));
                    this.OtmetkaProxod = sr.ReadLine().Replace("OtmetkaProxod:", "");
                    this.port = sr.ReadLine().Replace("port:", "");
                    sr.Close();
                    return true;
                }
                else
                {
                    if (System.Windows.Forms.MessageBox.Show("Не указана основная база данных, выбрать БД?", "База данных не указана", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (SetClientDB())
                        {
                            SaveSettings();
                            return true;
                        }
                        else
                        {
                            this.LoadSettings();
                        }
                    }
                    else return false;

                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }


       



    }
}
