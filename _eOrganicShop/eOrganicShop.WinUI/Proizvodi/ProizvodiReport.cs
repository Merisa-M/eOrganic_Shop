using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eOrganicShop.WinUI.Proizvodi
{
    public partial class ProizvodiReport : UserControl
    {
        private List<Model.Proizvodi> _source;

        public ProizvodiReport(List<Model.Proizvodi> source)
        {
            _source = source;
            InitializeComponent();
        }

        private void ProizvodiReport_Load(object sender, EventArgs e)
        {
            ProizvodiListVMBindingSource.DataSource = _source;
            this.rptProizvodi.RefreshReport();
        }
    }
}
