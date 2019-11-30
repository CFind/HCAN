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
    public partial class CANToolForm : Form
    {
        private CANController Controller;
        

        public CANToolForm()
        {
            InitializeComponent();
            Controller = new CANController(this);
            baudrateComboBox.Items.AddRange(new object[]  {"250 Kb/s","500 Kb/s"});
            baudrateComboBox.SelectedIndex = 0;
            Controller.NetStatusChange += netStatusChanged;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void netStatusChanged(object sender, EventArgs e)
        {
            netInitButton.Text = Controller.GetNetStatus() ? "Re-Initialize" : "Initialize";
        }

        private void baudrateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bStr = baudrateComboBox.SelectedItem.ToString();
            string baudrate = "250";
            int digitCount = 0;
            foreach (char c in bStr)
            {
                if (Char.IsDigit(c))
                    digitCount++;
                else
                    break;
            }
            baudrate = bStr.Substring(0, digitCount);
            Controller.ChangeBaudrate(bStr);
        }

        private void netInitButton_Click(object sender, EventArgs e)
        {
            Controller.InitializeNet();
        }

        public void Error(string message)
        {
            System.Media.SystemSounds.Exclamation.Play();
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
            
}



    

