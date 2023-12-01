using Microsoft.AspNetCore.SignalR;

namespace RabbitMQ.SignaiR
{
    public class ExamHub : Hub
    {
        public async Task SendExamResultNotification(string userId, int score)
        {
            await Clients.User(userId).SendAsync("ReceiveExamResult", score);
        }
    }
}
