using System.Collections.Generic;
using Newtonsoft.Json;

namespace PackageStore
{
  public class PlayStationManifest
  {
    [JsonProperty("originalFileSize")]
    public string OriginalFileSize {
      get;set;
    }

    [JsonProperty("packageDigest")]
    public string PackageDigest {
      get; set;
    }

    [JsonProperty("pieces")]
    public List<PiecesObject> Pieces = new List<PiecesObject>();

    [JsonObject("pieces")]
    public class PiecesObject
    {
      [JsonProperty("url")]
      public string Url {
        get; set;
      }

      [JsonProperty("fileOffset")]
      public string FileOffset {
        get; set;
      }

      [JsonProperty("fileSize")]
      public string FileSize {
        get; set;
      }

      [JsonProperty("hashValue")]
      public string Hash {
        get; set;
      }
    }
  }
}
