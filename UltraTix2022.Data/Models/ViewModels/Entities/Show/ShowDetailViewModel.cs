using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Location;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.TicketType;

namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show
{
    public class ShowDetailViewModel : ShowViewModel
    {
        public LocationViewModel LocationView { get; set; } = new();

        public List<TicketTypeViewModel> TicketTypeViews { get; set; } = new();

        public List<SaleStageViewModel> SaleStageViews { get; set; } = new();

        public List<CampaignViewModel> CampaignViews { get; set; } = new();

    }
}
