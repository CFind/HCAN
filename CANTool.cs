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
        TPCANBaudrate PCANBaudrate = TPCANBaudrate.PCAN_BAUD_250K;

        public CANTool()
        {
            InitializeComponent();
            baudrateComboBox.Items.AddRange(new object[] { new { str = "250Kb/s", TPCANBaudrate.PCAN_BAUD_250K }, new { str = "500Kb/s", TPCANBaudrate.PCAN_BAUD_500K } });
            baudrateComboBox.DisplayMember = "str";
            baudrateComboBox.SelectedIndex = 0;
        }

        private void baudrateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
            
}



    

