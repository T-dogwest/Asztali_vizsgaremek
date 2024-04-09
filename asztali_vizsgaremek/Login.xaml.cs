﻿using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using asztali_vizsgaremek.Admin;

namespace asztali_vizsgaremek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {

            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = tbFelhnev.Text;
            string password = tbJelszo.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Add meg a jelszavad és a felhasználó nevedet is", "Közlés", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            HttpClient client = new HttpClient();
            var content = new StringContent($"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}", Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:3000/auth/login", content);

                if(response.IsSuccessStatusCode)
{
                    var responseContent = await response.Content.ReadAsAsync<LoginResponse>();

                    if (responseContent.Role == "Admin")
                    {
                        MessageBox.Show("Sikeres bejelentkeztés.", "Közlés", MessageBoxButton.OK, MessageBoxImage.Information);

                      

                        // Admin ablak megnyitása
                       AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();

                        // Aktuális ablak bezárása
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Nincs jogosultságod belépni", "Közlés", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                
            }
                else
                {
                    MessageBox.Show("Nem sikerült bejelentkezni,ellenőrizd a felhasználó neved és a jelszavad", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                client.Dispose();
            }
        }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }

        public LoginResponse(string token, string role)
        {
            Token = token;
            Role = role;

          
            TokenM.SetToken(token);
        }
    }

}


 
    

