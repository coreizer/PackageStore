#region License Information (GPL v3)

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
   partial class frmAbout
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
         pictureBoxBrand = new PictureBox();
         labelName = new Label();
         labelVersion = new Label();
         labelAuthor = new Label();
         ((System.ComponentModel.ISupportInitialize)pictureBoxBrand).BeginInit();
         this.SuspendLayout();
         // 
         // pictureBoxBrand
         // 
         pictureBoxBrand.Image = (Image)resources.GetObject("pictureBoxBrand.Image");
         pictureBoxBrand.Location = new Point(12, 12);
         pictureBoxBrand.Name = "pictureBoxBrand";
         pictureBoxBrand.Size = new Size(58, 58);
         pictureBoxBrand.SizeMode = PictureBoxSizeMode.StretchImage;
         pictureBoxBrand.TabIndex = 0;
         pictureBoxBrand.TabStop = false;
         // 
         // labelName
         // 
         labelName.AutoSize = true;
         labelName.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
         labelName.Location = new Point(86, 12);
         labelName.Name = "labelName";
         labelName.Size = new Size(106, 20);
         labelName.TabIndex = 1;
         labelName.Text = "Package Store";
         // 
         // labelVersion
         // 
         labelVersion.AutoSize = true;
         labelVersion.Location = new Point(86, 45);
         labelVersion.Name = "labelVersion";
         labelVersion.Size = new Size(63, 15);
         labelVersion.TabIndex = 2;
         labelVersion.Text = "Version : ...";
         // 
         // labelAuthor
         // 
         labelAuthor.AutoSize = true;
         labelAuthor.Location = new Point(219, 78);
         labelAuthor.Name = "labelAuthor";
         labelAuthor.Size = new Size(97, 15);
         labelAuthor.TabIndex = 3;
         labelAuthor.Text = "Made by coreizer";
         // 
         // frmAbout
         // 
         this.AutoScaleDimensions = new SizeF(7F, 15F);
         this.AutoScaleMode = AutoScaleMode.Font;
         this.ClientSize = new Size(328, 102);
         this.Controls.Add(labelAuthor);
         this.Controls.Add(labelVersion);
         this.Controls.Add(labelName);
         this.Controls.Add(pictureBoxBrand);
         this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
         this.Icon = (Icon)resources.GetObject("$this.Icon");
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmAbout";
         this.StartPosition = FormStartPosition.CenterParent;
         this.Text = "About";
         ((System.ComponentModel.ISupportInitialize)pictureBoxBrand).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();
      }

      #endregion

      private PictureBox pictureBoxBrand;
      private Label labelName;
      private Label labelVersion;
      private Label labelAuthor;
   }
}