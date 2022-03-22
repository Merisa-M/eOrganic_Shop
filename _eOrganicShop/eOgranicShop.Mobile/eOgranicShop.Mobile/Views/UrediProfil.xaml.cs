using eOgranicShop.Mobile.Helpers;
using eOgranicShop.Mobile.ViewModels;
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
    public partial class UrediProfil : ContentPage
    {
        private readonly APIService korisnikService = new APIService("Korisnici");
        string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
        public UrediProfilVM model = null;

        public UrediProfil()
        {
            InitializeComponent();
            Ime.Text = SignIn.Korisnik.Ime;
            Prezime.Text = SignIn.Korisnik.Prezime;
            Email.Text = SignIn.Korisnik.Email;
            Telefon.Text = SignIn.Korisnik.Telefon;
            KorisnickoIme.Text = SignIn.Korisnik.KorisnickoIme;
            BindingContext = model = new UrediProfilVM(SignIn.Korisnik);
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Ime.Text) || string.IsNullOrWhiteSpace(Prezime.Text) ||
                   string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(Telefon.Text)
                   || string.IsNullOrWhiteSpace(KorisnickoIme.Text))
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Sva polja su obavezna.", "Pokusaj ponovo");
                    return;
                }

                if (KorisnickoIme.Text.Length < 3)
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Korisnicko ime mora imate najmanje 3 slova.", "Pokusaj ponovo");
                    return;
                }
                bool isValid = Regex.IsMatch(Email.Text, emailPattern);
                if (!isValid)
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Molim napisite mail u ispravnom formatu.", "Pokušaj ponovo");
                    return;
                }

                var listUsers = await korisnikService.Get<List<Korisnici>>(null);
                foreach (var item in listUsers)
                {
                    if (KorisnickoIme.Text == item.KorisnickoIme && APIService.Username != item.KorisnickoIme)
                    {
                        await Application.Current.MainPage.DisplayAlert("Greska", "Korisnicko ime je zauzeto.", "Pokusaj ponovo");
                        return;
                    }
                }

                if (IsDigitsOnly(Telefon.Text) == false)
                {
                    await Application.Current.MainPage.DisplayAlert("Greska", "Mozete koristiti samo brojeve za Vas telefon.", "Pokusaj ponovo");
                    return;
                }

                var noviKorisnik = await korisnikService.Update<Korisnici>(SignIn.Korisnik.KorisnikID, new KorisnikUpsertRequest
                {
                    KorisnickoIme = KorisnickoIme.Text,
                    Ime = Ime.Text,
                    Prezime = Prezime.Text,
                    Email = Email.Text,
                    Telefon = Telefon.Text,
                    Uloge = new List<int> { 1,2 },
                    Image = model.Slika,
                   
                });

                if (noviKorisnik == default(Korisnici))
                    return;

                await Application.Current.MainPage.DisplayAlert("Success", "Uspjesno spaseno.", "OK");
                await Navigation.PushAsync(new Login());
            }
            catch
            {

            }
        }
    }
}