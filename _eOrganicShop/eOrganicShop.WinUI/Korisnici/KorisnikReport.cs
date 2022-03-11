using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eOrganicShop.WinUI.Korisnici
{
    public partial class KorisnikReport : UserControl
    {
        private List<Model.Korisnici> _source;

        public KorisnikReport(List<Model.Korisnici> source)
        {
            _source = source;
            InitializeComponent();
        }

        private void KorisnikReport_Load(object sender, EventArgs e)
        {

            KorisnikListVMBindingSource.DataSource = _source;
            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

  
        }
    }
}
