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
   using System;
   using System.Windows.Forms;

   static class Program
   {
      /// <summary>
      /// アプリケーションのメイン エントリ ポイントです。
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);

         // ネットワークが利用可能どうかを確認します。
         if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
            throw new System.Net.NetworkInformation.NetworkInformationException();
         }

         Application.Run(new frmMain());
      }
   }
}
