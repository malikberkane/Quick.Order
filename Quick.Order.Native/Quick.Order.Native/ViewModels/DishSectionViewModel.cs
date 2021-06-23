using Quick.Order.AppCore.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Quick.Order.Native.ViewModels
{
    public class DishSectionViewModel: ObservableCollection<Dish>, INotifyPropertyChanged
    {
        public string SectionName { get; set; }
    }


}

