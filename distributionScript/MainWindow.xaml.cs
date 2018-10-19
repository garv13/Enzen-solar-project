using System;
using System.Collections.Generic;
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


namespace distributionScript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "iotsolar.database.windows.net";
                builder.UserID = "Axe";
                builder.Password = "G8summit";
                builder.InitialCatalog = "IoTSolar";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT deviceid,coins FROM MESSAGE ");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        SqlDataReader a = await command.ExecuteReaderAsync();
                        if (a.HasRows)
                        {
                            double coin = 0;
                            while (a.Read())
                            {

                                coin = a.GetDouble(a.GetOrdinal("coins"));

                            }
                            a.Close();
                            sb = new StringBuilder();
                            sb.Append("UPDATE MESSAGE SET coins =  " + (item.coins + coin).ToString() + " WHERE deviceid = '" + item.deviceId + "'");

                            sql = sb.ToString();
                            using (SqlCommand command2 = new SqlCommand(sql, connection))
                            {


                                command2.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            a.Close();
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO MESSAGE (deviceid,coins) VALUES ('" + item.deviceId + "'," + item.coins.ToString() + ")");

                            sql = sb.ToString();
                            using (SqlCommand command2 = new SqlCommand(sql, connection))
                            {


                                command2.ExecuteNonQuery();
                            }
                        }


                    }





                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
} 
