using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using System;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        protected IDataModel _context;

        public BaseCommand(IDataModel context)
        {
            _context = context;
            _context.PropertyChanged += OnCanExecutedChanged;
        }

        protected void OnCanExecutedChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, null);
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
