using Microsoft.AspNetCore.SignalR;

namespace DATN.API.Services
{
    public class BookingHub:Hub
    {
        public async Task SendBookingNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveBookingNotification", message);
        }
    }
}
