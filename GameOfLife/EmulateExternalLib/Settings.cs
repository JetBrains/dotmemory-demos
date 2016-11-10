using System;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace ExternalLib
{
    public sealed class Settings
    {
        private readonly DelegateCommand applyCommand;
        private readonly DelegateCommand cancelCommand;

        private int origHeight;
        private int origWidth;

        public Settings()
        {
            applyCommand = new DelegateCommand(Apply, () => true);
            cancelCommand = new DelegateCommand(Cancel, () => true);

            Width = origWidth = 80;
            Height = origHeight = 50;
        }

        public static Settings Instance { get; } = new Settings();

        public int Width { get; set; }
        public int Height { get; set; }

        public ICommand ApplyCommand
        {
            get { return applyCommand; }
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
        }

        private void Apply()
        {
            origWidth = Width;
            origHeight = Height;
            RaiseSizeChanged();
        }

        private void Cancel()
        {
            Width = origWidth;
            Height = origHeight;
        }

        public event EventHandler SizeChanged;

        private void RaiseSizeChanged()
        {
            var handler = SizeChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}