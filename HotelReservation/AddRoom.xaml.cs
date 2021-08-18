using HotelReservation.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace HotelReservation
{
    /// <summary>
    /// Interaction logic for AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Page
    {
        public AddRoom()
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
                sql = "select RoomType from RoomType;";
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                con.Close();
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

                    dg_room_type.ItemsSource = dt.DefaultView;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string theValue = dt.Rows[i]["RoomType"].ToString();
                        cmb_room_type.Items.Add(theValue);
                    }

                    dt = new DataTable();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    sql = "select RoomType as 'Room Type', RoomNumber as 'Room Number', Price from Room as r left join RoomType rt on r.RId = rt.RId;";
                    adp = new SqlDataAdapter(sql, con);
                    adp.Fill(dt);
                    con.Close();
                    dg_room.ItemsSource = dt.DefaultView;

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

        private void btn_add_room_type_Click(object sender, RoutedEventArgs e)
        {
            var room_type = txt_room_type.Text;
            
            if (validate())
            {
                dt = new DataTable();
                dt = Room.save_room_type(room_type);
                dg_room_type.ItemsSource = dt.DefaultView;
                clearFields();
                FillData();
            }
            else
            {
                MessageBox.Show("All fields are required for Room Type!");
            }
        }

        private void clearFields()
        {
            txt_room_type.Text = "";
        }

        private bool validate()
        {
            if(txt_room_type.Text == "")
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
            var room_type = cmb_room_type.SelectedItem;
            var room_number = txt_room_number.Text;
            var price = txt_price.Text;
            var extra_person_price = txt_extra_person_price.Text;

            if (validate_room())
            {
                int room_id = Room.find_room_type_id_by_room_type(room_type.ToString());

                Room r = new Room(room_number, (float)Convert.ToDouble(price), 0, (float)Convert.ToDouble(extra_person_price), room_id);
                dt = new DataTable();
                dt = r.save();
                dg_room.ItemsSource = dt.DefaultView;
            }
            else
            {
                MessageBox.Show("All fields are required!");
            }
        }

        private bool validate_room()
        {
            if(cmb_room_type.SelectedItem.ToString() == "" || txt_room_number.Text == "" || txt_price.Text == "" || txt_extra_person_price.Text == "")
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
