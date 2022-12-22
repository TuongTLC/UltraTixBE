using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.ShowReview;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowReview;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.TicketType;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowService
{
    public interface IShowService
    {
        public Task<Guid?> InsertShow(string token, ShowRequestInsertModel show);
        public Task<Guid?> InsertShowSaleStage(string token, ShowSaleStagesRequestInsertModel saleStage);
        public Task<Guid?> InsertShowCampaign(string token, ShowCampaignsRequestInsertModel campaign);
        public Task<Guid?> UpdateShow(string token, ShowRequestUpdateModel show);
        public Task<Guid?> UpdateShowSaleStage(string token, ShowSaleStagesRequestUpdateModel saleStage);
        public Task<Guid?> UpdateShowCampaign(string token, ShowCampaignsRequestUpdateModel campaign);
        public Task<List<ShowViewModel>> GetPublicShow();
        public Task<List<ShowViewModel>> GetShowsByType(string type);
        public Task<List<ShowViewModel>> GetShowsByLocation(string token, string location);
        public Task<ShowDetailViewModel> GetShowDetailByIdByManager(string token, Guid showID);
        public Task<ShowDetailViewModel> GetShowDetailByIdForThirdParty(Guid showID);
        public Task<bool> UpdateShowStatus(string token, Guid showID, string status);
        //public Task<bool> UpdateShow(string token, ShowRequestUpdateModel showUpdateModel);
        public Task<List<ShowViewModel>> GetShowsByOrganizer(string token);
        public Task<List<ShowViewModel>> GetShowsByOrganizerID(string token, Guid id);
        public Task<List<ShowViewModel>> GetShowsCreatedByStaff(string token);
        public Task<List<ShowViewModelForArtist>> GetShowsByArtist(string token);
        public Task<ShowDetailViewModelForCustomer> GetShowDetailById(Guid showID, Guid? campaignId);
        public Task<List<ShowViewModel>> GetShows();
        public Task<List<ShowViewModel>> GetPopularShows();
        public Task<List<ShowViewModel>> GetNewestShows();
        public Task<List<ShowViewModel>> GetUpCommingShows();
        public Task<bool> AddShowReview(string token, ShowReviewInsertRequestModel showReview);
        public Task<List<ShowReviewViewModel>> GetShowReviewsForAdmin(string token);
        public Task<List<ShowReviewViewModel>> GetShowReviews();
        public Task<List<ShowReviewOverviewModel>> GetShowReviewsForOrganizer(string token, Guid showId);
        public Task<List<ShowReviewViewModel>> GetShowReviewsForOrganizer(string token);
        public Task<bool> GenerateCampaignBookingLink(string token, Guid showId);
        public Task<List<TicketTypeViewModel>> GetTicketTypesByShowId(string token, Guid showId);
        public Task<List<SaleStageViewModel>> GetSaleStagesByShowId(string token, Guid showId);
        public Task<List<CampaignViewModel>> GetCampaignByShowId(string token, Guid showId);
        public Task<ShowDetailViewModelForCustomerForMobile> GetShowDetailByIdForMobile(Guid showID);
        public Task<SaleStageViewModelForCustomer> GetSaleStageDetailsByIdForMobile(Guid showId);
        public Task<CampaignViewModelForCustomer> GetCampaignDetailsByIdForMobile(Guid showId, Guid? campaignId);
        public Task<Guid> GetOrganizerByShowId(Guid showId);
        public Task<bool> IsAdminTrasferShowRevenueToOrganizer(string token, Guid showID);
        public Task<int> GetTotalTicketArtistSell(Guid showId, string token);
        public Task<List<TicketTypeChartView>> GetTicketTypeChartViewsByShowID(string token, Guid ShowID);
        public Task<bool> IsTicketsBookedTempClose(ShowOrderRequestModel order);
        public Task<bool> ReOpenBookingOrder(ShowOrderRequestModel order);
        public Task<List<ShowOrdersOverviewModel>> GetShowOrdersOverviewByOrganizer(string token);
    }
}
