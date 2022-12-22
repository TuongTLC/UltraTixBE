using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowTransaction;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.LocationRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowCategoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTransactionHistoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowTransactionService
{
    public class ShowTransactionService : IShowTransactionService
    {

        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;

        private readonly IShowOrderRepo _showOrderRepo;
        private readonly IShowOrderDetailRepo _showOrderDetailRepo;
        private readonly ICampaignDetailRepo _campaignDetailRepo;
        private readonly ITicketTypeRepo _ticketTypeRepo;
        private readonly IShowRepo _showRepo;
        private readonly ILocationRepo _locationRepo;
        private readonly ISaleStageRepo _saleStageRepo;
        private readonly ISaleStageDetailRepo _saleStageDetailRepo;
        private readonly ICampaignRepo _campaignRepo;
        private readonly IOrganizerRepo _organizerRepo;
        private readonly IShowCategoryRepo _showCategoryRepo;
        private readonly IShowTypeRepo _showTypeRepo;
        private readonly IUserRepo _userRepo;
        private readonly IShowTransactionHistoryRepo _showTransactionHistoryRepo;


        public ShowTransactionService(
           IShowOrderRepo showOrderRepo,
           IConfiguration configuration,
           IShowOrderDetailRepo showOrderDetailRepo,
           ICampaignDetailRepo campaignDetailRepo,
           ITicketTypeRepo ticketTypeRepo,
           IShowRepo showRepo,
           ILocationRepo locationRepo,
           ISaleStageRepo saleStageRepo,
           ISaleStageDetailRepo saleStageDetailRepo,
           ICampaignRepo campaignRepo,
           IOrganizerRepo organizerRepo,
           IShowCategoryRepo showCategoryRepo,
           IShowTypeRepo showTypeRepo,
           IUserRepo userRepo,
           IShowTransactionHistoryRepo showTransactionHistoryRepo
           )
        {
            _showOrderRepo = showOrderRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _showOrderDetailRepo = showOrderDetailRepo;
            _showRepo = showRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _locationRepo = locationRepo;
            _saleStageRepo = saleStageRepo;
            _saleStageDetailRepo = saleStageDetailRepo;
            _campaignRepo = campaignRepo;
            _campaignDetailRepo = campaignDetailRepo;
            _organizerRepo = organizerRepo;
            _showCategoryRepo = showCategoryRepo;
            _showTypeRepo = showTypeRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _userRepo = userRepo;
            _showTransactionHistoryRepo = showTransactionHistoryRepo;

        }

        public async Task<List<ShowTransactionRequestViewModel>> GetShowTransactions(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) && !roleID.Equals(Commons.ARTIST)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            var result = new List<ShowTransactionRequestViewModel>();

            switch (roleID)
            {
                case 0: // Admin
                    {
                        result = await _showTransactionHistoryRepo.GetShowTransactionsByAdmin();
                        foreach (var trans in result)
                        {
                            if (trans.CampaignId != null)
                            {
                                var artistName = await _campaignRepo.GetArtistNameByCampaignID(trans.CampaignId ?? new Guid());
                                trans.ArtistName = artistName;
                            }

                        }
                        break;
                    }
                case 1: // Organizer
                    {
                        result = await _showTransactionHistoryRepo.GetShowTransactionsByOrganizer(userID);
                        foreach (var trans in result)
                        {
                            if (trans.CampaignId != null)
                            {
                                var artistName = await _campaignRepo.GetArtistNameByCampaignID(trans.CampaignId ?? new Guid());
                                trans.ArtistName = artistName;
                            }

                        }
                        break;
                    }
                case 4: // Artist
                    {
                        result = await _showTransactionHistoryRepo.GetShowTransactionsByArtist(userID);
                        foreach (var trans in result)
                        {
                            if (trans.CampaignId != null)
                            {
                                var artistName = await _campaignRepo.GetArtistNameByCampaignID(trans.CampaignId ?? new Guid());
                                trans.ArtistName = artistName;
                            }
                        }
                        break;
                    }
            }

            return result;
        }

        public async Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByOrganizerID(string token, Guid organizerID)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var result = new List<ShowTransactionRequestViewModel>();

            result = await _showTransactionHistoryRepo.GetShowTransactionsByOrganizer(organizerID);
            foreach (var trans in result)
            {
                if (trans.CampaignId != null)
                {
                    var artistName = await _campaignRepo.GetArtistNameByCampaignID(trans.CampaignId ?? new Guid());
                    trans.ArtistName = artistName;
                }

            }

            return result;
        }

        public async Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByOrganizerForAdmin(string token, Guid organizerID)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                var result = new List<ShowTransactionRequestViewModel>();

                result = await _showTransactionHistoryRepo.GetShowTransactionsByOrganizer(organizerID);

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsForAdmin(string token)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                var result = new List<ShowTransactionRequestViewModel>();

                result = await _showTransactionHistoryRepo.GetShowTransactionsByAdmin();

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
