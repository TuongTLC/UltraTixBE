using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStageDetail;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageDetailRepo
{
    public interface ISaleStageDetailRepo : IRepository<SaleStageDetail>
    {
        //public Task<string> GetNameByID(Guid ID);
        public Task<List<SaleStageDetailViewModel>> GetSaleStageDetailBySaleStageID(Guid SaleStageID);      
        public Task<bool> UpdateSaleStageDetail(SaleStageDetailUpdateRequestModel saleStageDetail);
        public Task<int> UpdateUnitSold(Guid saleStageDetailId, int unitSold, bool isBuyViaLink);
        public Task<List<SaleStageDetail>> GetSaleStageDetailByTicketTypeId(Guid ticketTypeId);
        public Task<bool> RemoveSaleStageDetails(List<SaleStageDetail> saleStageDetails);
        public Task<List<SaleStageDetail>> GetSaleStageDetailBySaleStageId(Guid SaleStageID);
        public Task<int> ReOpenUnitSold(Guid saleStageDetailId, int unitSold, bool isBuyViaLink);
    }
}
