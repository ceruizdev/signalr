using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace signalrprotect{
    public class LogHub: Hub{
        public async Task SendMessage(string user, string message){
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}


    
