using System;
using System.Windows.Input;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[RenderWith(typeof(_CheckBoxRenderer))]
	public class CheckBox : View, IElementConfiguration<CheckBox>, IBorderElement, IColorElement
	{
		public const string IsCheckedVisualState = "IsChecked";

		public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox), false, propertyChanged: (bindable, oldValue, newValue) =>
		{
			var checkBox = (CheckBox)bindable;

			checkBox.CheckedChanged?.Invoke(bindable, new CheckedChangedEventArgs((bool)newValue));
			checkBox.ChangeVisualState();

			if ((bool)newValue && checkBox.CheckedCommand != null && checkBox.CheckedCommand.CanExecute(checkBox.CheckedCommandParameter))
			{
				checkBox.CheckedCommand?.Execute(checkBox.CheckedCommandParameter);
			}
			else if (checkBox.UnCheckedCommand != null && checkBox.UnCheckedCommand.CanExecute(checkBox.CheckedCommandParameter))
			{
				checkBox.UnCheckedCommand?.Execute(checkBox.CheckedCommandParameter);
			}

		}, defaultBindingMode: BindingMode.TwoWay);

		public static readonly BindableProperty ColorProperty = ColorElement.ColorProperty;

		public static readonly BindableProperty CheckedCommandProperty = BindableProperty.Create(nameof(CheckedCommand), typeof(ICommand), typeof(CheckBox));
		public static readonly BindableProperty UnCheckedCommandProperty = BindableProperty.Create(nameof(UnCheckedCommand), typeof(ICommand), typeof(CheckBox));
		public static readonly BindableProperty CheckedCommandParameterProperty = BindableProperty.Create(nameof(CheckedCommandParameter), typeof(ICommand), typeof(CheckBox));

		public Color Color
		{
			get { return (Color)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}

		readonly Lazy<PlatformConfigurationRegistry<CheckBox>> _platformConfigurationRegistry;

		public CheckBox()
		{
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<CheckBox>>(() => new PlatformConfigurationRegistry<CheckBox>(this));
		}

		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set
			{
				SetValue(IsCheckedProperty, value);
				ChangeVisualState();
			}
		}

		public ICommand CheckedCommand
		{
			get => GetValue(CheckedCommandProperty) as ICommand;
			set => SetValue(CheckedCommandProperty, value);
		}

		public ICommand UnCheckedCommand
		{
			get => GetValue(UnCheckedCommandProperty) as ICommand;
			set => SetValue(UnCheckedCommandProperty, value);
		}

		public object CheckedCommandParameter
		{
			get => GetValue(CheckedCommandParameterProperty);
			set => SetValue(CheckedCommandParameterProperty, value);
		}


		protected internal override void ChangeVisualState()
		{
			if (IsEnabled && IsChecked)
			{
				VisualStateManager.GoToState(this, IsCheckedVisualState);
			}
			else
			{
				base.ChangeVisualState();
			}
		}

		public event EventHandler<CheckedChangedEventArgs> CheckedChanged;



		public IPlatformElementConfiguration<T, CheckBox> On<T>() where T : IConfigPlatform
		{
			return _platformConfigurationRegistry.Value.On<T>();
		}

		void IBorderElement.OnBorderColorPropertyChanged(Color oldValue, Color newValue)
		{
		}


		Color IBorderElement.BorderColor => Color.Transparent;

		int IBorderElement.CornerRadius => 0;

		double IBorderElement.BorderWidth => 0;

		int IBorderElement.CornerRadiusDefaultValue => 0;

		Color IBorderElement.BorderColorDefaultValue => Color.Transparent;

		double IBorderElement.BorderWidthDefaultValue => 0;

		bool IBorderElement.IsCornerRadiusSet() => false;
		bool IBorderElement.IsBackgroundColorSet() => IsSet(BackgroundColorProperty);
		bool IBorderElement.IsBorderColorSet() => false;
		bool IBorderElement.IsBorderWidthSet() => false;
	}
}