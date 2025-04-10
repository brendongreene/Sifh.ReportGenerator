using SIFH_ERP_ClassLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_OutputClass
{
    class Program
    {
        private static readonly OdooModelsGet ReqApi = new OdooModelsGet();
        static void Main(string[] args)
        {
            try
            {
                ReqApi.RequestVersion();
                //ReqApi.Connect();
                var id = ReqApi.Authenticator(); //;Authenticate().Result
                ReqApi.User = id;

                var table = Console.ReadLine();

                if (id == -1 || table == null) return;

                switch (table.ToLower().Replace(" ", ""))
                {
                    case "receivingnote":
                        receivingNoteGet(id);
                        break;
                    case "p":
                        productGet("1234");
                        break;
                    case "salesorders":
                        Console.WriteLine("Enter date: ");
                        var date = Console.ReadLine();
                        airlineGet(id, date);
                        break;
                    case "t":
                        test(id);
                        break;
                    case "deliverynote":
                        Console.WriteLine("Enter delivery note ID: ");
                        var dId = Console.ReadLine();
                        GetDeliveryNote(dId);
                        break;
                    case "c":
                        customGet();
                        break;
                    //case "Jul":
                    //    receivingNoteGet();
                    //    break;
                    //case "Aug":
                    //    receivingNoteGet();
                    //    break;
                    //case "Sep":
                    //    receivingNoteGet();
                    //    break;
                    //case "Oct":
                    //    receivingNoteGet();
                    //    break;
                    //case "Nov":
                    //    receivingNoteGet();
                    //    break;
                    //case "Dec":
                    //    receivingNoteGet();
                    //    break;
                    default:
                        Console.WriteLine("Table not found");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        public static void receivingNoteGet(int userId)
        {
            ReqApi.SearchProduct(userId, DateTime.Now, DateTime.Now);
        }

        public static void productGet(string id)
        {
            //ReqApi.GetCustomers("3748");
            //ReqApi.test2();
            //ReqApi.GetAirlines();
            ReqApi.GetExportCustomers();
        }

        public static void airlineGet(int userId, string date)
        {
            var salesOrders = ReqApi.GetSaleOrder(DateTime.Parse(date));
            Console.WriteLine("DONE!");

            //ReqApi.SearchOrders(userId, DateTime.Now, DateTime.Now);
            //id
            //name
            //packingList
        }

        public static void test(int userId)
        {
            var deliveryNote = ReqApi.GetDeliveryNote("SO/2024/12670"); //11/25/2024 08:56:48
            //ReqApi.GetDeliveryNoteOperationLine("GM/PICK/26076"); //11/25/2024 08:56:48
            //ReqApi.GetSaleOrder(userId, DateTime.Now);
            //ReqApi.GetSaleLineOrder(userId, "2"); //getAirline
            //ReqApi.test(userId); //getAirline
        }

        public static void GetDeliveryNote(string id)
        {
            ReqApi.GetDeliveryNote(id); //getAirline
        }

        public static void customGet()
        {
            Console.WriteLine("Enter model name: ");
            var modelName = Console.ReadLine();

            Console.WriteLine("Enter requested field names (separated by commas): ");
            var fields = Console.ReadLine();

            var fieldNames = fields.Replace(" ", "").Split(',');

            var searchDomain = string.Empty;
            Console.WriteLine("Include search domain?");
            var yOrN = Console.ReadLine();

            if (yOrN.ToLower().Contains("y"))
            {
                Console.WriteLine("Enter requested search domain e.g Where: company_id = 1,2,3 & tag = overseas: ");
                Console.Write("Where: ");
                searchDomain = Console.ReadLine();
            }

            Console.WriteLine("Processing Output...");
            ReqApi.GetCustomModel(modelName, fieldNames, searchDomain);
            Console.WriteLine("Completed");
        }
    }
}
