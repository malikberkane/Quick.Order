using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts
{
    public interface IEmailService
    {
        Task SendEmailForOrder(AppCore.Models.Order order);
    }
}
