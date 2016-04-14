using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

    private readonly ObservableCollection<PetriDish> petriDishesCollection = new ObservableCollection<PetriDish>();
    private const int DefaultDishWidth = 80;
    private const int DefaultDishHeight = 50;

    private readonly TimerImpl timer = new TimerImpl();

    public MainScreenViewModel(int initialPetriDishesCount = 2)
    {

      addPetriDishCommand = new DelegateCommand(AddPetriDish, () => !isStarted);
      removePetriDishCommand = new DelegateCommand(RemovePetriDish, () => !isStarted);
      startCommand = new DelegateCommand(Start, () => !isStarted);
      stopCommand = new DelegateCommand(Stop, () => isStarted);
      oneStepCommand = new DelegateCommand(OneStep, () => !isStarted);
      clearCommand = new DelegateCommand(Clear, () => !isStarted);
      generateCommand = new DelegateCommand(Generate, () => !isStarted);

      for (var i = 0; i < initialPetriDishesCount; i++)
        petriDishesCollection.Add(CreatePetriDish());

      foreach (var field in petriDishesCollection)
        field.GenerateInitialState();
    }

    private PetriDish CreatePetriDish()
    {
      return new PetriDish(DefaultDishWidth, DefaultDishHeight, timer);
    }

    private void AddPetriDish()
    {
      var dish = CreatePetriDish();
      dish.GenerateInitialState();
      petriDishesCollection.Add(dish);
    }

    private void RemovePetriDish()
    {
      var petriDish = petriDishesCollection.LastOrDefault();
      if (petriDish != null)
      {
        petriDishesCollection.Remove(petriDish);
//        petriDish.Dispose(); //fix a leak
      }
    }

    private void OneStep()
    {
      foreach (var field in petriDishesCollection)
        field.PerformOneStep();
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
      timer.Start();
    }

    private void Stop()
    {
      isStarted = false;
      UpdateCommandCanExecuteState();
      timer.Stop();
    }

    private void Generate()
    {
      foreach (var field in petriDishesCollection)
        field.GenerateInitialState();
    }

    private void Clear()
    {
      foreach (var field in petriDishesCollection)
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