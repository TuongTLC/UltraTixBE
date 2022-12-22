using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRequestRepo;

namespace UltraTix2022.API.SignalR
{
    public interface ITestHubClient
    {
        Task BroadcastRequest(ShowRequestRepo request);
    }
}
