using Newtonsoft.Json;
using System.Text;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.DynamicLink;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.ShowReview;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.CampaignDetail;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowReview;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.TicketType;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.LocationRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowCategoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowReviewRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.StaffShowDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowService
{
    public class ShowService : IShowService
    {
        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;

        private readonly IShowRepo _showRepo;
        private readonly ILocationRepo _locationRepo;
        private readonly ISaleStageRepo _saleStageRepo;
        private readonly ISaleStageDetailRepo _saleStageDetailRepo;
        private readonly ITicketTypeRepo _ticketTypeRepo;
        private readonly ICampaignRepo _campaignRepo;
        private readonly ICampaignDetailRepo _campaignDetailRepo;
        private readonly IOrganizerRepo _organizerRepo;
        private readonly IStaffShowDetailRepo _staffShowDetailRepo;
        private readonly IUserRepo _userRepo;
        private readonly IShowCategoryRepo _showCategoryRepo;
        private readonly IShowTypeRepo _showTypeRepo;
        private readonly IShowReviewRepo _showReviewRepo;


        public ShowService(
           IShowRepo showRepo,
           ILocationRepo locationRepo,
           ISaleStageRepo saleStageRepo,
           ISaleStageDetailRepo saleStageDetailRepo,
           ITicketTypeRepo ticketTypeRepo,
           ICampaignRepo campaignRepo,
           ICampaignDetailRepo campaignDetailRepo,
           IConfiguration configuration,
           IOrganizerRepo organizerRepo,
           IStaffShowDetailRepo staffShowDetailRepo,
           IUserRepo userRepo,
           IShowCategoryRepo showCategoryRepo,
           IShowTypeRepo showTypeRepo,
           IShowReviewRepo showReviewRepo
           )
        {
            _showRepo = showRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _locationRepo = locationRepo;
            _saleStageRepo = saleStageRepo;
            _saleStageDetailRepo = saleStageDetailRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _campaignRepo = campaignRepo;
            _campaignDetailRepo = campaignDetailRepo;
            _organizerRepo = organizerRepo;
            _staffShowDetailRepo = staffShowDetailRepo;
            _userRepo = userRepo;
            _showCategoryRepo = showCategoryRepo;
            _showTypeRepo = showTypeRepo;
            _showReviewRepo = showReviewRepo;
        }

        public async Task<List<ShowViewModel>> GetPublicShow()
        {
            try
            {
                var publicShows = await _showRepo.GetShowsByStatus(Commons.PUBLIC);
                foreach (var publicShow in publicShows)
                {
                    var lowestPrice = await _ticketTypeRepo.GetLowestPrice(publicShow.Id);
                    publicShow.Category = await _showCategoryRepo.GetCategoryNameByID(publicShow.CategoryID ?? new Guid());
                    publicShow.LowestPrice = lowestPrice;
                    publicShow.StartDate = publicShow.ShowStartDate.ToShortDateString() + " " + publicShow.ShowStartDate.ToShortTimeString();
                    publicShow.EndDate = publicShow.ShowEndDate.ToShortDateString() + " " + publicShow.ShowEndDate.ToShortTimeString();
                    publicShow.IsShowComming = publicShow.ShowStartDate.CompareTo(DateTime.Now) > 0;
                    publicShow.IsShowHappening = publicShow.ShowStartDate.CompareTo(DateTime.Now) <= 0;
                    var saleStage = await GetShowDetailById(publicShow.Id, null);
                    if(saleStage.SaleStageView != null)
                    {
                        publicShow.SaleOff = saleStage.SaleStageView.SaleStageDiscount;
                    }            
                }
                return publicShows;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<ShowDetailViewModel> GetShowDetailByIdByManager(string token, Guid showID)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
                Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                var ShowEntity = await _showRepo.Get(showID);
                if (ShowEntity == null) throw new ArgumentException("Show Not Found");

                ShowDetailViewModel result = new();

                result.Id = ShowEntity.Id;
                result.Status = ShowEntity.Status;
                result.ShowName = ShowEntity.ShowName;
                result.ShowDescriptionDetail = ShowEntity.ShowDescription ?? "";
                result.ImgUrl = ShowEntity.ImgUrl;
                result.DescriptionImageUrl = ShowEntity.DescriptionImageUrl ?? "";
                result.OrganizerID = ShowEntity.ShowOrganizerId;
                result.Status = ShowEntity.Status;
                result.ShowStartDate = ShowEntity.ShowStartDate;
                result.ShowEndDate = ShowEntity.ShowEndDate;
                result.ShowTypeId = ShowEntity.ShowTypeId;
                result.ShowTypeName = await _showTypeRepo.GetShowTypeByID(result.ShowTypeId) ?? string.Empty;
                result.OrganizerName = await _userRepo.GetNameByID(ShowEntity.ShowOrganizerId);
                result.CategoryID = ShowEntity.CategoryId;
                result.Category = await _showCategoryRepo.GetCategoryNameByID(result.CategoryID ?? new Guid()) ?? string.Empty;
                result.StartDate = ShowEntity.ShowStartDate.ToShortDateString() + " " + ShowEntity.ShowStartDate.ToShortTimeString();
                result.EndDate = ShowEntity.ShowEndDate.ToShortDateString() + " " + ShowEntity.ShowEndDate.ToShortTimeString();

                //Location View
                var location = await _locationRepo.GetLocationByShowID(ShowEntity.Id);
                if (location == null) throw new ArgumentException("Location is NULL");

                result.LocationView.Id = location.Id;
                result.LocationView.LocationDescription = location.LocationDescription;
                result.LocationView.ShowId = location.ShowId;

                //Ticket Type
                var ticketTypeEnts = await _ticketTypeRepo.GetTicketTypesByShowID(ShowEntity.Id);
                if (ticketTypeEnts == null) throw new ArgumentException("TicketType is NULL");

                foreach (var ticketTypeEnt in ticketTypeEnts)
                {
                    TicketTypeViewModel ticketTypeView = new()
                    {
                        Id = ticketTypeEnt.Id,
                        TicketTypeName = ticketTypeEnt.TicketTypeName,
                        TicketTypeDescription = ticketTypeEnt.TicketTypeDescription,
                        OriginalPrice = ticketTypeEnt.OriginalPrice,
                        Quantity = ticketTypeEnt.Quantity,
                        TicketTypeDiscount = ticketTypeEnt.TicketTypeDiscount,
                        ShowId = ticketTypeEnt.ShowId
                    };

                    result.TicketTypeViews.Add(ticketTypeView);
                }

                //Sale Stage
                var saleStageEnts = await _saleStageRepo.GetSaleStagesByShowID(ShowEntity.Id);
                if (saleStageEnts == null) throw new ArgumentException("SaleStage is NULL");
                foreach (var saleStageEnt in saleStageEnts)
                {
                    SaleStageViewModel saleStageView = new()
                    {
                        Id = saleStageEnt.Id,
                        SaleStageOrder = saleStageEnt.SaleStageOrder,
                        SaleStageDescription = saleStageEnt.SaleStageDescription,
                        SaleStageStartDate = saleStageEnt.SaleStageStartDate,
                        SaleStageEndDate = saleStageEnt.SaleStageEndDate,
                        SaleStageDiscount = saleStageEnt.SaleStageDiscount,
                        ShowId = saleStageEnt.ShowId,
                        startDate = saleStageEnt.startDate,
                        endDate = saleStageEnt.endDate
                    };

                    var SaleStageDetailList = await _saleStageDetailRepo.GetSaleStageDetailBySaleStageID(saleStageView.Id);

                    foreach (var saleStageDetail in SaleStageDetailList)
                    {
                        saleStageDetail.SaleStageName = await _saleStageRepo.GetNameByID(saleStageDetail.SaleStageId);
                        saleStageDetail.TicketTypeName = await _ticketTypeRepo.GetNameByID(saleStageDetail.TicketTypeId);
                        saleStageView.SaleStageDetailViewModels.Add(saleStageDetail);
                    }

                    result.SaleStageViews.Add(saleStageView);
                }

                //Campaign
                var campaignEnts = await _campaignRepo.GetCampaignsByShowID(ShowEntity.Id);
                if (campaignEnts == null) throw new ArgumentException("Campaign is NULL");
                foreach (var campaignEnt in campaignEnts)
                {
                    CampaignViewModel campView = new()
                    {
                        Id = campaignEnt.Id,
                        ArtistId = campaignEnt.ArtistId,
                        ShowId = campaignEnt.ShowId,
                        MaxDiscount = campaignEnt.MaxDiscount,
                        MinDiscount = campaignEnt.MinDiscount,
                        BookingLink = campaignEnt.BookingLink
                    };

                    //Campaign Detail
                    var campDetailEnts = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(campaignEnt.Id);
                    if (campDetailEnts == null) throw new ArgumentException("CampaignDetail is NULL");
                    foreach (var campDetEnt in campDetailEnts)
                    {
                        CampaignDetailViewModel campgnDetailView = new()
                        {
                            Id = campDetEnt.Id,
                            ArtistDiscount = campDetEnt.ArtistDiscount,
                            CampaignDescription = campDetEnt.CampaignDescription,
                            CampaignName = campDetEnt.CampaignName,
                            CampaignStartDate = campDetEnt.CampaignStartDate,
                            CampaignEndDate = campDetEnt.CampaignEndDate,
                            CampaignId = campDetEnt.CampaignId,
                            TicketTypeQuantity = campDetEnt.TicketTypeQuantity,
                            TicketTypeSold = campDetEnt.TicketTypeSold,
                            SaleStageDetailId = campDetEnt.SaleStageDetailId,
                            CustomerDiscount = campDetEnt.CustomerDiscount,
                        };

                        var stageDel = await _saleStageDetailRepo.Get(campgnDetailView.SaleStageDetailId);

                        campgnDetailView.TicketTypeId = stageDel.TicketTypeId;
                        campgnDetailView.SaleStageId = stageDel.SaleStageId;
                        campgnDetailView.TicketTypeName = await _ticketTypeRepo.GetNameByID(campgnDetailView.TicketTypeId);
                        campgnDetailView.SaleStageName = await _saleStageRepo.GetNameByID(campgnDetailView.SaleStageId);

                        campView.CampaignDetails.Add(campgnDetailView);
                    }

                    result.CampaignViews.Add(campView);

                }

                if (roleID == Commons.ARTIST)
                {
                    var campainsJoined = new List<CampaignViewModel>();

                    foreach (var campaign in result.CampaignViews)
                    {
                        if (campaign.ArtistId.Equals(userID))
                        {
                            campainsJoined.Add(campaign);
                        }
                    }
                    result.CampaignViews = campainsJoined;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModel>> GetShowsByOrganizer(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            try
            {
                var result = await _showRepo.GetAllShows();

                result = result.Where(s => s.OrganizerID == userID).ToList();
                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowOrdersOverviewModel>> GetShowOrdersOverviewByOrganizer(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            try
            {
                var result = await _showRepo.GetAllShows();
                result = result.Where(s => s.OrganizerID == userID).ToList();
                List<ShowOrdersOverviewModel> shows = new();
                foreach(var show in result)
                {
                    ShowOrdersOverviewModel showView = new();
                    showView.Id = show.Id;
                    showView.ShowName = show.ShowName;
                    showView.ShowStartDate = show.ShowStartDate;
                    showView.ShowEndDate = show.ShowEndDate;
                    showView.ImgUrl = show.ImgUrl;
                    showView.Status = show.Status;
                    shows.Add(showView);
                }

                return shows;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModelForArtist>> GetShowsByArtist(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ARTIST)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            try
            {
                var result = await _showRepo.GetShowsJoinedForArtist(userID);
                List<ShowViewModelForArtist> showJoined = new();
                foreach(var show in result)
                {
                    ShowViewModelForArtist showArtistJoined = new()
                    {
                        Id = show.Id,
                        ShowName = show.ShowName,
                        ShowDescription = show.ShowDescription,
                        ShowDescriptionDetail = show.ShowDescriptionDetail,
                        ShowStartDate= show.ShowStartDate,
                        ShowEndDate  = show.ShowEndDate,
                        ShowTypeId = show.ShowTypeId,
                        ShowTypeName = show.ShowTypeName,
                        OrganizerName = show.OrganizerName,
                        OrganizerID  = show.OrganizerID,
                        ImgUrl = show.ImgUrl,
                        Status = show.Status,
                        DescriptionImageUrl = show.DescriptionImageUrl,
                        LowestPrice = show.LowestPrice,
                        Location = show.Location,
                        StartDate = show.StartDate,
                        EndDate = show.EndDate,
                        CategoryID = show.CategoryID,
                        Category = show.Category,
                        Step = show.Step,
                        CreationDate = show.CreationDate,
                        IsShowHappening = show.IsShowHappening,
                        IsShowComming = show.IsShowComming,
                    };

                    var showDetail = await GetShowDetailByIdByManager(token, show.Id);
                    var link = showDetail.CampaignViews.First().BookingLink ?? "";
                    showArtistJoined.BookingLink = link;
                    showJoined.Add(showArtistJoined);
                }
                return showJoined;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModel>> GetShowsCreatedByStaff(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.STAFF)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            try
            {
                var listShowID = await _staffShowDetailRepo.GetListShowIDByStaffID(userID);

                var result = new List<ShowViewModel>();

                var shows = await _showRepo.GetAllShows();

                foreach (var showID in listShowID)
                {
                    var show = shows.Where(s => s.Id == showID).First();
                    var creationStep = await _showRepo.GetShowCreationStep(show.Id);
                    show.Step = creationStep;
                    var creationDate = await _showRepo.GetShowCreationDate(show.Id);
                    show.CreationDate = creationDate;
                    result.Add(show);
                }
                result = result.OrderByDescending(x => x.CreationDate).ToList();

                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModel>> GetShowsByType(string type)
        {
            try
            {
                var result = await _showRepo.GetShowsByType(type);

                foreach (var show in result)
                {
                    show.LowestPrice = await _ticketTypeRepo.GetLowestPrice(show.Id);
                    show.StartDate = show.ShowStartDate.ToShortDateString() + " " + show.ShowStartDate.ToShortTimeString();
                    show.EndDate = show.ShowEndDate.ToShortDateString() + " " + show.ShowEndDate.ToShortTimeString();
                    show.Category = await _showCategoryRepo.GetCategoryNameByID(show.CategoryID ?? new Guid()) ?? "Live campaignUpdateModel";
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Guid?> InsertShow(string token, ShowRequestInsertModel show)
        {
            if (show == null)
            {
                throw new ArgumentNullException("Show Is Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid staffID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            if (!roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            try
            {
                if (show != null)
                {
                    //Get organizer manage staff who create this campaignUpdateModel
                    var organizer = await _organizerRepo.GetOrganizerbyStaffID(staffID);
                    var organizerID = organizer.ID;

                    //Mapping campaignUpdateModel 
                    Show showEntity = new()
                    {
                        Id = Guid.NewGuid(),
                        ShowTypeId = show.ShowType,
                        ShowName = show.ShowName,
                        ShowDescription = show.ShowDescription,
                        ShowDetail = show.ShowDescriptionDetail,
                        Status = Commons.DRAFT,
                        ShowStartDate = show.ShowStartDate,
                        ShowEndDate = show.ShowEndDate,
                        DescriptionImageUrl = show.DescriptionImageUrl,
                        ImgUrl = show.ImgUrl,
                        ShowOrganizerId = organizerID,
                        CategoryId = show.CategoryID
                    };

                    //Mapping location
                    Location location = new()
                    {
                        Id = Guid.NewGuid(),
                        ShowId = showEntity.Id,
                        LocationDescription = show.Location.LocationDescription
                    };

                    showEntity.Locations.Add(location);

                    if (show.TicketTypes != null)
                    {
                        //Mapping ticket type
                        foreach (var ticketTypeInsert in show.TicketTypes)
                        {
                            TicketType ticketTypeEntity = new()
                            {
                                Id = Guid.NewGuid(),
                                ShowId = showEntity.Id,
                                Quantity = ticketTypeInsert.Quantity,
                                OriginalPrice = ticketTypeInsert.OriginalPrice,
                                TicketTypeDescription = ticketTypeInsert.TicketTypeDescription,
                                TicketTypeName = ticketTypeInsert.TicketTypeName,
                                TicketTypeDiscount = ticketTypeInsert.TicketTypeDiscount,
                                UnitSold = 0
                            };

                            //Prepare for campaign detail
                            showEntity.TicketTypes.Add(ticketTypeEntity);
                        }
                    }

                    //Mapping staff-campaignUpdateModel-detail: this campaignUpdateModel was created by which staff
                    StaffShowDetail staffShowDetail = new()
                    {
                        Id = Guid.NewGuid(),
                        ShowId = showEntity.Id,
                        ShowStaffId = staffID
                    };

                    showEntity.StaffShowDetails.Add(staffShowDetail);
                    showEntity.Step = 1;
                    showEntity.CreationDate = DateTime.Now;
                    //Add campaignUpdateModel to DB
                    await _showRepo.Insert(showEntity);

                    return showEntity.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        public async Task<bool> UpdateShowStatus(string token, Guid showID, string status)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0) && !roleID.Equals(1) && !roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            try
            {
                if (await _showRepo.UpdateShowStatus(showID, status))
                {
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModel>> GetShowsByLocation(string token, string location)
        {
            try
            {
                var publicShows = await _showRepo.GetShowsByLocation(location);
                foreach (var publicShow in publicShows)
                {
                    var lowestPrice = await _ticketTypeRepo.GetLowestPrice(publicShow.Id);
                    publicShow.Category = await _showCategoryRepo.GetCategoryNameByID(publicShow.CategoryID ?? new Guid());
                    publicShow.LowestPrice = lowestPrice;
                    publicShow.StartDate = publicShow.ShowStartDate.ToShortDateString() + " " + publicShow.ShowStartDate.ToShortTimeString();
                    publicShow.EndDate = publicShow.ShowEndDate.ToShortDateString() + " " + publicShow.ShowEndDate.ToShortTimeString();
                }
                return publicShows;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModel>> GetShows()
        {
            try
            {
                var publicShows = await _showRepo.GetAllShows();
                foreach (var publicShow in publicShows)
                {
                    var lowestPrice = await _ticketTypeRepo.GetLowestPrice(publicShow.Id);
                    publicShow.Category = await _showCategoryRepo.GetCategoryNameByID(publicShow.CategoryID ?? new Guid());
                    publicShow.LowestPrice = lowestPrice;
                    publicShow.StartDate = publicShow.ShowStartDate.ToShortDateString() + " " + publicShow.ShowStartDate.ToShortTimeString();
                    publicShow.EndDate = publicShow.ShowEndDate.ToShortDateString() + " " + publicShow.ShowEndDate.ToShortTimeString();
                }
                return publicShows;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<ShowDetailViewModelForCustomer> GetShowDetailById(Guid showID, Guid? campaignId)
        {
            try
            {
                var ShowEntity = await _showRepo.Get(showID);
                if (ShowEntity == null) throw new ArgumentException("Show Not Found");

                var organizer = await _userRepo.Get(ShowEntity.ShowOrganizerId);

                ShowDetailViewModelForCustomer result = new();

                result.Id = ShowEntity.Id;
                result.Status = ShowEntity.Status;
                result.ShowName = ShowEntity.ShowName;
                result.ShowDescription = ShowEntity.ShowDescription ?? "";
                result.ShowDescriptionDetail = ShowEntity.ShowDetail ?? "";
                result.ImgUrl = ShowEntity.ImgUrl;
                result.DescriptionImageUrl = ShowEntity.DescriptionImageUrl ?? "";
                result.OrganizerID = ShowEntity.ShowOrganizerId;
                result.Status = ShowEntity.Status;
                result.ShowStartDate = ShowEntity.ShowStartDate;
                result.ShowEndDate = ShowEntity.ShowEndDate;
                result.ShowTypeId = ShowEntity.ShowTypeId;
                result.ShowTypeName = await _showTypeRepo.GetShowTypeByID(result.ShowTypeId) ?? string.Empty;
                result.OrganizerName = organizer.FullName ?? string.Empty;
                result.OrganizerImageUrl = organizer.AvatarImageUrl ?? string.Empty;
                result.CategoryID = ShowEntity.CategoryId;
                result.Category = await _showCategoryRepo.GetCategoryNameByID(result.CategoryID ?? new Guid()) ?? string.Empty;

                //Location View
                var location = await _locationRepo.GetLocationByShowID(ShowEntity.Id);
                if (location == null) throw new ArgumentException("Location is NULL");

                result.LocationView.Id = location.Id;
                result.LocationView.LocationDescription = location.LocationDescription;
                result.LocationView.ShowId = location.ShowId;

                //Sale Stage
                var saleStageEnts = await _saleStageRepo.GetSaleStagesByShowID(ShowEntity.Id);

                if (saleStageEnts == null) throw new ArgumentException("SaleStages is NULL");

                var saleStageEnt = saleStageEnts.Where(x => DateTime.Compare(DateTime.Now, x.SaleStageStartDate) >= 0
                        && DateTime.Compare(DateTime.Now, x.SaleStageEndDate) <= 0).FirstOrDefault();

                if (saleStageEnt != null)
                {
                    SaleStageViewModelForCustomer saleStageView = new()
                    {
                        Id = saleStageEnt.Id,
                        SaleStageDiscount = saleStageEnt.SaleStageDiscount,
                        SaleStageStartDate = saleStageEnt.SaleStageStartDate,
                        SaleStageEndDate = saleStageEnt.SaleStageEndDate
                    };

                    var SaleStageDetailList = await _saleStageDetailRepo.GetSaleStageDetailBySaleStageID(saleStageView.Id);

                    foreach (var saleStageDetail in SaleStageDetailList)
                    {
                        saleStageDetail.TicketTypeName = await _ticketTypeRepo.GetNameByID(saleStageDetail.TicketTypeId);

                        var ticketType = await _ticketTypeRepo.Get(saleStageDetail.TicketTypeId);
                        var ticketsSoldViaCampaign = await _campaignDetailRepo.GetTotalTicketsBySaleStageDetailId(saleStageDetail.Id);
                        var saleStageDetailEnt = await _saleStageDetailRepo.Get(saleStageDetail.Id);

                        if(ticketType != null && saleStageDetailEnt != null)
                        {
                            var saleStageDetailView = new SaleStageDetailViewModelForCustomer()
                            {
                                Id = saleStageDetail.Id,
                                TicketTypeId = saleStageDetail.TicketTypeId,
                                TicketTypeName = saleStageDetail.TicketTypeName,
                                TicketTypeDiscount = ticketType.TicketTypeDiscount,
                                TicketTypeQuantity = saleStageDetail.TicketTypeQuantity,
                                TicketTypeLeft = saleStageDetail.TicketTypeQuantity - ticketsSoldViaCampaign - (saleStageDetailEnt.TicketTypeNormalUnitSold ?? 0),
                                OriginalPrice = ticketType.OriginalPrice
                            };

                            saleStageView.SaleStageDetailViewModels.Add(saleStageDetailView);
                        }               
                    }
                    result.SaleStageView = saleStageView;
                }

                if (campaignId != null)
                {
                    var campaignEnts = await _campaignRepo.GetCampaignsByShowID(ShowEntity.Id);
                    var campaignEnt = campaignEnts.Where(x => x.Id.Equals(campaignId)).First();
                    if (campaignEnts == null || campaignEnt == null) throw new ArgumentException("Campaign is NULL");

                    CampaignViewModelForCustomer campView = new()
                    {
                        Id = campaignEnt.Id,
                        ArtistId = campaignEnt.ArtistId,
                        ArtistName = await _userRepo.GetNameByID(campaignEnt.ArtistId),
                    };

                    //Campaign Detail
                    var campDetailEnts = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(campaignEnt.Id);
                    if (campDetailEnts == null) throw new ArgumentException("CampaignDetail is NULL");

                    foreach (var campDetEnt in campDetailEnts)
                    {
                        CampaignDetailViewModelForCustomer campgnDetailView = new()
                        {
                            Id = campDetEnt.Id,
                            TicketTypeQuantity = campDetEnt.TicketTypeQuantity,
                            TicketTypeLeft = campDetEnt.TicketTypeQuantity - campDetEnt.TicketTypeSold,
                            SaleStageDetailId = campDetEnt.SaleStageDetailId,
                            CustomerDiscount = campDetEnt.CustomerDiscount,
                        };

                        var stageDel = await _saleStageDetailRepo.Get(campgnDetailView.SaleStageDetailId);

                        if(stageDel != null)
                        {
                            campgnDetailView.TicketTypeId = stageDel.TicketTypeId;
                            campgnDetailView.SaleStageId = stageDel.SaleStageId;
                            campgnDetailView.TicketTypeName = await _ticketTypeRepo.GetNameByID(campgnDetailView.TicketTypeId);
                            campgnDetailView.SaleStageName = await _saleStageRepo.GetNameByID(campgnDetailView.SaleStageId);

                            campView.CampaignDetails.Add(campgnDetailView);
                        }
                        
                    }

                    //Mapping ticketTypeLeft from compaigndetail to newCampaignDetail
                    if (result.SaleStageView != null && result.SaleStageView.SaleStageDetailViewModels != null 
                        && campView != null && campView.CampaignDetails != null)
                    {
                        foreach (var saleStageDetail in result.SaleStageView.SaleStageDetailViewModels)
                        {
                            foreach(var campDetail in campView.CampaignDetails)
                            {
                                if (saleStageDetail.Id.Equals(campDetail.SaleStageDetailId))
                                {
                                    saleStageDetail.TicketTypeQuantity = campDetail.TicketTypeQuantity;
                                    saleStageDetail.TicketTypeLeft = campDetail.TicketTypeLeft;
                                }
                            }
                        }
                    }                 

                    result.CampaignView = campView;
                }

                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<ShowDetailViewModelForCustomerForMobile> GetShowDetailByIdForMobile(Guid showID)
        {
            try
            {
                var ShowEntity = await _showRepo.Get(showID);
                if (ShowEntity == null) throw new ArgumentException("Show Not Found");

                var organizer = await _userRepo.Get(ShowEntity.ShowOrganizerId);

                ShowDetailViewModelForCustomerForMobile result = new();

                result.Id = ShowEntity.Id;
                result.Status = ShowEntity.Status;
                result.ShowName = ShowEntity.ShowName;
                result.ShowDescription = ShowEntity.ShowDescription ?? "";
                result.ShowDescriptionDetail = ShowEntity.ShowDetail ?? "";
                result.ImgUrl = ShowEntity.ImgUrl;
                result.DescriptionImageUrl = ShowEntity.DescriptionImageUrl ?? "";
                result.OrganizerID = ShowEntity.ShowOrganizerId;
                result.Status = ShowEntity.Status;
                result.ShowStartDate = ShowEntity.ShowStartDate;
                result.ShowEndDate = ShowEntity.ShowEndDate;
                result.ShowTypeId = ShowEntity.ShowTypeId;
                result.ShowTypeName = await _showTypeRepo.GetShowTypeByID(result.ShowTypeId) ?? string.Empty;
                result.OrganizerName = organizer.FullName ?? string.Empty;
                result.OrganizerImageUrl = organizer.AvatarImageUrl ?? string.Empty;
                result.CategoryID = ShowEntity.CategoryId;
                result.Category = await _showCategoryRepo.GetCategoryNameByID(result.CategoryID ?? new Guid()) ?? string.Empty;

                //Location View
                var location = await _locationRepo.GetLocationByShowID(ShowEntity.Id);
                if (location == null) throw new ArgumentException("Location is NULL");

                result.LocationView.Id = location.Id;
                result.LocationView.LocationDescription = location.LocationDescription;
                result.LocationView.ShowId = location.ShowId;

                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<SaleStageViewModelForCustomer> GetSaleStageDetailsByIdForMobile(Guid showId)
        {
            try
            {
                SaleStageViewModelForCustomer result = new();

                //Sale Stage
                var saleStageEnts = await _saleStageRepo.GetSaleStagesByShowID(showId);

                if (saleStageEnts == null) throw new ArgumentException("SaleStages is NULL");

                var saleStageEnt = saleStageEnts.Where(x => DateTime.Compare(DateTime.Now, x.SaleStageStartDate) >= 0
                        && DateTime.Compare(DateTime.Now, x.SaleStageEndDate) <= 0).FirstOrDefault();

                if (saleStageEnt != null)
                {
                    SaleStageViewModelForCustomer saleStageView = new()
                    {
                        Id = saleStageEnt.Id,
                        SaleStageDiscount = saleStageEnt.SaleStageDiscount,
                        SaleStageStartDate = saleStageEnt.SaleStageStartDate,
                        SaleStageEndDate = saleStageEnt.SaleStageEndDate
                    };

                    var SaleStageDetailList = await _saleStageDetailRepo.GetSaleStageDetailBySaleStageID(saleStageView.Id);

                    foreach (var saleStageDetail in SaleStageDetailList)
                    {
                        saleStageDetail.TicketTypeName = await _ticketTypeRepo.GetNameByID(saleStageDetail.TicketTypeId);

                        var ticketType = await _ticketTypeRepo.Get(saleStageDetail.TicketTypeId);
                        var ticketsSoldViaCampaign = await _campaignDetailRepo.GetTotalTicketsBySaleStageDetailId(saleStageDetail.Id);
                        var saleStageDetailEnt = await _saleStageDetailRepo.Get(saleStageDetail.Id);

                        var saleStageDetailView = new SaleStageDetailViewModelForCustomer()
                        {
                            Id = saleStageDetail.Id,
                            TicketTypeId = saleStageDetail.TicketTypeId,
                            TicketTypeName = saleStageDetail.TicketTypeName,
                            TicketTypeDiscount = ticketType.TicketTypeDiscount,
                            TicketTypeQuantity = saleStageDetail.TicketTypeQuantity,
                            TicketTypeLeft = saleStageDetail.TicketTypeQuantity - ticketsSoldViaCampaign - (saleStageDetailEnt.TicketTypeNormalUnitSold ?? 0),
                            OriginalPrice = ticketType.OriginalPrice
                        };

                        saleStageView.SaleStageDetailViewModels.Add(saleStageDetailView);

                    }

                    result = saleStageView;
                }

                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<CampaignViewModelForCustomer> GetCampaignDetailsByIdForMobile(Guid showId, Guid? campaignId)
        {
            try
            {
                CampaignViewModelForCustomer result = new();

                if (campaignId != null)
                {
                    var campaignEnts = await _campaignRepo.GetCampaignsByShowID(showId);
                    var campaignEnt = campaignEnts.Where(x => x.Id.Equals(campaignId)).First();
                    if (campaignEnts == null || campaignEnt == null) throw new ArgumentException("Campaign is NULL");

                    CampaignViewModelForCustomer campView = new()
                    {
                        Id = campaignEnt.Id,
                        ArtistId = campaignEnt.ArtistId,
                        ArtistName = await _userRepo.GetNameByID(campaignEnt.ArtistId),
                    };

                    //Campaign Detail
                    var campDetailEnts = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(campaignEnt.Id);
                    if (campDetailEnts == null) throw new ArgumentException("CampaignDetail is NULL");

                    foreach (var campDetEnt in campDetailEnts)
                    {
                        CampaignDetailViewModelForCustomer campgnDetailView = new()
                        {
                            Id = campDetEnt.Id,
                            TicketTypeQuantity = campDetEnt.TicketTypeQuantity,
                            TicketTypeLeft = campDetEnt.TicketTypeQuantity - campDetEnt.TicketTypeSold,
                            SaleStageDetailId = campDetEnt.SaleStageDetailId,
                            CustomerDiscount = campDetEnt.CustomerDiscount,
                        };

                        var stageDel = await _saleStageDetailRepo.Get(campgnDetailView.SaleStageDetailId);

                        campgnDetailView.TicketTypeId = stageDel.TicketTypeId;
                        campgnDetailView.SaleStageId = stageDel.SaleStageId;
                        campgnDetailView.TicketTypeName = await _ticketTypeRepo.GetNameByID(campgnDetailView.TicketTypeId);
                        campgnDetailView.SaleStageName = await _saleStageRepo.GetNameByID(campgnDetailView.SaleStageId);

                        campView.CampaignDetails.Add(campgnDetailView);
                    }

                    result = campView;
                }

                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModel>> GetPopularShows()
        {
            try
            {
                List<ShowViewModel> result = new();

                var publicShows = await _showRepo.GetShowsByStatus(Commons.PUBLIC);

                foreach (var publicShow in publicShows)
                {
                    var lowestPrice = await _ticketTypeRepo.GetLowestPrice(publicShow.Id);
                    publicShow.Category = await _showCategoryRepo.GetCategoryNameByID(publicShow.CategoryID ?? new Guid());
                    publicShow.LowestPrice = lowestPrice;
                    publicShow.StartDate = publicShow.ShowStartDate.ToShortDateString() + " " + publicShow.ShowStartDate.ToShortTimeString();
                    publicShow.EndDate = publicShow.ShowEndDate.ToShortDateString() + " " + publicShow.ShowEndDate.ToShortTimeString();
                    var saleStage = await GetShowDetailById(publicShow.Id, null);
                    if (saleStage.SaleStageView != null)
                    {
                        publicShow.SaleOff = saleStage.SaleStageView.SaleStageDiscount;
                    }
                }

                List<PopularShowViewModel> popularShows = new();

                foreach (var item in publicShows)
                {
                    PopularShowViewModel popularShowViewModel = new()
                    {
                        Category = item.Category,
                        CategoryID = item.CategoryID,
                        DescriptionImageUrl = item.DescriptionImageUrl,
                        EndDate = item.EndDate,
                        Id = item.Id,
                        ImgUrl = item.ImgUrl,
                        Location = item.Location,
                        LowestPrice = item.LowestPrice,
                        OrganizerID = item.OrganizerID,
                        OrganizerName = item.OrganizerName,
                        ShowDescriptionDetail = item.ShowDescriptionDetail,
                        ShowDescription = item.ShowDescription,
                        ShowEndDate = item.ShowEndDate,
                        ShowName = item.ShowName,
                        ShowStartDate = item.ShowStartDate,
                        ShowTypeId = item.ShowTypeId,
                        ShowTypeName = item.ShowTypeName,
                        StartDate = item.StartDate,
                        Status = item.Status,
                        SaleOff = item.SaleOff
                    };

                    popularShows.Add(popularShowViewModel);
                }

                foreach (var popularShow in popularShows)
                {
                    int totalTicketsSold = 0;
                    var tickets = await _ticketTypeRepo.GetTicketTypesByShowID(popularShow.Id);

                    foreach (var ticket in tickets)
                    {
                        totalTicketsSold += ticket.UnitSold;
                    }

                    popularShow.TotalTicketSold = totalTicketsSold;

                }

                popularShows = popularShows.OrderByDescending(x => x.TotalTicketSold).ToList();

                foreach (var item in popularShows)
                {
                    ShowViewModel showView = new()
                    {
                        Category = item.Category,
                        CategoryID = item.CategoryID,
                        DescriptionImageUrl = item.DescriptionImageUrl,
                        EndDate = item.EndDate,
                        Id = item.Id,
                        ImgUrl = item.ImgUrl,
                        Location = item.Location,
                        LowestPrice = item.LowestPrice,
                        OrganizerID = item.OrganizerID,
                        OrganizerName = item.OrganizerName,
                        ShowDescriptionDetail = item.ShowDescriptionDetail,
                        ShowDescription = item.ShowDescription,
                        ShowEndDate = item.ShowEndDate,
                        ShowName = item.ShowName,
                        ShowStartDate = item.ShowStartDate,
                        ShowTypeId = item.ShowTypeId,
                        ShowTypeName = item.ShowTypeName,
                        StartDate = item.StartDate,
                        Status = item.Status,
                        IsShowComming = item.ShowStartDate.CompareTo(DateTime.Now) > 0,
                        IsShowHappening = item.ShowStartDate.CompareTo(DateTime.Now) <= 0,
                        SaleOff = item.SaleOff
                };

                    result.Add(showView);
                }

                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<ShowViewModel>> GetNewestShows()
        {
            List<ShowViewModel> publicShows = new();

            List<ShowViewModel> newestShows = new();

            List<ShowDetailViewModelForCustomer> newestShowsDetailView = new();

            publicShows = await _showRepo.GetShowsByStatus(Commons.PUBLIC);

            foreach (var publicShow in publicShows)
            {
                var lowestPrice = await _ticketTypeRepo.GetLowestPrice(publicShow.Id);
                publicShow.Category = await _showCategoryRepo.GetCategoryNameByID(publicShow.CategoryID ?? new Guid());
                publicShow.LowestPrice = lowestPrice;
                publicShow.StartDate = publicShow.ShowStartDate.ToShortDateString() + " " + publicShow.ShowStartDate.ToShortTimeString();
                publicShow.EndDate = publicShow.ShowEndDate.ToShortDateString() + " " + publicShow.ShowEndDate.ToShortTimeString();
            }

            foreach (var publicShow in publicShows)
            {
                var showDetail = await GetShowDetailById(publicShow.Id, null);

                if (showDetail.SaleStageView != null)
                {
                    newestShowsDetailView.Add(showDetail);
                }
            }

            newestShowsDetailView = newestShowsDetailView.OrderByDescending(x => x.SaleStageView?.SaleStageStartDate).ToList();

            foreach (var newestShowDetail in newestShowsDetailView)
            {
                ShowViewModel showView = new()
                {
                    Category = newestShowDetail.Category,
                    CategoryID = newestShowDetail.CategoryID,
                    DescriptionImageUrl = newestShowDetail.DescriptionImageUrl,
                    EndDate = newestShowDetail.ShowEndDate.ToShortDateString() + " " + newestShowDetail.ShowEndDate.ToShortTimeString(),
                    Id = newestShowDetail.Id,
                    ImgUrl = newestShowDetail.ImgUrl,
                    Location = newestShowDetail.LocationView.LocationDescription,
                    LowestPrice = newestShowDetail.LowestPrice,
                    OrganizerID = newestShowDetail.OrganizerID,
                    OrganizerName = newestShowDetail.OrganizerName,
                    ShowDescriptionDetail = newestShowDetail.ShowDescriptionDetail,
                    ShowDescription = newestShowDetail.ShowDescription,
                    ShowEndDate = newestShowDetail.ShowEndDate,
                    ShowName = newestShowDetail.ShowName,
                    ShowStartDate = newestShowDetail.ShowStartDate,
                    ShowTypeId = newestShowDetail.ShowTypeId,
                    ShowTypeName = newestShowDetail.ShowTypeName,
                    StartDate = newestShowDetail.ShowStartDate.ToShortDateString() + " " + newestShowDetail.ShowStartDate.ToShortTimeString(),
                    Status = newestShowDetail.Status,
                    IsShowComming = newestShowDetail.ShowStartDate.CompareTo(DateTime.Now) > 0,
                    IsShowHappening = newestShowDetail.ShowStartDate.CompareTo(DateTime.Now) <= 0,                  
                };

                if (newestShowDetail.SaleStageView != null)
                {
                    showView.SaleOff = newestShowDetail.SaleStageView.SaleStageDiscount;
                }

                newestShows.Add(showView);
            }

            return newestShows;
        }

        public async Task<List<ShowViewModel>> GetUpCommingShows()
        {

            List<ShowViewModel> publicShows = new();

            List<ShowViewModel> upCommingShows = new();

            List<ShowDetailViewModelForCustomer> upCommingShowsDetailView = new();

            publicShows = await _showRepo.GetShowsByStatus(Commons.PUBLIC);

            foreach (var publicShow in publicShows)
            {
                var lowestPrice = await _ticketTypeRepo.GetLowestPrice(publicShow.Id);
                publicShow.Category = await _showCategoryRepo.GetCategoryNameByID(publicShow.CategoryID ?? new Guid());
                publicShow.LowestPrice = lowestPrice;
                publicShow.StartDate = publicShow.ShowStartDate.ToShortDateString() + " " + publicShow.ShowStartDate.ToShortTimeString();
                publicShow.EndDate = publicShow.ShowEndDate.ToShortDateString() + " " + publicShow.ShowEndDate.ToShortTimeString();
            }


            foreach (var publicShow in publicShows)
            {
                var showDetail = await GetShowDetailById(publicShow.Id, null);

                if (showDetail.SaleStageView == null)
                {
                    upCommingShowsDetailView.Add(showDetail);
                }
            }

            upCommingShowsDetailView = upCommingShowsDetailView.OrderByDescending(x => x.ShowStartDate).ToList();

            foreach (var upCommingShowDetail in upCommingShowsDetailView)
            {
                ShowViewModel showView = new()
                {
                    Category = upCommingShowDetail.Category,
                    CategoryID = upCommingShowDetail.CategoryID,
                    DescriptionImageUrl = upCommingShowDetail.DescriptionImageUrl,
                    EndDate = upCommingShowDetail.ShowEndDate.ToShortDateString() + " " + upCommingShowDetail.ShowEndDate.ToShortTimeString(),
                    Id = upCommingShowDetail.Id,
                    ImgUrl = upCommingShowDetail.ImgUrl,
                    Location = upCommingShowDetail.LocationView.LocationDescription,
                    LowestPrice = upCommingShowDetail.LowestPrice,
                    OrganizerID = upCommingShowDetail.OrganizerID,
                    OrganizerName = upCommingShowDetail.OrganizerName,
                    ShowDescriptionDetail = upCommingShowDetail.ShowDescriptionDetail,
                    ShowDescription = upCommingShowDetail.ShowDescription,
                    ShowEndDate = upCommingShowDetail.ShowEndDate,
                    ShowName = upCommingShowDetail.ShowName,
                    ShowStartDate = upCommingShowDetail.ShowStartDate,
                    ShowTypeId = upCommingShowDetail.ShowTypeId,
                    ShowTypeName = upCommingShowDetail.ShowTypeName,
                    StartDate = upCommingShowDetail.ShowStartDate.ToShortDateString() + " " + upCommingShowDetail.ShowStartDate.ToShortTimeString(),
                    Status = upCommingShowDetail.Status,
                    IsShowComming = upCommingShowDetail.ShowStartDate.CompareTo(DateTime.Now) > 0,
                    IsShowHappening = upCommingShowDetail.ShowStartDate.CompareTo(DateTime.Now) <= 0
                };

                if (upCommingShowDetail.SaleStageView != null)
                {
                    showView.SaleOff = upCommingShowDetail.SaleStageView.SaleStageDiscount;
                }

                upCommingShows.Add(showView);
            }

            return upCommingShows;
        }

        public async Task<bool> AddShowReview(string token, ShowReviewInsertRequestModel showReview)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid customerID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            if (!roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            if (showReview == null) throw new ArgumentException("Review Is Null");

            var customerName = await _userRepo.GetNameByID(customerID);

            ShowReview reviewEntity = new()
            {
                Id = Guid.NewGuid(),
                DateTimeReview = DateTime.Now,
                ReviewerId = customerID,
                ReviewerName = customerName,
                ReviewMessage = showReview.ReviewMessage,
                Rate = showReview.Rate,
                ShowId = showReview.ShowId
            };

            await _showReviewRepo.Insert(reviewEntity);

            return true;
        }

        public async Task<List<ShowReviewViewModel>> GetShowReviewsForAdmin(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var result = await _showReviewRepo.GetShowReviews();

            return result != null ? result : new();
        }

        public async Task<List<ShowReviewViewModel>> GetShowReviews()
        {
            var result = await _showReviewRepo.GetShowReviews();

            return result != null ? result : new();
        }

        public async Task<List<ShowReviewViewModel>> GetShowReviewsForOrganizer(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid organizerId = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var reviews = await _showReviewRepo.GetShowReviews();

            var result = new List<ShowReviewViewModel>();

            foreach (var review in reviews)
            {
                var isShowBelongToOrganizer = await _showRepo.IsShowBelongToOrganizer(organizerId, review.ShowId);
                if (isShowBelongToOrganizer)
                {
                    result.Add(review);
                }
            }

            return result != null ? result : new();
        }

        public async Task<bool> GenerateCampaignBookingLink(string token, Guid showId)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid customerID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var show = await GetShowDetailByIdForThirdParty(showId);

            if (show.CampaignViews != null)
            {
                foreach (var campaign in show.CampaignViews)
                {
                    var bookingLink =
                        Commons.BOOKING_LINK_PREFIX + campaign.ShowId
                        + Commons.CAMPAIGN_ID_PARAMETER + campaign.Id;

                    var dynamicLink = await GenerateDynamicLink(bookingLink);

                    var isUpdated = await _campaignRepo.UpdateBookingLink(campaign.Id, dynamicLink);

                    if (!isUpdated) throw new ArgumentException("Update BookingLink to DB Failed In Campaign: " + campaign.Id);
                }
                return true;
            }

            return false;
        }

        public async Task<ShowDetailViewModel> GetShowDetailByIdForThirdParty(Guid showID)
        {
            try
            {
                var ShowEntity = await _showRepo.Get(showID);
                if (ShowEntity == null) throw new ArgumentException("Show Not Found");

                ShowDetailViewModel result = new();

                result.Id = ShowEntity.Id;
                result.Status = ShowEntity.Status;
                result.ShowName = ShowEntity.ShowName;
                result.ShowDescriptionDetail = ShowEntity.ShowDescription ?? "";
                result.ImgUrl = ShowEntity.ImgUrl;
                result.DescriptionImageUrl = ShowEntity.DescriptionImageUrl ?? "";
                result.OrganizerID = ShowEntity.ShowOrganizerId;
                result.Status = ShowEntity.Status;
                result.ShowStartDate = ShowEntity.ShowStartDate;
                result.ShowEndDate = ShowEntity.ShowEndDate;
                result.ShowTypeId = ShowEntity.ShowTypeId;
                result.ShowTypeName = await _showTypeRepo.GetShowTypeByID(result.ShowTypeId) ?? string.Empty;
                result.OrganizerName = await _userRepo.GetNameByID(ShowEntity.ShowOrganizerId);
                result.CategoryID = ShowEntity.CategoryId;
                result.Category = await _showCategoryRepo.GetCategoryNameByID(result.CategoryID ?? new Guid()) ?? string.Empty;
                result.StartDate = ShowEntity.ShowStartDate.ToShortDateString() + " " + ShowEntity.ShowStartDate.ToShortTimeString();
                result.EndDate = ShowEntity.ShowEndDate.ToShortDateString() + " " + ShowEntity.ShowEndDate.ToShortTimeString();

                //Location View
                var location = await _locationRepo.GetLocationByShowID(ShowEntity.Id);
                if (location == null) throw new ArgumentException("Location is NULL");

                result.LocationView.Id = location.Id;
                result.LocationView.LocationDescription = location.LocationDescription;
                result.LocationView.ShowId = location.ShowId;

                //Ticket Type
                var ticketTypeEnts = await _ticketTypeRepo.GetTicketTypesByShowID(ShowEntity.Id);
                if (ticketTypeEnts == null) throw new ArgumentException("TicketType is NULL");

                foreach (var ticketTypeEnt in ticketTypeEnts)
                {
                    TicketTypeViewModel ticketTypeView = new()
                    {
                        Id = ticketTypeEnt.Id,
                        TicketTypeName = ticketTypeEnt.TicketTypeName,
                        TicketTypeDescription = ticketTypeEnt.TicketTypeDescription,
                        OriginalPrice = ticketTypeEnt.OriginalPrice,
                        Quantity = ticketTypeEnt.Quantity,
                        TicketTypeDiscount = ticketTypeEnt.TicketTypeDiscount,
                        ShowId = ticketTypeEnt.ShowId
                    };

                    result.TicketTypeViews.Add(ticketTypeView);
                }

                //Sale Stage
                var saleStageEnts = await _saleStageRepo.GetSaleStagesByShowID(ShowEntity.Id);
                if (saleStageEnts == null) throw new ArgumentException("SaleStage is NULL");
                foreach (var saleStageEnt in saleStageEnts)
                {
                    SaleStageViewModel saleStageView = new()
                    {
                        Id = saleStageEnt.Id,
                        SaleStageOrder = saleStageEnt.SaleStageOrder,
                        SaleStageDescription = saleStageEnt.SaleStageDescription,
                        SaleStageStartDate = saleStageEnt.SaleStageStartDate,
                        SaleStageEndDate = saleStageEnt.SaleStageEndDate,
                        SaleStageDiscount = saleStageEnt.SaleStageDiscount,
                        ShowId = saleStageEnt.ShowId,
                        startDate = saleStageEnt.startDate,
                        endDate = saleStageEnt.endDate
                    };

                    var SaleStageDetailList = await _saleStageDetailRepo.GetSaleStageDetailBySaleStageID(saleStageView.Id);

                    foreach (var saleStageDetail in SaleStageDetailList)
                    {
                        saleStageDetail.SaleStageName = await _saleStageRepo.GetNameByID(saleStageDetail.SaleStageId);
                        saleStageDetail.TicketTypeName = await _ticketTypeRepo.GetNameByID(saleStageDetail.TicketTypeId);
                        saleStageView.SaleStageDetailViewModels.Add(saleStageDetail);
                    }

                    result.SaleStageViews.Add(saleStageView);
                }

                //Campaign
                var campaignEnts = await _campaignRepo.GetCampaignsByShowID(ShowEntity.Id);
                if (campaignEnts == null) throw new ArgumentException("Campaign is NULL");
                foreach (var campaignEnt in campaignEnts)
                {
                    CampaignViewModel campView = new()
                    {
                        Id = campaignEnt.Id,
                        ArtistId = campaignEnt.ArtistId,
                        ShowId = campaignEnt.ShowId,
                        MaxDiscount = campaignEnt.MaxDiscount,
                        MinDiscount = campaignEnt.MinDiscount,
                    };

                    //Campaign Detail
                    var campDetailEnts = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(campaignEnt.Id);
                    if (campDetailEnts == null) throw new ArgumentException("CampaignDetail is NULL");
                    foreach (var campDetEnt in campDetailEnts)
                    {
                        CampaignDetailViewModel campgnDetailView = new()
                        {
                            Id = campDetEnt.Id,
                            ArtistDiscount = campDetEnt.ArtistDiscount,
                            CampaignDescription = campDetEnt.CampaignDescription,
                            CampaignName = campDetEnt.CampaignName,
                            CampaignStartDate = campDetEnt.CampaignStartDate,
                            CampaignEndDate = campDetEnt.CampaignEndDate,
                            CampaignId = campDetEnt.CampaignId,
                            TicketTypeQuantity = campDetEnt.TicketTypeQuantity,
                            TicketTypeSold = campDetEnt.TicketTypeSold,
                            SaleStageDetailId = campDetEnt.SaleStageDetailId,
                            CustomerDiscount = campDetEnt.CustomerDiscount,
                        };

                        var stageDel = await _saleStageDetailRepo.Get(campgnDetailView.SaleStageDetailId);

                        campgnDetailView.TicketTypeId = stageDel.TicketTypeId;
                        campgnDetailView.SaleStageId = stageDel.SaleStageId;
                        campgnDetailView.TicketTypeName = await _ticketTypeRepo.GetNameByID(campgnDetailView.TicketTypeId);
                        campgnDetailView.SaleStageName = await _saleStageRepo.GetNameByID(campgnDetailView.SaleStageId);

                        campView.CampaignDetails.Add(campgnDetailView);
                    }

                    result.CampaignViews.Add(campView);

                }

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Guid?> InsertShowSaleStage(string token, ShowSaleStagesRequestInsertModel saleStageInsertModel)
        {
            if (saleStageInsertModel == null)
            {
                throw new ArgumentNullException("SaleStage Is Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid staffID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            if (!roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);


            if (await _showRepo.Get(saleStageInsertModel.ShowID) == null)
                throw new ArgumentNullException("Show ID: " + saleStageInsertModel.ShowID + " not found in DB ");
            if (saleStageInsertModel != null)
            {

                if (saleStageInsertModel.SaleStages != null)
                {
                    //Mapping Sale-Stage
                    foreach (var saleStageInsert in saleStageInsertModel.SaleStages)
                    {
                        SaleStage salestageEntity = new()
                        {
                            Id = Guid.NewGuid(),
                            ShowId = saleStageInsertModel.ShowID,
                            SaleStageOrder = saleStageInsert.SaleStageOrder,
                            SaleStageStartDate = saleStageInsert.SaleStageStartDate,
                            SaleStageEndDate = saleStageInsert.SaleStageEndDate,
                            SaleStageDescription = saleStageInsert.SaleStageDescription,
                            SaleStageDiscount = saleStageInsert.SaleStageDiscount
                        };

                        foreach (var saleStageDetailInsert in saleStageInsert.SaleStageDetails)
                        {
                            SaleStageDetail saleStageDetailEntity = new()
                            {
                                Id = Guid.NewGuid(),
                                SaleStageId = salestageEntity.Id,
                                TicketTypeId = saleStageDetailInsert.TicketTypeId,
                                TicketTypeQuantity = saleStageDetailInsert.TicketTypeQuantity
                            };

                            salestageEntity.SaleStageDetails.Add(saleStageDetailEntity);
                        }

                        await _saleStageRepo.Insert(salestageEntity);
                    }

                    await _showRepo.UpdateShowCreationStep(saleStageInsertModel.ShowID, 2);
                    return saleStageInsertModel.ShowID;
                }
            }



            return null;
        }

        public async Task<Guid?> InsertShowCampaign(string token, ShowCampaignsRequestInsertModel campaignInsertModel)
        {
            if (campaignInsertModel == null)
            {
                throw new ArgumentNullException("Show Is Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            try
            {
                if (await _showRepo.Get(campaignInsertModel.ShowID) == null)
                    throw new ArgumentNullException("Show ID: " + campaignInsertModel.ShowID + " not found in DB ");
                if (campaignInsertModel != null)
                {
                    if (campaignInsertModel.Campaigns != null)
                    {
                        //Mapping Campaign 
                        foreach (var campaign in campaignInsertModel.Campaigns)
                        {
                            var campaignDetailCount = campaign.CampaignDetails.Count;

                            Campaign campaignEntity = new()
                            {
                                Id = Guid.NewGuid(),
                                ShowId = campaignInsertModel.ShowID,
                                ArtistId = campaign.ArtistId,
                                MaxDiscount = campaign.MaxDiscount,
                                MinDiscount = campaign.MinDiscount,
                            };

                            var saleStages = await GetSaleStagesByShowId(token, campaignInsertModel.ShowID);
                            var show = await GetShowDetailByIdByManager(token, campaignInsertModel.ShowID);

                            foreach(var saleStage in saleStages)
                            {
                                var campaignDet = campaign.CampaignDetails.Where(x => x.SaleStageId.Equals(saleStage.Id)).FirstOrDefault();
                                if (campaignDet != null)
                                {
                                    
                                    foreach (var saleDet in saleStage.SaleStageDetailViewModels)
                                    {
                                        var saleTik = Convert.ToDouble(saleDet.TicketTypeQuantity);
                                        var numTik = saleTik * campaignDet.TicketTypeQuantity / 100.0;
                                        var ceilingUnit = Math.Ceiling(numTik);
                                        int ticketTypeQuantity = (int)ceilingUnit;

                                        CampaignDetail campaignDetailEntity = new()
                                        {
                                            Id = Guid.NewGuid(),
                                            CampaignName = campaignDet.CampaignName,
                                            ArtistDiscount = campaignDet.ArtistDiscount,
                                            CampaignDescription = campaignDet.CampaignDescription,
                                            CampaignStartDate = show.ShowStartDate,
                                            CampaignEndDate = show.ShowEndDate,
                                            TicketBookingPageLink = string.Empty,
                                            TicketTypeQuantity = ticketTypeQuantity,
                                            TicketTypeSold = 0,
                                            CampaignId = campaignEntity.Id,
                                            CustomerDiscount = campaignDet.CustomerDiscount,
                                            SaleStageDetailId = saleDet.Id
                                        };
                                        campaignEntity.CampaignDetails.Add(campaignDetailEntity);
                                    }
                                }
                            }
                            await _campaignRepo.Insert(campaignEntity);
                        }
                        await _showRepo.UpdateShowCreationStep(campaignInsertModel.ShowID, 3);
                        return campaignInsertModel.ShowID;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        public async Task<List<TicketTypeViewModel>> GetTicketTypesByShowId(string token, Guid showId)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                //Ticket Type
                List<TicketTypeViewModel> ticketTypes = new();
                var ticketTypeEnts = await _ticketTypeRepo.GetTicketTypesByShowID(showId);
                if (ticketTypeEnts == null) throw new ArgumentException("TicketType is NULL");

                foreach (var ticketTypeEnt in ticketTypeEnts)
                {
                    TicketTypeViewModel ticketTypeView = new()
                    {
                        Id = ticketTypeEnt.Id,
                        TicketTypeName = ticketTypeEnt.TicketTypeName,
                        TicketTypeDescription = ticketTypeEnt.TicketTypeDescription,
                        OriginalPrice = ticketTypeEnt.OriginalPrice,
                        Quantity = ticketTypeEnt.Quantity,
                        TicketTypeDiscount = ticketTypeEnt.TicketTypeDiscount,
                        ShowId = ticketTypeEnt.ShowId
                    };

                    ticketTypes.Add(ticketTypeView);
                }

                return ticketTypes;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<SaleStageViewModel>> GetSaleStagesByShowId(string token, Guid showId)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                //Ticket Type
                List<SaleStageViewModel> saleStages = new();

                //Sale Stage
                var saleStageEnts = await _saleStageRepo.GetSaleStagesByShowID(showId);
                if (saleStageEnts == null) throw new ArgumentException("SaleStage is NULL");
                foreach (var saleStageEnt in saleStageEnts)
                {
                    SaleStageViewModel saleStageView = new()
                    {
                        Id = saleStageEnt.Id,
                        SaleStageOrder = saleStageEnt.SaleStageOrder,
                        SaleStageDescription = saleStageEnt.SaleStageDescription,
                        SaleStageStartDate = saleStageEnt.SaleStageStartDate,
                        SaleStageEndDate = saleStageEnt.SaleStageEndDate,
                        SaleStageDiscount = saleStageEnt.SaleStageDiscount,
                        ShowId = saleStageEnt.ShowId,
                        startDate = saleStageEnt.startDate,
                        endDate = saleStageEnt.endDate
                    };

                    var SaleStageDetailList = await _saleStageDetailRepo.GetSaleStageDetailBySaleStageID(saleStageView.Id);

                    foreach (var saleStageDetail in SaleStageDetailList)
                    {
                        saleStageDetail.SaleStageName = await _saleStageRepo.GetNameByID(saleStageDetail.SaleStageId);
                        saleStageDetail.TicketTypeName = await _ticketTypeRepo.GetNameByID(saleStageDetail.TicketTypeId);
                        saleStageView.SaleStageDetailViewModels.Add(saleStageDetail);
                    }

                    saleStages.Add(saleStageView);
                }
                return saleStages;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<CampaignViewModel>> GetCampaignByShowId(string token, Guid showId)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                //Ticket Type
                List<CampaignViewModel> campaign = new();

                //Campaign
                var campaignEnts = await _campaignRepo.GetCampaignsByShowID(showId);
                if (campaignEnts == null) throw new ArgumentException("Campaign is NULL");
                foreach (var campaignEnt in campaignEnts)
                {
                    CampaignViewModel campView = new()
                    {
                        Id = campaignEnt.Id,
                        ArtistId = campaignEnt.ArtistId,
                        ShowId = campaignEnt.ShowId,
                        MaxDiscount = campaignEnt.MaxDiscount,
                        MinDiscount = campaignEnt.MinDiscount,
                    };

                    //Campaign Detail
                    var campDetailEnts = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(campaignEnt.Id);
                    if (campDetailEnts == null) throw new ArgumentException("CampaignDetail is NULL");
                    foreach (var campDetEnt in campDetailEnts)
                    {
                        CampaignDetailViewModel campgnDetailView = new()
                        {
                            Id = campDetEnt.Id,
                            ArtistDiscount = campDetEnt.ArtistDiscount,
                            CampaignDescription = campDetEnt.CampaignDescription,
                            CampaignName = campDetEnt.CampaignName,
                            CampaignStartDate = campDetEnt.CampaignStartDate,
                            CampaignEndDate = campDetEnt.CampaignEndDate,
                            CampaignId = campDetEnt.CampaignId,
                            TicketTypeQuantity = campDetEnt.TicketTypeQuantity,
                            TicketTypeSold = campDetEnt.TicketTypeSold,
                            SaleStageDetailId = campDetEnt.SaleStageDetailId,
                            CustomerDiscount = campDetEnt.CustomerDiscount,
                        };

                        var stageDel = await _saleStageDetailRepo.Get(campgnDetailView.SaleStageDetailId);

                        campgnDetailView.TicketTypeId = stageDel.TicketTypeId;
                        campgnDetailView.SaleStageId = stageDel.SaleStageId;
                        campgnDetailView.TicketTypeName = await _ticketTypeRepo.GetNameByID(campgnDetailView.TicketTypeId);
                        campgnDetailView.SaleStageName = await _saleStageRepo.GetNameByID(campgnDetailView.SaleStageId);

                        campView.CampaignDetails.Add(campgnDetailView);
                    }

                    campaign.Add(campView);

                }
                return campaign;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        private async Task<string> GenerateDynamicLink(string link)
        {
            var client = new HttpClient();
            Uri uri = new("https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=AIzaSyCopthNKjHMIgHQBGwqYJNxcbVux2tGcCk");
            DynamicLinkRequestModel dynamicLinkRequestModel = new();

            dynamicLinkRequestModel.dynamicLinkInfo.domainUriPrefix = "https://ultratix.page.link";
            dynamicLinkRequestModel.dynamicLinkInfo.link = link;
            dynamicLinkRequestModel.dynamicLinkInfo.androidInfo.androidPackageName = "com.example.ultratix_mobile_v03";
            dynamicLinkRequestModel.dynamicLinkInfo.iosInfo.iosBundleId = "com.ultratix.ultratixMobile";

            JsonSerializerSettings jsonSerializerSettings = new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var reqJson = JsonConvert.SerializeObject(dynamicLinkRequestModel, Formatting.Indented,
                jsonSerializerSettings);

            var content = new StringContent(reqJson, Encoding.UTF8);

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = content;
            request.RequestUri = uri;

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var resJson = JsonConvert.DeserializeObject<DynamicLinkReponseModel>(responseContent) ?? new();

            var dynamicLink = resJson.shortLink ?? string.Empty;

            return dynamicLink;
        }

        public async Task<List<ShowViewModel>> GetShowsByOrganizerID(string token, Guid id)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            try
            {
                var result = await _showRepo.GetAllShows();

                result = result.Where(s => s.OrganizerID == id).ToList();
                return result;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Guid> GetOrganizerByShowId(Guid showId)
        {
            var showEntity = await _showRepo.Get(showId);
            if (showEntity != null)
            {
                return showEntity.ShowOrganizerId;
            }
            return new Guid();
        }

        public async Task<bool> IsAdminTrasferShowRevenueToOrganizer(string token, Guid showID)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ARTIST) && !roleID.Equals(Commons.ORGANIZER)
                ) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var show = await _showRepo.Get(showID);

            if (show == null) throw new ArgumentException("Show not found with ID: " + showID);

            return show.IsTransferRevenueToOrganizer ?? false;
        }

        public async Task<int> GetTotalTicketArtistSell(Guid showId, string token)
        {
            Guid artistId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            var campaigns = await _campaignRepo.GetCampaignsByShowID(showId);

            var campaign = campaigns.Where(x => x.ArtistId.Equals(artistId)).FirstOrDefault();

            if (campaign == null) throw new ArgumentException("Campaign not found with artist id: " + artistId);

            var campaignDetail = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(campaign.Id);

            var totalTicketSold = 0;

            foreach(var detail in campaignDetail)
            {
                totalTicketSold += detail.TicketTypeQuantity;
            }

            return totalTicketSold;
        }

        public async Task<List<ShowReviewOverviewModel>> GetShowReviewsForOrganizer(string token, Guid showId)
        {
            var roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ORGANIZER) && !roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var showReviews = await _showReviewRepo.GetShowReviews();
            showReviews = showReviews.Where(x => x.ShowId.Equals(showId)).ToList();
            List<ShowReviewOverviewModel> rates = new();
            for(double i = 1; i <= 5; i++)
            {
                ShowReviewOverviewModel reviewOverview = new();

                var count = 0;
                foreach(var review in showReviews)
                {
                    if(review.Rate >= i && review.Rate < i + 1)
                    {
                        count += 1;
                    }
                }

                reviewOverview.Rate = i + "";
                reviewOverview.RateCount = count;

                rates.Add(reviewOverview);
            }
            return rates;
        }

        public async Task<Guid?> UpdateShow(string token, ShowRequestUpdateModel show)
        {
            if (show == null)
            {
                throw new ArgumentNullException("Show Is Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            try
            {
                if (show != null)
                {

                    //Mapping campaignUpdateModel 
                    Show? showEntity = await _showRepo.Get(show.ShowID);
                    if(showEntity == null)
                    {
                        throw new Exception("Show ID" + show.ShowID + "in DB");
                    }

                    showEntity.ShowTypeId = show.ShowType;
                    showEntity.ShowName = show.ShowName;
                    showEntity.ShowDescription = show.ShowDescription;
                    showEntity.ShowDetail = show.ShowDescriptionDetail;
                    showEntity.Status = Commons.DRAFT;
                    showEntity.ShowStartDate = show.ShowStartDate;
                    showEntity.ShowEndDate = show.ShowEndDate;
                    showEntity.DescriptionImageUrl = show.DescriptionImageUrl;
                    showEntity.ImgUrl = show.ImgUrl;
                    showEntity.CategoryId = show.CategoryID;

                    await _showRepo.Update();

                    //Mapping location
                    Location? location = await _locationRepo.Get(show.Location.LocationID);
                    if(location == null)
                    {
                        throw new Exception("Location ID" + location.Id + "in DB");
                    }

                    location.ShowId = showEntity.Id;
                    location.LocationDescription = show.Location.LocationDescription;

                    await _locationRepo.Update();

                    if (show.TicketTypes != null)
                    {
                        //Mapping ticket type
                        foreach (var ticketTypeInsert in show.TicketTypes)
                        {
                            //TicketType? campEntity = await _ticketTypeRepo.Get(campaignUpdate.TicketTypeID ?? new Guid());
                            if(ticketTypeInsert.TicketTypeID == null)
                            {

                                TicketType newTicketTypeEntity = new()
                                {
                                    Id = Guid.NewGuid(),
                                    ShowId = showEntity.Id,
                                    Quantity = ticketTypeInsert.Quantity,
                                    OriginalPrice = ticketTypeInsert.OriginalPrice,
                                    TicketTypeDescription = ticketTypeInsert.TicketTypeDescription,
                                    TicketTypeName = ticketTypeInsert.TicketTypeName,
                                    TicketTypeDiscount = ticketTypeInsert.TicketTypeDiscount,
                                    UnitSold = 0
                                };
                                await _ticketTypeRepo.Insert(newTicketTypeEntity);
                            }
                            else
                            {
                                if (ticketTypeInsert.IsDeleted)
                                {
                                    var saleStageDetails = await _saleStageDetailRepo.GetSaleStageDetailByTicketTypeId(ticketTypeInsert.TicketTypeID ?? new Guid());
                                    
                                    if (saleStageDetails != null)
                                    {
                                        var isRemovedCampaignDetails = false;

                                        foreach (var saleStageDetail in saleStageDetails)
                                        {
                                            var campaignDetails = await _campaignDetailRepo.GetCampaignDetailBySaleStageDetailId(saleStageDetail.Id);
                                            
                                            if(campaignDetails != null)
                                            {
                                                isRemovedCampaignDetails = await _campaignDetailRepo.RemoveCampaignDetails(campaignDetails);                                                                                               
                                            }                                                  
                                        }

                                        if (isRemovedCampaignDetails)
                                        {
                                            var isRemovedAllSaleStageDetailHasThisTicketType = await _saleStageDetailRepo.RemoveSaleStageDetails(saleStageDetails);

                                            if (isRemovedAllSaleStageDetailHasThisTicketType)
                                            {
                                                await _ticketTypeRepo.RemoveTicketType(ticketTypeInsert.TicketTypeID ?? new Guid());
                                            }
                                        }

                                        var showCheck = await GetShowDetailByIdByManager(token, show.ShowID);
                                        foreach(var saleStage in showCheck.SaleStageViews)
                                        {
                                            if(saleStage.SaleStageDetailViewModels == null 
                                                || saleStage.SaleStageDetailViewModels.Count <=0)
                                            {
                                                await _saleStageRepo.RemoveSaleStage(saleStage.Id);
                                            }
                                        }

                                        foreach (var campaign in showCheck.CampaignViews)
                                        {
                                            if (campaign.CampaignDetails == null
                                                || campaign.CampaignDetails.Count <= 0)
                                            {
                                                await _campaignRepo.RemoveCampaign(campaign.Id);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        await _ticketTypeRepo.RemoveTicketType(ticketTypeInsert.TicketTypeID ?? new Guid());
                                    }
                                    
                                }
                                else
                                {
                                    TicketType? ticketTypeEntity = await _ticketTypeRepo.Get(ticketTypeInsert.TicketTypeID ?? new Guid());

                                    if (ticketTypeEntity != null)
                                    {
                                        ticketTypeEntity.Quantity = ticketTypeInsert.Quantity;
                                        ticketTypeEntity.OriginalPrice = ticketTypeInsert.OriginalPrice;
                                        ticketTypeEntity.TicketTypeDescription = ticketTypeInsert.TicketTypeDescription;
                                        ticketTypeEntity.TicketTypeName = ticketTypeInsert.TicketTypeName;
                                        ticketTypeEntity.TicketTypeDiscount = ticketTypeInsert.TicketTypeDiscount;
                                        ticketTypeEntity.UnitSold = 0;

                                        await _ticketTypeRepo.Update();
                                    }
                                }                                
                            }
                        }
                    }
                    return showEntity.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        public async Task<Guid?> UpdateShowSaleStage(string token, ShowSaleStagesRequestUpdateModel saleStageUpdateModel)
        {
            if (saleStageUpdateModel == null)
            {
                throw new ArgumentNullException("SaleStage Is Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            if (saleStageUpdateModel != null)
            {
                if (saleStageUpdateModel.SaleStages != null)
                {
                        // Mapping sale stage
                        foreach (var saleStage in saleStageUpdateModel.SaleStages)
                        {
                        //TicketType? campEntity = await _ticketTypeRepo.Get(campaignUpdate.TicketTypeID ?? new Guid());
                        if (saleStage.SaleStageId == null)
                        {

                            SaleStage newSaleStage = new()
                            {
                                Id = Guid.NewGuid(),
                                ShowId = saleStageUpdateModel.ShowId,
                                SaleStageDescription = saleStage.SaleStageDescription,
                                SaleStageDiscount = saleStage.SaleStageDiscount,
                                SaleStageStartDate = saleStage.SaleStageStartDate,
                                SaleStageEndDate = saleStage.SaleStageEndDate,
                                SaleStageOrder = saleStage.SaleStageOrder,
                                
                            };

                            foreach(var salStageDet in saleStage.SaleStageDetails)
                            {
                                SaleStageDetail newSaleStageDetail = new()
                                {
                                    Id = Guid.NewGuid(),
                                    SaleStageId = newSaleStage.Id,
                                    TicketTypeId = salStageDet.TicketTypeId,
                                    TicketTypeQuantity = salStageDet.TicketTypeQuantity,
                                    TicketTypeNormalUnitSold = 0,
                                    TicketTypeQuantitySold = 0,
                                    TicketTypeViaLinkUnitSold = 0
                                };
                                newSaleStage.SaleStageDetails.Add(newSaleStageDetail);
                            }

                            await _saleStageRepo.Insert(newSaleStage);
                        }
                        else
                        {
                            if (saleStage.IsDeleted)
                            {
                                var saleStageDetails = await _saleStageDetailRepo.GetSaleStageDetailBySaleStageId(saleStage.SaleStageId ?? new Guid());

                                if (saleStageDetails != null && saleStageDetails.Count > 0)
                                {
                                    var isRemovedCampaignDetails = false;

                                    foreach (var saleStageDetail in saleStageDetails)
                                    {
                                        var campaignDetails = await _campaignDetailRepo.GetCampaignDetailBySaleStageDetailId(saleStageDetail.Id);

                                        if (campaignDetails != null)
                                        {
                                            isRemovedCampaignDetails = await _campaignDetailRepo.RemoveCampaignDetails(campaignDetails);
                                        }
                                    }

                                    if (isRemovedCampaignDetails)
                                    {
                                        var isRemovedAllSaleStageDetailHasThisTicketType = await _saleStageDetailRepo.RemoveSaleStageDetails(saleStageDetails);

                                        if (isRemovedAllSaleStageDetailHasThisTicketType)
                                        {
                                            await _saleStageRepo.RemoveSaleStage(saleStage.SaleStageId ?? new Guid());
                                        }
                                    }
                                }
                                else
                                {
                                    await _saleStageRepo.RemoveSaleStage(saleStage.SaleStageId ?? new Guid());
                                }

                            }
                            else
                            {
                                SaleStage? saleStageEntity = await _saleStageRepo.Get(saleStage.SaleStageId ?? new Guid());

                                if (saleStageEntity != null)
                                {
                                    saleStageEntity.SaleStageDiscount = saleStage.SaleStageDiscount;
                                    saleStageEntity.SaleStageStartDate = saleStage.SaleStageStartDate;
                                    saleStageEntity.SaleStageOrder = saleStage.SaleStageOrder;
                                    saleStageEntity.SaleStageEndDate = saleStage.SaleStageEndDate;
                                    saleStageEntity.SaleStageDescription = saleStage.SaleStageDescription;

                                    await _saleStageRepo.Update();

                                    foreach(var saleStageDetailEntity in saleStage.SaleStageDetails)
                                    {
                                        var saleStageDetailEnt = await _saleStageDetailRepo.Get(saleStageDetailEntity.SaleStageDetailId);
                                        if(saleStageDetailEnt != null)
                                        {
                                            saleStageDetailEnt.TicketTypeQuantity = saleStageDetailEntity.TicketTypeQuantity;
                                            saleStageDetailEnt.TicketTypeId = saleStageDetailEntity.TicketTypeId;
                                            saleStageDetailEnt.TicketTypeQuantitySold = 0;
                                            saleStageDetailEnt.TicketTypeViaLinkUnitSold = 0;
                                            saleStageDetailEnt.TicketTypeNormalUnitSold = 0;

                                            await _saleStageDetailRepo.Update();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    
                    return saleStageUpdateModel.ShowId;
                }
            }



            return null;
        }

        public async Task<Guid?> UpdateShowCampaign(string token, ShowCampaignsRequestUpdateModel campaignUpdateModel)
        {
            if (campaignUpdateModel == null)
            {
                throw new ArgumentNullException("Show Is Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            try
            {
                var show = await _showRepo.Get(campaignUpdateModel.ShowId);
                if (show == null)
                    throw new ArgumentNullException("Show ID: " + campaignUpdateModel.ShowId + " not found in DB ");
                if (campaignUpdateModel != null)
                {
                    if (campaignUpdateModel.Campaigns != null)
                    {
                        // Mapping campaign
                        foreach (var campaignUpdate in campaignUpdateModel.Campaigns)
                        {
                            //TicketType? campEntity = await _ticketTypeRepo.Get(campaignUpdate.TicketTypeID ?? new Guid());
                            if (campaignUpdate.CampaignId == null)
                            {

                                Campaign newCampaign = new()
                                {
                                    Id = Guid.NewGuid(),
                                    ShowId = campaignUpdateModel.ShowId,
                                    MinDiscount = campaignUpdate.MinDiscount,
                                    MaxDiscount = campaignUpdate.MaxDiscount,
                                    ArtistId = campaignUpdate.ArtistId,
                                    BookingLink = string.Empty
                                };

                                foreach (var campDet in campaignUpdate.CampaignDetails)
                                {
                                    CampaignDetail newCampaignDetail = new()
                                    {
                                        Id = Guid.NewGuid(),
                                        CampaignName = campDet.CampaignName,
                                        CampaignDescription = campDet.CampaignDescription,
                                        CampaignStartDate = show.ShowStartDate,
                                        CampaignEndDate = show.ShowEndDate,
                                        TicketBookingPageLink = string.Empty,
                                        TicketTypeQuantity = campDet.TicketTypeQuantity,
                                        TicketTypeSold = 0,
                                        SaleStageDetailId = campDet.SaleStageDetailId
                                    };
                                    newCampaign.CampaignDetails.Add(newCampaignDetail);
                                }

                                await _campaignRepo.Insert(newCampaign);
                            }
                            else
                            {
                                if (campaignUpdate.IsDeleted)
                                {
                                    var campDetails = await _campaignDetailRepo.GetCampaignDetailsByCampaignId(campaignUpdate.CampaignId ?? new Guid());

                                    if (campDetails != null && campDetails.Count > 0)
                                    {
                                        var isRemovedCampaignDetails = false;

                                        isRemovedCampaignDetails = await _campaignDetailRepo.RemoveCampaignDetails(campDetails);
                                        if (isRemovedCampaignDetails)
                                        {
                                            await _campaignRepo.RemoveCampaign(campaignUpdate.CampaignId ?? new Guid());
                                        }
                                    }
                                    else
                                    {
                                        await _campaignRepo.RemoveCampaign(campaignUpdate.CampaignId ?? new Guid());
                                    }

                                }
                                else
                                {
                                    Campaign? campEntity = await _campaignRepo.Get(campaignUpdate.CampaignId ?? new Guid());

                                    if (campEntity != null)
                                    {
                                        campEntity.ShowId = campaignUpdateModel.ShowId;
                                        campEntity.MinDiscount = campaignUpdate.MinDiscount;
                                        campEntity.MaxDiscount = campaignUpdate.MaxDiscount;
                                        campEntity.ArtistId = campaignUpdate.ArtistId;
                                        campEntity.BookingLink = string.Empty;

                                        await _campaignRepo.Update();

                                        foreach (var campDetai in campaignUpdate.CampaignDetails)
                                        {
                                            var campDetailEnt = await _campaignDetailRepo.Get(campDetai.CampaignDetailId);
                                            if (campDetailEnt != null)
                                            {
                                                campDetailEnt.CampaignName = campDetai.CampaignName;
                                                campDetailEnt.CampaignDescription = campDetai.CampaignDescription;
                                                campDetailEnt.CampaignStartDate = show.ShowStartDate;
                                                campDetailEnt.CampaignEndDate = show.ShowEndDate;
                                                campDetailEnt.TicketBookingPageLink = string.Empty;
                                                campDetailEnt.TicketTypeQuantity = campDetai.TicketTypeQuantity;
                                                campDetailEnt.TicketTypeSold = 0;
                                                campDetailEnt.SaleStageDetailId = campDetai.SaleStageDetailId;

                                                await _campaignDetailRepo.Update();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return campaignUpdateModel.ShowId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        public Task<List<TicketTypeChartView>> GetTicketTypeChartViewsByShowID(string token, Guid ShowID)
        {
            var roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            try
            {
                var list = _ticketTypeRepo.GetTicketTypesForChartView(ShowID);
                return list;
            }catch(Exception e)
            {
                throw new ArgumentException(e.ToString());
            }

        }

        public async Task<bool> IsTicketsBookedTempClose(ShowOrderRequestModel order)
        {
            var isBuyViaLink = false;

            if (order.CampaignId != null) 
            {
                isBuyViaLink = true; 
            }
            foreach(var orderDetail in order.ShowOrderDetail)
            {
                //Buy via link
                if (order.CampaignId != null)
                {
                    var campDetail = await _campaignDetailRepo.Get(orderDetail.CampaignDetailId ?? new Guid());
                    if (campDetail != null)
                    {
                        var ticketTypeLeft = campDetail.TicketTypeQuantity - campDetail.TicketTypeSold;
                        //Check if quantity left is available for booking
                        if (orderDetail.QuantityBuy <= ticketTypeLeft)
                        {
                            //Close order temporary
                            await _ticketTypeRepo.UpdateUnitSold(orderDetail.TicketTypeId, orderDetail.QuantityBuy, isBuyViaLink);
                            await _saleStageDetailRepo.UpdateUnitSold(orderDetail.SaleStageDetailId ?? new Guid(), orderDetail.QuantityBuy, isBuyViaLink);
                            if (isBuyViaLink)
                            {
                                if (orderDetail.CampaignDetailId != null)
                                {
                                    await _campaignDetailRepo.UpdateUnitSold(orderDetail.CampaignDetailId ?? new Guid(), orderDetail.QuantityBuy);
                                    //return true;
                                }
                            }
                            //return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }                   
                }
                else //Buy normal
                {
                    var saleStageDet = await _saleStageDetailRepo.Get(orderDetail.SaleStageDetailId ?? new Guid());
                    if (saleStageDet != null)
                    {
                        var ticketsSoldViaCampaign = await _campaignDetailRepo.GetTotalTicketsBySaleStageDetailId(saleStageDet.Id);
                        var ticketTypeLeft = saleStageDet.TicketTypeQuantity - ticketsSoldViaCampaign - (saleStageDet.TicketTypeNormalUnitSold ?? 0);
                        //Check if quantity left is available for booking
                        if (orderDetail.QuantityBuy <= ticketTypeLeft)
                        {
                            //Close order temporary
                            saleStageDet.TicketTypeQuantitySold -= orderDetail.QuantityBuy;
                            await _ticketTypeRepo.UpdateUnitSold(saleStageDet.TicketTypeId, orderDetail.QuantityBuy, isBuyViaLink);
                            await _saleStageDetailRepo.UpdateUnitSold(saleStageDet.Id, orderDetail.QuantityBuy, isBuyViaLink);
                            if (isBuyViaLink)
                            {
                                if (orderDetail.CampaignDetailId != null)
                                {
                                    await _campaignDetailRepo.UpdateUnitSold(orderDetail.CampaignDetailId ?? new Guid(), orderDetail.QuantityBuy);
                                    //return true;
                                }
                            }
                            //return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }      
            return true;
        }

        public async Task<bool> ReOpenBookingOrder(ShowOrderRequestModel order)
        {
            var isBuyViaLink = false;

            if (order.CampaignId != null)
            {
                isBuyViaLink = true;
            }
            foreach (var orderDetail in order.ShowOrderDetail)
            {
                //Buy via link
                if (order.CampaignId != null)
                {
                    var campDetail = await _campaignDetailRepo.Get(orderDetail.CampaignDetailId ?? new Guid());
                    if (campDetail != null)
                    {
                        //var ticketTypeLeft = campDetail.TicketTypeQuantity - campDetail.TicketTypeSold;
                        //Check if quantity left is available for booking
                        await _ticketTypeRepo.ReOpenUnitSold(orderDetail.TicketTypeId, orderDetail.QuantityBuy, isBuyViaLink);
                        await _saleStageDetailRepo.ReOpenUnitSold(orderDetail.SaleStageDetailId ?? new Guid(), orderDetail.QuantityBuy, isBuyViaLink);
                        if (isBuyViaLink)
                        {
                            if (orderDetail.CampaignDetailId != null)
                            {
                                await _campaignDetailRepo.ReOpenUnitSold(orderDetail.CampaignDetailId ?? new Guid(), orderDetail.QuantityBuy);
                                //return true;
                            }
                        }                        
                    }
                    else
                    {
                        return false;
                    }
                }
                else //Buy normal
                {
                    var saleStageDet = await _saleStageDetailRepo.Get(orderDetail.SaleStageDetailId ?? new Guid());
                    if (saleStageDet != null)
                    {
                        //var ticketTypeLeft = saleStageDet.TicketTypeQuantity - saleStageDet.TicketTypeQuantitySold;
                        //Check if quantity left is available for booking
                        await _ticketTypeRepo.ReOpenUnitSold(saleStageDet.TicketTypeId, orderDetail.QuantityBuy, isBuyViaLink);
                        await _saleStageDetailRepo.ReOpenUnitSold(saleStageDet.Id, orderDetail.QuantityBuy, isBuyViaLink);
                        if (isBuyViaLink)
                        {
                            if (orderDetail.CampaignDetailId != null)
                            {
                                await _campaignDetailRepo.ReOpenUnitSold(orderDetail.CampaignDetailId ?? new Guid(), orderDetail.QuantityBuy);
                                //return true;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
