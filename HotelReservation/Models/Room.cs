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
    class Room
    {
        private int room_id;
        private string room_number;
        private float price;
        private int is_occupied;
        private float extra_person_price;

        public Room(string room_number, float price, int is_occupied, float extra_person_price, int room_id = 0)
        {
            this.room_number = room_number;
            this.price = price;
            this.is_occupied = is_occupied;
            this.extra_person_price = extra_person_price;
            this.room_id = room_id;
        }

        public int RoomId
        {
            get { return this.room_id; }
            set { this.room_id = value; }
        }

        public string RoomNumber
        {
            get { return this.room_number; }
            set { this.room_number = value; }
        }

        public float Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public int IsOccupied
        {
            get { return this.is_occupied; }
            set { this.is_occupied = value; }
        }

        public float ExtraPersonPrice
        {
            get { return this.extra_person_price; }
            set { this.extra_person_price = value; }
        }

        public void save(bool isUpdate)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string sql = "";
            Room r = null;
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                if (isUpdate)
                {
                    sql = "UPDATE Room SET RoomNumber = " + this.RoomNumber + ", Price = " + this.Price + ", IsOccupied = " + this.IsOccupied + " WHERE RoomId='" + this.RoomId + "'";
                }
                
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    /// MessageBox.Show("Data not available...");
                }
                else
                {
                    MessageBox.Show("Reservation Added.");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
        }

        public static Room find_by_room_number(string room_number)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string sql = "";
            Room r = null;
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "select RoomId, RId, RoomNumber, Price, IsOccupied, ExtraPersonPrice from Room WHERE RoomNumber='" + room_number + "'";
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    /// MessageBox.Show("Data not available...");
                }
                else
                {
                    r = new Room(dt.Rows[0]["RoomNumber"].ToString(), (float)Convert.ToDecimal(dt.Rows[0]["Price"].ToString()), Convert.ToInt32(dt.Rows[0]["IsOccupied"].ToString()), (float)Convert.ToDecimal(dt.Rows[0]["ExtraPersonPrice"].ToString()), Convert.ToInt32(dt.Rows[0]["RoomId"].ToString()));
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
            return r;
        }
    }
}
