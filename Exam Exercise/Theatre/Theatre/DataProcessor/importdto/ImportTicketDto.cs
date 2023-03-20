using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto;

[JsonObject]
public class ImportTicketDto
{
    [JsonProperty("Price")]
    [Range(typeof(decimal), ValidationConstants.TicketMinPrice, ValidationConstants.TicketMaxPrice)]
    public decimal Price { get; set; }

    [JsonProperty("RowNumber")]
    [Range(ValidationConstants.TicketMinRowNumber, ValidationConstants.TicketMaxRowNumber)]
    public sbyte RowNumber { get; set; }

    public int PlayId { get; set; }
}
