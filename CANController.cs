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
        private TPCANBaudrate Baudrate;
        private UInt16 PCANHandle = PCANBasic.PCAN_USBBUS1;

        CANController()
        {

        }

        public bool SetBaudrate(uint value)
        {
            switch (value)
            {
                case 250:
                    Baudrate = TPCANBaudrate.PCAN_BAUD_250K;
                    break;
                case 500:
                    Baudrate = TPCANBaudrate.PCAN_BAUD_500K;
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
