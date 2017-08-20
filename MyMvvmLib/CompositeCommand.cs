using System.Collections.Generic;

// Allows combining individual commands that can be executed together later or undone
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
