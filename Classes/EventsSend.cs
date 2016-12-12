using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace SMSapplication.Classes
{
    class EventsSend
    {
        public DataGridView dgvEvents;
        public EventsSend(DataGridView dgv)
        {
            dgvEvents = dgv;
                       
        }
        
        public void SendOneByOne()
        {
            DataTable dtEvents = (DataTable)dgvEvents.DataSource;
            foreach (DataRow dr in dtEvents.Rows)
            {
                if ((bool)dr["IsSend"] == false)
                {
                    
                }
            }
        }


    }
}
