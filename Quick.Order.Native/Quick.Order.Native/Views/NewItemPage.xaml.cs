using Quick.Order.Native.Models;
using Quick.Order.Native.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = FreshMvvm.FreshIOC.Container.Resolve<NewItemViewModel>();
        }
    }
}