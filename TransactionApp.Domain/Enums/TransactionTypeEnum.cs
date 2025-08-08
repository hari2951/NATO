using System.Text.Json.Serialization;

namespace TransactionApp.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionTypeEnum
    {
        Debit,
        Credit
    }
}
