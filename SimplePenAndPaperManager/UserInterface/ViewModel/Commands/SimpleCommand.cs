using System;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool ExecutionEnabled
        {
            get { return _executionEnabled; }
            set
            {
                _executionEnabled = value;
                CanExecuteChanged?.Invoke(this, null);
            }
        }
        private bool _executionEnabled;

        protected Action _action;

        public SimpleCommand(Action action, bool executionEnabled = true)
        {
            _action = action;
            ExecutionEnabled = executionEnabled;
        }

        public bool CanExecute(object parameter)
        {
            return _executionEnabled;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }
}
