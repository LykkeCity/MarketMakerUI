using System.Text;
using Lykke.AzureStorage.Blob;
using Lykke.Common;

namespace LykkeMarketMakers.AzureRepositories
{
    public static class GeneralSettingsReader
    {
        public static T ReadGeneralSettings<T>(string connectionString, string container, string fileName)
        {
            var settingsStorage = new AzureBlobStorage(connectionString);
            var settingsData = settingsStorage.GetAsync(container, fileName).Result.ToBytes();
            var str = Encoding.UTF8.GetString(settingsData);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
