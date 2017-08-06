using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMvvmLib
{
	public interface IDialogService 
    {
		bool? ShowDialog();
		object ViewModel { get; set; }
	}
}
