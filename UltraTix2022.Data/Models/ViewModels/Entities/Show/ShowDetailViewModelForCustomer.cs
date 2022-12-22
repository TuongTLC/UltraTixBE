using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Location;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;

namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show
{
    public class ShowDetailViewModelForCustomer
    {
        public Guid Id { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public string ShowDescription { get; set; } = string.Empty;
        public string ShowDescriptionDetail { get; set; } = string.Empty;
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public Guid ShowTypeId { get; set; }
        public string ShowTypeName { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string OrganizerImageUrl { get; set; } = string.Empty;
        public Guid OrganizerID { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string DescriptionImageUrl { get; set; } = null!;
        public double LowestPrice { get; set; }
        public Guid? CategoryID { get; set; }
        public string Category { get; set; } = string.Empty;

        public LocationViewModel LocationView { get; set; } = new();

        public SaleStageViewModelForCustomer? SaleStageView { get; set; }

        public CampaignViewModelForCustomer? CampaignView { get; set; }

    }

    public class ShowDetailViewModelForCustomerForMobile
    {
        public Guid Id { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public string ShowDescription { get; set; } = string.Empty;
        public string ShowDescriptionDetail { get; set; } = string.Empty;
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public Guid ShowTypeId { get; set; }
        public string ShowTypeName { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string OrganizerImageUrl { get; set; } = string.Empty;
        public Guid OrganizerID { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string DescriptionImageUrl { get; set; } = null!;
        public double LowestPrice { get; set; }
        public Guid? CategoryID { get; set; }
        public string Category { get; set; } = string.Empty;

        public LocationViewModel LocationView { get; set; } = new();

        //public SaleStageViewModelForCustomer? SaleStageView { get; set; }

        //public CampaignViewModelForCustomerForMobile? CampaignView { get; set; }

    }
}
