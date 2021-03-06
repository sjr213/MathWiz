﻿namespace MyMvvmLib
{
    public enum MessageBoxResult 
    {
		OK,
		Cancel,
		Yes,
		No
	}

	public enum MessageBoxIcon 
    {
		Informatiom, Exclamation, Question
	}

	public interface IMessageBoxService 
    {
		MessageBoxResult Show(string text, string title = null, MessageBoxIcon icon = MessageBoxIcon.Informatiom);
	}
}
