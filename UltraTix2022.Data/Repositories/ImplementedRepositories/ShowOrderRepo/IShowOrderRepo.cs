using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderRepo
{
    public interface IShowOrderRepo : IRepository<ShowOrder>
    {
        public Task<List<ShowOrderRequestViewModel>> GetShowOrdersByCustomerId(Guid CustomerId);
        public Task<List<ShowOrderRequestViewModel>> GetShowOrders( );
        public Task<List<ShowOrderRequestViewModel>> GetShowOrdersByOrganizerId(Guid OrganizerId);
        public Task<List<ShowOrderRequestViewModel>> GetShowOrdersByArtistId(Guid ArtistId);
        public Task<bool> UpdateShowOrderStatusAfterScanned(Guid orderId);
    }
}
