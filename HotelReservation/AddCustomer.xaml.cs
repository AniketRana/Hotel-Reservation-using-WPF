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
    public partial class AddCustomer : Page
    {
        public AddCustomer()
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
                sql = "select FirstName, LastName, MobileNo, Email, City, DocumentName, DocumentNumber from Customer as c left join [User] as u on c.UserId=u.UId;";
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

                    dg_customer.ItemsSource = dt.DefaultView;

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
            this.NavigationService.Navigate(new AddEmployee());
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
            this.NavigationService.Navigate(new AddRoom());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var first_name = txt_first_name.Text;
            var last_name = txt_last_name.Text;
            var mobile_number = txt_mobile_number.Text;
            var email = txt_email.Text;
            var city = txt_city.Text;
            var document_name = txt_document_name.Text;
            var document_number = txt_document_number.Text;
            if (validate())
            {
                Customer customer = new Customer(first_name, last_name, mobile_number, email, city, document_name, document_number);
                dt = customer.Save();
                dg_customer.ItemsSource = dt.DefaultView;
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
            txt_city.Text = "";
            txt_document_name.Text = "";
            txt_document_number.Text = "";
        }

        public bool validate()
        {
            if(txt_first_name.Text == "" || txt_last_name.Text == "" || txt_mobile_number.Text == "" || txt_email.Text == "" || txt_city.Text == "" || txt_document_name.Text == "" || txt_document_number.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
