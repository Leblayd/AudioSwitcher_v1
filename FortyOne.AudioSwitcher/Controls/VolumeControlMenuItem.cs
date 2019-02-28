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

        public int Change
        {
            get => TrackBar.SmallChange;
            set
            {
                TrackBar.SmallChange = value;
                TrackBar.LargeChange = value;
                TextBox.Increment = value;
            }
        }

        public event MouseEventHandler Scroll;
        public void OnScroll(HandledMouseEventArgs args)
        {
            Scroll?.Invoke(this, args);
        }

        private void MenuItem_Scroll(object sender, MouseEventArgs e)
        {
            if (!(e is HandledMouseEventArgs args) || args.Handled) throw new ArgumentException();
            
            Value += args.Delta > 0 ? Change : -Change;
            args.Handled = true;
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

        #endregion
        
        public VolumeControlMenuItem() : base(new GroupBox())
        {
            int boxWidth = Width * 2 / 10;
            int barWidth = Width - boxWidth;
            
            TextBox = new NumericUpDown
            {
                Width = boxWidth,
                Minimum = 0,
                Maximum = 100
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
            TrackBar.MouseWheel += MenuItem_Scroll;
            TextBox.MouseWheel += MenuItem_Scroll;
            Scroll += MenuItem_Scroll;
        }
    }
}
