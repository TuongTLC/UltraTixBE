using System;
namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.TicketType
{
	public class TicketTypeChartView
	{
		public string? TicketTypeName { get; set; }
        public double? TicketTypePrice { get; set; }
        public int? TicketTypeUnitSold { get; set; }
        public int? TicketTypeUnitSoldNormal { get; set; }
        public int? TicketTypeUnitSoldArtist { get; set; }
    }
}

