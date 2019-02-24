using AudioSwitcher.AudioApi;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FortyOne.AudioSwitcher.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    public class VolumeControlMenuItem : ToolStripControlHost
    {
        public GroupBox GroupBox { get => Control as GroupBox; }
        private NumericUpDown TextBox { get; }
        private TrackBar TrackBar { get; }

        public int Value {
            get => TrackBar.Value;
            set
            {
                OnValueChanged(value);
            }
        }

        public delegate void ValueEventHandler(object sender, int value);
        public event ValueEventHandler ValueChanged;
        private void OnValueChanged(int value) => ValueChanged?.Invoke(this, value);

        public VolumeControlMenuItem() : base(new GroupBox())
        {
            int boxWidth = Width * 2 / 10;
            int barWidth = Width - boxWidth;
            
            TextBox = new NumericUpDown()
            {
                Width = boxWidth
            };

            TrackBar = new TrackBar()
            {
                Minimum = 0,
                Maximum = 100,
                Width = barWidth,
                Location = new Point(boxWidth),
                TickStyle = TickStyle.None
            };

            GroupBox.Controls.Add(TextBox);
            GroupBox.Controls.Add(TrackBar);

            TextBox.ValueChanged += NumericUpDown_ValueChanged;
            TrackBar.ValueChanged += TrackBar_ValueChanged;
            ValueChanged += SetValues;
        }

        private void SetValues(object sender, int value)
        {
            TextBox.Value = value;
            TrackBar.Value = value;
        }

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(((TrackBar)sender).Value);
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged((int)((NumericUpDown)sender).Value);
        }
    }
}
