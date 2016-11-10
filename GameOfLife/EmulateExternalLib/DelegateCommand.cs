using System;
using System.Windows.Input;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace

namespace ExternalLib
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> myExecute;

        private readonly Predicate<object> myCanExecute;

        public DelegateCommand([NotNull] Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            myExecute = execute;
            myCanExecute = canExecute;
        }

        public DelegateCommand([NotNull] Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            myExecute = _ => execute();
            myCanExecute = canExecute != null 
                ? _ => canExecute() 
                : (Predicate<object>) null;
        }

        public bool CanExecute(object parameter)
        {
            return myCanExecute == null || myCanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            myExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExcuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}