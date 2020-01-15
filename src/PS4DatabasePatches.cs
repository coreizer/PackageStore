using Newtonsoft.Json;

namespace PackageStore
{
  public class PS4DatabasePatches
  {
    [JsonProperty("error")]
    public bool Error {
      get; set;
    }

    [JsonProperty("tag")]
    public TagObject Tag = new TagObject();

    [JsonObject("tag")]
    public class TagObject
    {
      [JsonProperty("package")]
      public PackageObject Package {
        get;set;
      }

      [JsonObject("package")]
      public class PackageObject
      {
        [JsonProperty("@attributes")]
        public AttributesObject Attributes = new AttributesObject();

        [JsonObject("@attributes")]
        public class AttributesObject
        {
          [JsonProperty("version")]
          public string Version {
            get; set;
          }

          [JsonProperty("size")]
          public string Size {
            get; set;
          }

          [JsonProperty("digest")]
          public string Digest {
            get; set;
          }

          [JsonProperty("manifest_url")]
          public string ManifestUrl {
            get; set;
          }

          [JsonProperty("content_id")]
          public string ContentId {
            get; set;
          }

          [JsonProperty("system_ver")]
          public string SystemVersion {
            get; set;
          }
        }

        [JsonProperty("delta_info_set")]
        public DeltaInfoSetObject DeltaInfoSet {
          get;set;
        }

        [JsonObject("delta_info_set")]
        public class DeltaInfoSetObject
        {
          [JsonProperty("@attributes")]
          public AttributesObject Attributes = new AttributesObject();

          [JsonObject("@attributes")]
          public class AttributesObject
          {
            [JsonProperty("url")]
            public string Url {
              get; set;
            }
          }
        }
      }
    }
  }
}
