using Newtonsoft.Json;
using SoftJail.Common;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportCellDto
    {
        
        [JsonProperty("CellNumber")]
        [Range(ValidationConstants.CellMinNumber,ValidationConstants.CellMaxNumber)]
        public int CellNumber { get; set; }

        [JsonProperty("HasWindow")]
        public bool HasWindow { get; set; }
    }
}
