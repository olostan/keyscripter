namespace Scripter
{
    partial class ScriptRunnerControl
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnScript = new System.Windows.Forms.Button();
            this.btnSelectWindow = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.chooseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnScript
            // 
            this.btnScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScript.Location = new System.Drawing.Point(3, 3);
            this.btnScript.Name = "btnScript";
            this.btnScript.Size = new System.Drawing.Size(261, 23);
            this.btnScript.TabIndex = 0;
            this.btnScript.Text = "Выбор скрипта";
            this.btnScript.UseVisualStyleBackColor = true;
            this.btnScript.Click += new System.EventHandler(this.BtnScriptClick);
            // 
            // btnSelectWindow
            // 
            this.btnSelectWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectWindow.Location = new System.Drawing.Point(270, 3);
            this.btnSelectWindow.Name = "btnSelectWindow";
            this.btnSelectWindow.Size = new System.Drawing.Size(250, 23);
            this.btnSelectWindow.TabIndex = 1;
            this.btnSelectWindow.Text = "Выбор окна";
            this.btnSelectWindow.UseVisualStyleBackColor = true;
            this.btnSelectWindow.Click += new System.EventHandler(this.BtnSelectWindowClick);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(532, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(71, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStartClick);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(603, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(68, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(677, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(25, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "X";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // chooseFileDialog
            // 
            this.chooseFileDialog.FileName = "openFileDialog1";
            // 
            // ScriptRunnerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSelectWindow);
            this.Controls.Add(this.btnScript);
            this.Name = "ScriptRunnerControl";
            this.Size = new System.Drawing.Size(705, 30);
            this.Load += new System.EventHandler(this.ScriptRunnerControlLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnScript;
        private System.Windows.Forms.Button btnSelectWindow;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.OpenFileDialog chooseFileDialog;
    }
}
