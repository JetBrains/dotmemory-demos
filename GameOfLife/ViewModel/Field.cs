using System;
using System.Threading;

namespace GameOfLife.ViewModel
{
  public class Field
  {
    private const int UpdateOnceIn = 200;

    private readonly Cell[,] currentCells;
    private readonly Cell[,] nextGenerationCells;
    private readonly int height;
    private readonly int width;
    private readonly Timer timer;

    public Field(int width, int height)
    {
      this.width = width;
      this.height = height;
      timer = new Timer(UpdateCellsState);
      currentCells = new Cell[width, height];
      nextGenerationCells = new Cell[width, height];

      Clear();
    }

    public Cell[,] Cells
    {
      get { return currentCells; }
    }

    public void Clear()
    {
      for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
          currentCells[i,j] = new Cell(0, false);
          nextGenerationCells[i,j] = new Cell(0, false);
        }

      RaiseUpdated();
    }

    public void GenerateInitialState()
    {
      var random = new Random(DateTime.Now.Second);
      for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
          currentCells[i, j].IsAlive = random.NextDouble() > 0.8;
      RaiseUpdated();
    }

    private void UpdateCellsState(object state)
    {
      for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
          nextGenerationCells[i, j] = GetNextGenerationCellUnoptimized(i, j);

      for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
          currentCells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
          currentCells[i, j].Age = nextGenerationCells[i, j].Age;
        }

      RaiseUpdated();
    }

    public void Start()
    {
      timer.Change(0, UpdateOnceIn);
    }

    public void PerformOneStep()
    {
      UpdateCellsState(null);
    }

    public void Stop()
    {
      timer.Change(-1, UpdateOnceIn);
    }

    public event EventHandler Updated;

    private Cell GetNextGenerationCellUnoptimized(int i, int j) // UNOPTIMIZED
    {
      var cell = currentCells[i, j];

      var isAlive = cell.IsAlive;
      var neighborsCount = CountNeighbors(i, j);

      if (isAlive && neighborsCount < 2)
        return new Cell(0, false);

      if (isAlive && (neighborsCount == 2 || neighborsCount == 3))
        return new Cell(cell.Age + 1, true);

      if (isAlive && neighborsCount > 3)
        return new Cell(0, false);

      if (!isAlive && neighborsCount == 3)
        return new Cell(0, true);

      return new Cell(0, false);
    }

    private Cell GetNextGenerationCell(int i, int j) // OPTIMIZED
    {
      var currentCell = currentCells[i, j];

      var isAlive = currentCell.IsAlive;
      var neighborsCount = CountNeighbors(i, j);

      var cell = nextGenerationCells[i, j];
      if (!isAlive)
      {
        if (neighborsCount == 3)
        {
          cell.IsAlive = true;
          cell.Age = 0;
        }
      }
      else if (neighborsCount == 2 || neighborsCount == 3)
      {
        cell.IsAlive = true;
        cell.Age = currentCell.Age + 1;
      }
      else
      {
        cell.IsAlive = false;
        cell.Age = 0;
      }

      return cell;
    }

    private int CountNeighbors(int i, int j)
    {
      var count = 0;

      if (i != width - 1 && currentCells[i + 1, j].IsAlive) count++;
      if (i != width - 1 && j != height - 1 && currentCells[i + 1, j + 1].IsAlive) count++;
      if (j != height - 1 && currentCells[i, j + 1].IsAlive) count++;
      if (i != 0 && j != height - 1 && currentCells[i - 1, j + 1].IsAlive) count++;
      if (i != 0 && currentCells[i - 1, j].IsAlive) count++;
      if (i != 0 && j != 0 && currentCells[i - 1, j - 1].IsAlive) count++;
      if (j != 0 && currentCells[i, j - 1].IsAlive) count++;
      if (i != width - 1 && j != 0 && currentCells[i + 1, j - 1].IsAlive) count++;

      return count;
    }

    private void RaiseUpdated()
    {
      var handler = Updated;
      if (handler != null) 
        handler(this, EventArgs.Empty);
    }
  }
}