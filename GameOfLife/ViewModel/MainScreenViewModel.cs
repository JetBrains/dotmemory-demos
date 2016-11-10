using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ExternalLib;
using GameOfLife.Common;

namespace GameOfLife.ViewModel
{
    public class MainScreenViewModel : IDisposable, INotifyPropertyChanged
    {
        private bool isStarted;
        private readonly DelegateCommand addPetriDishCommand;
        private readonly DelegateCommand removePetriDishCommand;
        private readonly DelegateCommand startCommand;
        private readonly DelegateCommand stopCommand;
        private readonly DelegateCommand oneStepCommand;
        private readonly DelegateCommand clearCommand;
        private readonly DelegateCommand generateCommand;
        private readonly DelegateCommand showSettingsCommand;

        private Action showSettingsView;

        private readonly NotifyCollection<PetriDish> petriDishesCollection = new NotifyCollection<PetriDish>();

        private readonly TimerImpl centralTimer = new TimerImpl();

        public MainScreenViewModel(int initialPetriDishesCount)
        {
            addPetriDishCommand = new DelegateCommand(AddPetriDish, () => !isStarted);
            removePetriDishCommand = new DelegateCommand(RemovePetriDish, () => !isStarted);
            startCommand = new DelegateCommand(Start, () => !isStarted);
            stopCommand = new DelegateCommand(Stop, () => isStarted);
            oneStepCommand = new DelegateCommand(OneStep, () => !isStarted);
            clearCommand = new DelegateCommand(Clear, () => !isStarted);
            generateCommand = new DelegateCommand(Generate, () => !isStarted);
            showSettingsCommand = new DelegateCommand(ShowSettings, () => !isStarted);

            FillPetriDishesCollection(initialPetriDishesCount);

            Settings.Instance.SizeChanged += SettingsOnSizeChanged;
        }

        private void SettingsOnSizeChanged(object sender, EventArgs args)
        {
            var count = petriDishesCollection.Count;
            while (petriDishesCollection.Count > 0)
            {
                RemovePetriDish();
            }

            FillPetriDishesCollection(count);
        }

        private void FillPetriDishesCollection(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var petriDish = CreatePetriDish();
                petriDish.GenerateInitialState();
                petriDishesCollection.Add(petriDish);
            }
        }

        private PetriDish CreatePetriDish()
        {
            return new PetriDish(Settings.Instance.Width, Settings.Instance.Height, centralTimer);
        }

        private void AddPetriDish()
        {
            var dish = CreatePetriDish();
            dish.GenerateInitialState();
            petriDishesCollection.Add(dish);
        }

        private void RemovePetriDish()
        {
            var removedDish = petriDishesCollection.RemoveLast();
            // removedDish.Dispose(); //fix MainScreenViewModelTest.RemovePetriDish
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

        public ICommand AddPetriDishCommand
        {
            get { return addPetriDishCommand; }
        }

        public ICommand RemovePetriDishCommand
        {
            get { return removePetriDishCommand; }
        }

        public ICommand ShowSettingsCommand
        {
            get { return showSettingsCommand; }
        }

        public Action ShowSettingsView
        {
            get { return showSettingsView; }
            set { showSettingsView = value; }
        }

        private void Start()
        {
            isStarted = true;
            UpdateCommandCanExecuteState();
            centralTimer.Start();
        }

        private void Stop()
        {
            isStarted = false;
            UpdateCommandCanExecuteState();
            centralTimer.Stop();
        }

        private void Generate()
        {
            foreach (var field in petriDishesCollection)
            {
                field.GenerateInitialState();
            }
        }

        private void Clear()
        {
            foreach (var field in petriDishesCollection)
            {
                field.Clear();
            }
        }

        private void ShowSettings()
        {
            showSettingsView();
        }

        private void UpdateCommandCanExecuteState()
        {
            startCommand.RaiseCanExcuteChanged();
            stopCommand.RaiseCanExcuteChanged();
            clearCommand.RaiseCanExcuteChanged();
            generateCommand.RaiseCanExcuteChanged();
            addPetriDishCommand.RaiseCanExcuteChanged();
            removePetriDishCommand.RaiseCanExcuteChanged();
            showSettingsCommand.RaiseCanExcuteChanged();
        }

        public void Dispose()
        {
            //     Settings.Instance.SizeChanged -= SettingsOnSizeChanged; // fix MainScreenViewModelTest.NoObjectsLeakedOnEventHandler and AllObjectsAreReleased
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}