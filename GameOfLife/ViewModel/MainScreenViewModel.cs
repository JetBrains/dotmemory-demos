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

    private readonly ObservableCollection<PetriDish> petriDishesCollection;
    private static readonly int DefaultDishWidth = 80;

    public MainScreenViewModel()
    {
      addPetriDishCommand = new DelegateCommand(AddPetriDish, () => !isStarted);
      removePetriDishCommand = new DelegateCommand(RemovePetriDish, () => !isStarted);
      startCommand = new DelegateCommand(Start, () => !isStarted);
      stopCommand = new DelegateCommand(Stop, () => isStarted);
      oneStepCommand = new DelegateCommand(OneStep, () => !isStarted);
      clearCommand = new DelegateCommand(Clear, () => !isStarted);
      generateCommand = new DelegateCommand(Generate, () => !isStarted);

      petriDishesCollection = new ObservableCollection<PetriDish>{new PetriDish(DefaultDishWidth, 50), new PetriDish(DefaultDishWidth, 50)};

      foreach (var dish in petriDishesCollection)
        dish.GenerateInitialState();
    }

    private void AddPetriDish()
    {
      var dish = new PetriDish(DefaultDishWidth, 50);
      dish.GenerateInitialState();
      petriDishesCollection.Add(dish);
    }

    private void RemovePetriDish()
    {
      petriDishesCollection.RemoveAt(petriDishesCollection.Count - 1);
    }

    private void OneStep()
    {
      foreach (var dish in petriDishesCollection)
        dish.PerformOneStep();
    }

    public IEnumerable<PetriDish> PetriDishesCollection
    {
      get { return petriDishesCollection; }
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
      foreach (var dish in petriDishesCollection)
        dish.Start();
    }

    private void Stop()
    {
      isStarted = false;
      UpdateCommandCanExecuteState();
      foreach (var dish in petriDishesCollection)
        dish.Stop();
    }

    private void Generate()
    {
      foreach (var dish in petriDishesCollection)
        dish.GenerateInitialState();
    }

    private void Clear()
    {
      foreach (var dish in petriDishesCollection)
        dish.Clear();
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