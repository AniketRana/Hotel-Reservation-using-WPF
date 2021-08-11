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
using System.Windows.Shapes;

namespace HotelReservation
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Window
    {
        public Reservations()
        {
            InitializeComponent();
        }

        private void btn_employees_Click(object sender, RoutedEventArgs e)
        {
            Employees emp = new Employees();
            emp.Show();
        }

        private void btn_reservation_Click(object sender, RoutedEventArgs e)
        {
            Reservations reserv = new Reservations();
            reserv.Show();

        }

        private void btn_customers_Click(object sender, RoutedEventArgs e)
        {
            Customers cust = new Customers();
            cust.Show();
        }

        private void btn_room_Click(object sender, RoutedEventArgs e)
        {
            Rooms rm = new Rooms();
            rm.Show();
        }
    }
}
