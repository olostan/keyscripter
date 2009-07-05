using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Scripter
{
    public partial class ScriptRunnerControl : UserControl
    {
        private DateTime loadedDateTime;
        private Script script;
        private int selectedWindow;
        private Thread thread;

        public ScriptRunnerControl()
        {
            InitializeComponent();
        }

        private void BtnScriptClick(object sender, EventArgs e)
        {
            if (chooseFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadScript();
            }
            SyncEnabledControls();
        }

        private void LoadScript()
        {
            StreamReader reader = File.OpenText(chooseFileDialog.FileName);
            string text = reader.ReadToEnd();
            reader.Close();
            try
            {
                script = new Script(text);
                btnScript.Text = chooseFileDialog.SafeFileName;
                var info = new FileInfo(chooseFileDialog.FileName);
                loadedDateTime = info.LastWriteTime;
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show("Error: " + exception.Message, "Invalid script", MessageBoxButtons.OK,
                                MessageBoxIcon.Hand);
            }
        }

        private void SyncEnabledControls()
        {
            bool flag = thread != null;
            btnScript.Enabled = !flag;
            btnSelectWindow.Enabled = !flag;
            btnStart.Enabled = (!flag && (script != null)) && (selectedWindow != 0);
            btnStop.Enabled = flag;
            btnRemove.Enabled = !flag;
            if (!((selectedWindow <= 0) || Funcs.CheckValidWindow((uint) selectedWindow)))
            {
                btnSelectWindow.Text = "[Окно отсутвует]";
                selectedWindow = 0;
                SyncEnabledControls();
            }
        }

        private void BtnSelectWindowClick(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                int foregroundWindow;
                btnSelectWindow.Text = "Кликните на окне";
                btnSelectWindow.Enabled = false;
                ParentForm.Opacity = 0.0;
                Application.DoEvents();
                var handle = ParentForm.Handle;
                do
                {
                    Thread.Sleep(200);
                    foregroundWindow = Funcs.GetForegroundWindow();
                } while ((handle.ToInt32() == foregroundWindow) || (foregroundWindow == 0));
                selectedWindow = foregroundWindow;
                var text = new StringBuilder(0x200);
                var str = "[" + selectedWindow + "] ";
                if (Funcs.GetWindowText(selectedWindow, text, 0x200) > 0)
                {
                    str = str + text;
                }
                ParentForm.Opacity = 1.0;
                ParentForm.BringToFront();
                ParentForm.Activate();
                btnSelectWindow.Text = str;
                btnSelectWindow.Enabled = true;
            }
            SyncEnabledControls();
        }

        private void BtnStartClick(object sender, EventArgs e)
        {
            var info = new FileInfo(chooseFileDialog.FileName);
            if (info.LastWriteTime > loadedDateTime)
            {
                LoadScript();
            }
            thread = new Thread(RunScript);
            thread.Start();
            SyncEnabledControls();
        }


        private void RunScript()
        {
            var num = 0;
            
            try
            {
                var now = DateTime.Now;
                loop: 
                foreach (var action in script.Actions)
                {
                    if (!Funcs.CheckValidWindow((uint) selectedWindow))
                    {
                        thread.Abort();
                    }
                    else
                    {
                        action.Do((uint) selectedWindow);
                        if (num++ > 100)
                        {
                            var span = DateTime.Now - now;
                            if (span.Milliseconds < 200)
                            {
                                Thread.Sleep(100);
                            }
                            num = 0;
                            now = DateTime.Now;
                        }
                    }
                }
                goto loop;
                
            }
            catch (ThreadAbortException)
            {
                Invoke(new StopScriptDelegate(delegate
                                                       {
                                                           thread = null;
                                                           SyncEnabledControls();
                                                       }));
            }
        }

        private void BtnStopClick(object sender, EventArgs e)
        {
            thread.Abort();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                ((ScriptRunnerForm) ParentForm).RemoveRunner(this);
            }
        }

        private void ScriptRunnerControlLoad(object sender, EventArgs e)
        {
            SyncEnabledControls();
        }

        #region Nested type: StopScriptDelegate

        private delegate void StopScriptDelegate();

        #endregion
    }
}