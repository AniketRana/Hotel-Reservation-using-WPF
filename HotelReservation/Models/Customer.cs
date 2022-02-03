using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;

namespace HotelReservation.Models
{
    public class Customer : User
    {
        private int customer_id;
        private string city;
        private string document_name;
        private string document_number;


        public Customer(string first_name, string last_name, string mobile_number, string email, string city, string document_name, string document_number, int customer_id=0) : 
            base(first_name, last_name, mobile_number, email)
        {
            this.city = city;
            this.document_name = document_name;
            this.document_number = document_number;
            this.customer_id = customer_id;
        }

        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }

        public string DocumentName
        {
            get { return this.document_name; }
            set { this.document_name = value; }
        }

        public string DocumentNumber
        {
            get { return this.document_number; }
            set { this.document_number = value; }
        }

        public DataTable Save()
        {
            base.Save(this.FirstName, this.LastName, this.MobileNumber, this.Email, this.City, this.DocumentName, this.DocumentNumber);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string sql = "";
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "select FirstName, LastName, MobileNo, Email, City, DocumentName, DocumentNumber from Customer as c inner join [User] as u on c.UserId=u.UId";
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    /// MessageBox.Show("Data not available...");
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
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
            return dt;
        }

        public static Customer find(int customer_id)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string sql = "";
            Customer c = null;
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "select CId, UserId, FirstName, LastName, MobileNo, Email, City, DocumentName, DocumentNumber from Customer as c left join [User] as u on c.UserId=u.UId WHERE CID="+customer_id;
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    /// MessageBox.Show("Data not available...");
                }
                else
                {
                    c = new Customer(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["Email"].ToString(), dt.Rows[0]["City"].ToString(), dt.Rows[0]["DocumentName"].ToString(), dt.Rows[0]["DocumentNumber"].ToString(), Convert.ToInt32(dt.Rows[0]["CId"].ToString()));
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
            return c;
        }

        public static int get_customer_id_from_name(string full_name)
        {
            int id = 0;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string sql = "";

            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                sql = "select CId from Customer as c left join [User] as u on c.UserId=u.UId WHERE FirstName='" + full_name.Split(' ')[0] + "' and LastName = '" + full_name.Split(' ')[1] + "'";
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    id = Convert.ToInt32(dt.Rows[0]["CId"].ToString());
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
            }
            return id;
        }
    }
}
