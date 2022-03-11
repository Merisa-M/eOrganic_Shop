
namespace eOrganicShop.WinUI.Proizvodi
{
    partial class ProizvodiReport
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.rptProizvodi = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ProizvodiListVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ProizvodiListVMBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rptProizvodi
            // 
            reportDataSource2.Name = "ProizvodiDataSet";
            reportDataSource2.Value = this.ProizvodiListVMBindingSource;
            this.rptProizvodi.LocalReport.DataSources.Add(reportDataSource2);
            this.rptProizvodi.LocalReport.ReportEmbeddedResource = "eOrganicShop.WinUI.Izvještaji.ProizvodiIzvjestaj.rdlc";
            this.rptProizvodi.Location = new System.Drawing.Point(0, 0);
            this.rptProizvodi.Name = "rptProizvodi";
            this.rptProizvodi.ServerReport.BearerToken = null;
            this.rptProizvodi.Size = new System.Drawing.Size(895, 411);
            this.rptProizvodi.TabIndex = 0;
            // 
            // ProizvodiListVMBindingSource
            // 
            this.ProizvodiListVMBindingSource.DataSource = typeof(eOrganicShop.Model.ViewModel.ProizvodiListVM);
            // 
            // ProizvodiReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rptProizvodi);
            this.Name = "ProizvodiReport";
            this.Size = new System.Drawing.Size(913, 414);
            this.Load += new System.EventHandler(this.ProizvodiReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProizvodiListVMBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rptProizvodi;
        private System.Windows.Forms.BindingSource ProizvodiListVMBindingSource;
    }
}
