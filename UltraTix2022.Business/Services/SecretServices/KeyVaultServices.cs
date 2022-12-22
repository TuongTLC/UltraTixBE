using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace UltraTix2022.API.UltraTix2022.Business.Services.SecretServices
{
    public static class KeyVaultServices
    {
        public static string GetConnectionString()
        {
            string conn = null;
            try
            {
                SecretClientOptions options = new SecretClientOptions()
                {
                    Retry =
                    {
                        Delay= TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                    }
                };
                var client = new SecretClient(new Uri("https://ultratix2022keyvault.vault.azure.net/"), new DefaultAzureCredential(), options);

                KeyVaultSecret secret = client.GetSecret("UltraTixDbConnectionString");

                conn = secret.Value;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return conn;
        }
        public static List<string> GetPaymentSecrets()
        {
            List<string> secrets = new List<string>();
            string partnerCode = null;
            string accessKey = null;
            string secretKey = null;
            try
            {
                SecretClientOptions options = new SecretClientOptions()
                {
                    Retry =
                    {
                        Delay= TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                    }
                };
                var client = new SecretClient(new Uri("https://ultratix2022keyvault.vault.azure.net/"), new DefaultAzureCredential(), options);

                KeyVaultSecret partner = client.GetSecret("PartnerCode");
                partnerCode = partner.Value;
                KeyVaultSecret access = client.GetSecret("AccessKey");
                accessKey = access.Value;
                KeyVaultSecret secret = client.GetSecret("Serectkey");
                secretKey = secret.Value;
                secrets.Add(partnerCode);
                secrets.Add(accessKey);
                secrets.Add(secretKey);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return secrets;
        }
        public static string GetImgConnectionString()
        {
            string connString = null;
            try
            {
                SecretClientOptions options = new SecretClientOptions()
                {
                    Retry =
                            {
                                Delay= TimeSpan.FromSeconds(2),
                                MaxDelay = TimeSpan.FromSeconds(16),
                                MaxRetries = 5,
                                Mode = RetryMode.Exponential
                             }
                };
                var client = new SecretClient(new Uri("https://ultratix2022keyvault.vault.azure.net/"), new DefaultAzureCredential(), options);

                KeyVaultSecret secret = client.GetSecret("ImgConnString");
                connString = secret.Value.ToString();
                return connString;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }

}
