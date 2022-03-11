
namespace eOrganicShop.WinUI.Korisnici
{
    partial class KorisnikReport
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.KorisnikListVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.KorisnikListVMBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // KorisnikListVMBindingSource
            // 
            this.KorisnikListVMBindingSource.DataSource = typeof(eOrganicShop.Model.ViewModel.KorisnikListVM);
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "KorisnikDataSet";
            reportDataSource1.Value = this.KorisnikListVMBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "eOrganicShop.WinUI.Izvjestaji.KorisnikReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(37, 21);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(876, 365);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // KorisnikReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.reportViewer1);
            this.Name = "KorisnikReport";
            this.Size = new System.Drawing.Size(965, 449);
            this.Load += new System.EventHandler(this.KorisnikReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.KorisnikListVMBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource KorisnikListVMBindingSource;
    }
}
