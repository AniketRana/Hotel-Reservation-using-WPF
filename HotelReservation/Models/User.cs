using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Windows;

namespace HotelReservation.Models
{
    public class User
    {
        private string first_name;
        private string last_name;
        private string mobile_number;
        private string email;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        string sql = "";

        public User(string first_name, string last_name, string mobile_number, string email)
        {
            this.first_name = first_name;
            this.last_name = last_name;
            this.mobile_number = mobile_number;
            this.email = email;
        }

        public string FirstName
        {
            get { return this.first_name; }
            set { this.first_name = value; }
        }

        public string LastName
        {
            get { return this.last_name; }
            set { this.last_name = value; }
        }

        public string MobileNumber
        {
            get { return this.mobile_number; }
            set { this.mobile_number = value; }
        }
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        public void SaveEmployee(string first_name, string last_name, string mobile_number, string email, string Username, string Password)
        {
            sql = "INSERT INTO [User] (FirstName, LastName, MobileNo, Email) values ('" + first_name + "','" + last_name + "','" + mobile_number + "','" + email + "') select scope_identity() as Id";
            //cmd = new SqlCommand(sql, con);

            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                int result = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                con.Close();
                if (result > 0)
                {
                    sql = "INSERT INTO Employee(UserId ,Username, Password) values(" + result + ", '" + Username + "', '" + Password+ "') select scope_identity() as Id";
                    cmd = new SqlCommand(sql, con);

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    adp = new SqlDataAdapter(sql, con);
                    adp.Fill(dt);
                    result = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                    con.Close();
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
        }


        public void Save(string first_name, string last_name, string mobile_number, string email, string city, string document_name, string document_number)
        {
            sql = "INSERT INTO [User] (FirstName, LastName, MobileNo, Email) values ('"+first_name+ "','" + last_name + "','" + mobile_number + "','" + email + "') select scope_identity() as Id";
            //cmd = new SqlCommand(sql, con);
           
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                adp = new SqlDataAdapter(sql, con);
                adp.Fill(dt);
                int result = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                con.Close();
                if (result > 0)
                {
                    sql = "INSERT INTO Customer(UserId, City, DocumentName, DocumentNumber) values(" + result + ", '" + city + "', '" + document_name + "', '" + document_number + "') select scope_identity() as Id";
                    cmd = new SqlCommand(sql, con);
                    
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    adp = new SqlDataAdapter(sql, con);
                    adp.Fill(dt);
                    result = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                    con.Close();
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
        }

    }   
}
