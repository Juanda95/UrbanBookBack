using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task CheckAndSendNotifications(int eventId);
    }
}
