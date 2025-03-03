using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;



namespace TermsApp
{
	public partial class TermPage : ContentPage
	{
		public TermPage()
		{
			InitializeComponent();

            BindingContext = new TermViewModel();
        }

		private async void OnAddCourse(object sender, EventArgs e)
		{
            //await Navigation.PushModalAsync(new MainPage());
            await Navigation.PopModalAsync();
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnSave(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}