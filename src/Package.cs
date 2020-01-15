/*
 * Copyright (c) 2017-2019 Coreizer
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
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using ByteSizeLib;

namespace PackageStore
{

  public class Package
  {
    public string Name = "Unknown";
    public string Version = "Unknown";
    public string SystemVersion = "Unknown";
    public string SupportVersion = "Unknown";
    public ByteSize Size;
    public string Digest = "Unknown";
    public string Hash = "Unknown";
    public Uri Url;
    public Enums.Platform Platform;
  }
}
