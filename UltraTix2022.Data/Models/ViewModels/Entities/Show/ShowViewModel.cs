namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show
{
    public class ShowViewModel
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
        public Guid OrganizerID { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string DescriptionImageUrl { get; set; } = null!;
        public double LowestPrice { get; set; }
        public string Location { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public Guid? CategoryID { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Step { get; set; } = 1;
        public DateTime CreationDate { get; set; }
        public bool IsShowHappening { get; set; } = false;
        public bool IsShowComming { get; set; } = false;
        public double? SaleOff { get; set; }
    }

    public class ShowViewModelForArtist : ShowViewModel
    {
        public string BookingLink { get; set; } = string.Empty;
        public double TotalArtistCommission { get; set; } = 0;
        public int TotalTicketSell { get; set; } = 0;
        public int TotalTicketSold { get; set; } = 0;
        public bool IsAdminTransferCommission { get; set; } = false;
    }

    public class ShowOrdersOverviewModel
    {
        public Guid Id { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = null!;
        //public string ShowDescription { get; set; } = string.Empty;
        //public string ShowDescriptionDetail { get; set; } = string.Empty;
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        //public Guid ShowTypeId { get; set; }
        //public string ShowTypeName { get; set; } = string.Empty;
        //public string OrganizerName { get; set; } = string.Empty;
        //public Guid OrganizerID { get; set; }     
        public string Status { get; set; } = null!;
        //public string DescriptionImageUrl { get; set; } = null!;
        //public double LowestPrice { get; set; }
        //public string Location { get; set; } = string.Empty;
        //public string StartDate { get; set; } = string.Empty;
        //public string EndDate { get; set; } = string.Empty;
        //public Guid? CategoryID { get; set; }
        //public string Category { get; set; } = string.Empty;
        //public int Step { get; set; } = 1;
        //public DateTime CreationDate { get; set; }
        //public bool IsShowHappening { get; set; } = false;
        //public bool IsShowComming { get; set; } = false;
        //public double? SaleOff { get; set; }
        public int NumberOrders { get; set; } = 0;

    }
}
