using DATN.Data.EF;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class NotificationHub : Hub
{
    private readonly DATNDbContext _context;
    IConfiguration _configuration;
    IHubContext<NotificationHub> _notification;
    public NotificationHub(DATNDbContext dATNDbContext, IConfiguration configuration, IHubContext<NotificationHub> notification)
    {
        _context = dATNDbContext;
        _configuration = configuration;
        _notification = notification;
    }
    public async Task Notification(string message)
    {
        await base.OnConnectedAsync();
        var connectionString = _configuration.GetConnectionString("DATN");
        var dependency = new SqlDependency(new SqlCommand("Select * from Bookings", (SqlConnection)_context.Database.GetDbConnection()));
        dependency.OnChange += (sender, e) =>
        {

        };
        await _notification.Clients.All.SendAsync("ReceiveMessage", "Server", message);

    }
}