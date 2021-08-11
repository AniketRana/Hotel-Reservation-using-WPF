using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;

namespace HotelReservation
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        string sql = "";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "Select * from Employee where Username = '" + txt_email.Text + "' and password = '" + txt_password.Password + "'";
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Username or Password...Try again");
                }
                else
                {
                    //redirect to home page
                    //MessageBox.Show("Login Done");
                    //Reservations reserv = new Reservations();
                    //reserv.Show();
                    this.NavigationService.Navigate(new ReservationPage());
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
        }
    }
}
