using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Peak.Can.Basic;

namespace HCAN
{
    class CANController
    {


        #region Members
        private TPCANBaudrate PCANBaudrate = TPCANBaudrate.PCAN_BAUD_250K;
        private readonly ushort PCANHandle = PCANBasic.PCAN_USBBUS1;
        private bool netInitialized = false;
        private CANToolForm Main;
        private System.Threading.AutoResetEvent ReceiveEvent;
        private System.Threading.Thread RxThread;
        private ArrayList MessagesList;
        #endregion

        #region Public members
        public class OnPCANErrorData : EventArgs { public string ErrorString { get; set; } }
        public class OnNetStatusChangeData : EventArgs 
        { 
            public bool Initialized { get; set; } 
            public enum Statuses : byte
            {
                OK,
                OFF,
                BUSHEAVY,
                BUSLIGHT
            }
            public Statuses status { get; set; }
        }
        public EventHandler<OnNetStatusChangeData> NetStatusChange;
        public EventHandler<OnPCANErrorData> PCANError;
        #endregion



        public CANController(CANToolForm mainForm)
        {
            Main = mainForm;
            ReceiveEvent = new System.Threading.AutoResetEvent(false);
        }



        #region Events
        protected virtual void OnPCANError(OnPCANErrorData e)
        {
            EventHandler<OnPCANErrorData> handler = PCANError;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnNetStatusChange(OnNetStatusChangeData e)
        {
            EventHandler<OnNetStatusChangeData> handler = NetStatusChange;
            handler?.Invoke(this, e);
        }
        #endregion


        public bool GetNetInitialized()
        {
            return netInitialized;
        }

        public void ChangeBaudrate(string baudrate)
        {
            switch(baudrate)
            {
                case "500":
                    PCANBaudrate = TPCANBaudrate.PCAN_BAUD_500K;
                    break;
                default:
                    PCANBaudrate = TPCANBaudrate.PCAN_BAUD_250K;
                    break;
            }
        }
        public void InitializeNet()
        {
            TPCANStatus result;
            if ((PCANBasic.GetStatus(PCANHandle) == TPCANStatus.PCAN_ERROR_OK))
            {
                result = PCANBasic.Uninitialize(PCANHandle);
                if (result != TPCANStatus.PCAN_ERROR_OK)
                {
                    ThrowInitError(result);
                    return;
                }

            }
            result = PCANBasic.Initialize(PCANHandle, PCANBaudrate);
            if (result != TPCANStatus.PCAN_ERROR_OK)
            {
                ThrowInitError(result);
                return;
            }
            netInitialized = true;
            StartReadThread();
            OnNetStatusChangeData data = new OnNetStatusChangeData();
            data.Initialized = true;
            data.status = OnNetStatusChangeData.Statuses.OK;
            OnNetStatusChange(data);
        }

        public void UninitializeNet()
        {
            TPCANStatus result;
            result = PCANBasic.Uninitialize(PCANHandle);
            if (result != TPCANStatus.PCAN_ERROR_OK && result != TPCANStatus.PCAN_ERROR_INITIALIZE)
            {
                ThrowInitError(result);
                return;
            }
            netInitialized = false;
        }

        private void StartReadThread()
        {
            System.Threading.ThreadStart threadDelegate = new System.Threading.ThreadStart(RxThreadFunction);
            RxThread = new System.Threading.Thread(threadDelegate);
            RxThread.IsBackground = true;
            RxThread.Start();
        }

        private void ThrowInitError(TPCANStatus result)
        {
            string errMsg = BuildErrorString(result);
            ThrowErrorMessage(errMsg);
        }

        private string BuildErrorString(TPCANStatus result)
        {
            string errMsg;
            StringBuilder errString = new StringBuilder(256);
            TPCANStatus errCheckResult = PCANBasic.GetErrorText(result, 0, errString);
            if (errCheckResult != TPCANStatus.PCAN_ERROR_OK)
            {
                errMsg = "Error checking errors. Probably not good.";
            }
            else
            {
                errMsg = errString.ToString();
            }
            return errMsg;
        }

        private void ThrowErrorMessage(string errorMsg)
        {
            OnPCANErrorData errorData = new OnPCANErrorData();
            errorData.ErrorString = errorMsg;
            OnPCANError(errorData);
        }

        private void ErrorReadHandle(TPCANStatus result)
        {

        }

        private void RxThreadFunction()
        {
            UInt32 buffer;
            TPCANStatus result;

            buffer = Convert.ToUInt32(ReceiveEvent.SafeWaitHandle.DangerousGetHandle().ToInt32());
            result = PCANBasic.SetValue(PCANHandle, TPCANParameter.PCAN_RECEIVE_EVENT, ref buffer, sizeof(UInt32));
            if (result != TPCANStatus.PCAN_ERROR_OK)
            {
                lock (this)
                {
                    ThrowInitError(result);
                    return;
                }
            }

            while (true)
            {
                if (ReceiveEvent.WaitOne(50))
                    RxEventRead();
            }
        }

        private void RxEventRead()
        {
            TPCANMsg msg;
            TPCANTimestamp timestamp;
            TPCANStatus result;

            result = PCANBasic.Read(PCANHandle, out msg, out timestamp);

            if (result != TPCANStatus.PCAN_ERROR_OK)
                ErrorReadHandle(result);


            

        }

        private void ProcessMessage(TPCANMsg msg, TPCANTimestamp timestamp)
        {

        }

    }
}
