﻿#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2023 coreizer
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
   using System;
   using System.Windows.Forms;

   internal class Utils
   {
      public static string SelectDirectoryPath(bool isForce = false)
      {
         if (!isForce && !string.IsNullOrEmpty(Properties.Settings.Default.DirectoryPath))
            return Properties.Settings.Default.DirectoryPath;

         try {
            using (var FBD = new FolderBrowserDialog()) {
               if (FBD.ShowDialog() != DialogResult.OK) return null;
               Properties.Settings.Default.DirectoryPath = FBD.SelectedPath;
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Environment.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally {
            Properties.Settings.Default.Save();
         }
         return Properties.Settings.Default.DirectoryPath;
      }
   }
}
