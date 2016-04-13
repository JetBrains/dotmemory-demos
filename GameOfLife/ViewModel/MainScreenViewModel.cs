using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JetBrains.Annotations;

namespace GameOfLife.ViewModel
{
  public class MainScreenViewModel : INotifyPropertyChanged
  {
    private bool isStarted;
    private readonly DelegateCommand addFieldCommand;
    private readonly DelegateCommand removeFieldCommand;
    private readonly DelegateCommand startCommand;
    private readonly DelegateCommand stopCommand;
    private readonly DelegateCommand oneStepCommand;
    private readonly DelegateCommand clearCommand;
    private readonly DelegateCommand generateCommand;

    private Field field;

    public MainScreenViewModel()
    {
      addFieldCommand = new DelegateCommand(AddField, () => !isStarted);
      removeFieldCommand = new DelegateCommand(RemoveField, () => !isStarted);
      startCommand = new DelegateCommand(Start, () => !isStarted);
      stopCommand = new DelegateCommand(Stop, () => isStarted);
      oneStepCommand = new DelegateCommand(OneStep, () => !isStarted);
      clearCommand = new DelegateCommand(Clear, () => !isStarted);
      generateCommand = new DelegateCommand(Generate, () => !isStarted);

      field = new Field(160, 100);
      field.GenerateInitialState();
    }

    private void RemoveField()
    {
      throw new NotImplementedException();
    }

    private void AddField()
    {
      throw new NotImplementedException();
    }

    private void OneStep()
    {
      field.PerformOneStep();
    }

    public Field Field
    {
      get { return field; }
      private set
      {
        field = value;
        RaisePropertyChanged();
      }
    }

    public ICommand StartCommand
    {
      get { return startCommand; }
    }

    public ICommand OneStepCommand
    {
      get { return oneStepCommand; }
    }

    public ICommand StopCommand
    {
      get { return stopCommand; }
    }

    public ICommand ClearCommand
    {
      get { return clearCommand; }
    }

    public ICommand GenerateCommand
    {
      get { return generateCommand; }
    }

    private void Start()
    {
      isStarted = true;
      UpdateCommandCanExecuteState();
      field.Start();
    }

    private void Stop()
    {
      isStarted = false;
      UpdateCommandCanExecuteState();
      field.Stop();
    }

    private void Generate()
    {
      field.GenerateInitialState();
    }

    private void Clear()
    {
      field.Clear();
    }

    private void UpdateCommandCanExecuteState()
    {
      startCommand.RaiseCanExcuteChanged();
      stopCommand.RaiseCanExcuteChanged();
      clearCommand.RaiseCanExcuteChanged();
      generateCommand.RaiseCanExcuteChanged();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null) 
        handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}