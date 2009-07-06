using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scripter
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            cbSendMode.Checked = Config.Instance.SendMode;
            cbKeyUp.Checked = Config.Instance.SendKeyUp;
            tbPause.Value = Config.Instance.PauseAfterSend;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Config.Instance.SendMode = cbSendMode.Checked;
            Config.Instance.SendKeyUp = cbKeyUp.Checked;
            Config.Instance.PauseAfterSend = (int) tbPause.Value;

        }
    }
}
