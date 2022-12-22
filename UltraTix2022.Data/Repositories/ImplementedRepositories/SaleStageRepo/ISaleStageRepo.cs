using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStage;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageRepo
{
    public interface ISaleStageRepo : IRepository<SaleStage>
    {
        public Task<string> GetNameByID(Guid ID);
        public Task<List<SaleStageViewModel>> GetSaleStagesByShowID(Guid ShowID);
        //public Task<bool> UpdateSaleStage(SaleStageRequestUpdateModel saleStage);
        public Task<bool> RemoveSaleStage(Guid saleStageId);
    }
}
