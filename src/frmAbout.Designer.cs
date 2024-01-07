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
         this.pictureBoxBrand = new PictureBox();
         this.labelName = new Label();
         this.labelVersion = new Label();
         this.labelAuthor = new Label();
         ((System.ComponentModel.ISupportInitialize)this.pictureBoxBrand).BeginInit();
         this.SuspendLayout();
         // 
         // pictureBoxBrand
         // 
         this.pictureBoxBrand.Image = (Image)resources.GetObject("pictureBoxBrand.Image");
         this.pictureBoxBrand.Location = new Point(12, 12);
         this.pictureBoxBrand.Name = "pictureBoxBrand";
         this.pictureBoxBrand.Size = new Size(58, 58);
         this.pictureBoxBrand.SizeMode = PictureBoxSizeMode.StretchImage;
         this.pictureBoxBrand.TabIndex = 0;
         this.pictureBoxBrand.TabStop = false;
         // 
         // labelName
         // 
         this.labelName.AutoSize = true;
         this.labelName.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
         this.labelName.Location = new Point(86, 12);
         this.labelName.Name = "labelName";
         this.labelName.Size = new Size(106, 20);
         this.labelName.TabIndex = 1;
         this.labelName.Text = "Package Store";
         // 
         // labelVersion
         // 
         this.labelVersion.AutoSize = true;
         this.labelVersion.Location = new Point(86, 45);
         this.labelVersion.Name = "labelVersion";
         this.labelVersion.Size = new Size(63, 15);
         this.labelVersion.TabIndex = 2;
         this.labelVersion.Text = "Version : ...";
         // 
         // labelAuthor
         // 
         this.labelAuthor.AutoSize = true;
         this.labelAuthor.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
         this.labelAuthor.Location = new Point(235, 81);
         this.labelAuthor.Name = "labelAuthor";
         this.labelAuthor.Size = new Size(81, 12);
         this.labelAuthor.TabIndex = 3;
         this.labelAuthor.Text = "Made by coreizer";
         // 
         // frmAbout
         // 
         this.AutoScaleDimensions = new SizeF(7F, 15F);
         this.AutoScaleMode = AutoScaleMode.Font;
         this.ClientSize = new Size(328, 102);
         this.Controls.Add(this.labelAuthor);
         this.Controls.Add(this.labelVersion);
         this.Controls.Add(this.labelName);
         this.Controls.Add(this.pictureBoxBrand);
         this.DoubleBuffered = true;
         this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
         this.FormBorderStyle = FormBorderStyle.FixedSingle;
         this.Icon = (Icon)resources.GetObject("$this.Icon");
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmAbout";
         this.SizeGripStyle = SizeGripStyle.Hide;
         this.StartPosition = FormStartPosition.CenterParent;
         this.Text = "About";
         ((System.ComponentModel.ISupportInitialize)this.pictureBoxBrand).EndInit();
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