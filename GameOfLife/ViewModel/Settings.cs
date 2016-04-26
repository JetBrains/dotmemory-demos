using System;
using System.Windows.Input;

namespace GameOfLife.ViewModel
{
  public sealed class Settings
  {
    private int origWidth;
    private int origHeight;

    private readonly DelegateCommand applyCommand;
    private readonly DelegateCommand cancelCommand;

    public Settings()
    {
      applyCommand = new DelegateCommand(Apply, () => true);
      cancelCommand = new DelegateCommand(Cancel, () => true);

      Width = origWidth = 80;
      Height = origHeight = 50;
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

    public event EventHandler SizeChanged;

    private void RaiseSizeChanged()
    {
      var handler = SizeChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }
  }
}