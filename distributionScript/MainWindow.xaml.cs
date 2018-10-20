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
                            string id = "";
                            while (a.Read())
                            {

                                coin = a.GetDouble(a.GetOrdinal("coins"));
                                id = a.GetString(a.GetOrdinal("deviceid"));
                            }
                            a.Close();
                            sb = new StringBuilder();
                            sb.Append("SELECT RoofId FROM MAPPING WHERE DeviceId = '" + id+"'");


                            sql = sb.ToString();
                            using (SqlCommand command2 = new SqlCommand(sql, connection))
                            {
                                SqlDataReader a2 = await command2.ExecuteReaderAsync();
                                if (a2.HasRows)
                                {
                                    Int32 k = 0;
                                    while (a2.Read())
                                    {


                                        k = int.Parse(a2.GetValue(0).ToString());
                                    }
                                    a2.Close();
                                    sb = new StringBuilder();
                                    sb.Append("SELECT userid,id FROM share WHERE roofid =  " + k);


                                    sql = sb.ToString();
                                    using (SqlCommand command3 = new SqlCommand(sql, connection))
                                    {
                                        SqlDataReader a3 = await command3.ExecuteReaderAsync();
                                        if (a3.HasRows)

                                        {
                                            int count = 0;
                                            List<int> user = new List<int>();
                                            while (a3.Read())
                                            {


                                                k = int.Parse(a3.GetValue(a3.GetOrdinal("userid")).ToString());
                                                user.Add(k);
                                                //add coin



                                            }
                                            a3.Close();

                                            foreach (int j in user)
                                            {
                                                sb = new StringBuilder();
                                                sb.Append("SELECT tradecoins FROM usercredit WHERE userid =  " + j);


                                                sql = sb.ToString();
                                                using (SqlCommand command4 = new SqlCommand(sql, connection))
                                                {
                                                    SqlDataReader a4 = await command4.ExecuteReaderAsync();
                                                    if (a4.HasRows)

                                                    {
                                                        double coi = 0;

                                                        while (a4.Read())
                                                        {


                                                            coi = a4.GetDouble(a4.GetOrdinal("tradecoins"));
                                                            //add coin



                                                        }
                                                        a4.Close();
                                                        coi += 0.9 * coin / user.Count;
                                                        sb = new StringBuilder();
                                                        sb.Append("UPDATE usercredit SET TradeCoins =  " + coi + " WHERE userid = " +j);

                                                        sql = sb.ToString();
                                                        using (SqlCommand command5 = new SqlCommand(sql, connection))
                                                        {


                                                            command5.ExecuteNonQuery();
                                                        }
                                                        //

                                                    }
                                                    else
                                                    {
                                                        a4.Close();
                                                    }
                                                }
                                            }// coins distrubuted to users.. now we need to give coins to admins and update coin in message as 0;
                                        }
                                        else
                                        {
                                            a3.Close();

                                        }

                                    }
                                }
                                else
                                {
                                    a2.Close();

                                }

                            }
                        }
                        else
                        {
                            a.Close();
                            
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
