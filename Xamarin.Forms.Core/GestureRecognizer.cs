using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	public class GestureRecognizer : Element, IGestureRecognizer
	{
		internal GestureRecognizer()
		{
		}

		public static BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(GestureRecognizer), true);

		public bool IsEnabled
		{
			get => (bool)GetValue(IsEnabledProperty);
			set => SetValue(IsEnabledProperty, value);
		}
	}
}