using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            FillData();
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        string sql = "";
        public void FillData()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "Select FirstName, LastName, MobileNo, Email, UserName from Employee as e inner join [User] as u on e.UserId=u.UId";
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data not available...");
                }
                else
                {
                    //redirect to home page
                    //MessageBox.Show("Login Done");
                    List<DataRow> list = new List<DataRow>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(dr);
                    }

                    dg_employees.ItemsSource = dt.DefaultView;

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
        }

        private void btn_employees_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EmployeesPage());
            
        }

        private void btn_reservation_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ReservationPage());

        }

        private void btn_customers_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CustomersPage());
        }

        private void btn_room_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RoomPage());
        }
    }
}
