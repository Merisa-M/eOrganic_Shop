using eOrganicShop.Model.Request;
using eOrganicShop.WinUI.Helper;
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
    public partial class KorisnikEdit : UserControl
    {
        private readonly APIService korisnikService = new APIService("Korisnici");
        private readonly APIService ulogeService = new APIService("Uloge");
        string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";

        private readonly int _ID;

        public KorisnikEdit(int ID)
        {
            _ID = ID;
            InitializeComponent();
        }
       

        private async void KorisnikEdit_Load(object sender, EventArgs e)
        {
            var korisnik = await korisnikService.GetById<Model.Korisnici>(_ID);

            txtIme.Text = korisnik.Ime;
            txtPrezime.Text = korisnik.Prezime;
            txtKorisnickoIme.Text = korisnik.KorisnickoIme;
            txtEmail.Text = korisnik.Email;
            txtTelefon.Text = korisnik.Telefon;


            if (korisnik.Image.Length > 3)
            {
                pbKorisnik.Image = ImageHelper.ByteArrayToSystemDrawing(korisnik.Image);
                pbKorisnik.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            var roles = await ulogeService.Get<List<Model.Uloge>>(null);
            clbUloge.DataSource = roles;
            clbUloge.ValueMember = "UlogaID";
            clbUloge.DisplayMember = "Naziv";

            var rolesList = clbUloge.Items.Cast<Model.Uloge>().Select(i => i.UlogaID).ToList();
            foreach (var korisnikUloge in korisnik.KorisnikUloge)
            {
                for (int i = 0; i < clbUloge.Items.Count; i++)
                {
                    if (rolesList[i] == korisnikUloge.UlogeID)
                    {
                        clbUloge.SetItemChecked(i, true);
                    }
                }
            }
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            PanelHelper.SwapPanels(this.Parent, this, new KorisniciList());
        }

        private async void btnSacuvaj_Click(object sender, EventArgs e)
        {
            var user = await korisnikService.GetById<Model.Korisnici>(_ID);
            if (ValidateChildren())
            {
                var ulogeList = clbUloge.CheckedItems.Cast<Model.Uloge>().Select(x => x.UlogaID).ToList();
                List<int> uncheckedUloge = new List<int>();
                for (int i = 0; i < clbUloge.Items.Count; i++)
                {
                    if (!clbUloge.GetItemChecked(i))
                    {
                        int UlogaID = clbUloge.Items.Cast<Model.Uloge>().ToList()[i].UlogaID;
                        uncheckedUloge.Add(UlogaID);
                    }
                }

                var request = new KorisnikUpsertRequest
                {
                    Ime = txtIme.Text,
                    Prezime = txtPrezime.Text,
                    KorisnickoIme = txtKorisnickoIme.Text,
                    Email = txtEmail.Text,
                    Telefon = txtTelefon.Text,
                    Image = pbKorisnik.Image != null ? ImageHelper.SystemDrawingToByteArray(pbKorisnik.Image) : null,
                    Uloge = ulogeList,
                    UlogeBrisanje = uncheckedUloge,
                    
                };

                await korisnikService.Update<Model.Korisnici>(_ID, request);
                MessageBox.Show("Uspjesno spaseno.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PanelHelper.SwapPanels(this.Parent, this, new KorisniciList());
            }
        }

        private void txtIme_TextChanged(object sender, EventArgs e)
        {

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

        private void clbUloge_Validated(object sender, EventArgs e)
        {

        }

        private void clbUloge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
