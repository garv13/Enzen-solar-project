using Microsoft.Hadoop.Avro;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Data.SqlClient;
using System.Data;

namespace IOTSolar
{
    /// <summary>
    /// Interaction logic for DBCreate.xaml
    /// </summary>
    public partial class DBCreate : Window
    {
        public DBCreate()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            List<Message> li = new List<Message>();
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;

            List<CloudBlockBlob> delete = new List<CloudBlockBlob>();


            int files = 0;
        
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=iotsolarmessages;AccountKey=r2XazbW3wcNLJMX4+ZBR8p/Nnsz5759ck5pJh94D16tq/ih7ZS6n8FV+HP3lwz9Em9/MnFJB2p/YCfwEl9v/Xw==;EndpointSuffix=core.windows.net";
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer blobContainer = cloudBlobClient.GetContainerReference("messages");
                    string blobPrefix = null;
                    bool useFlatBlobListing = false;
                    var blobs = blobContainer.ListBlobs(blobPrefix, useFlatBlobListing, BlobListingDetails.None);
                    var folders = blobs.Where(b => b as CloudBlobDirectory != null).ToList();
                    foreach (CloudBlobDirectory folder in folders)
                    {
                        Console.WriteLine(folder.Uri);
                        List<IListBlobItem> blobs2 = folder.ListBlobs().ToList();
                        foreach (CloudBlobDirectory sub in blobs2)
                        {
                            List<IListBlobItem> blobs3 = sub.ListBlobs().ToList();//get year
                            foreach (CloudBlobDirectory sub2 in blobs3)
                            {
                                List<IListBlobItem> blobs4 = sub2.ListBlobs().ToList(); // get month


                                foreach (CloudBlobDirectory sub3 in blobs4)
                                {
                                    List<IListBlobItem> blobs5 = sub3.ListBlobs().ToList(); //get dates
                                    foreach (CloudBlobDirectory sub4 in blobs5)
                                    {
                                        List<IListBlobItem> blobs6 = sub4.ListBlobs().ToList();// get hours
                                        foreach (CloudBlobDirectory sub5 in blobs6)
                                        {
                                            List<IListBlobItem> blobs7 = sub5.ListBlobs().ToList();//get minutes
                                            foreach (CloudBlockBlob sub6 in blobs7)
                                            {
                                                var memoryStream = new MemoryStream();
                                                await sub6.DownloadToStreamAsync(memoryStream);
                                                files++;
                                                memoryStream.Seek(0, SeekOrigin.Begin);
                                                List<AvroRecord> avroRecords;
                                                using (var reader = AvroContainer.CreateGenericReader(memoryStream))
                                                {
                                                    using (var sequentialReader = new SequentialReader<object>(reader))
                                                    {
                                                        avroRecords = sequentialReader.Objects.OfType<AvroRecord>().ToList();
                                                    }
                                                }
                                                foreach (AvroRecord r in avroRecords)
                                                {
                                                    var body = r.GetField<byte[]>("Body");
                                                    var dataBody = Encoding.UTF8.GetString(body);
                                                    Message myObj = JsonConvert.DeserializeObject<Message>(dataBody);
                                                    bool flag = true;
                                                    foreach (Message m in li)
                                                    {
                                                        if (m.deviceId == myObj.deviceId)
                                                        {
                                                            m.coins = m.coins + myObj.coins;
                                                            flag = false;
                                                        }
                                                    }
                                                    if (flag)
                                                    {
                                                        li.Add(myObj);
                                                    }


                                                }

                                                // logic to upload to mobile service table li



                                                //Deleting blobs
                                                // blob has been processed and uploaded and will be deleted
                                                delete.Add(sub6);
                                                //await sub6.DeleteAsync();

                                            }//minutes
                                          

                                        }//hour

                                       
                                    }//day
                                    
                                }//month

                               
                            }//year

                            foreach (Message item in li)
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
                                            sb.Append("SELECT deviceid,coins FROM MESSAGE WHERE deviceid = '" + item.deviceId + "'");

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
                            foreach (var item in delete)
                            {
                                try
                                {
                                    await item.DeleteIfExistsAsync();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    
                   // var a =  cloudBlobClient.ListContainers("messages");
                   
                    // List the blobs in the container.
                    //Console.WriteLine("Listing blobs in container.");
                    //BlobContinuationToken blobContinuationToken = null;
                    //do
                    //{
                    //    var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                    //    // Get the value of the continuation token returned by the listing call.
                    //    blobContinuationToken = results.ContinuationToken;
                    //    foreach (IListBlobItem item in results.Results)
                    //    {
                    //        Console.WriteLine(item.Uri);
                    //    }
                    //} while (blobContinuationToken != null); // Loop while the continuation token is not null.
                    //Console.WriteLine();

                   
                }
                catch (StorageException ex)
                {
                    Console.WriteLine("Error returned from the service: {0}", ex.Message);
                }
                finally
                {
                    Console.WriteLine("Press any key to delete the sample files and example container.");
                    Console.ReadLine();
                    // Clean up resources. This includes the container and the two temp files.
                    
                   
                }
            }
            else
            {
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add a environment variable named 'storageconnectionstring' with your storage " +
                    "connection string as a value.");
            }
        }
    }
}
