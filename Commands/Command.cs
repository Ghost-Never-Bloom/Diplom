using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskTracking3.Commands
{
    public class Command : ICommand
    {
        protected Action<object> _action;
        protected Predicate<object>? _predicate = null;
        public event EventHandler? CanExecuteChanged;

        public Command(Action<object> Action)
        {
            _action = Action;
        }

        public bool CanExecute(object? parameter)
        {
            if (_predicate == null)
                return true;

            return _predicate(parameter);
        }

        public void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
