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
        class ComboElement
        {
            string m_dispString; uint m_dispValue; 
            public ComboElement(string dispString, uint value) { DispString = dispString; DispValue = value; }
            public string DispString { get => m_dispString; set => m_dispString = value; }
            public uint DispValue { get => m_dispValue; set => m_dispValue = value; }
        };
        public Main()
        {
            InitializeComponent();
            baudrateComboBox.Items.AddRange(new object[] { new ComboElement("250 Kb/s", 250), new ComboElement("500 Kb/s", 500) });
            baudrateComboBox.DisplayMember = "DispString";
        }

        private void baudrateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
            
}



    

