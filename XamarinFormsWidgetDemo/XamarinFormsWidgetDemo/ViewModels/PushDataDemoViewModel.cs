using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsWidgetDemo.Services;

namespace XamarinFormsWidgetDemo.ViewModels
{
    public class PushDataDemoViewModel : BaseViewModel
    {
        private string text;

        public PushDataDemoViewModel()
        {
            Title = "Push Demo";
            PushUpdateCommand = new Command(WidgetUpdate);

        }

        private void WidgetUpdate()
        {
            var dependencyService = DependencyService.Get<IUpdateWidget>();

            dependencyService?.PushUpdate(Text);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public ICommand PushUpdateCommand { get; }
    }
}
