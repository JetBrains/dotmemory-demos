using System;
using GameOfLife.ViewModel;

namespace GameOfLife
{
  public class ComponentContainer : IDisposable
  {
    private MainScreenViewModel mainViewModel;
    private Settings settingsSingleton = new Settings();

    public MainScreenViewModel CreateMainViewModel()
    {
      mainViewModel = new MainScreenViewModel(2, settingsSingleton);
      return mainViewModel;
    }

    public void Dispose()
    {
      mainViewModel.Dispose(); // fix a leak
      mainViewModel = null;
      settingsSingleton = null;
    }
  }
}