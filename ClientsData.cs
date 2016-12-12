using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace SMSapplication
{   

    class ClientsData
    {
        public class DataSettings
        {
            public OleDbConnection connection;
            public Dictionary<string, OleDbDataAdapter> adapters;

            public DataSettings()
            {
                adapters = new Dictionary<string, OleDbDataAdapter>();
 
            }

        }


        //public string ConnectionString;
        public string errorMessage;
        public DataSet ds;
        public static DataSettings dataSettings;
        public string selectDate;
        public string OtmetkaProxod = "";
        //public string selectTime;

        private string SelectQuery()
        {
            string proxod="";
            if (OtmetkaProxod == "Отметка")
            {
                proxod = OtmetkaProxod;

            }
            else if (OtmetkaProxod == "Проход")
            {
                proxod = "по карточке";
            }

            string selectstring = "select (EvDate & EvTime & ClBdg) as IDNum,  ClBdg, CDate(Format(EvDate, \"Short Date\")+\" \"+Format(EvTime, \"Long Time\")) AS EventTime, [Event] from Evntlog_tbl ";
            string where = " where ";
            if (selectDate != null) where += "( CDate(Format(EvDate, \"Short Date\")+\" \"+Format(EvTime, \"Long Time\"))>=CDate(\"" + selectDate + "\") and (Event like \"%"+proxod+"%\"))";
            string orderby = " order by (Evntlog_tbl.EvDate+Evntlog_tbl.EvTime) asc";
            string query = selectstring + (where.Length > 7 ? where : "") + orderby;
            return query;

        }

        public bool ConnectingBase(string dbfile)
        {
            
            try 
            {
                OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbfile + ";User Id=admin;Jet OLEDB:Database Password=\"DthbCjabcnbrt\"");//Provider=Microsoft.Jet.OLEDB.4.0;Data Source="H:\docs\SMSapplication\22-01-2015_08-06-56\contrdb (1).mdb";Jet OLEDB:Database Password=DthbCjabcnbrt
                OleDbDataAdapter adapterClients = new OleDbDataAdapter("select BadgeNum as BdgNum, Name as ClientName from Clients_tbl", connection);
                this.ds = new DataSet();
                DataTable clients = new DataTable("clients");
                adapterClients.Fill(clients);
                ds.Tables.Add(clients);

                
                //SELECT (Evntlog_tbl.EvDate+Evntlog_tbl.EvTime) AS CurrentTime, Evntlog_tbl.ClBdg, Evntlog_tbl.Name, Evntlog_tbl.Event, Evntlog_tbl.Zone, Evntlog_tbl.Dev, Evntlog_tbl.DepNum, Evntlog_tbl.Link
                //FROM Evntlog_tbl
                //WHERE ( (Evntlog_tbl.EvDate+Evntlog_tbl.EvTime)>#2/28/2014 1:05:29 AM#) order by (Evntlog_tbl.EvDate+Evntlog_tbl.EvTime) asc ;
                //if(selectDate!=null) where += (where.Length>7 ?"and EvTime>'"+selectTime+"' ":"EvTime>#"+selectTime+"'#");

                
                if (connection.State == ConnectionState.Open) connection.Close();
                OleDbDataAdapter adapterEvents = new OleDbDataAdapter(this.SelectQuery() , connection);//"select * from Evntlog_tbl", connection);
                DataTable events = new DataTable("eventlog");
                events.Columns.Add("IDNum", typeof(string));
                events.PrimaryKey = new DataColumn[] { events.Columns["IDNum"] };

                events.Columns.Add("ClBdg", typeof(int));
                events.Columns.Add("EventTime", typeof(DateTime));
                //events.Columns.Add("EvTime", typeof(DateTime));
                //events.Columns.Add("Name", typeof(string));
                events.Columns.Add("Event", typeof(string));

                DataRow r = events.NewRow();
                r["IDNum"] = "00:00:00000";
                events.Rows.Add(r);
                //adapterEvents.Fill(events);
                ds.Tables.Add(events);

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                dataSettings = new DataSettings();
                dataSettings.adapters.Add("clients", adapterClients);
                dataSettings.adapters.Add("eventlog", adapterEvents);
                dataSettings.connection = connection;
                return true; 
            }catch(OleDbException ex)
            {
                this.errorMessage = ex.Message;
            }
            return false;
        }

        public bool SyncroniseTables(string tablename)
        {
           OleDbDataAdapter adapter;
           try
           {
               switch (tablename)
               {
                   case "clients":

                       adapter = dataSettings.adapters["clients"];
                       adapter.Fill(ds.Tables["clients"]);

                       break;
                   case "eventlog":
                       adapter = dataSettings.adapters["eventlog"];
                       adapter.SelectCommand.CommandText = SelectQuery();
                       adapter.Fill(ds.Tables["eventlog"]);
                       break;

               }
               if (dataSettings.connection.State == ConnectionState.Open)
                   dataSettings.connection.Close();
               return true;
           }
           catch (OleDbException ex)
           { this.errorMessage = ex.Message; }
           return false;
        }

        private bool SyncroniseTables()
        {
            bool result;
            result = SyncroniseTables("clients");
            if (!result) return false;
            result = SyncroniseTables("eventlog");
            if (!result) return false;
            else return true;
 
        }

        public DataTable CopyTable(DataTable fromTable, DataTable toTable, string checkCol)
        {
            
                foreach (DataRow row in fromTable.Rows)
                {
                    try
                    {

                        //DataRow[] rows = toTable.Select("BdgNum=" + row["BdgNum"]);
                        //if (rows.Length == 0)
                        //{
                        toTable.Rows.Add(row.ItemArray);
                        //}
                        //else
                        //{

                        //}
                    }
                    catch (ConstraintException ex)
                    {
                        continue;
                    }
                       

                }
            
            //toTable.Merge(fromTable,true);

            //DataRow[] rowsto = new DataRow[fromTable.Rows.Count];
            //fromTable.Rows.CopyTo(rowsto, 0);

            return toTable;

        }


    }
}
