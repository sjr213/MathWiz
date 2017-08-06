using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MyMvvmLib
{
	public class CompositeCommand : CommandBase 
    {
		List<IUndoCommand> _commands = new List<IUndoCommand>();

		public void Add(IUndoCommand cmd) 
        {
			_commands.Add(cmd);
		}

		public override void Execute(object parameter) 
        {
			foreach(var cmd in _commands)
				cmd.Execute(parameter);
		}

		public override void Undo() 
        {
			for(int i = _commands.Count - 1; i >= 0; i--)
				_commands[i].Undo();
		}
	}
}
