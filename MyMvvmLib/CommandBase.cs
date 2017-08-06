using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MyMvvmLib 
{
	public abstract class CommandBase : IUndoCommand 
    {
		public virtual bool CanExecute(object parameter) 
        {
			return true;
		}

		public event EventHandler CanExecuteChanged 
        {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public abstract void Execute(object parameter);
		public abstract void Undo();
	}
}
