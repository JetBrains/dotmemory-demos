using System;
using GameOfLife.View;

namespace GameOfLife
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private readonly ComponentContainer container = new ComponentContainer();

    public MainWindow()
    {
      InitializeComponent();

      var mainScreenViewModel = container.CreateMainViewModel();
      mainScreenViewModel.ShowSettingsView = ShowSettingsView;
      DataContext = mainScreenViewModel;
    }

    protected override void OnClosed(EventArgs e)
    {
      container.Dispose();
    }

    private static void ShowSettingsView()
    {
      var settingsWindow = new SettingsWindow();
      settingsWindow.ShowDialog();
    }
  }
}
