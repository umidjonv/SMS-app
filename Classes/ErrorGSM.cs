using System;
using System.Collections.Generic;
using System.Text;

namespace SMSapplication.Classes
{
    public static class ErrorGSM
    {
        static Dictionary<int, string> CMSE = new Dictionary<int, string>();
        static Dictionary<int, string> CMEE = new Dictionary<int, string>();
        static ErrorGSM()
        {
            CMSE.Add(1, "Unassigned number");
            CMSE.Add(8, "Operator determined barring");
            CMSE.Add(10, "Call bared");
            CMSE.Add(21, "Short message transfer rejected");
            CMSE.Add(27, "Destination out of service");
            CMSE.Add(28, "Unindentified subscriber");
            CMSE.Add(29, "Facility rejected");
            CMSE.Add(30, "Unknown subscriber");
            CMSE.Add(38, "Network out of order");
            CMSE.Add(41, "Temporary failure");
            CMSE.Add(42, "Congestion");
            CMSE.Add(47, "Recources unavailable");
            CMSE.Add(50, "Requested facility not subscribed");
            CMSE.Add(69, "Requested facility not implemented");
            CMSE.Add(81, "Invalid short message transfer reference value");
            CMSE.Add(95, "Invalid message unspecified");
            CMSE.Add(96, "Invalid mandatory information");
            CMSE.Add(97, "Message type non existent or not implemented");
            CMSE.Add(98, "Message not compatible with short message protocol");
            CMSE.Add(99, "Information element non-existent or not implemente");
            CMSE.Add(111, "Protocol error, unspecified");
            CMSE.Add(127, "Internetworking , unspecified");
            CMSE.Add(128, "Telematic internetworking not supported");
            CMSE.Add(129, "Short message type 0 not supported");
            CMSE.Add(130, "Cannot replace short message");
            CMSE.Add(143, "Unspecified TP-PID error");
            CMSE.Add(144, "Data code scheme not supported");
            CMSE.Add(145, "Message class not supported");
            CMSE.Add(159, "Unspecified TP-DCS error");
            CMSE.Add(160, "Command cannot be actioned");
            CMSE.Add(161, "Command unsupported");
            CMSE.Add(175, "Unspecified TP-Command error");
            CMSE.Add(176, "TPDU not supported");
            CMSE.Add(192, "SC busy");
            CMSE.Add(193, "No SC subscription");
            CMSE.Add(194, "SC System failure");
            CMSE.Add(195, "Invalid SME address");
            CMSE.Add(196, "Destination SME barred");
            CMSE.Add(197, "SM Rejected-Duplicate SM");
            CMSE.Add(198, "TP-VPF not supported");
            CMSE.Add(199, "TP-VP not supported");
            CMSE.Add(208, "D0 SIM SMS Storage full");
            CMSE.Add(209, "No SMS Storage capability in SIM");
            CMSE.Add(210, "Error in MS");
            CMSE.Add(211, "Memory capacity exceeded");
            CMSE.Add(212, "Sim application toolkit busy");
            CMSE.Add(213, "SIM data download error");
            CMSE.Add(255, "Unspecified error cause");
            CMSE.Add(300, "ME Failure");
            CMSE.Add(301, "SMS service of ME reserved");
            CMSE.Add(302, "Operation not allowed");
            CMSE.Add(303, "Operation not supported");
            CMSE.Add(304, "Invalid PDU mode parameter");
            CMSE.Add(305, "Invalid Text mode parameter");
            CMSE.Add(310, "SIM not inserted");
            CMSE.Add(311, "SIM PIN required");
            CMSE.Add(312, "PH-SIM PIN required");
            CMSE.Add(313, "SIM failure");
            CMSE.Add(314, "SIM busy");
            CMSE.Add(315, "SIM wrong");
            CMSE.Add(316, "SIM PUK required");
            CMSE.Add(317, "SIM PIN2 required");
            CMSE.Add(318, "SIM PUK2 required");
            CMSE.Add(320, "Memory failure");
            CMSE.Add(321, "Invalid memory index");
            CMSE.Add(322, "Memory full");
            CMSE.Add(330, "SMSC address unknown");
            CMSE.Add(331, "No network service");
            CMSE.Add(332, "Network timeout");
            CMSE.Add(340, "No +CNMA expected");
            CMSE.Add(500, "Unknown error");
            CMSE.Add(512, "User abort");
            CMSE.Add(513, "Unable to store");
            CMSE.Add(514, "Invalid Status");
            CMSE.Add(515, "Device busy or Invalid Character in string");
            CMSE.Add(516, "Invalid length");
            CMSE.Add(517, "Invalid character in PDU");
            CMSE.Add(518, "Invalid parameter");
            CMSE.Add(519, "Invalid length or character");
            CMSE.Add(520, "Invalid character in text");
            CMSE.Add(521, "Timer expired");
            CMSE.Add(522, "Operation temporary not allowed");
            CMSE.Add(532, "SIM not ready");
            CMSE.Add(534, "Cell Broadcast error unknown");
            CMSE.Add(535, "Protocol stack busy");
            CMSE.Add(538, "Invalid parameter");

            CMEE.Add(0, "Phone failure");
            CMEE.Add(1, "No connection to phone");
            CMEE.Add(2, "Phone adapter link reserved");
            CMEE.Add(3, "Operation not allowed");
            CMEE.Add(4, "Operation not supported");
            CMEE.Add(5, "PH_SIM PIN required");
            CMEE.Add(6, "PH_FSIM PIN required");
            CMEE.Add(7, "PH_FSIM PUK required");
            CMEE.Add(10, "SIM not inserted");
            CMEE.Add(11, "SIM PIN required");
            CMEE.Add(12, "SIM PUK required");
            CMEE.Add(13, "SIM failure");
            CMEE.Add(14, "SIM busy");
            CMEE.Add(15, "SIM wrong");
            CMEE.Add(16, "Incorrect password");
            CMEE.Add(17, "SIM PIN2 required");
            CMEE.Add(18, "SIM PUK2 required");
            CMEE.Add(20, "Memory full");
            CMEE.Add(21, "Invalid index");
            CMEE.Add(22, "Not found");
            CMEE.Add(23, "Memory failure");
            CMEE.Add(24, "Text string too long");
            CMEE.Add(25, "Invalid characters in text string");
            CMEE.Add(26, "Dial string too long");
            CMEE.Add(27, "Invalid characters in dial string");
            CMEE.Add(30, "No network service");
            CMEE.Add(31, "Network timeout");
            CMEE.Add(32, "Network not allowed, emergency calls only");
            CMEE.Add(40, "Network personalization PIN required");
            CMEE.Add(41, "Network personalization PUK required");
            CMEE.Add(42, "Network subset personalization PIN required");
            CMEE.Add(43, "Network subset personalization PUK required");
            CMEE.Add(44, "Service provider personalization PIN required");
            CMEE.Add(45, "Service provider personalization PUK required");
            CMEE.Add(46, "Corporate personalization PIN required");
            CMEE.Add(47, "Corporate personalization PUK required");
            CMEE.Add(48, "PH-SIM PUK required");
            CMEE.Add(100, "Unknown error");
            CMEE.Add(103, "Illegal MS");
            CMEE.Add(106, "Illegal ME");
            CMEE.Add(107, "GPRS services not allowed");
            CMEE.Add(111, "PLMN not allowed");
            CMEE.Add(112, "Location area not allowed");
            CMEE.Add(113, "Roaming not allowed in this location area");
            CMEE.Add(126, "Operation temporary not allowed");
            CMEE.Add(132, "Service operation not supported");
            CMEE.Add(133, "Requested service option not subscribed");
            CMEE.Add(134, "Service option temporary out of order");
            CMEE.Add(148, "Unspecified GPRS error");
            CMEE.Add(149, "PDP authentication failure");
            CMEE.Add(150, "Invalid mobile class");
            CMEE.Add(256, "Operation temporarily not allowed");
            CMEE.Add(257, "Call barred");
            CMEE.Add(258, "Phone is busy");
            CMEE.Add(259, "User abort");
            CMEE.Add(260, "Invalid dial string");
            CMEE.Add(261, "SS not executed");
            CMEE.Add(262, "SIM Blocked");
            CMEE.Add(263, "Invalid block");
            CMEE.Add(772, "SIM powered down");
            

        }

        public static string GetError(string str)
        {
            try
            {  //+CMS ERROR: 69
                str = str.Substring(str.IndexOf('+'));
                string[] errParts = str.Split(new char[] {' '});
                if (errParts[0][0] == '+')
                {
                    errParts[0] = errParts[0].Replace("\r", "");
                    errParts[errParts.Length - 1] = errParts[errParts.Length - 1].Replace("\r", "");
                    errParts[0] = errParts[0].Replace("\n", "");
                    errParts[errParts.Length - 1] = errParts[errParts.Length - 1].Replace("\n", "");
                    Dictionary<int, string> errDic = gettype(errParts[0].Substring(1));
                    string error;
                    if(errDic.TryGetValue(Int32.Parse(errParts[errParts.Length - 1]), out error))
                    {
                        return error + " Code:" + errParts[errParts.Length - 1];
                    }
                    else return "Unknown modem error"+errParts[errParts.Length - 1];

                }else return  "Unknown modem error "+errParts[errParts.Length - 1] ;
                
            }
            catch (Exception ex) { throw ex; }
        }

        private static Dictionary<int, string> gettype(string type)
        {
            //Dictionary<int, string> err = CMSE;
            if (type == "CMS" || type == "cms")
            {
                return CMSE; 
            }
            return CMEE;
            
        }
        /*
   
         */
    }
}
