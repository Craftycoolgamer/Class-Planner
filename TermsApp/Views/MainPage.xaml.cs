using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TermsApp
{ 
    public partial class MainPage : ContentPage
    {
        
        
        public MainPage()
        {
            InitializeComponent();
            
            BindingContext = new MainViewModel();
        }


        private void OnAddTerm(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TermPage());
        }

        private void OnTermClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TermPage());
        }

    }

}
 