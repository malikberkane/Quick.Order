using Quick.Order.AppCore.Models;
using Xamarin.Forms;
namespace Quick.Order.Native.Views
{
    public class MenuDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ValidTemplate { get; set; }
        public DataTemplate InvalidTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(item is Dish dish && dish.IsValid())
            {
                return ValidTemplate;
            }

            return InvalidTemplate;
        }
    }
}
