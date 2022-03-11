using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eOrganicShop.WinUI.Transakcije
{
    public partial class TranksacijeIzvjestaj : UserControl
    {
        private List<Model.Transakcije> _source;
        public TranksacijeIzvjestaj(List<Model.Transakcije> source)
        {
            _source = source;
            InitializeComponent();
        }

        private void TranksacijeIzvjestaj_Load(object sender, EventArgs e)
        {
            TransakcijeListVMBindingSource.DataSource = _source;
            this.reportViewer1.RefreshReport();
        }
    }
}
