using System;
using System.Windows.Forms;

namespace Attuned
{
    public partial class LoadingForm : Form
    {
        private int _MaxValue = 0;
        public int MaxValue
        {
            get { return _MaxValue; }
            set
            {
                _MaxValue = value;
                Value = Math.Min(ProgressBar.Value, value);
                ProgressBar.Maximum = value;
            }
        }

        private int _Value = 0;
        public int Value
        {
            get { return _Value; }
            set
            {
                // return if not changed
                if (_Value == value) { return; }
                _Value = value;

                // if set to refresh instantly
                if (RefreshInstantly)
                {
                    // set ui value now
                    ProgressBar.Value = value;

                    UpdateProgressLB();
                }

                // if set to close on max value and max value reached
                if (CloseOnMaxValue && value != 0 && MaxValue == value ) 
                {
                    this.Close(); 
                }
            }
        }

        public string Message
        {
            get { return MessageLB.Text; }
            set { MessageLB.Text = value; }
        }

        public bool CloseOnMaxValue { get; set; }

        public bool RefreshInstantly { get; set; }

        public TimeSpan RefreshInterval { get; set; }

        private Timer RefreshTimer;

        private void UpdateProgressLB()
        {
            ProgressLB.Text = $"Progress: {Value}/{MaxValue}";
        }

        public LoadingForm()
        {
            InitializeComponent();

            CenterToScreen();

            TopMost = true;
            RefreshInstantly = false;
            CloseOnMaxValue = true;
            RefreshInterval = TimeSpan.FromMilliseconds(200);

            RefreshTimer = new Timer()
            {
                Interval = RefreshInterval.Milliseconds
            };

            // timer action
            RefreshTimer.Tick += (s, e) => 
            {
                // update if value changed necessary
                if (ProgressBar.Value != Value)
                {
                    ProgressBar.Value = Value;

                    UpdateProgressLB();
                }
            };

            // start the timer
            RefreshTimer.Start();
        }
    }
}
