using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Peak.Can.Basic;


namespace HCAN
{
    public partial class CANTool : Form
    {
        private TPCANBaudrate PCANBaudrate = TPCANBaudrate.PCAN_BAUD_250K;
        readonly ushort PCANHandle = PCANBasic.PCAN_USBBUS1;
        private StringBuilder errString = new StringBuilder();

        class BaudrateElement
        {
            public string DisplayString { get; set; }
            public TPCANBaudrate Baudrate { get; set; }

            public BaudrateElement(string s, TPCANBaudrate baud)
            {
                DisplayString = s;
                Baudrate = baud;
            }
        }

        public CANTool()
        {
            InitializeComponent();
            baudrateComboBox.Items.AddRange(new object[] { new BaudrateElement("250Kb/s", TPCANBaudrate.PCAN_BAUD_250K), new BaudrateElement("500Kb/s", TPCANBaudrate.PCAN_BAUD_500K) });
            baudrateComboBox.DisplayMember = "DisplayString";
            baudrateComboBox.SelectedIndex = 0;
            
        }

        private void baudrateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PCANBaudrate = ((BaudrateElement)baudrateComboBox.SelectedItem).Baudrate;
            baudrateLabel.Text = PCANBaudrate.ToString();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void netInitButton_Click(object sender, EventArgs e)
        {
            InitializeNet();
        }

        private void InitializeNet()
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
            }
        }

        private void ErrorHandle(TPCANStatus result)
        {
            string errMsg;
            if (PCANBasic.GetErrorText(result, 0, errString) != TPCANStatus.PCAN_ERROR_OK)
            {
                
                errMsg = "Error checking errors. Probably not good.";
            }
            else
            {
                errMsg = errString.ToString();
            }
            Error(errMsg);
        }

        private void Error(string message)
        {
            System.Media.SystemSounds.Exclamation.Play();
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
            
}



    

