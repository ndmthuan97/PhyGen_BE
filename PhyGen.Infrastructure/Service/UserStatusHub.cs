using Microsoft.AspNetCore.SignalR;

public class UserStatusHub : Hub
{
    public Task Join(string userId)
        => Groups.AddToGroupAsync(Context.ConnectionId, userId);
}
