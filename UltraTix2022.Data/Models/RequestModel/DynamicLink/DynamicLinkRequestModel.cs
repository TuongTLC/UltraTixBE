namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.DynamicLink
{
    public class DynamicLinkRequestModel
    {
        public DynamicLinkInfo dynamicLinkInfo { get; set; } = new();
    }

    public class DynamicLinkInfo
    {
        public string domainUriPrefix { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public AndroidInfo androidInfo { get; set; } = new();
        public IosInfo iosInfo { get; set; } = new();
    }

    public class AndroidInfo
    {
        public string androidPackageName { get; set; } = string.Empty;
    }

    public class IosInfo
    {
        public string iosBundleId { get; set; } = string.Empty;
    }
}
