namespace Project1
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
            this.InputBox = new System.Windows.Forms.TextBox();
            this.OutputBox = new System.Windows.Forms.TextBox();
            this.kValueBox = new System.Windows.Forms.TextBox();
            this.SolveButton = new System.Windows.Forms.Button();
            this.InputLabel = new System.Windows.Forms.Label();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.kValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(85, 0);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(150, 20);
            this.InputBox.TabIndex = 0;
            // 
            // OutputBox
            // 
            this.OutputBox.Location = new System.Drawing.Point(85, 139);
            this.OutputBox.Multiline = true;
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.Size = new System.Drawing.Size(150, 88);
            this.OutputBox.TabIndex = 1;
            // 
            // kValueBox
            // 
            this.kValueBox.Location = new System.Drawing.Point(85, 45);
            this.kValueBox.Name = "kValueBox";
            this.kValueBox.Size = new System.Drawing.Size(150, 20);
            this.kValueBox.TabIndex = 2;
            // 
            // SolveButton
            // 
            this.SolveButton.Location = new System.Drawing.Point(85, 87);
            this.SolveButton.Name = "SolveButton";
            this.SolveButton.Size = new System.Drawing.Size(75, 23);
            this.SolveButton.TabIndex = 3;
            this.SolveButton.Text = "Solve";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(this.SolveButton_Click);
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Location = new System.Drawing.Point(23, 3);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(31, 13);
            this.InputLabel.TabIndex = 4;
            this.InputLabel.Text = "Input";
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Location = new System.Drawing.Point(23, 142);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(39, 13);
            this.OutputLabel.TabIndex = 5;
            this.OutputLabel.Text = "Output";
            // 
            // kValueLabel
            // 
            this.kValueLabel.AutoSize = true;
            this.kValueLabel.Location = new System.Drawing.Point(23, 52);
            this.kValueLabel.Name = "kValueLabel";
            this.kValueLabel.Size = new System.Drawing.Size(42, 13);
            this.kValueLabel.TabIndex = 6;
            this.kValueLabel.Text = "k value";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.kValueLabel);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.SolveButton);
            this.Controls.Add(this.kValueBox);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.InputBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.TextBox OutputBox;
        private System.Windows.Forms.TextBox kValueBox;
        private System.Windows.Forms.Button SolveButton;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.Label kValueLabel;
    }
}

