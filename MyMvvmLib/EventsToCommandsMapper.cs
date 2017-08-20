using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Reflection;

namespace MyMvvmLib
{
    public static class EventsToCommandsMapper 
    {
		public static EventToCommandCollection GetMapper(DependencyObject obj) 
        {
			return obj.GetValue(MapperProperty) as EventToCommandCollection;
		}

		public static void SetMapper(DependencyObject obj, EventToCommandCollection value) 
        {
			obj.SetValue(MapperProperty, value);
		}

		public static readonly DependencyProperty MapperProperty =
			 DependencyProperty.RegisterAttached("Mapper", typeof(EventToCommandCollection), typeof(EventsToCommandsMapper),
			 new FrameworkPropertyMetadata(null, OnMapperChanged));

		private static void OnMapperChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) 
        {
			var eventCollection = (EventToCommandCollection)e.NewValue;

			if(eventCollection == null) 
                return;

			foreach(var evt in eventCollection) 
            {
				if(evt.Command == null || evt.Event == null) 
                    continue;

				var eventInfo = obj.GetType().GetEvent(evt.Event);

				if(eventInfo == null) 
                    throw new ArgumentException("no such event exists");

				var c = new Closure { EventToCommand = evt };

				eventInfo.AddEventHandler(obj, Delegate.CreateDelegate(eventInfo.EventHandlerType, c,
					typeof(Closure).GetMethod("EventHandler", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));
			}
		}

		class Closure 
        {
			public EventToCommand EventToCommand;

			public void EventHandler(object sender, RoutedEventArgs e) 
            {
				var param = EventToCommand.CommandParameter ?? e;

				if(EventToCommand.Command.CanExecute(param)) 
                {
					EventToCommand.Command.Execute(param);
					e.Handled = EventToCommand.HandledIfExecuted;
				}
			}
		}
	}

	public class EventToCommand : Freezable 
    {
		[TypeConverter(typeof(CommandConverter))]
		public ICommand Command 
        {
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public static readonly DependencyProperty CommandProperty =
			 DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand),
			 new UIPropertyMetadata(null));

		public bool HandledIfExecuted 
        {
			get { return (bool)GetValue(HandledIfExecutedProperty); }
			set { SetValue(HandledIfExecutedProperty, value); }
		}

		public static readonly DependencyProperty HandledIfExecutedProperty =
			 DependencyProperty.Register("HandledIfExecuted", typeof(bool), typeof(EventToCommand), new UIPropertyMetadata(true));

		
		public object CommandParameter 
        {
			get { return (object)GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public static readonly DependencyProperty CommandParameterProperty =
			 DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new UIPropertyMetadata(null));

		public string Event 
        {
			get { return (string)GetValue(EventProperty); }
			set { SetValue(EventProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Event.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EventProperty =
			 DependencyProperty.Register("Event", typeof(string), typeof(EventToCommand), new UIPropertyMetadata(null));



		protected override Freezable CreateInstanceCore() 
        {
			throw new NotImplementedException();
		}
	}

	public class EventToCommandCollection : FreezableCollection<EventToCommand> 
    {
	}
}
