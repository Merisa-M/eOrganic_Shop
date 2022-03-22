using eOgranicShop.Mobile.Helpers;
using eOrganicShop.Model;
using eOrganicShop.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eOgranicShop.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lozinka : ContentPage
    {
        private readonly APIService korisnikService = new APIService("Korisnici");

        public Lozinka()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasMinimum8Chars = new Regex(@".{8,}");


                if (TrenutnaLozinka.Text != APIService.Password)
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Pogresna lozinka.", "Pokusaj ponovo");
                    return;
                }

                if (PotvrdiLozinku.Text != NovaLozinka.Text)
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Lozinke se ne poklapaju.", "Pokusaj ponovo");
                    return;
                }

                if (NovaLozinka.Text.Length < 8)
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Lozinka mora imati najmanje 8 karaktera.", "Pokusaj ponovo");
                    return;
                }

                if (!hasNumber.IsMatch(NovaLozinka.Text) || !hasUpperChar.IsMatch(NovaLozinka.Text) || !hasMinimum8Chars.IsMatch(NovaLozinka.Text))
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Lozinka mora da ima brojeve, velika i mala slova i najmanje 8 karaktera!", "Pokusaj ponovo");
                    return;
                }


                if (!hasNumber.IsMatch(PotvrdiLozinku.Text) || !hasUpperChar.IsMatch(PotvrdiLozinku.Text) || !hasMinimum8Chars.IsMatch(PotvrdiLozinku.Text))
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Lozinka mora da ima brojeve, velika i mala slova i najmanje 8 karaktera!", "Pokusaj ponovo");
                    return;
                }

                await korisnikService.Update<Korisnici>(SignIn.Korisnik.KorisnikID, new KorisnikUpsertRequest
                {
                    Ime = SignIn.Korisnik.Ime,
                    Prezime = SignIn.Korisnik.Prezime,
                    Email = SignIn.Korisnik.Email,
                    Telefon = SignIn.Korisnik.Telefon,
                    KorisnickoIme = SignIn.Korisnik.KorisnickoIme,
                    Password = NovaLozinka.Text,
                    PasswordConfirmation = PotvrdiLozinku.Text
                });

                await Application.Current.MainPage.DisplayAlert("Success", "Uspjesno spremljeno, ponovo se logirajte.", "OK");
                await Navigation.PushAsync(new Login());
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong.", "Try again");
            }
        }

    }
}