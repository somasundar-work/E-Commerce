using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Product : BaseEntity
{
    [Key]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("pictureUrl")]
    public string? PictureUrl { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("brand")]
    public string? Brand { get; set; }
}