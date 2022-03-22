using eOrganicShop.Model.Request;
using eOrganicShop.WinUI.Helper;
using eOrganicShop.WinUI.Main;
using eOrganicShop.WinUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eOrganicShop.WinUI.Korisnici
{
    public partial class RacunEdit : UserControl
    {
        private readonly APIService korisnikService = new APIService("Korisnici");
        private readonly APIService ulogaService = new APIService("Uloge");

        string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
        private readonly int _ID;

        public RacunEdit(int ID)
        {
            _ID = ID;

            InitializeComponent();
        }

        private async void RacunEdit_Load(object sender, EventArgs e)
        {
            var korisnik = await korisnikService.GetById<Model.Korisnici>(_ID);


            txtIme.Text = korisnik.Ime;
            txtPrezime.Text = korisnik.Prezime;
            txtEmail.Text = korisnik.Email;
            txtTelefon.Text = korisnik.Telefon;
            txtKorisnickoIme.Text = korisnik.KorisnickoIme;

            var uloge = await ulogaService.Get<List<Model.Uloge>>(null);
            clbUloge.DataSource = uloge;
            clbUloge.ValueMember = "UlogaID";
            clbUloge.DisplayMember = "Naziv";
            //clbUloge.SelectionMode = SelectionMode.None;

            if (korisnik.Image.Length > 3)
            {
                pbSlika.Image = ImageHelper.ByteArrayToSystemDrawing(korisnik.Image);
                pbSlika.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            var rolesList = clbUloge.Items.Cast<Model.Uloge>().Select(i => i.UlogaID).ToList();
            foreach (var userRole in korisnik.KorisnikUloge)
            {
                for (int i = 0; i < clbUloge.Items.Count; i++)
                {
                    if (rolesList[i] == userRole.UlogeID)
                    {
                        clbUloge.SetItemChecked(i, true);
                    }
                }
            }
        }

        private async void btnSacuvaj_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                var roleList = clbUloge.CheckedItems.Cast<Model.Uloge>().Select(x => x.UlogaID).ToList();

                List<int> uncheckedUloge = new List<int>();
                for (int i = 0; i < clbUloge.Items.Count; i++)
                {
                    if (!clbUloge.GetItemChecked(i))
                    {
                        int RoleID = clbUloge.Items.Cast<Model.Uloge>().ToList()[i].UlogaID;
                        uncheckedUloge.Add(RoleID);
                    }
                }
                var request = new KorisnikUpsertRequest
                {
                    Ime = txtIme.Text,
                    Prezime = txtPrezime.Text,
                    KorisnickoIme = txtKorisnickoIme.Text,
                    Email = txtEmail.Text,
                    Telefon = txtTelefon.Text,
                    Image = pbSlika.Image != null ? ImageHelper.SystemDrawingToByteArray(pbSlika.Image) : null,
                    Uloge = roleList,
                UlogeBrisanje = uncheckedUloge

                };

                await korisnikService.Update<Model.Korisnici>(_ID, request);
                MessageBox.Show("Uspjesno editovano!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PanelHelper.SwapPanels(this.Parent, this, new Home());
            }
        }

        private void pbSlika_Click(object sender, EventArgs e)
        {

        }

        private void btnUcitajSliku_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfd = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.gif;)|*.jpg;*.jpeg;*.gif;"
            };
            if (openfd.ShowDialog() == DialogResult.OK)
            {
                pbSlika.SizeMode = PictureBoxSizeMode.StretchImage;
                pbSlika.Image = new Bitmap(openfd.FileName);
            }
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            PanelHelper.SwapPanels(this.Parent, this, new Home());

        }

   

        private void btnLozinka_Click(object sender, EventArgs e)
        {
            PanelHelper.SwapPanels(this.Parent, this, new Lozinka(_ID));
        }

        private void txtPrezime_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPrezime.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPrezime, Resources.Validation_RequiredField);
            }
            else
            {
                errorProvider1.SetError(txtPrezime, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, Resources.Validation_RequiredField);
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
                bool isEmailValid = Regex.IsMatch(txtEmail.Text, emailPattern);
                if (isEmailValid == false)
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtEmail, "Napisi email u ispravnom formatu (nesto@example.com)!");
                }
            }
        }

        private void txtTelefon_TextChanged(object sender, EventArgs e)
        {

        }
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void txtTelefon_Validating(object sender, CancelEventArgs e)
        {
            string phone = txtTelefon.Text.ToString();
            if (string.IsNullOrWhiteSpace(txtTelefon.Text) || txtTelefon.Text.Length < 9 || txtTelefon.Text.Length > 9)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTelefon, "Sadrzi 9 brojeva!");
            }
            else
            {
                errorProvider1.SetError(txtTelefon, null);
                if (IsDigitsOnly(phone) == false)
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtTelefon, "Mozete koristiti samo brojeve!");
                }
                else
                {
                    errorProvider1.SetError(txtTelefon, null);
                }
            }
        }

        private void txtKorisnickoIme_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtKorisnickoIme_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKorisnickoIme.Text) || txtKorisnickoIme.Text.Length < 3)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtKorisnickoIme, "Mora da sadrzi najmanje 3 karaktera");
            }
            else
            {
                errorProvider1.SetError(txtKorisnickoIme, null);
            }
        }

        private void txtIme_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtIme, Resources.Validation_RequiredField);
            }
            else
            {
                errorProvider1.SetError(txtIme, null);
            }
        }

        private void clbUloge_Validating(object sender, CancelEventArgs e)
        {
            var ulogeList = clbUloge.CheckedItems.Cast<Model.Uloge>().Select(x => x.UlogaID).ToList();
            if (ulogeList.Count() == 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(clbUloge, "Moras izabrati jednu ulogu!");
            }
            else
            {
                errorProvider1.SetError(clbUloge, null);
            }
        }
    }
}