using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.TicketType;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.TicketType;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo
{
    public interface ITicketTypeRepo : IRepository<TicketType>
    {
        public Task<double> GetLowestPrice(Guid ShowID);
        public Task<string> GetNameByID(Guid ID);
        public Task<List<TicketTypeViewModel>> GetTicketTypesByShowID(Guid ShowID);
        //public Task<bool> UpdateTicketType(TicketTypeRequestUpdateModel ticketType);
        public Task<int> UpdateUnitSold(Guid ticketTypeID, int unitSold, bool isBuyViaLink);
        public Task<List<TicketTypeChartView>> GetTicketTypesForChartView(Guid ShowID);
        public Task<bool> RemoveTicketType(Guid ticketTypeID);
        public Task<int> ReOpenUnitSold(Guid ticketTypeID, int unitSold, bool isBuyViaLink);
    }
}
