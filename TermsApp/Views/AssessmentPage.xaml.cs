using Microsoft.Maui.Controls;

namespace TermsApp
{
    public partial class AssessmentPage : ContentPage
    {
        public AssessmentPage()
        {
            //InitializeComponent();
            //TODO: entire file
        }

        private async void OnSave(object sender, EventArgs e)
        {
            await DisplayAlert("Save", "Assessment details saved!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Delete", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (confirm)
            {
                await Navigation.PopAsync();
            }
        }
    }
}
