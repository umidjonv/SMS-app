using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace SMSapplication
{
    public class clsSMS
    {
        public static bool AT_CMGF= false;
        public static int command_timeout = 2000;
        public bool textMode = false;
        #region Open and Close Ports
        //Open Port
        public SerialPort OpenPort(string p_strPortName, int p_uBaudRate, int p_uDataBits, int p_uReadTimeout, int p_uWriteTimeout)
        {
            receiveNow = new AutoResetEvent(false);
            SerialPort port = new SerialPort();

            try
            {           
                port.PortName = p_strPortName;                 //COM1
                port.BaudRate = p_uBaudRate;                   //9600
                port.DataBits = p_uDataBits;                   //8
                port.StopBits = StopBits.One;                  //1
                port.Parity = Parity.None;                     //None
                port.ReadTimeout = p_uReadTimeout;             //300
                port.WriteTimeout = p_uWriteTimeout;           //300
                port.Encoding = Encoding.GetEncoding("iso-8859-1");
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                port.Open();
                port.DtrEnable = true;
                port.RtsEnable = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return port;
        }

        //Close Port
        public void ClosePort(SerialPort port)
        {
            try
            {
                port.Close();
                port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                port = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Execute AT Command
        public string ExecCommand(SerialPort port,string command, int responseTimeout, string errorMessage)
        {
            try
            {
               
                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                receiveNow.Reset();
                port.Write(command + "\r");
           
                string input = ReadResponse(port, responseTimeout, errorMessage);
                if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n") && input.ToUpper().Contains("ERROR"))))
                    throw new ApplicationException("No success message was received."+input);
                return input;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   

        //Receive data from port
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    receiveNow.Set();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ReadResponse(SerialPort port,int timeout, string errorMessage)
        {
            string buffer = string.Empty;
            try
            {    
                do
                {
                    if (receiveNow.WaitOne(timeout, false))
                    {
                        string t = port.ReadExisting();
                        buffer += t;
                    }
                    else
                    {
                        if (buffer.Length > 0)
                        {
                            if (buffer.ToUpper().Contains("ERROR"))
                            {
                                throw new ApplicationException("err:" + buffer+"."+errorMessage);
                            }
                            else
                            {
                                buffer += "buffer not have error and ok";
                            }
                            
                        }
                        else
                            throw new ApplicationException("No data received from phone. " + errorMessage);
                    }
                }
                while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> "));//(!buffer.EndsWith("\r\nERROR\r\n")
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }

        #region Count SMS
        public int CountSMSmessages(SerialPort port)
        {
            int CountTotalMessages = 0;
            try
            {

                #region Execute Command

                string recievedData = ExecCommand(port, "AT", 300, "No phone connected at ");
                recievedData = ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                String command = "AT+CPMS?";
                recievedData = ExecCommand(port, command, 1000, "Failed to count SMS message");
                int uReceivedDataLength = recievedData.Length;

                #endregion

                #region If command is executed successfully
                if ((recievedData.Length >= 45) && (recievedData.StartsWith("AT+CPMS?")))
                {

                    #region Parsing SMS
                    string[] strSplit = recievedData.Split(',');
                    string strMessageStorageArea1 = strSplit[0];     //SM
                    string strMessageExist1 = strSplit[1];           //Msgs exist in SM
                    #endregion

                    #region Count Total Number of SMS In SIM
                    CountTotalMessages = Convert.ToInt32(strMessageExist1);
                    #endregion

                }
                #endregion

                #region If command is not executed successfully
                else if (recievedData.Contains("ERROR"))
                {

                    #region Error in Counting total number of SMS
                    string recievedError = recievedData;
                    recievedError = recievedError.Trim();
                    recievedData = "Following error occured while counting the message" + recievedError;
                    #endregion

                }
                #endregion

                return CountTotalMessages;

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        #endregion

        #region Read SMS

        public AutoResetEvent receiveNow;

        public ShortMessageCollection ReadSMS(SerialPort port, string p_strCommand)
        {

            // Set up the phone and read the messages
            ShortMessageCollection messages = null;
            try
            {

                #region Execute Command
                // Check connection
                ExecCommand(port,"AT", 300, "No phone connected");
                // Use message format "Text mode"
                ExecCommand(port,"AT+CMGF=1", 300, "Failed to set message format.");
                // Use character set "PCCP437"
                ExecCommand(port,"AT+CSCS=\"PCCP437\"", 300, "Failed to set character set.");
                // Select SIM storage
                ExecCommand(port,"AT+CPMS=\"SM\"", 300, "Failed to select message storage.");
                // Read the messages
                string input = ExecCommand(port, p_strCommand, 5000, "Failed to read the messages.");
                #endregion

                #region Parse messages
                messages = ParseMessages(input);
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (messages != null)
                return messages;
            else
                return null;
        
        }
        public ShortMessageCollection ParseMessages(string input)
        {
            ShortMessageCollection messages = new ShortMessageCollection();
            try
            {     
                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)\r\n");
                Match m = r.Match(input);
                while (m.Success)
                {
                    ShortMessage msg = new ShortMessage();
                    //msg.Index = int.Parse(m.Groups[1].Value);
                    msg.Index = m.Groups[1].Value;
                    msg.Status = m.Groups[2].Value;
                    msg.Sender = m.Groups[3].Value;
                    msg.Alphabet = m.Groups[4].Value;
                    msg.Sent = m.Groups[5].Value;
                    msg.Message = m.Groups[6].Value;
                    messages.Add(msg);

                    m = m.NextMatch();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return messages;
        }

        #endregion

        #region Send SMS
       
        static AutoResetEvent readNow = new AutoResetEvent(false);
        public void SetTextMode(SerialPort port)
        {
            try 
            {
                string receive = this.ExecCommand(port, "AT+CSMP?", command_timeout+1000, "Message 'text mode' params not received");
                
                string[] blocks = receive.Split(new char[] { ',' });
                int len = blocks.Length;
                if (len > 0 && blocks[len - 1].ToString().Contains("8"))
                {
                    string csmp = "";
                    blocks[len - 1] = "0";
                    for (int i = 0; i < len; i++)
                    {
                        csmp += blocks[i];
                    }
                    string recievedData = this.ExecCommand(port, "AT+CSMP=" + csmp, command_timeout+1000, "Set message text mode error");
                    this.textMode = true;
                    
                    this.textMode = true;
                }
                else
                {
                    if (len > 0 && blocks[len - 1].ToString().Contains("0"))
                    {
                        this.textMode = true;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public string r_message;
        public bool sendMsg(SerialPort port, string PhoneNo, string Message)
        {
            bool isSend = false;

            try
            {
                string recievedData;
                //if (!AT_CMGF)
                //{
                recievedData = ExecCommand(port, "AT", command_timeout, "No phone connected");
                //if (!this.textMode)
                //this.SetTextMode(port);
                recievedData = ExecCommand(port, "AT+CMGF=1", command_timeout, "Failed to set message format.");
                
                AT_CMGF = true;
                //}
                
                String command = "AT+CMGS=\"" + PhoneNo + "\"";
                recievedData = ExecCommand(port, command, command_timeout, "Failed to accept phoneNo");         
                command = Message + char.ConvertFromUtf32(26) + "\r";
                recievedData = ExecCommand(port,command, 60000, "Failed to send message"); //3 seconds
                
                if (recievedData.Contains("\r\nOK\r\n"))
                {
                    r_message = null;
                    return true;
                }
                else if (recievedData.Contains("ERROR"))
                {
                    r_message = null;
                    return false;
                }
                r_message = recievedData;
                return true;
            }
            catch (Exception ex)
            {
                //no data  received from phone
                throw ex; 
            }
          
        }

        public bool sendMsgUnicode(SerialPort port, string phone, string Message)
        {
            bool isSend = false;

            try
            {
                Message = StringToUCS22(Message);

                string SCA = "00";
                string pdu_type = "01";
                string tp_mr = "00";
                string tp_da = phone.Replace("+", "").Length.ToString("X2") + "91" + this.EncodePhoneNumber(phone);//"0C91999863063336";//998936603363//999893052520";
                string tp_pid = "00";
                string tp_dcs = "08";
                string tp_udl = (Message.Length * 2).ToString("X2");//"12";
                string tp_ud = Message;//"041F04400438043204350442002100210021";

                Message = SCA + pdu_type + tp_mr + tp_da + tp_pid + tp_dcs + tp_udl + tp_ud;



                string recievedData = ExecCommand(port, "AT", 300, "No phone connected");
                recievedData = ExecCommand(port, "AT+CMGF=0", 300, "Failed to set message format.");
                String command = "AT+CMGS=" + ((Message.Length/2)-1);
                recievedData = ExecCommand(port, command, 500, "Failed to accept phoneNo");
                command = Message + char.ConvertFromUtf32(26) + "\r";
                recievedData = ExecCommand(port, command, 3000, "Failed to send message"); //3 seconds
                if (recievedData.EndsWith("\r\nOK\r\n"))
                {
                    isSend = true;
                }
                else if (recievedData.Contains("ERROR"))
                {
                    isSend = false;
                }
                return isSend;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string StringToUCS2(string str)
        {
            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] ucs2 = ue.GetBytes(str);
            int i = 0;
            while (i < ucs2.Length)
            {
                byte b = ucs2[i + 1];
                ucs2[i + 1] = ucs2[i];
                ucs2[i] = b;
                i += 2;
            }
            
            return BitConverter.ToString(ucs2).Replace("-", "");
        }

        public string StringToUCS22(string str)
        {
            Encoding iso_8859_1 = Encoding.GetEncoding("windows-1251");//"iso-8859-1");
            byte[] isoBytes = iso_8859_1.GetBytes(str);

            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] ucs2 = Encoding.Convert(iso_8859_1, ue, isoBytes);
            // = isoBytes;
            int i = 0;
            while (i < ucs2.Length)
            {
                byte b = ucs2[i + 1];
                ucs2[i + 1] = ucs2[i];
                ucs2[i] = b;
                i += 2;
            }

            return BitConverter.ToString(ucs2).Replace("-", "");
        }

        public string EncodePhoneNumber(string phoneNumber)
        {
            string result = "";
            phoneNumber = phoneNumber.Replace("+", "");
            if ((phoneNumber.Length % 2) > 0) phoneNumber += "F";
            int i = 0;
            while (i < phoneNumber.Length)
            {
                result += phoneNumber[i + 1].ToString() + phoneNumber[i].ToString();
                i += 2;
            }
            return result.Trim();
 
        }

        static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                    readNow.Set();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Delete SMS
        public bool DeleteMsg(SerialPort port , string p_strCommand)
        {
            bool isDeleted = false;
            try
            {

                #region Execute Command
                string recievedData = ExecCommand(port,"AT", 300, "No phone connected");
                recievedData = ExecCommand(port,"AT+CMGF=1", 300, "Failed to set message format.");
                String command = p_strCommand;
                recievedData = ExecCommand(port,command, 300, "Failed to delete message");
                #endregion

                if (recievedData.EndsWith("\r\nOK\r\n"))
                {
                    isDeleted = true;
                }
                if (recievedData.Contains("ERROR"))
                {
                    isDeleted = false;
                }
                return isDeleted;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            
        }  
        #endregion

    }
}
