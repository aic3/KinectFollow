namespace KinectMonitor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._run = new System.Windows.Forms.Button();
            this._debug = new System.Windows.Forms.TextBox();
            this._start = new System.Windows.Forms.Button();
            this._stop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _run
            // 
            this._run.Location = new System.Drawing.Point(859, 603);
            this._run.Name = "_run";
            this._run.Size = new System.Drawing.Size(75, 23);
            this._run.TabIndex = 0;
            this._run.Text = "Run";
            this._run.UseVisualStyleBackColor = true;
            this._run.Click += new System.EventHandler(this._run_Click);
            // 
            // _debug
            // 
            this._debug.Location = new System.Drawing.Point(13, 13);
            this._debug.Multiline = true;
            this._debug.Name = "_debug";
            this._debug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._debug.Size = new System.Drawing.Size(921, 584);
            this._debug.TabIndex = 1;
            // 
            // _start
            // 
            this._start.Location = new System.Drawing.Point(13, 604);
            this._start.Name = "_start";
            this._start.Size = new System.Drawing.Size(75, 23);
            this._start.TabIndex = 2;
            this._start.Text = "Start";
            this._start.UseVisualStyleBackColor = true;
            this._start.Click += new System.EventHandler(this._start_Click);
            // 
            // _stop
            // 
            this._stop.Location = new System.Drawing.Point(94, 603);
            this._stop.Name = "_stop";
            this._stop.Size = new System.Drawing.Size(75, 23);
            this._stop.TabIndex = 3;
            this._stop.Text = "Stop";
            this._stop.UseVisualStyleBackColor = true;
            this._stop.Click += new System.EventHandler(this._stop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 638);
            this.Controls.Add(this._stop);
            this.Controls.Add(this._start);
            this.Controls.Add(this._debug);
            this.Controls.Add(this._run);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _run;
        private System.Windows.Forms.TextBox _debug;
        private System.Windows.Forms.Button _start;
        private System.Windows.Forms.Button _stop;
    }
}

