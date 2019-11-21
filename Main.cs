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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            int[] baudrates = { 100, 125, 250, 500, 800, 1000 };
            foreach (int baudrate in baudrates)
            {
                 
            }
        }


    }
}
