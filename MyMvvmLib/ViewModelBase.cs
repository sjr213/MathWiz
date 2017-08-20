namespace MyMvvmLib
{
    public abstract class ViewModelBase : ObservableObject 
    {
	}
	
	public abstract class ViewModelBase<TModel> : ViewModelBase 
    {
		TModel _model;

		public TModel Model 
        {
			get { return _model; }
			set { SetProperty(ref _model, value, () => Model); }
		}

		public ViewModelBase(TModel model = default(TModel)) 
        {
			Model = model;
		}

	}

    public abstract class ViewModelBase<TModel, TParentVM> : ViewModelBase<TModel> 
    {
	    public ViewModelBase(TModel model = default(TModel), TParentVM parentVM = default(TParentVM)) 
        {
		    Model = model;
		    Parent = parentVM;
	    }

	    public TParentVM Parent { get; set; }
    }
}
