using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace XPlaneWPF.Commands
{
    public class Command<T> : ICommand
    {
        private Action<T> execute;
        private Predicate<T> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public Command(Action<T> execute)
            : this(execute, p => true)
        {

        }

        public Command(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(T parameter)
        {
            var result= canExecute != null && canExecute(parameter);

            return result;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }

        public void OnCanExecutedChanged()
        {
            var handler = CanExecuteChangedInternal;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ => { return; };
        }
    }

    public class Command : Command<object>
    {
        public Command(Action<object> execute) : base(execute) { }

        public Command(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute) { }
    }
}