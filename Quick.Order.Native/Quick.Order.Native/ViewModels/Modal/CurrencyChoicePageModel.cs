using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class CurrencyChoicePageModel: ModalPageModelBase<object, Currency>
    {
        private readonly ICurrencyRepository currencyRepository;

        public ICommand SelectCurrencyCommand { get; set; }

        public CurrencyChoicePageModel(ICurrencyRepository currencyRepository) 
        {
            this.currencyRepository = currencyRepository;
            SelectCurrencyCommand = CreateAsyncCommand<Currency>(async (currency) => await SetResult(currency));

        }

        public List<Currency> Currencies { get; set; }

        public override async Task InitAsync()
        {
            var currencies = await currencyRepository.Get();

            Currencies = new List<Currency>(currencies);

        }
    }

    

    
}

