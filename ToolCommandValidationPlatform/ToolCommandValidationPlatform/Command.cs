using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMEIC.TMdN.DriveController
{
    public class Command
    {

        # region Member Variables Declaration

        private string m_CommandMessage;
        private int m_ID;
        //private CDriveConnectionInformation m_DriveConnectionInfo;
        //private MessageCallBackDelegate m_messageCallBack;
        //public delegate void ReceiveCallBackDelegate(Command a_Command);
        //private ReceiveCallBackDelegate m_ReceiveCallBack;
        //public delegate void DriveReceiveCallBackDelegate(Command a_Command);
        //private DriveReceiveCallBackDelegate m_DriveReceiveCallBack;
        //private ArrayList m_ReturnData;
        //private string m_CommandType;
        //private ArrayList m_SendArgList;
        //private ArrayList m_ArgListRequired;
        //private ArrayList m_RealTimeTrendListRef;
        //private int m_PasswordID;
        //private string m_ParamStartingAddress;
        //private int m_Position;
        //private ArrayList m_DataPersistAtRAMtoEEPROM;
        //private int m_ResponseDataCount = 0;
        //private int m_RAMStringDataCount = 0;
        //private int m_RAMReceivedStringDataCount = 0;
        //private CEventInfo m_FunctionEvent;
        //private ArrayList m_RequestList;
        //private ERequestTypes m_RequestType;
        //private string m_UserName;
        //private string m_BaseAddressForString;
        //private ArrayList m_InfParamList;
        private string m_InternalCommandType;
        private object m_DataStore;
        private string m_DriveId;   //bug 1929
        private int m_ResendCount = 0; //2292
        private bool m_IsDriveResponding = true; //2292
        private string m_CommVerion;    //Bug 1929
        private int m_ResendCommandCount = 0;//bug 2405
        private int m_NewAnswerLength = 0; // bug 2534
        private int m_NewRequestLength = 0; // bug 2534
        private string a_BitGroupName; // bug 2552

        //ForRemoting
        private string m_Uri;

        //For bug #824
        private bool m_SendCommandWithoutPassword = false;

        #region Bug 1929

        private System.Timers.Timer m_DriveResponseCheckTimer;

        public delegate void CommandResendDelegate(Command a_Command);
        private CommandResendDelegate m_CommandResendCallback;

        #endregion

        #endregion

        #region Constants

        private const int WAIT_TIME_TO_CHECK_DRIVE_RESPONSE = 4000;

        #endregion

        #region Properties

        public bool SendCommandWithoutPassword
        {
            get
            {
                return m_SendCommandWithoutPassword;
            }
            set
            {
                m_SendCommandWithoutPassword = value;
            }
        }

        //public MessageCallBackDelegate MessageCallBack
        //{
        //    get
        //    {
        //        return m_messageCallBack;
        //    }
        //    set
        //    {
        //        m_messageCallBack = value;
        //    }
        //}

        //public ReceiveCallBackDelegate ReceiveCallBack
        //{
        //    get
        //    {
        //        return m_ReceiveCallBack;
        //    }
        //    set
        //    {
        //        m_ReceiveCallBack = value;
        //    }
        //}

        //public DriveReceiveCallBackDelegate DriveReceiveCallBack
        //{
        //    get
        //    {
        //        return m_DriveReceiveCallBack;
        //    }
        //    set
        //    {
        //        m_DriveReceiveCallBack = value;
        //    }
        //}

        public string CommandMessage
        {

            get
            {
                return m_CommandMessage;
            }

            set
            {
                m_CommandMessage = value;
            }

        }

        public int ID
        {

            get
            {
                return m_ID;

            }

            set
            {
                m_ID = value;
            }

        }

        //public CDriveConnectionInformation DriveConnectionInfo
        //{

        //    get
        //    {
        //        return m_DriveConnectionInfo;
        //    }

        //    set
        //    {
        //        m_DriveConnectionInfo = value;
        //    }

        //}

        //public ArrayList ReturnData
        //{

        //    get
        //    {
        //        return m_ReturnData;
        //    }

        //    set
        //    {
        //        m_ReturnData = value;
        //    }

        //}

        public string CommandType
        {

            get;
            set;

        }

        //public ArrayList SendArgList
        //{

        //    get
        //    {
        //        return m_SendArgList;
        //    }

        //    set
        //    {
        //        m_SendArgList = value;
        //    }

        //}

        //public ArrayList ArgListRequired
        //{

        //    get
        //    {
        //        return m_ArgListRequired;
        //    }

        //    set
        //    {
        //        m_ArgListRequired = value;
        //    }

        //}

        public int PasswordID
        {
            get;
            set;
        }

        public string ClientURI
        {
            get
            {
                return m_Uri;
            }
            set
            {
                m_Uri = value;
            }
        }

        //public ArrayList RealTimeTrendListRef
        //{

        //    get
        //    {
        //        return m_RealTimeTrendListRef;
        //    }

        //    set
        //    {
        //        m_RealTimeTrendListRef = value;
        //    }

        //}

        public string StartingAddress
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        //public ArrayList DataPersistAtRAMtoEEPROM
        //{

        //    get
        //    {
        //        return m_DataPersistAtRAMtoEEPROM;
        //    }

        //    set
        //    {
        //        m_DataPersistAtRAMtoEEPROM = value;
        //    }

        //}

        public int ResponseDataCount
        {
            get;
            set;
        }

        public int RAMStringDataCount
        {
            get;
            set;
        }

        public int RAMReceivedStringDataCount
        {
            get;
            set;
        }

        //public CEventInfo FunctionEvent
        //{
        //    get
        //    {
        //        return m_FunctionEvent;
        //    }
        //    set
        //    {
        //        m_FunctionEvent = value;
        //    }
        //}

        //public ArrayList RequestList
        //{
        //    get
        //    {
        //        return m_RequestList;
        //    }
        //    set
        //    {
        //        m_RequestList = value;
        //    }
        //}

        //public ERequestTypes RequestType
        //{
        //    get
        //    {
        //        return m_RequestType;
        //    }
        //    set
        //    {
        //        m_RequestType = value;
        //    }
        //}

        public string UserName
        {
            get;
            set;
        }

        public string BaseAddressForString
        {
            get;
            set;
        }

        //public ArrayList InfParamList
        //{

        //    get
        //    {
        //        return m_InfParamList;
        //    }

        //    set
        //    {
        //        m_InfParamList = value;
        //    }

        //}

        public string InternalCommandType
        {
            get
            {
                return m_InternalCommandType;
            }
            set
            {
                m_InternalCommandType = value;
            }
        }

        public object DataStore
        {
            get
            {
                return m_DataStore;
            }
            set
            {
                m_DataStore = value;
            }
        }

        ///<summary>
        ///Added for bug 1676 ,
        ///TMdNClientURI will store a URI of TMdNClient , 
        ///(While ClientURI stores the uri to which the responce is to be sent)
        ///</summary>
        private string m_TMdNClientURI = string.Empty;

        public string TMdNClientURI
        {
            get
            {
                return m_TMdNClientURI;
            }
            set
            {
                m_TMdNClientURI = value;
            }
        }

        #region Bug 1929

        //Bug 1929
        /// <summary>
        /// Delegate to resend command if response not received
        /// </summary>
        public CommandResendDelegate CommandResendCallback
        {
            get { return m_CommandResendCallback; }
            set { m_CommandResendCallback = value; }
        }

        public System.Timers.Timer DriveResponseCheckTimer
        {
            get { return m_DriveResponseCheckTimer; }
            set { m_DriveResponseCheckTimer = value; }
        }

        public string DriveId
        {
            get { return m_DriveId; }
            set { m_DriveId = value; }
        }

        public string CommVerion
        {
            get { return m_CommVerion; }
            set { m_CommVerion = value; }
        }

        #endregion
        public bool IsDriveResponding //2292
        {
            get { return m_IsDriveResponding; }
            set { m_IsDriveResponding = value; }
        }

        public int ResendCount //2292
        {
            get { return m_ResendCount; }
            set { m_ResendCount = value; }
        }

        public int ResendCommandCount //bug 2405
        {
            get { return m_ResendCommandCount; }
            set { m_ResendCommandCount = value; }
        }

        /// <summary>
        /// Gets or sets new request length - 2534
        /// </summary>
        public int NewRequestLength
        {
            get { return m_NewRequestLength; }
            set { m_NewRequestLength = value; }
        }

        public string BitGroupName // bug 2552
        {
            get { return a_BitGroupName; }
            set { a_BitGroupName = value; }
        }

        #endregion

        #region Clone
        /// <summary>
        /// Returns clone of current command - 2534
        /// </summary>
        /// <returns></returns>
        public Command Clone()
        {
            Command command = new Command();
           // command.ArgListRequired = this.ArgListRequired;
           // command.BaseAddressForString = this.BaseAddressForString;
           // command.ClientURI = this.ClientURI;
           // command.CommandMessage = this.CommandMessage;
           // command.CommandResendCallback = this.CommandResendCallback;
           // command.CommandType = this.CommandType;
           // command.CommVerion = this.CommVerion;
           // command.DataPersistAtRAMtoEEPROM = this.DataPersistAtRAMtoEEPROM;
           // command.DataStore = this.DataStore;
           // command.DriveConnectionInfo = this.DriveConnectionInfo;
           // command.DriveId = this.DriveId;
           // command.DriveReceiveCallBack = this.DriveReceiveCallBack;
           // command.DriveResponseCheckTimer = this.DriveResponseCheckTimer;
           // command.FunctionEvent = this.FunctionEvent;
           // command.ID = this.ID;
           // command.InfParamList = this.InfParamList;
           // command.InternalCommandType = this.InternalCommandType;
           // command.IsDriveResponding = this.IsDriveResponding;
           // command.MessageCallBack = this.MessageCallBack;
           // command.PasswordID = this.PasswordID;
           // command.Position = this.Position;
           // command.RAMReceivedStringDataCount = this.RAMReceivedStringDataCount;
           // command.RAMStringDataCount = this.RAMStringDataCount;
           // command.RealTimeTrendListRef = this.RealTimeTrendListRef;
           // command.ReceiveCallBack = this.ReceiveCallBack;
           // command.RequestList = this.RequestList;
           // command.RequestType = this.RequestType;
           // command.ResendCommandCount = this.ResendCommandCount;
           // command.ResendCount = this.ResendCount;
           // command.ResponseDataCount = this.ResponseDataCount;
           // command.ReturnData = this.ReturnData;
           //// command.SendArgList = new ArrayList(m_SendArgList);
           // command.SendCommandWithoutPassword = this.SendCommandWithoutPassword;
           // command.StartingAddress = this.StartingAddress;
           // command.TMdNClientURI = this.TMdNClientURI;
           // command.UserName = this.UserName;
            return command;
        }
        #endregion


        #region Constructor

        /// <summary>
        /// Constructs CCommand class object
        /// </summary>
        /// <permission cref="">Everyone can access this method.</permission>         
        /// <history>
        /// [Patni]	7/07/2008	Created 
        /// </history>

        public Command()
        {
            //m_ReturnData = new ArrayList();
            //m_ArgListRequired = new ArrayList();
           // m_RealTimeTrendListRef = new ArrayList();
            m_DriveResponseCheckTimer = new System.Timers.Timer(WAIT_TIME_TO_CHECK_DRIVE_RESPONSE);
            m_DriveResponseCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(DriveResponseCheckTimer_Elapsed);
        }

        void DriveResponseCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                ++m_ResendCount; //2292
                m_DriveResponseCheckTimer.Enabled = false;
                m_CommandResendCallback(this);
            }
            catch (Exception a_Exception)
            {
            }
        }


        #endregion

    }
}
