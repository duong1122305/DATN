using DATN.Data.EF;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class NotificationHub : Hub
{
    private readonly DATNDbContext _context;
    IConfiguration _configuration;

    public NotificationHub(DATNDbContext dATNDbContext, IConfiguration configuration)
    {
        _context = dATNDbContext;
        _configuration = configuration;
    }
    public async Task Notification(string message)
    {
        await base.OnConnectedAsync();
        var connectionString = _configuration.GetConnectionString("DATN");
        SqlDependency.Start(connectionString);
        var dependency = new SqlDependency(new SqlCommand("Select * from Bookings", (SqlConnection)_context.Database.GetDbConnection()));
        dependency.OnChange += async (sender, e) =>
        {
            await Clients.All.SendAsync("ReceiveMessage", "Server", message);
        };
    }
}