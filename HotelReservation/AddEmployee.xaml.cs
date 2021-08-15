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
using HotelReservation.Models;

namespace HotelReservation
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddEmployee : Page
    {
        public AddEmployee()
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
                sql = "select FirstName, LastName, MobileNo, Email,Username, Password from Employee as e left join [User] as u on e.UserId=u.UId;";
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    /// MessageBox.Show("Data not available...");
                }
                else
                {
                    //List<DataRow> list = new List<DataRow>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    list.Add(dr);
                    //}

                    dg_Employee.ItemsSource = dt.DefaultView;

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
            this.NavigationService.Navigate(new AddReservation());
        }

        private void btn_customers_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddCustomer());
        }

        private void btn_room_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RoomPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var first_name = txt_first_name.Text;
            var last_name = txt_last_name.Text;
            var mobile_number = txt_mobile_number.Text;
            var email = txt_email.Text;
            var Username = txt_Username.Text;
            var Password = txt_Password.Text;
            if (validate())
            {
                Employee employee = new Employee(first_name, last_name, mobile_number, email, Username, Password);
                dt = employee.Save();
                dg_Employee.ItemsSource = dt.DefaultView;
                clearFields();
            }
            else
            {
                MessageBox.Show("All fields are required!");
            }

        }
        public void clearFields()
        {
            txt_first_name.Text = "";
            txt_last_name.Text = "";
            txt_mobile_number.Text = "";
            txt_email.Text = "";
            txt_Username.Text = "";
            txt_Password.Text = "";
        }

        public bool validate()
        {
            if (txt_first_name.Text == "" || txt_last_name.Text == "" || txt_mobile_number.Text == "" || txt_email.Text == "" || txt_Username.Text == "" || txt_Password.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            var first_name = txt_first_name.Text;
            var last_name = txt_last_name.Text;
            var mobile_number = txt_mobile_number.Text;
            var email = txt_email.Text;
            var document_username = txt_Username.Text;
            var document_password = txt_Password.Text;
            if (validate())
            {
                Employee employee = new Employee(first_name, last_name, mobile_number, email, document_username , document_password);
                dt = employee.Save();
                dg_Employee.ItemsSource = dt.DefaultView;
                clearFields();
            }
            else
            {
                MessageBox.Show("All fields are required!");
            }

        }
    }
}
