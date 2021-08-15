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
using System.Globalization;
using HotelReservation.Models;

namespace HotelReservation
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    public partial class AddReservation : Page
    {
        public AddReservation()
        {
            InitializeComponent();
            FillData();
            LoadCustomers();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        string sql = "";

        public void LoadCustomers()
        {
            adp = new SqlDataAdapter();
            dt = new DataTable();
            ds = new DataSet();
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "select CId, FirstName, LastName from Customer as c left join [User] as u on c.UserId=u.UId;";
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string theValue = dt.Rows[i]["FirstName"].ToString() + " " + dt.Rows[i]["LastName"].ToString();
                        cmb_customer.Items.Add(theValue);
                    }

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
        }

        public void FillData()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "select RoomType as 'Room Type', RoomNumber as 'Room Number', Price, CASE When IsOccupied=0 THEN 'Vacant' ELSE 'Occupied' END as Occupancy from RoomType as rt inner join Room as r on rt.RId=r.RId";
                

                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data not available...");
                }
                else
                {
                    List<DataRow> list = new List<DataRow>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(dr);
                    }

                    dg_room.ItemsSource = dt.DefaultView;

                }
                con.Close();

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                adp = new SqlDataAdapter();
                //sql = "select CheckInDate as 'Check In Date', CheckOutDate as 'Check Out Date', TotalPrice as 'Total Price', FirstName as 'First Name', LastName as 'Last Name', RoomNumber as 'Room Number' from Reservation as rs left join Room as r on rs.RoomId=r.RId left join Customer c on rs.CId = c.Cid left join [User] as u on c.UserId = u.UId";
                sql = "select CheckInDate as 'Check In Date', CheckOutDate as 'Check Out Date', TotalPrice as 'Total Price', ";
                sql += " FirstName as 'First Name', LastName as 'Last Name', RoomNumber as 'Room Number' from Reservation as rs";
                sql += " left join Room as r on rs.RoomId = r.RId";
                sql += " left join Customer c on rs.CId = c.Cid";
                sql += " left join[User] as u on c.UserId = u.UId";
                sql += " where GETDATE() >= CheckInDate and GETDATE() <= CheckOutDate";
                adp = new SqlDataAdapter(sql, con);
                dt = new DataTable();
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    //MessageBox.Show("Data not available...");
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

                    dg_reservations.ItemsSource = dt.DefaultView;

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

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                DateTime CheckIn;
                DateTime CheckOut;
                DateTime Todate;

                //Date validation for not select previous date and check out date must be greater than checkin date
                CheckIn = Convert.ToDateTime(txt_check_in.Text);
                CheckOut = Convert.ToDateTime(txt_check_out.Text);
                Todate = System.DateTime.Now;
                if (CheckIn < Todate)
                {
                    MessageBox.Show("Past date selection not possible, kindly select valid Check-in date");
                }
                else if (CheckIn > CheckOut)
                {
                    MessageBox.Show("Check out date must be greater than check-in date");
                }
                else
                {
                    var customer_name = cmb_customer.SelectedItem;
                    var check_in_date = DateTime.ParseExact(txt_check_in.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    var check_out_date = DateTime.ParseExact(txt_check_out.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    var room_number = txt_rooms.Text;

                    int customer_id = Customer.get_customer_id_from_name(customer_name.ToString());

                    List<DataTable> listData = new List<DataTable>();

                    listData = Reservation.ReserveBooking(customer_id, room_number, check_in_date, check_out_date);
                    dg_reservations.ItemsSource = listData[0].DefaultView;
                    dg_room.ItemsSource = listData[1].DefaultView;

                    clearFields();
                }

            }
            else
            {
                MessageBox.Show("All fields are required!");
            }
        }

        public bool validate()
        {
            if (txt_check_in.Text == "" || txt_check_out.Text == "" || txt_rooms.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void clearFields()
        {
            cmb_customer.SelectedItem = "";
            txt_check_in.Text = "";
            txt_check_out.Text = "";
            txt_rooms.Text = "";
        }

        private void btn_filter_Click(object sender, RoutedEventArgs e)
        {
            this.dg_reservations.ItemsSource = null;
           

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            //sql = "select RoomType as 'Room Type', RoomNumber as 'Room Number', Price, CASE When IsOccupied=0 THEN 'Vacant' ELSE 'Occupied' END as Occupancy from RoomType as rt inner join Room as r on rt.RId=r.RId";
            sql = "";
            if (cmb_filter.SelectedIndex == 0)
            {
                sql = "select CheckInDate as 'Check In Date', CheckOutDate as 'Check Out Date', TotalPrice as 'Total Price', ";
                sql += " FirstName as 'First Name', LastName as 'Last Name', RoomNumber as 'Room Number' from Reservation as rs";
                sql += " left join Room as r on rs.RoomId = r.RId";
                sql += " left join Customer c on rs.CId = c.Cid";
                sql += " left join[User] as u on c.UserId = u.UId";
                sql += " where GETDATE() >= CheckInDate and GETDATE() <= CheckOutDate";
            }
            else if (cmb_filter.SelectedIndex == 1)
            {
                sql = "select CheckInDate as 'Check In Date', CheckOutDate as 'Check Out Date', TotalPrice as 'Total Price', ";
                sql += " FirstName as 'First Name', LastName as 'Last Name', RoomNumber as 'Room Number' from Reservation as rs";
                sql += " left join Room as r on rs.RoomId = r.RId";
                sql += " left join Customer c on rs.CId = c.Cid";
                sql += " left join[User] as u on c.UserId = u.UId";
                sql += " where GETDATE() > CheckOutDate";
            }
            else
            {
                sql = "select CheckInDate as 'Check In Date', CheckOutDate as 'Check Out Date', TotalPrice as 'Total Price', ";
                sql += " FirstName as 'First Name', LastName as 'Last Name', RoomNumber as 'Room Number' from Reservation as rs";
                sql += " left join Room as r on rs.RoomId = r.RId";
                sql += " left join Customer c on rs.CId = c.Cid";
                sql += " left join[User] as u on c.UserId = u.UId";
                sql += " where GETDATE() < CheckInDate";
            }

            adp = new SqlDataAdapter(sql, con);
            dt = new DataTable();
            adp.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Data not available...");
            }
            else
            {
                dg_reservations.ItemsSource = dt.DefaultView;
            }

        }
    }
}
