using System;
using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.AzureStorage;
using LykkeMarketMakers.AzureRepositories.Entities;

namespace LykkeMarketMakers.AzureRepositories.Log
{
    public class LogToTableRepository : ILog
    {
        private readonly INoSQLTableStorage<LogEntity> _tableStorage;

        public LogToTableRepository(INoSQLTableStorage<LogEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        private async Task Insert(string level, string component, string process, string context, string type, string stack,
            string msg, DateTime? dateTime)
        {
                var dt = dateTime ?? DateTime.UtcNow;
                var newEntity = LogEntity.Create(level, component, process, context, type, stack, msg, dt);
                await _tableStorage.InsertAndGenerateRowKeyAsTimeAsync(newEntity, dt);
        }

        public async Task WriteInfoAsync(string component, string process, string context, string info, DateTime? dateTime = null)
        {
            await Insert("info", component, process, context, null, null, info, dateTime);
        }

        public async Task WriteWarningAsync(string component, string process, string context, string info, DateTime? dateTime = null)
        {
            await Insert("warning", component, process, context, null, null, info, dateTime);
        }

        public async Task WriteErrorAsync(string component, string process, string context, Exception type, DateTime? dateTime = null)
        {
            await Insert("error", component, process, context, type.GetType().ToString(), type.StackTrace, type.Message, dateTime);
        }

        public async Task WriteFatalErrorAsync(string component, string process, string context, Exception type, DateTime? dateTime = null)
        {
            await Insert("fatalerror", component, process, context, type.GetType().ToString(), type.StackTrace, type.Message, dateTime);
        }

        public int Count { get { return 0; } }
    }
}
