using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peak.Can.Basic;

namespace HCAN
{
    class CANController
    {

        private TPCANBaudrate PCANBaudrate = TPCANBaudrate.PCAN_BAUD_250K;
        private readonly ushort PCANHandle = PCANBasic.PCAN_USBBUS1;
        private StringBuilder errString = new StringBuilder();
        private bool netInitialized = false;
        private CANToolForm Main;

        private System.Threading.AutoResetEvent ReceiveEvent;
        private System.Threading.Thread RxThread;

        public EventHandler NetStatusChange;


        public CANController(CANToolForm mainForm)
        {
            Main = mainForm;
            ReceiveEvent = new System.Threading.AutoResetEvent(false);
            StartReadThread();
        }



        protected virtual void OnNetStatusChange(EventArgs e)
        {
            EventHandler handler = NetStatusChange;
            handler?.Invoke(this, e);
        }

        public bool GetNetStatus()
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
                    ErrorHandle(result);
                    return;
                }

            }
            result = PCANBasic.Initialize(PCANHandle, PCANBaudrate);
            if (result != TPCANStatus.PCAN_ERROR_OK)
            {
                ErrorHandle(result);
                return;
            }
            netInitialized = true;
            
            OnNetStatusChange(EventArgs.Empty);
        }

        private void StartReadThread()
        {
            System.Threading.ThreadStart threadDelegate = new System.Threading.ThreadStart(RxThreadFunction);
            RxThread = new System.Threading.Thread(threadDelegate);
            RxThread.IsBackground = true;
            RxThread.Start();
        }

        private void ErrorHandle(TPCANStatus result)
        {
            string errMsg;
            StringBuilder errString = new StringBuilder();
            if (PCANBasic.GetErrorText(result, 0, errString) != TPCANStatus.PCAN_ERROR_OK)
            {

                errMsg = "Error checking errors. Probably not good.";
            }
            else
            {
                errMsg = errString.ToString();
            }
            Main.Error(errMsg);
        }

        private void RxThreadFunction()
        {
            UInt32 buffer;
            TPCANStatus result;

            buffer = Convert.ToUInt32(ReceiveEvent.SafeWaitHandle.DangerousGetHandle().ToInt32());
            result = PCANBasic.SetValue(PCANHandle, TPCANParameter.PCAN_RECEIVE_EVENT, ref buffer, sizeof(UInt32));

            if (result != TPCANStatus.PCAN_ERROR_OK)
            {
                ErrorHandle(result);
            }

            while (true)
            {
                if (ReceiveEvent.WaitOne(50));
            }
        }

        static private void RxEventRead()
        {


        }

    }
}
