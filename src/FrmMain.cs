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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using ByteSizeLib;
using Newtonsoft.Json;
using PackageStore.Enums;
using PackageStore.Exceptions;
using RestSharp;

namespace PackageStore
{
  public partial class frmMain : Form
  {
    // details https://www.psdevwiki.com/ps3/Environments
    private readonly string[] Environments = {
      "https://a0.ww.np.dl.playstation.net/tpl/np/",
      "http://b0.ww.np.dl.playstation.net/tppkg/np/",
      "https://a0.ww.sp-int.dl.playstation.net/tpl/sp-int/",
      "http://b0.ww.sp-int.dl.playstation.net/tppkg/sp-int/",
      "https://a0.ww.prod-qa.dl.playstation.net/tpl/prod-qa/"
    };

    private Platform CurrentPlatform = Platform.PS3;
    private List<Package> PackageItems = new List<Package>();

    public new bool UseWaitCursor {
      get => base.UseWaitCursor;

      set {
        base.UseWaitCursor = value;

        this.buttonSearch.Enabled = !value;
        this.textBoxPackageId.Enabled = !value;
        this.checkBoxForce.Enabled = !value;
        this.comboBoxEnvironments.Enabled = !value;
        this.comboBoxPlatform.Enabled = !value;
        this.listViewPackage.Enabled = !value;
      }
    }

    public frmMain()
    {
      this.InitializeComponent();
      this.comboBoxPlatform.DataSource = Enum.GetNames(typeof(Platform));
      ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
    }

    private async void ButtonSearch_Click(object sender, EventArgs e)
    {
      try
      {
        this.Text = Application.ProductName;
        this.UseWaitCursor = true;
        this.listViewPackage.Items.Clear();

        if (!this.IsValid(this.CurrentPlatform, this.textBoxPackageId.Text))
          throw new InvalidPackageException($"The package id '{this.textBoxPackageId.Text}' is Invalid");

        string packageId = this.textBoxPackageId.Text;
        string environments = this.comboBoxEnvironments.Text;

        this.PackageItems = await Task.Run(() => this.PackageSearch(this.CurrentPlatform, packageId, environments));
        this.PackageItems.ForEach(x =>
        {
          ListViewItem item = new ListViewItem { Text = x.Name };
          item.SubItems.AddRange(new[] {
            x.Size.ToString(),
            x.Version,
            !string.IsNullOrEmpty(x.SystemVersion) ? x.SystemVersion : x.SupportVersion,
            x.Hash,
            x.Digest
          });
          this.listViewPackage.Items.Add(item);
        });
      }
      catch (PackageNotFoundException ex)
      {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        this.UseWaitCursor = false;
      }
    }

    private List<Package> PackageSearch(Platform type, string name, string environments = "NP")
    {
      List<Package> items = new List<Package>();

      if (type == Platform.PS4)
      {
        RestClient client = new RestClient($"https://ps4.octolus.net");
        RestRequest request = new RestRequest($"https://ps4.octolus.net/dataApi?id={ name }&env={ environments }&method=patches", DataFormat.Json);
        IRestResponse response = client.Get(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
          PS4DatabasePatches patches = JsonConvert.DeserializeObject<PS4DatabasePatches>(response.Content);
          items.Add(new Package
          {
            Name = "Latest Delta",
            Hash = "Unknown",
            Digest = patches.Tag.Package.Attributes.Digest,
            Platform = Platform.PS4,
            Url = new Uri(patches.Tag.Package.DeltaInfoSet.Attributes.Url),
            Version = patches.Tag.Package.Attributes.Version,
            SupportVersion = "",
            SystemVersion = patches.Tag.Package.Attributes.SystemVersion,
            Size = ByteSize.FromMegaBytes(0)
          });

          this.ManifestPS4(new Uri(patches.Tag.Package.Attributes.ManifestUrl), ref items);
        }
      }
      else if (type == Platform.PS3)
      {
        foreach (string url in this.Environments)
        {
          try
          {
            XmlTextReader reader = new XmlTextReader(url + name + "/" + name + "-ver.xml");
            do
            {
              if (reader.NodeType == XmlNodeType.Element && reader.Name == "package")
                items.Add(this.AttributeReaderPS3(ref reader));
            } while (reader.Read());
          }
          catch (WebException ex)
          {
            HttpWebResponse response = (HttpWebResponse)ex.Response;
            switch (response.StatusCode)
            {
              case HttpStatusCode.NotFound:
              case HttpStatusCode.Forbidden:
                break;

              default:
              {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                break;
              }
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }

      return items;
    }

    private void ManifestPS4(Uri manifestUri, ref List<Package> items)
    {
      RestClient client = new RestClient(manifestUri.GetLeftPart(UriPartial.Authority));
      RestRequest request = new RestRequest(manifestUri.PathAndQuery, DataFormat.Json);
      IRestResponse response = client.Get(request);
      if (response.StatusCode != HttpStatusCode.OK)
        throw new ManifestNotFoundException("Manifest notfound");

      PlayStationManifest manifest = JsonConvert.DeserializeObject<PlayStationManifest>(response.Content);
      foreach(PlayStationManifest.PiecesObject item in manifest.Pieces)
      {
        items.Add(new Package {
          Name = Path.GetFileName(item.Url),
          Hash = item.Hash,
          Platform = Platform.PS4,
          Size = ByteSize.FromBytes(double.Parse(item.FileSize)),
          Url = new Uri(item.Url),
          Digest = manifest.PackageDigest,
        });
      }
    }

    private Package AttributeReaderPS3(ref XmlTextReader reader)
    {
      Package package = new Package();

      for (int i = 0; i < reader.AttributeCount; i++)
      {
        reader.MoveToNextAttribute();
        switch (reader.Name)
        {
          case "version":
          {
            package.Version = reader.Value;
            break;
          }

          case "size":
          {
            package.Size = ByteSize.FromBytes(double.Parse(reader.Value.ToString()));
            break;
          }

          case "sha1sum":
          {
            package.Hash = reader.Value;
            break;
          }

          case "ps3_system_ver":
          {
            package.SupportVersion = reader.Value;
            break;
          }

          case "url":
          {
            package.Name = Path.GetFileName(reader.Value);
            package.Url = new Uri(reader.Value);
            break;
          }
        }
      }

      return package;
    }

    private bool IsValid(Platform type, string name)
    {
      if (string.IsNullOrWhiteSpace(name)) return false;

      if (type == Platform.PS3)
      {
        return System.Text.RegularExpressions.Regex.IsMatch(name, "^[A-Z0-9]");
      }
      else if (type == Platform.PS4)
      {
        RestClient client = new RestClient($"https://ps4.octolus.net");
        RestRequest request = new RestRequest($"submit_api?id={ name }", DataFormat.Json);
        IRestResponse<PS4DatabaseValid> response = client.Get<PS4DatabaseValid>(request);
        return (response.StatusCode == HttpStatusCode.OK && response.Data.Success);
      }

      return false;
    }

    private void ListViewPackages_ItemActivate(object sender, EventArgs e)
    {
      if (this.listViewPackage.SelectedIndices.Count == 0) return;

      try
      {
        foreach (ListViewItem item in this.listViewPackage.SelectedItems)
          Process.Start(this.PackageItems[item.Index].Url.ToString());
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show("PS4 database by @OctolusNET\n\rMade by Coreizer", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void GithubToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Process.Start("https://www.github.com/coreizer/PackageStore");
    }

    private void CopyToURLToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.listViewPackage.SelectedIndices.Count == 0) return;

      try
      {
        Clipboard.SetText(this.PackageItems[this.listViewPackage.SelectedItems[0].Index].Url.ToString());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void comboBoxPlatform_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.CurrentPlatform = EnumsNET.Enums.Parse<Platform>(this.comboBoxPlatform.SelectedItem.ToString());
      this.comboBoxEnvironments.Visible = (this.CurrentPlatform == Platform.PS4);
      this.comboBoxEnvironments.SelectedIndex = 0;
    }
  }
}
