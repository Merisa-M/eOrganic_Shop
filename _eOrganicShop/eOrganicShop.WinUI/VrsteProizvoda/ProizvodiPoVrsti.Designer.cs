
namespace eOrganicShop.WinUI.VrsteProizvoda
{
    partial class ProizvodiPoVrsti
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
            this.rptVrsta = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ProizvodiListVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ProizvodiListVMBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rptVrsta
            // 
            reportDataSource1.Name = "ProizvodDb";
            reportDataSource1.Value = this.ProizvodiListVMBindingSource;
            this.rptVrsta.LocalReport.DataSources.Add(reportDataSource1);
            this.rptVrsta.LocalReport.ReportEmbeddedResource = "eOrganicShop.WinUI.Izvjestaji.ProizvodiPoVrstiReport.rdlc";
            this.rptVrsta.Location = new System.Drawing.Point(0, 31);
            this.rptVrsta.Name = "rptVrsta";
            this.rptVrsta.ServerReport.BearerToken = null;
            this.rptVrsta.Size = new System.Drawing.Size(580, 357);
            this.rptVrsta.TabIndex = 0;
            // 
            // ProizvodiListVMBindingSource
            // 
            this.ProizvodiListVMBindingSource.DataSource = typeof(eOrganicShop.Model.ViewModel.ProizvodiListVM);
            // 
            // ProizvodiPoVrsti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rptVrsta);
            this.Name = "ProizvodiPoVrsti";
            this.Size = new System.Drawing.Size(718, 472);
            ((System.ComponentModel.ISupportInitialize)(this.ProizvodiListVMBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rptVrsta;
        private System.Windows.Forms.BindingSource ProizvodiListVMBindingSource;
    }
}
