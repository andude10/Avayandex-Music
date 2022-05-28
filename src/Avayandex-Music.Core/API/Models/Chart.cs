using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Yandex.Music.Api.Models.Playlist;

namespace Avayandex_Music.Core.API.Models;

public class Chart
{
    public string Id { get; set; }
    
    public string Type { get; set; }
    
    public string TypeForFrom { get; set; }
    
    public string Title { get; set; }
    
    public string ChartDescription { get; set; }
    
    [JsonProperty("chart")]
    public YPlaylist ChartPlaylist { get; set; }
}