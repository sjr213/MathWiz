using System.Windows.Input;

namespace MyMvvmLib
{
    public interface IUndoCommand : ICommand 
    {
		void Undo();
	}
}
