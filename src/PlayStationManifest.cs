#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2022 coreizer
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

namespace PackageStore
{
   using System.Collections.Generic;
   using Newtonsoft.Json;

   public class PlayStationManifest
   {
      [JsonProperty("originalFileSize")]
      public string OriginalFileSize { get; set; }

      [JsonProperty("packageDigest")]
      public string PackageDigest { get; set; }

      [JsonProperty("pieces")]
      public List<PiecesObject> Pieces = new List<PiecesObject>();

      [JsonObject("pieces")]
      public class PiecesObject
      {
         [JsonProperty("url")]
         public string Url { get; set; }

         [JsonProperty("fileOffset")]
         public string FileOffset { get; set; }

         [JsonProperty("fileSize")]
         public string FileSize { get; set; }

         [JsonProperty("hashValue")]
         public string Hash { get; set; }
      }
   }
}
