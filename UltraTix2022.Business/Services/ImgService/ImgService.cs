using Azure.Storage.Blobs;
using UltraTix2022.API.UltraTix2022.Business.Services.SecretServices;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
namespace UltraTix2022.API.UltraTix2022.Business.Services.ImgService
{
    public class ImgService
    {
        public async Task<List<string>> UploadImage(IList<IFormFile> files)
        {
            List<string> urls = new List<string>();
            try
            {

                BlobContainerClient blobContainerClient = new BlobContainerClient(KeyVaultServices.GetImgConnectionString(), "ultratixshowimg");
                foreach (IFormFile file in files)
                {
                    using (var stream = new MemoryStream())
                    {
                        Guid id = Guid.NewGuid();
                        string format = Path.GetExtension(file.FileName);
                        await file.CopyToAsync(stream);
                        stream.Position = 0;
                        await blobContainerClient.UploadBlobAsync($"{id}{format}", stream);
                        urls.Add("https://ultratiximg.blob.core.windows.net/ultratixshowimg/" + id + format);
                    }


                }
                return urls;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

        }

        public async Task<string?> UploadAnImage(IFormFile file)
        {
            string currentYear = DateTime.Now.Year.ToString();
            string currentMonth = DateTime.Now.Month.ToString();
            try
            {
                Guid id = Guid.NewGuid();
                string format = Path.GetExtension(file.FileName);

                BlobContainerClient blobContainerClient = new BlobContainerClient(KeyVaultServices.GetImgConnectionString(), "ultratixshowimg");
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    await blobContainerClient.UploadBlobAsync($"{id}{format}", stream);
                }
                string imgURL = "https://ultratiximg.blob.core.windows.net/ultratixshowimg/" + id + format;
                return imgURL;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Upload Image" + file.FileName + "Failed -----" + e.Message);
            }
        }
        public async Task DeleteImages(List<string> fileURLs)
        {
            if (!fileURLs.Any()) throw new ArgumentException("URL list is empty!");
            try
            {

                string removePart = "https://ultratiximg.blob.core.windows.net/ultratixshowimg/";
                BlobContainerClient blobContainerClient = new BlobContainerClient(KeyVaultServices.GetImgConnectionString(), "ultratixshowimg");
                foreach (string file in fileURLs)
                {
                    await blobContainerClient.DeleteBlobAsync(file.Replace(removePart, ""));
                }

            }
            catch (Exception e)
            {
                throw new ArgumentException("Image Not Found" + e.Message);
            }
        }


        public FileData? GetImage(string fileName)
        {
            try
            {
                BlobClient blobClient = new BlobClient(KeyVaultServices.GetImgConnectionString(), "ultratixshowimg", fileName);

                using (var stream = new MemoryStream())
                {
                    blobClient.DownloadToAsync(stream);
                    stream.Position = 0;
                    var contenType = (blobClient.GetProperties()).Value.ContentType;
                    return new FileData(stream.ToArray(), contenType, blobClient.Name);
                    //return file;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
}
