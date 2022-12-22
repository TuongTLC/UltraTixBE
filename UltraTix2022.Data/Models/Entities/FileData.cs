namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public class FileData
    {
        public FileData(byte[]? bytes, string? contenType, string? name)
        {
            this.bytes = bytes;
            this.contenType = contenType;
            this.name = name;
        }

        public byte[]? bytes { get; set; }
        public string? contenType { get; set; }
        public string? name { get; set; }

    }
}
