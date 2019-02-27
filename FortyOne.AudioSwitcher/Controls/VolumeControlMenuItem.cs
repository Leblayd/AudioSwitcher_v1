using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FortyOne.AudioSwitcher.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    public class VolumeControlMenuItem : ToolStripControlHost
    {
        private GroupBox GroupBox => Control as GroupBox;
        private NumericUpDown TextBox { get; }
        private TrackBar TrackBar { get; }

        #region Scroll

        public int Change { get; set; }
        
        public event MouseEventHandler Scroll;
        public void OnScroll(MouseEventArgs args)
        {
            Scroll?.Invoke(this, args);
        }

        private void MenuItem_Scroll(object sender, MouseEventArgs args)
        {
            Value += args.Delta > 0 ? Change : -Change;
        }

        #endregion

        #region Value
        
        public int Value
        {
            get => TrackBar.Value;
            set => TrackBar.Value = value;
        }
        
        public event EventHandler ValueChanged;
        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
        
        public VolumeControlMenuItem() : base(new GroupBox())
        {
            int boxWidth = Width * 2 / 10;
            int barWidth = Width - boxWidth;
            
            TextBox = new NumericUpDown
            {
                Width = boxWidth
            };

            TrackBar = new TrackBar
            {
                Minimum = 0,
                Maximum = 100,
                Width = barWidth,
                Location = new Point(boxWidth),
                TickFrequency = 10
            };

            GroupBox.Controls.Add(TextBox);
            GroupBox.Controls.Add(TrackBar);

            TextBox.ValueChanged += NumericUpDown_ValueChanged;
            TrackBar.ValueChanged += TrackBar_ValueChanged;
            Scroll += MenuItem_Scroll;
        }

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            TextBox.Value = TrackBar.Value;
            OnValueChanged();
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            TrackBar.Value = (int)TextBox.Value;
            OnValueChanged();
        }
    }
}
