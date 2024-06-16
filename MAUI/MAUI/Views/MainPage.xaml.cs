
using Microsoft.Maui.Controls;
using Microsoft.CSharp;

namespace MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new BlackJackViewModel();
        }
    }
}


