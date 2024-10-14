using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private readonly Font baseFont = new Font("Microsoft Sans Serif", 12);

        private List<Control> currentDictionary = new List<Control>();

        private string[] pivotRules = new string[] { "Klasszikus", "Bland" };
        public MainForm()
        {
            InitializeComponent();

            InstantiateControls();

            PivotBox.DataSource = pivotRules;

            PivotBox.SelectedIndex = 0;
        }

        // Enable double buffering to combat flickering
        // Adds to smoothness
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void InstantiateControls()
        {
            int currY = startPoint.Y;

            // Add base variables, non-base variables
            for (int i = 0; i < BaseVariableBox.Value; i++)
            {
                int baseIndex = (i + 1) + (int)NonBaseVariableBox.Value;
                Label baseVariableLabel = CreateDefaultLabel("x" + baseIndex + " = ", new Point(startPoint.X - baseLabelWidth, currY));

                baseVariableLabel.Width = baseLabelWidth;
                baseVariableLabel.ForeColor = Color.DarkRed;

                this.Controls.Add(baseVariableLabel);
                this.currentDictionary.Add(baseVariableLabel);

                AddRow(new Point(startPoint.X, currY));

                currY += yPadding;
            }

            // Add function 
        }

        private void AddRow(Point location)
        {
            int currX = location.X;
            int y = location.Y;

            NumericUpDown constantBox = CreateDefaultUpDown(new Point(currX, y));

            currentDictionary.Add(constantBox);
            this.Controls.Add(constantBox);

            currX += 75;

            for (int i = 0; i < NonBaseVariableBox.Value;  i++)
            {
                Label plusLabel = CreateDefaultLabel("+", new Point(currX, y));

                this.Controls.Add(plusLabel);
                currentDictionary.Add(plusLabel);

                currX += xPadding;

                NumericUpDown upDown = CreateDefaultUpDown(new Point(currX, y));
                
                currX += xPadding;

                Label label = CreateDefaultLabel("x" + (i + 1), new Point(currX, y));

                this.Controls.Add(label);
                this.Controls.Add(upDown);

                currentDictionary.Add(label);
                currentDictionary.Add(upDown);

                currX += xPadding;
            }
        }

        private void ClearAddedControls()
        {
            // TODO optimize
            foreach(Control control in currentDictionary)
            {
                control.Dispose();
                this.Controls.Remove(control);
            }

            currentDictionary.Clear();
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
            upDown.Minimum = -9999;
            upDown.Font = baseFont;
            upDown.Enabled = true;
            upDown.Size = upDownSize;
            upDown.TextAlign = HorizontalAlignment.Center;

            return upDown;
        }
    }
}
