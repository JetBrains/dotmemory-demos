using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace GameOfLife.ViewModel
{
  public class MainScreenViewModel : INotifyPropertyChanged
  {
    private bool isStarted;
    private readonly DelegateCommand addPetriDishCommand;
    private readonly DelegateCommand removePetriDishCommand;
    private readonly DelegateCommand startCommand;
    private readonly DelegateCommand stopCommand;
    private readonly DelegateCommand oneStepCommand;
    private readonly DelegateCommand clearCommand;
    private readonly DelegateCommand generateCommand;

    private readonly ObservableCollection<Field> fields;
    private static readonly int DefaultDishWidth = 80;

    public MainScreenViewModel()
    {
      addPetriDishCommand = new DelegateCommand(AddField, () => !isStarted);
      removePetriDishCommand = new DelegateCommand(RemoveField, () => !isStarted);
      startCommand = new DelegateCommand(Start, () => !isStarted);
      stopCommand = new DelegateCommand(Stop, () => isStarted);
      oneStepCommand = new DelegateCommand(OneStep, () => !isStarted);
      clearCommand = new DelegateCommand(Clear, () => !isStarted);
      generateCommand = new DelegateCommand(Generate, () => !isStarted);

      fields = new ObservableCollection<Field>{new Field(DefaultDishWidth, 50), new Field(DefaultDishWidth, 50)};

      foreach (var field in fields)
        field.GenerateInitialState();
    }

    private void AddField()
    {
      var dish = new Field(DefaultDishWidth, 50);
      dish.GenerateInitialState();
      fields.Add(dish);
    }

    private void RemoveField()
    {
      fields.RemoveAt(fields.Count - 1);
    }

    private void OneStep()
    {
      foreach (var field in fields)
        field.PerformOneStep();
    }

    public IEnumerable<Field> Fields
    {
      get { return fields; }
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

    public DelegateCommand AddPetriDishCommand
    {
      get { return addPetriDishCommand; }
    }

    public DelegateCommand RemovePetriDishCommand
    {
      get { return removePetriDishCommand; }
    }

    private void Start()
    {
      isStarted = true;
      UpdateCommandCanExecuteState();
      foreach (var field in fields)
        field.Start();
    }

    private void Stop()
    {
      isStarted = false;
      UpdateCommandCanExecuteState();
      foreach (var field in fields)
        field.Stop();
    }

    private void Generate()
    {
      foreach (var field in fields)
        field.GenerateInitialState();
    }

    private void Clear()
    {
      foreach (var field in fields)
        field.Clear();
    }

    private void UpdateCommandCanExecuteState()
    {
      startCommand.RaiseCanExcuteChanged();
      stopCommand.RaiseCanExcuteChanged();
      clearCommand.RaiseCanExcuteChanged();
      generateCommand.RaiseCanExcuteChanged();
      addPetriDishCommand.RaiseCanExcuteChanged();
      removePetriDishCommand.RaiseCanExcuteChanged();
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}