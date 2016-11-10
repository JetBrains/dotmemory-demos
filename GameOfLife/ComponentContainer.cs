using System;
using GameOfLife.ViewModel;

namespace GameOfLife
{
    public class ComponentContainer : IDisposable
    {
        private MainScreenViewModel mainViewModel;

        public MainScreenViewModel CreateMainViewModel()
        {
//      if(mainViewModel == null) // fix 
            mainViewModel = new MainScreenViewModel(2);
            return mainViewModel;
        }

        public void Dispose()
        {
            mainViewModel.Dispose(); // fix a leak
            mainViewModel = null;
        }
    }
}