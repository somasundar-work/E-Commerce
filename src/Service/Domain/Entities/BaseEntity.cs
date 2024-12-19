using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain;

public class BaseEntity
{
    [JsonProperty("isActive")]
    public bool IsActive { get; set; }

    [JsonProperty("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonProperty("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonProperty("lastUpdated")]
    public DateTime LastUpdated { get; set; }

    [JsonProperty("createdBy")]
    public DateTime Createdby { get; set; }

    [JsonProperty("lastUpdatedBy")]
    public DateTime LastUpdatedBy { get; set; }
}
