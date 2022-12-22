using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderDetailRepo
{
    public interface IShowOrderDetailRepo : IRepository<ShowOrderDetail>
    {
        public Task<List<ShowOrderDetailViewRequestModel>> GetShowOrdersDetailByShowOrderId(Guid showOrderId);
    }
}
