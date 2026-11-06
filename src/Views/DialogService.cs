#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2026 coreizer
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

using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using PackageStore.Services;

namespace PackageStore.Views;

/// <summary>
/// <see cref="IDialogService"/> の Avalonia 実装。オーナーウィンドウは遅延解決します。
/// </summary>
public sealed class DialogService : IDialogService {
  private readonly Func<Window> _ownerResolver;

  public DialogService(Func<Window> ownerResolver) {
    this._ownerResolver = ownerResolver;
  }

  private Window Owner => this._ownerResolver();

  public Task ShowMessageAsync(string heading, string text, MessageKind kind = MessageKind.Information) =>
    MessageDialog.ShowAsync(this.Owner, heading, text, kind);

  public Task<bool> ConfirmAsync(string heading, string text, MessageKind kind = MessageKind.Warning) =>
    MessageDialog.ConfirmAsync(this.Owner, heading, text, kind);

  public async Task<string?> PickFolderAsync() {
    var result = await this.Owner.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions {
      AllowMultiple = false
    });
    return result.Count > 0 ? result[0].TryGetLocalPath() : null;
  }

  public async Task<string?> PickSaveFileAsync(string suggestedFileName, string filterName, string extension) {
    var file = await this.Owner.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions {
      SuggestedFileName = suggestedFileName,
      DefaultExtension = extension,
      ShowOverwritePrompt = true,
      FileTypeChoices = [new FilePickerFileType(filterName) { Patterns = [$"*.{extension}"] }]
    });
    return file?.TryGetLocalPath();
  }

  public Task SetClipboardTextAsync(string text) =>
    this.Owner.Clipboard?.SetTextAsync(text) ?? Task.CompletedTask;
}
