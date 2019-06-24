using System;
using System.Windows.Input;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[RenderWith(typeof(_SwitchRenderer))]
	public class Switch : View, IElementConfiguration<Switch>
	{
		public static readonly BindableProperty IsToggledProperty = BindableProperty.Create("IsToggled", typeof(bool), typeof(Switch), false, propertyChanged: (bindable, oldValue, newValue) =>
		{
			var switchControl = (Switch)bindable;

			EventHandler<ToggledEventArgs> eh = switchControl.Toggled;
			if (eh != null)
				eh(bindable, new ToggledEventArgs((bool)newValue));

			if((bool)newValue && switchControl.ToggledCommand != null && switchControl.ToggledCommand.CanExecute(switchControl.ToggledCommandParameter))
			{
				switchControl.ToggledCommand.Execute(switchControl.ToggledCommandParameter);
			}
			else if(switchControl.UnToggledCommand != null && switchControl.UnToggledCommand.CanExecute(switchControl.ToggledCommandParameter))
			{
				switchControl.UnToggledCommand.Execute(switchControl.ToggledCommandParameter);
			}

		}, defaultBindingMode: BindingMode.TwoWay);

		public static readonly BindableProperty OnColorProperty = BindableProperty.Create(nameof(OnColor), typeof(Color), typeof(Switch), Color.Default);

		public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(Switch), Color.Default);
		public static readonly BindableProperty ToggledCommandProperty = BindableProperty.Create(nameof(ToggledCommand), typeof(ICommand), typeof(Switch));
		public static readonly BindableProperty UnToggledCommandProperty = BindableProperty.Create(nameof(UnToggledCommand), typeof(ICommand), typeof(Switch));
		public static readonly BindableProperty ToggledCommandParameterProperty = BindableProperty.Create(nameof(ToggledCommandParameter), typeof(ICommand), typeof(Switch));

		public Color OnColor
		{
			get { return (Color)GetValue(OnColorProperty); }
			set { SetValue(OnColorProperty, value); }
		}

		public Color ThumbColor
		{
			get { return (Color)GetValue(ThumbColorProperty); }
			set { SetValue(ThumbColorProperty, value); }
		}

		readonly Lazy<PlatformConfigurationRegistry<Switch>> _platformConfigurationRegistry;

		public Switch()
		{
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<Switch>>(() => new PlatformConfigurationRegistry<Switch>(this));
		}

		public bool IsToggled
		{
			get { return (bool)GetValue(IsToggledProperty); }
			set { SetValue(IsToggledProperty, value); }
		}

		public ICommand ToggledCommand
		{
			get => GetValue(ToggledCommandProperty) as ICommand;
			set => SetValue(ToggledCommandProperty, value);
		}

		public ICommand UnToggledCommand
		{
			get => GetValue(UnToggledCommandProperty) as ICommand;
			set => SetValue(UnToggledCommandProperty, value);
		}

		public object ToggledCommandParameter
		{
			get => GetValue(ToggledCommandParameterProperty);
			set => SetValue(ToggledCommandParameterProperty, value);
		}

		public event EventHandler<ToggledEventArgs> Toggled;

		public IPlatformElementConfiguration<T, Switch> On<T>() where T : IConfigPlatform
		{
			return _platformConfigurationRegistry.Value.On<T>();
		}
	}
}