namespace MyMvvmLib
{
    public interface IDialogService 
    {
		bool? ShowDialog();
		object ViewModel { get; set; }
	}
}
