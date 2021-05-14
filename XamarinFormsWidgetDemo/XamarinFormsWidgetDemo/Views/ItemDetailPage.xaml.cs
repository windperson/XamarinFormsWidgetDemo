using System.ComponentModel;
using Xamarin.Forms;
using XamarinFormsWidgetDemo.ViewModels;

namespace XamarinFormsWidgetDemo.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}