using System;
using System.Windows.Forms;

namespace Scripter
{
    public partial class ScriptRunnerForm : Form
    {
        public ScriptRunnerForm()
        {
            InitializeComponent();
        }
        private void AddRunner()
        {
            var runner = new ScriptRunnerControl {Dock = DockStyle.Top};
            panel1.Controls.Add(runner);
            ResizeWindow();
        }

        private void ResizeWindow()
        {
            var num = 0;
            foreach (Control control in panel1.Controls)
            {
                num += control.Height;
            }
            Height = num + 0x37;
        }

        private void miAddScript_Click(object sender, EventArgs e)
        {
            AddRunner();
        }
        public void RemoveRunner(ScriptRunnerControl runner)
        {
            panel1.Controls.Remove(runner);
            ResizeWindow();
        }

        private void ScriptRunnerForm_Load(object sender, EventArgs e)
        {
            AddRunner();
        }


    }
}
