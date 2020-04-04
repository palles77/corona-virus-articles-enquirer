﻿using System;
using System.Windows.Forms;

namespace CovidLib
{
    public class DelayTypingTextBox : TextBox
    {
        private Timer m_delayedTextChangedTimer;

        public event EventHandler DelayedTextChanged;

        public DelayTypingTextBox() : base()
        {
            this.DelayedTextChangedTimeout = 5 * 1000; // 10 seconds
        }

        protected override void Dispose(bool disposing)
        {
            if (m_delayedTextChangedTimer != null)
            {
                m_delayedTextChangedTimer.Stop();
                if (disposing)
                {
                    m_delayedTextChangedTimer.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        public int DelayedTextChangedTimeout { get; set; }

        protected virtual void OnDelayedTextChanged(EventArgs e)
        {
            this.DelayedTextChanged?.Invoke(this, e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.InitializeDelayedTextChangedEvent();
            base.OnTextChanged(e);
        }

        private void InitializeDelayedTextChangedEvent()
        {
            if (m_delayedTextChangedTimer != null)
            {
                m_delayedTextChangedTimer.Stop();
            }

            if (m_delayedTextChangedTimer == null || m_delayedTextChangedTimer.Interval != this.DelayedTextChangedTimeout)
            {
                m_delayedTextChangedTimer = new Timer();
                m_delayedTextChangedTimer.Tick += new EventHandler(HandleDelayedTextChangedTimerTick);
                m_delayedTextChangedTimer.Interval = this.DelayedTextChangedTimeout;
            }

            m_delayedTextChangedTimer.Start();
        }

        private void HandleDelayedTextChangedTimerTick(object sender, EventArgs e)
        {
            Timer timer = sender as Timer;
            timer.Stop();

            this.OnDelayedTextChanged(EventArgs.Empty);
        }
    }
}
