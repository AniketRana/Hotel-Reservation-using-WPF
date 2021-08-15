using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;

namespace HotelReservation.Models
{
    public class Employee: User
    {
        private int EId;
        private string document_Username;
        private string document_Password;

        public Employee(string first_name, string last_name, string mobile_number, string email,string document_Username,  string document_Password, int EId = 0) : base(first_name, last_name, mobile_number, email)
        { 
            this.EId = EId;
            this.document_Username = document_Username;
            this.document_Password = document_Password;
        }

        public string Username 
        {
            get { return this.document_Username; }
            set { this.document_Username = value; }
        }

        public string Password
         {
            get { return this.document_Password; }
            set { this.document_Password = value; }
        }
        public DataTable Save()
        { 
            base.SaveEmployee(this.FirstName, this.LastName, this.MobileNumber, this.Email, this.Username, this.Password);
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
                sql = "select FirstName, LastName, MobileNo, Email, Username, [Password] from Employee as e inner join [User] as u on e.UserId=u.UId";
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
    }
}
