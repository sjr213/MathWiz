﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MyMvvmLib
{
	public interface IUndoCommand : ICommand 
    {
		void Undo();
	}
}
