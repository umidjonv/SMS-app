using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SMSapplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //if (DateTime.Now.Month >= 6 && DateTime.Now.Day >= 1)
            //{
            //    MessageBox.Show("Истек период использования ПО!");
            //    return;
            //}
            SMSapplication mainapp = new SMSapplication();
            if (!mainapp.isexit) Application.Run();
        }
    }
}