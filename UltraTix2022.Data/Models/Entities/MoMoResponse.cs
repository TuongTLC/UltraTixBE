using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class MoMoResponse
    {
        public Guid Id { get; set; }
        public string? OrderId { get; set; }
        public string? PartnerCode { get; set; }
        public string? RequestId { get; set; }
        public string? Amount { get; set; }
        public string? OrderInfo { get; set; }
        public string? OrderType { get; set; }
        public string? TransId { get; set; }
        public string? ResultCode { get; set; }
        public string? Message { get; set; }
        public string? PayType { get; set; }
        public string? ResponseTime { get; set; }
        public string? Signature { get; set; }
        public string? ExtraData { get; set; }
    }
}
