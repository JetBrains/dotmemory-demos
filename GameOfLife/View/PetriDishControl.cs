using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GameOfLife.ViewModel;

namespace GameOfLife.View
{
    public class PetriDishControl : ContentControl
    {
        private const int CellSize = 5;

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", 
            typeof(PetriDish), 
            typeof(PetriDishControl),
            new PropertyMetadata(default(PetriDish), OnViewModelChanged));

        private static readonly SolidColorBrush BackgroundBrush = Brushes.Gray;
        private static readonly SolidColorBrush YoungCellBrush = Brushes.White;
        private static readonly SolidColorBrush OldCellBrush = Brushes.DarkGray;
        private Canvas canvas;
        private Ellipse[,] ellipses;

        public PetriDish ViewModel
        {
            get { return (PetriDish) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void ViewModelOnUpdated(object sender, EventArgs eventArgs)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Func<object>(() =>
                {
                    UpdateUi();
                    return null;
                }));
            }
            else
            {
                UpdateUi();
            }
        }

        private void UpdateUi()
        {
            var cells = ViewModel.Cells;
            var width = cells.GetLength(0);
            var height = cells.GetLength(1);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var cell = cells[i, j];
                    ellipses[i, j].Fill =
                        cell.IsAlive
                            ? cell.Age < 2
                                ? YoungCellBrush
                                : OldCellBrush
                            : Brushes.Transparent;
                }
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var width = ViewModel.Cells.GetLength(0);
            var height = ViewModel.Cells.GetLength(1);

            return new Size(width * CellSize, height * CellSize);
        }

        private void OnViewModelChanged(PetriDish oldValue, PetriDish newValue)
        {
            if (oldValue != null)
            {
                oldValue.Updated -= ViewModelOnUpdated;
            }

            if (newValue != null)
            {
                var width = newValue.Cells.GetLength(0);
                var height = newValue.Cells.GetLength(1);

                canvas = new Canvas {Background = BackgroundBrush};
                ellipses = new Ellipse[width, height];
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        var ellipse = new Ellipse
                        {
                            Width = CellSize,
                            Height = CellSize,
                            Fill = Brushes.Gray,
                            Margin = new Thickness(i * CellSize, j * CellSize, 0, 0)
                        };

                        ellipse.MouseLeftButtonDown += EllipseOnMouseMove;
                        ellipse.MouseMove += EllipseOnMouseMove;

                        ellipses[i, j] = ellipse;
                        canvas.Children.Add(ellipse);
                    }
                }

                Content = canvas;
                newValue.Updated += ViewModelOnUpdated;
                UpdateUi();
            }
        }

        private void EllipseOnMouseMove(object sender, MouseEventArgs e)
        {
            var cellVisual = sender as Ellipse;
            if (cellVisual == null)
            {
                return;
            }

            var cells = ViewModel.Cells;

            var i = (int) cellVisual.Margin.Left / CellSize;
            var j = (int) cellVisual.Margin.Top / CellSize;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!cells[i, j].IsAlive)
                {
                    cells[i, j].IsAlive = true;
                    cells[i, j].Age = 0;
                    cellVisual.Fill = YoungCellBrush;
                }
            }
        }

        private static void OnViewModelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ((PetriDishControl) dependencyObject).OnViewModelChanged(
                args.OldValue as PetriDish,
                args.NewValue as PetriDish);
        }
    }
}