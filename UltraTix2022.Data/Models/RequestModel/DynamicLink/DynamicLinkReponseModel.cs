namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.DynamicLink
{
    public class DynamicLinkReponseModel
    {
        public string shortLink { get; set; } = string.Empty;
        public List<Warning> warning { get; set; } = new();
        public string previewLink { get; set; } = string.Empty;
    }

    public class Warning
    {
        public string warningCode { get; set; } = string.Empty;
        public string warningMessage { get; set; } = string.Empty;
    }
}
