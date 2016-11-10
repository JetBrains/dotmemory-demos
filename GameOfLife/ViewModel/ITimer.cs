using System;

namespace GameOfLife.ViewModel
{
    public interface ITimer
    {
        event Action Tick;
    }
}