﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simplex
{
    public partial class MainForm : Form
    {
        private readonly Point startPoint = new Point(80, 200);

        private readonly Size labelSize = new Size(35, 20);
        private readonly Size upDownSize = new Size(50, 30);

        private const int baseLabelWidth = 70;

        private const int xPadding = 50;
        private const int yPadding = 40;

        private const string DefaultTag = "default";

        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string logPath = Path.Combine(AppDataPath, "simplex_log.txt");

        private readonly Font baseFont = new Font("Microsoft Sans Serif", 12);

        private string[] pivotRules = new string[] { "Klasszikus", "Bland" };

        private List<List<NumericUpDown>> currentDict = new List<List<NumericUpDown>>();

        public MainForm()
        {
            InitializeComponent();

            InstantiateControls();

            PivotBox.DataSource = pivotRules;

            PivotBox.SelectedIndex = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void InstantiateControls()
        {
            int currY = startPoint.Y;
            int x = startPoint.X;

            // Add base variables, non-base variables
            for (int i = 0; i < BaseVariableBox.Value; i++)
            {
                int baseIndex = (i + 1) + (int)NonBaseVariableBox.Value;
                Label baseVariableLabel = CreateDefaultLabel("x" + baseIndex + " = ", new Point(x - baseLabelWidth, currY));

                baseVariableLabel.Width = baseLabelWidth;
                baseVariableLabel.ForeColor = Color.DarkRed;

                this.Controls.Add(baseVariableLabel);

                AddRow(new Point(x, currY));

                currY += yPadding;
            }

            currY += yPadding;

            // Add function 
            Label functionLabel = CreateDefaultLabel("max: z = ", new Point(x - baseLabelWidth, currY));

            functionLabel.ForeColor = Color.DarkViolet;
            functionLabel.Width = baseLabelWidth;

            this.Controls.Add(functionLabel);

            AddRow(new Point(x, currY));

        }

        private void AddRow(Point location)
        {
            int currX = location.X;
            int y = location.Y;

            List<NumericUpDown> row = new List<NumericUpDown>();

            NumericUpDown constantBox = CreateDefaultUpDown(new Point(currX, y));

            this.Controls.Add(constantBox);
            row.Add(constantBox);

            currX += 75;

            for (int i = 0; i < NonBaseVariableBox.Value;  i++)
            {
                Label plusLabel = CreateDefaultLabel("+", new Point(currX, y));

                this.Controls.Add(plusLabel);

                currX += xPadding;

                NumericUpDown upDown = CreateDefaultUpDown(new Point(currX, y));
                
                currX += xPadding;

                Label label = CreateDefaultLabel("x" + (i + 1), new Point(currX, y));

                this.Controls.Add(label);
                this.Controls.Add(upDown);

                row.Add(upDown);

                currX += xPadding;
            }

            currentDict.Add(row);
        }

        private void ClearAddedControls()
        {
            List<Control> toRemove = GetNotTaggedControls(DefaultTag);

            foreach (Control control in toRemove)
            {
                this.Controls.Remove(control);
                control.Dispose();
            }

            currentDict.Clear();
        }

        private List<Control> GetNotTaggedControls(string tag)
        {
            List<Control> retval = new List<Control>();

            foreach(Control control in this.Controls)
            {
                string tagStr = control.Tag == null ? "" : control.Tag.ToString();
                if (!tagStr.Equals(tag)) retval.Add(control);
            }

            return retval;
        }

        private void NonBaseVariableBox_ValueChanged(object sender, EventArgs e)
        {
            ClearAddedControls();

            InstantiateControls();
        }

        private void BaseVariableBox_ValueChanged(object sender, EventArgs e)
        {
            ClearAddedControls();

            InstantiateControls();
        }

        private void BeginButton_Click(object sender, EventArgs e)
        {
            int nonBaseVariableCount = (int)NonBaseVariableBox.Value;

            Simplex.dict.Clear();

            for(int i = 0; i < currentDict.Count - 1; i++)
            {
                string baseVarName = "x" + (i + 1 + nonBaseVariableCount);

                BaseVariable baseVar = new BaseVariable(baseVarName);

                var current = currentDict[i];

                baseVar.Constant = (double)current.First().Value;

                for (int k = 1; k < current.Count; k++)
                {
                    string nonBaseVarName = "x" + k;

                    baseVar.AddVariable(new NonBaseVariable(nonBaseVarName, (double)current[k].Value));
                }

                Simplex.dict.Add(baseVar);
            }

            var functionRow = currentDict.Last();

            Simplex.function.Constant = (double)functionRow.First().Value;

            for (int i = 1; i < functionRow.Count; i++)
            {
                string nonBaseVarName = "x" + i;

                var current = functionRow[i];

                Simplex.function.AddVariable(new NonBaseVariable(nonBaseVarName, (double)current.Value));
            }

            Simplex.Optimize((PivotRule)PivotBox.SelectedIndex);

            MessageBox.Show(Simplex.GetEventLog());

            if(SaveEventLogBox.Checked)
            {
                if(!Simplex.SaveLog(logPath)) MessageBox.Show("Hiba tortent a mentes soran!");
            }
        }

        private void PivotBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private Label CreateDefaultLabel(string text, Point location)
        {
            Label label = new Label();

            label.Text = text;
            label.Font = baseFont;
            label.Location = location;
            label.Size = labelSize;

            return label;
        }

        private NumericUpDown CreateDefaultUpDown(Point location)
        {
            NumericUpDown upDown = new NumericUpDown();

            upDown.Location = location;
            upDown.Value = 0;
            upDown.Minimum = -999;
            upDown.Maximum = 999;
            upDown.Font = baseFont;
            upDown.Enabled = true;
            upDown.Size = upDownSize;
            upDown.TextAlign = HorizontalAlignment.Center;
            upDown.DecimalPlaces = 1;

            return upDown;
        }

        private void SaveEventLogBox_CheckedChanged(object sender, EventArgs e)
        {
            OpenLogFolderButton.Enabled = SaveEventLogBox.Checked;
        }

        private void OpenLogFolderButton_Click(object sender, EventArgs e)
        {
            try { Process.Start(AppDataPath); }
            catch (Exception) { MessageBox.Show("Nem sikerult megnyitni a mappat!"); }
        }
    }
}
