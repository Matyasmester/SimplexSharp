﻿namespace Simplex
{
    partial class MainForm
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
            this.NonBaseVariableBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BaseVariableBox = new System.Windows.Forms.NumericUpDown();
            this.BeginButton = new System.Windows.Forms.Button();
            this.PivotBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SaveEventLogBox = new System.Windows.Forms.CheckBox();
            this.OpenLogFolderButton = new System.Windows.Forms.Button();
            this.SaveDictButton = new System.Windows.Forms.Button();
            this.LoadDictButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NonBaseVariableBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseVariableBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NonBaseVariableBox
            // 
            this.NonBaseVariableBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.NonBaseVariableBox.Location = new System.Drawing.Point(1347, 18);
            this.NonBaseVariableBox.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.NonBaseVariableBox.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NonBaseVariableBox.Name = "NonBaseVariableBox";
            this.NonBaseVariableBox.Size = new System.Drawing.Size(68, 30);
            this.NonBaseVariableBox.TabIndex = 0;
            this.NonBaseVariableBox.Tag = "default";
            this.NonBaseVariableBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NonBaseVariableBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NonBaseVariableBox.ValueChanged += new System.EventHandler(this.NonBaseVariableBox_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(1147, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 25);
            this.label1.TabIndex = 1;
            this.label1.Tag = "default";
            this.label1.Text = "Nembázis változók:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(1147, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 25);
            this.label2.TabIndex = 3;
            this.label2.Tag = "default";
            this.label2.Text = "Bázisváltozók:";
            // 
            // BaseVariableBox
            // 
            this.BaseVariableBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BaseVariableBox.Location = new System.Drawing.Point(1347, 65);
            this.BaseVariableBox.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.BaseVariableBox.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.BaseVariableBox.Name = "BaseVariableBox";
            this.BaseVariableBox.Size = new System.Drawing.Size(68, 30);
            this.BaseVariableBox.TabIndex = 2;
            this.BaseVariableBox.Tag = "default";
            this.BaseVariableBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BaseVariableBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.BaseVariableBox.ValueChanged += new System.EventHandler(this.BaseVariableBox_ValueChanged);
            // 
            // BeginButton
            // 
            this.BeginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BeginButton.Location = new System.Drawing.Point(771, 12);
            this.BeginButton.Name = "BeginButton";
            this.BeginButton.Size = new System.Drawing.Size(262, 107);
            this.BeginButton.TabIndex = 4;
            this.BeginButton.Tag = "default";
            this.BeginButton.Text = "Számolj!";
            this.BeginButton.UseVisualStyleBackColor = true;
            this.BeginButton.Click += new System.EventHandler(this.BeginButton_Click);
            // 
            // PivotBox
            // 
            this.PivotBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PivotBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PivotBox.FormattingEnabled = true;
            this.PivotBox.Location = new System.Drawing.Point(1294, 128);
            this.PivotBox.Name = "PivotBox";
            this.PivotBox.Size = new System.Drawing.Size(150, 33);
            this.PivotBox.TabIndex = 5;
            this.PivotBox.Tag = "default";
            this.PivotBox.SelectedIndexChanged += new System.EventHandler(this.PivotBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(1147, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 25);
            this.label3.TabIndex = 6;
            this.label3.Tag = "default";
            this.label3.Text = "Pivot szabály:";
            // 
            // SaveEventLogBox
            // 
            this.SaveEventLogBox.AutoSize = true;
            this.SaveEventLogBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SaveEventLogBox.Location = new System.Drawing.Point(25, 25);
            this.SaveEventLogBox.Name = "SaveEventLogBox";
            this.SaveEventLogBox.Size = new System.Drawing.Size(277, 36);
            this.SaveEventLogBox.TabIndex = 7;
            this.SaveEventLogBox.Tag = "default";
            this.SaveEventLogBox.Text = "Log fájlba mentése";
            this.SaveEventLogBox.UseVisualStyleBackColor = true;
            this.SaveEventLogBox.CheckedChanged += new System.EventHandler(this.SaveEventLogBox_CheckedChanged);
            // 
            // OpenLogFolderButton
            // 
            this.OpenLogFolderButton.Enabled = false;
            this.OpenLogFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OpenLogFolderButton.Location = new System.Drawing.Point(25, 67);
            this.OpenLogFolderButton.Name = "OpenLogFolderButton";
            this.OpenLogFolderButton.Size = new System.Drawing.Size(262, 107);
            this.OpenLogFolderButton.TabIndex = 8;
            this.OpenLogFolderButton.Tag = "default";
            this.OpenLogFolderButton.Text = "Mappa megnyitása";
            this.OpenLogFolderButton.UseVisualStyleBackColor = true;
            this.OpenLogFolderButton.Click += new System.EventHandler(this.OpenLogFolderButton_Click);
            // 
            // SaveDictButton
            // 
            this.SaveDictButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SaveDictButton.Location = new System.Drawing.Point(436, 12);
            this.SaveDictButton.Name = "SaveDictButton";
            this.SaveDictButton.Size = new System.Drawing.Size(204, 93);
            this.SaveDictButton.TabIndex = 9;
            this.SaveDictButton.Tag = "default";
            this.SaveDictButton.Text = "Szótár mentése";
            this.SaveDictButton.UseVisualStyleBackColor = true;
            this.SaveDictButton.Click += new System.EventHandler(this.SaveDictButton_Click);
            // 
            // LoadDictButton
            // 
            this.LoadDictButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LoadDictButton.Location = new System.Drawing.Point(436, 111);
            this.LoadDictButton.Name = "LoadDictButton";
            this.LoadDictButton.Size = new System.Drawing.Size(204, 93);
            this.LoadDictButton.TabIndex = 10;
            this.LoadDictButton.Tag = "default";
            this.LoadDictButton.Text = "Szótár betöltése";
            this.LoadDictButton.UseVisualStyleBackColor = true;
            this.LoadDictButton.Click += new System.EventHandler(this.LoadDictButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.BeginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1472, 653);
            this.Controls.Add(this.LoadDictButton);
            this.Controls.Add(this.SaveDictButton);
            this.Controls.Add(this.OpenLogFolderButton);
            this.Controls.Add(this.SaveEventLogBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PivotBox);
            this.Controls.Add(this.BeginButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BaseVariableBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NonBaseVariableBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Simplex Algoritmus";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NonBaseVariableBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseVariableBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown NonBaseVariableBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown BaseVariableBox;
        private System.Windows.Forms.Button BeginButton;
        private System.Windows.Forms.ComboBox PivotBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox SaveEventLogBox;
        private System.Windows.Forms.Button OpenLogFolderButton;
        private System.Windows.Forms.Button SaveDictButton;
        private System.Windows.Forms.Button LoadDictButton;
    }
}

