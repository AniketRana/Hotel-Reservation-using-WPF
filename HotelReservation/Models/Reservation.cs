using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelReservation.Models
{
    class Reservation
    {
        private int customer_id;
        private int room_id;
        private string room_number;
        private DateTime check_in_date;
        private DateTime check_out_date;
        private float total_price;

        private Customer customer;
        private Room room;

        public Reservation(int customer_id, string room_number, DateTime check_in_date, DateTime check_out_date)
        {
            this.customer_id = customer_id;
            room = Room.find_by_room_number(room_number);
            this.room_id = room.RoomId;
            this.check_in_date = check_in_date;
            this.check_out_date = check_out_date;
            this.total_price = 0;
            this.customer = Customer.find(customer_id);
        }

        public int CustomerId
        {
            get { return this.customer_id; }
            set { this.customer_id = value; }
        }

        public int RoomId
        {
            get { return this.room_id; }
            set { this.room_id = value; }
        }

        public DateTime CheckInDate
        {
            get { return this.check_in_date; }
            set { this.check_in_date = value; }
        }

        public DateTime CheckOutDate
        {
            get { return this.check_out_date; }
            set { this.check_out_date = value; }
        }

        public float TotalPrice
        {
            get { return this.total_price; }
            set { this.total_price = value; }
        }

        public static List<DataTable> ReserveBooking(int customer_id, string room_number, DateTime check_in_date, DateTime check_out_date, float total_price = 0)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            List<DataTable> dataList = new List<DataTable>();
            DataSet ds = new DataSet();
            string sql = "";

            Room room = Room.find_by_room_number(room_number);
            TimeSpan difference = check_out_date - check_in_date;
            int number_of_days = (int)difference.TotalDays;
            total_price = (float)(number_of_days * room.Price);

            room.IsOccupied = 1;
            room.update();

            sql = "INSERT INTO Reservation(CId, RoomId, CheckInDate, CheckOutDate, TotalPrice) values (" + customer_id + "," + room.RoomId + ",convert(datetime2,'" + check_in_date + "',103),convert(datetime2,'" + check_out_date + "',103), " + total_price + ") select scope_identity() as RId";
            
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt1);
                int result = Convert.ToInt32(dt1.Rows[0]["RId"].ToString());
                con.Close();
                adp = new SqlDataAdapter();
                dt1 = new DataTable();
                ds = new DataSet();
                if (result > 0)
                {
                    sql = "select CheckInDate as 'Check In Date', CheckOutDate as 'Check Out Date', TotalPrice as 'Total Price', FirstName as 'First Name', LastName as 'Last Name', RoomNumber as 'Room Number' from Reservation as rs left join Room as r on rs.RoomId=r.RId left join Customer c on rs.CId = c.Cid left join [User] as u on c.UserId = u.UId";

                    con.Open();
                    adp = new SqlDataAdapter(sql, con);
                    adp.Fill(dt1);
                    con.Close();

                    sql = "select RoomType as 'Room Type', RoomNumber as 'Room Number', Price, CASE When IsOccupied=0 THEN 'Vacant' ELSE 'Occupied' END as Occupancy from RoomType as rt inner join Room as r on rt.RId=r.RId";
                    con.Open();
                    adp = new SqlDataAdapter(sql, con);
                    adp.Fill(dt2);
                    con.Close();

                    dataList.Add(dt1);
                    dataList.Add(dt2);
                }
                else
                {
                    MessageBox.Show("Something went wrong...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
            return dataList;
        }
    }
}
