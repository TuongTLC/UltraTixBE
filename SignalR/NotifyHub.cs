using Microsoft.AspNetCore.SignalR;

namespace UltraTix2022.API.SignalR
{
    public class NotifyHub : Hub
    {
        public async Task SendMsg(string req)
        {
            await Clients.All.SendAsync("ReceiveRequest", req);
        }

        public void ReponseShowRequest(string response)
        {
            Clients.All.SendAsync("Receive RequestReponseData", response);
        }
    }
}
