using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simplex
{
    public partial class PopupForm : Form
    {
        public PopupForm()
        {
            InitializeComponent();
        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(MainBox.Text);
        }

        public void Show(string text)
        {
            MainBox.Text = text;

            base.ShowDialog();
        }
    }
}
