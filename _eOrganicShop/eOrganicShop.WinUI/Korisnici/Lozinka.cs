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
    public partial class Lozinka : UserControl
    {
        private readonly APIService korisnikService = new APIService("Korisnici");
        private readonly int _ID;
        public Lozinka(int ID)
        {
            _ID = ID;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btnSacuvaj_Click(object sender, EventArgs e)
        {
            try
            {
                var korisnik = await korisnikService.GetById<Model.Korisnici>(_ID);
                KorisnikUpsertRequest request = null;
                if (txtLozinkaStara.Text == APIService.Password)
                {
                    request = new KorisnikUpsertRequest
                    {
                        Ime = korisnik.Ime,
                        Prezime = korisnik.Prezime,
                        KorisnickoIme = korisnik.KorisnickoIme,
                        Email = korisnik.Email,
                        Telefon = korisnik.Telefon,
                        Image = korisnik.Image,
                        Password = txtNovaLozinka.Text,
                        PasswordConfirmation = txtPotvrdiLozinku.Text
                    };
                }
                else
                {
                    MessageBox.Show("Vasa trenutna lozinka nije ispravna!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtNovaLozinka.Text != txtPotvrdiLozinku.Text)
                {
                    MessageBox.Show("Lozinke se ne podudaraju!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                await korisnikService.Update<Model.Korisnici>(_ID, request);

                MessageBox.Show("Uspjesno ste promijenili lozinku.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var form = new frmLogin();
                form.Show();
                ParentForm.Close();
            }
            catch
            {
                MessageBox.Show("Ups, something went wrong!");
            }
        }

        private void btnNazad_Click(object sender, EventArgs e)
        {
            PanelHelper.SwapPanels(this.Parent, this, new KorisnikEdit(_ID));

        }

        private void txtNovaLozinka_Validating(object sender, CancelEventArgs e)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            if (string.IsNullOrWhiteSpace(txtNovaLozinka.Text))
            {
                errorProvider1.SetError(txtNovaLozinka, Resources.Validation_RequiredField);
                e.Cancel = true;
            }
            else
            {
                if (hasNumber.IsMatch(txtNovaLozinka.Text) && hasUpperChar.IsMatch(txtNovaLozinka.Text) && hasMinimum8Chars.IsMatch(txtNovaLozinka.Text))
                {
                    errorProvider1.SetError(txtNovaLozinka, null);
                }
                else
                {
                    errorProvider1.SetError(txtNovaLozinka, "Lozinka mora da ima mala, velika slova, brojeve i minimalno 8 karaktera!");
                    e.Cancel = true;

                }
            }
        }

        private void txtPotvrdiLozinku_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPotvrdiLozinku.Text))
            {
                errorProvider1.SetError(txtPotvrdiLozinku, Resources.Validation_RequiredField);
                e.Cancel = true;
            }
            else
            {
                if (txtPotvrdiLozinku.Text == txtPotvrdiLozinku.Text)
                {
                    errorProvider1.SetError(txtPotvrdiLozinku, null);
                }
                else
                {
                    errorProvider1.SetError(txtPotvrdiLozinku, "Lozinke se ne podudaraju");
                    e.Cancel = true;
                }
            }
        }
    }
}
