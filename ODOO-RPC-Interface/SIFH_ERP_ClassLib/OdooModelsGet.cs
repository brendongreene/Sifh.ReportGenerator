using CookComputing.XmlRpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIFH_ERP_ClassLib.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using SIFH_ERP_ClassLib.Models;
using System.Collections.Concurrent;

namespace SIFH_ERP_ClassLib
{
    public class OdooModelsGet
    {


        public string Url { get; set; } = "https://sifhstaging.odoo.com";

        private const string DbName = "adeel9911-sifh17-uat1-19557728";
        private const string Username = "admin";
        private const string Password = "60b9394cf114453e690f7ddae58b335f30696ba3"; //"admin@123!!"; //
        public int User;
        public interface IOdooStart : IXmlRpcProxy
        {
            [XmlRpcMethod("start")]
            XmlRpcStruct Start();
        }

        public interface IOdooCommon : IXmlRpcProxy
        {
            [XmlRpcMethod("version")]
            XmlRpcStruct GetVersion();

        }

        public interface IOdooXmlRpc : IXmlRpcProxy
        {
            [XmlRpcMethod("authenticate")]
            object Authenticate(string db, string username, string password, object args);

            [XmlRpcMethod("execute_kw")]
            object ExecuteKw(string db, int uid, string password, string model, string method, object[] args, object kwargs);
        }

        public void RequestVersion()
        {
            try
            {
                const string url = "https://sifhstaging.odoo.com/xmlrpc/2/common"; //"htttps://adeel9911-sifh17-uat1-19004030.dev.odoo.com/xmlrpc/2/common";

                var client = XmlRpcProxyGen.Create<IOdooCommon>();
                client.Url = url;

                var versionInfo = client.GetVersion();

                foreach (var key in versionInfo.Keys)
                {
                    Console.WriteLine($"{key}: {versionInfo[key]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        public bool Login(string username, string password, string databaseName)
        {
            try
            {
                var userid = this.Authenticator();
                this.User = userid;

                return true;
            }
            catch
            {
                return false;
            }
        }
        public int Authenticator()
        {
            try
            {
                const string url = "https://sifhstaging.odoo.com/xmlrpc/2/common"; //"htttps://adeel9911-sifh17-uat1-19004030.dev.odoo.com";

                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = url;

                object x = new { };

                var userId = (int)client.Authenticate(DbName, Username, Password, x);
                Console.WriteLine(userId);

                Console.WriteLine("Connected!");
                return userId;
            }
            catch (CookComputing.XmlRpc.XmlRpcException xmlRpcEx)
            {
                Console.WriteLine($"XML-RPC Error: {xmlRpcEx.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return 0;
            }
        }

        private static readonly HttpClient client = new HttpClient();

        public async Task<int> Authenticate()
        {
            try
            {
                const string url = "https://sifhstaging.odoo.com/jsonrpc/2/common/authenticate"; // JSON-RPC URL

                var requestData = new
                {
                    jsonrpc = "2.0",
                    method = "call",
                    @params = new
                    {
                        db = "adeel9911-sifh17-uat1-19357358", // Replace with your actual DB name
                        login = "admin",
                        password = "1570294945baa5fb63997dbf7a8f4f86ab2e16a0"
                    },
                    id = 1
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);

                if (result.result != null)
                {
                    int userId = result.result;
                    Console.WriteLine($"User ID: {userId}");
                    return userId;
                }
                else
                {
                    Console.WriteLine($"Authentication failed: {result.error.message} Error: {result}");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }


        public void GetFields(int uid)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";


                var fields = new { attributes = new string[] { "string", "help", "type" } };

                var results = client.ExecuteKw(
                    DbName, uid, Password, "receiving.note.line", "fields_get", new object[] { }, fields);


                Console.WriteLine(results);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public object[] SearchRead(int userId)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";


                var searchDomain = new object[]
                {
                    new object[] { "create_date", ">=", "2025-02-02 00:00:00" },
                    new object[] { "create_date", "<=", "2025-02-02 23:59:59" }
                };

                var fieldsAndLimit = new XmlRpcStruct
                {
                    { "fields", new object[]
                        { "create_date", "create_uid", "display_name", "grade", "id", "price_subtotal", "product_id", "product_uom_category_id",
                            "product_uom_id", "quantity", "receiving_note_id", "reference", "set_location_id", "temp", "unit_price", "write_date",
                            "write_uid"
                        }
                    }//,
                    //{ "limit", 5 }
                };


                var result = (object[])client.ExecuteKw(
                    DbName,
                    userId,
                    Password,
                    "receiving.note.line",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                foreach (var item in result)
                {
                    var product = (XmlRpcStruct)item;

                    if (product.ContainsKey("id"))
                        Console.WriteLine($"ID: {product["id"]}");

                    if (product.ContainsKey("product_id"))
                    {
                        var loc = (object[])product["product_id"];
                        Console.WriteLine($"Product Id: {loc[1]}");
                    }

                    if (product.ContainsKey("create_date"))
                        Console.WriteLine($"Create Date: {product["create_date"]}");

                    if (product.ContainsKey("create_uid"))
                    {
                        var loc = (object[])product["create_uid"];
                        Console.WriteLine($"Create UID: {loc[1]}");
                    }

                    if (product.ContainsKey("display_name"))
                        Console.WriteLine($"Display Name: {product["display_name"]}");

                    if (product.ContainsKey("grade"))
                        Console.WriteLine($"Grade: {product["grade"]}");

                    if (product.ContainsKey("price_subtotal"))
                        Console.WriteLine($"Price Subtotal: {product["price_subtotal"]}");

                    if (product.ContainsKey("set_location_id"))
                    {
                        var loc = (object[])product["set_location_id"];
                        Console.WriteLine($"Set Location Id: {loc[1]}");
                    }

                    if (product.ContainsKey("product_uom_category_id"))
                    {
                        var loc = (object[])product["product_uom_category_id"];
                        Console.WriteLine($"Product uom Category Id: {loc[1]}");
                    }

                    if (product.ContainsKey("product_uom_id"))
                    {
                        var loc = (object[])product["product_uom_id"];
                        Console.WriteLine($"Product uom Id: {loc[1]}");
                    }

                    if (product.ContainsKey("quantity"))
                        Console.WriteLine($"Quantity: {product["quantity"]}");

                    if (product.ContainsKey("receiving_note_id"))
                    {
                        var loc = (object[])product["receiving_note_id"];
                        Console.WriteLine($"Receiving Note Id: {loc[1]}");
                    }

                    if (product.ContainsKey("reference"))
                        Console.WriteLine($"Reference: {product["reference"]}");

                    if (product.ContainsKey("temp"))
                        Console.WriteLine($"Temp: {product["temp"]}");

                    if (product.ContainsKey("unit_price"))
                        Console.WriteLine($"Unit Price: {product["unit_price"]}");

                    if (product.ContainsKey("write_date"))
                        Console.WriteLine($"Write Date: {product["write_date"]}");

                    if (product.ContainsKey("write_uid"))
                    {
                        var loc = (object[])product["write_uid"];
                        Console.WriteLine($"Write UID: {loc[1]}");
                    }



                    //if (partner.ContainsKey("country_id"))
                    //{
                    //    var country = (object[])partner["country_id"];
                    //    Console.WriteLine($"Country: {country[1]}"); // country_id is [ID, Name]
                    //}

                    //if (partner.ContainsKey("comment"))
                    //    Console.WriteLine($"Comment: {partner["comment"]}");

                    Console.WriteLine("------------------");
                }

                Console.WriteLine("Done!");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new object[0];
            }
        }

        public void SearchProduct(int userId, DateTime start, DateTime end)
        {
            try
            {
#if DEBUG
                end = DateTime.Parse("2025-02-02 ");
#endif
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var endDateOnly = DateTime.Parse($"{end.ToShortDateString()} 23:59:59");
                var searchDomain = new object[]
                {
                    new object[] { "create_date", ">=", "2025-02-02 00:00:00" },//start.ToString() },
                    new object[] { "create_date", "<=", "2025-02-02 23:59:59" }//endDateOnly.ToString() }
                };

                var fieldsAndLimit = new XmlRpcStruct
                {
                    { "fields", new object[]
                        { "create_date", "create_uid", "display_name", "grade", "id", "price_subtotal", "product_id", "product_uom_category_id",
                            "product_uom_id", "quantity", "receiving_note_id", "reference", "set_location_id", "temp", "unit_price", "write_date",
                            "write_uid"
                        }
                    }//,
                    //{ "limit", 5 }
                };


                var result = (object[])client.ExecuteKw(
                    DbName,
                    userId,
                    Password,
                    "receiving.note.line",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                foreach (var item in result)
                {
                    var product = (XmlRpcStruct)item;

                    if (product.ContainsKey("id"))
                        Console.WriteLine($"ID: {product["id"]}");

                    if (product.ContainsKey("product_id"))
                    {
                        var loc = (object[])product["product_id"];
                        Console.WriteLine($"Product Id: {loc[1]}");
                    }

                    if (product.ContainsKey("create_date"))
                        Console.WriteLine($"Create Date: {product["create_date"]}");

                    if (product.ContainsKey("create_uid"))
                    {
                        var loc = (object[])product["create_uid"];
                        Console.WriteLine($"Create UID: {loc[1]}");
                    }

                    if (product.ContainsKey("display_name"))
                        Console.WriteLine($"Display Name: {product["display_name"]}");

                    if (product.ContainsKey("grade"))
                        Console.WriteLine($"Grade: {product["grade"]}");

                    if (product.ContainsKey("price_subtotal"))
                        Console.WriteLine($"Price Subtotal: {product["price_subtotal"]}");

                    if (product.ContainsKey("set_location_id"))
                    {
                        var loc = (object[])product["set_location_id"];
                        Console.WriteLine($"Set Location Id: {loc[1]}");
                    }

                    if (product.ContainsKey("product_uom_category_id"))
                    {
                        var loc = (object[])product["product_uom_category_id"];
                        Console.WriteLine($"Product uom Category Id: {loc[1]}");
                    }

                    if (product.ContainsKey("product_uom_id"))
                    {
                        var loc = (object[])product["product_uom_id"];
                        Console.WriteLine($"Product uom Id: {loc[1]}");
                    }

                    if (product.ContainsKey("quantity"))
                        Console.WriteLine($"Quantity: {product["quantity"]}");

                    if (product.ContainsKey("receiving_note_id"))
                    {
                        var loc = (object[])product["receiving_note_id"];
                        Console.WriteLine($"Receiving Note Id: {loc[1]}");
                    }

                    if (product.ContainsKey("reference"))
                        Console.WriteLine($"Reference: {product["reference"]}");

                    if (product.ContainsKey("temp"))
                        Console.WriteLine($"Temp: {product["temp"]}");

                    if (product.ContainsKey("unit_price"))
                        Console.WriteLine($"Unit Price: {product["unit_price"]}");

                    if (product.ContainsKey("write_date"))
                        Console.WriteLine($"Write Date: {product["write_date"]}");

                    if (product.ContainsKey("write_uid"))
                    {
                        var loc = (object[])product["write_uid"];
                        Console.WriteLine($"Write UID: {loc[1]}");
                    }

                    Console.WriteLine("------------------");
                }

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public List<DeliveryNote> GetDeliveryNoteByDateRange(DateTime startDate, DateTime endDate, long[] customersIdFilter = null)
        {
            try
            {        
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var customers = customersIdFilter;

                var searchDomain = new object[]
                {
                 new object[] { "date_done", ">=", startDate.ToString("yyyy-MM-dd")},
                 new object[] { "date_done", "<=", endDate.ToString("yyyy-MM-dd")},
                 new object[] { "partner_id", "in", customers },
                 new object[] { "name", "ilike", "DN" }
                };

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "partner_id", "route_id", "location_id", "scheduled_date", "date_done", "origin", "payment_term_id",
                            "carrier_id", "carrier_tracking_ref", "weight", "shipping_weight", "move_type", "user_id", "group_id",
                            "airway_no", "signature_name", "fda", "ufi", "batch_id", "create_date","move_ids", "lot_id"
                        }
                    }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "stock.picking",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var deliveryNotes = new List<DeliveryNote>();
                var moveIdList = new List<Dictionary<string, object>>();
                //var vesselList = new List<Dictionary<string, object>>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    var jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<DeliveryNote>(jsonRecord);
                    
                    if (!item.MoveIds.ToString().Equals("False",StringComparison.InvariantCultureIgnoreCase))
                    {
                        var moveRecord = JsonConvert.SerializeObject(item.MoveIds);
                        var moveIdArray = JsonConvert.DeserializeObject<JArray>(moveRecord);
                        
                        foreach (int moveId in moveIdArray)
                        {
                            var moveIdDict = new Dictionary<string, object>();
                            moveIdDict.Add(item.Id, moveId);
                            moveIdList.Add(moveIdDict);

                        }
                        
                    }
                            //var vesselDict = new Dictionary<string, object>();
                            //vesselDict.Add(item.Id, item.LotNumber);
                            //vesselList.Add(vesselDict);


                    deliveryNotes.Add(item);
                }
                
                //extract and send all move ids as object to getOperation f(x)

                var moveIdObjects = moveIdList.SelectMany(d => d.Values).ToArray();
                //var vesselObjects = vesselList.SelectMany(d => d.Values).ToArray();

                //return list of oerations
                var deliveryOperationsList = new List<DeliveryNoteOperation>();
                if (moveIdObjects != null)
                {
                    deliveryOperationsList = GetDeliveryNoteOperations(moveIdObjects);
                }

                //var vessels = new List<object>();
                //if (vesselObjects != null)
                //{
                //    var idsList = new List<object>();

                //    foreach (var vessel in vesselObjects)
                //    {
                //        if (!vessel.ToString().Equals("false", StringComparison.InvariantCultureIgnoreCase))
                //        {
                //            var moveRecord = JsonConvert.SerializeObject(vessel);
                //            var moveIdArray = JsonConvert.DeserializeObject<JArray>(moveRecord);

                //            idsList.Add(int.Parse(moveIdArray.First().ToString()));
                //        }
                //    }

                //    //vessels = GetVesselsById(idsList.ToArray());
                //}

                //foreach moveid in list of operaion noteId = id, operationList = moveList
                foreach (var deliveryNote in deliveryNotes)
                {
                    var moveRecord = JsonConvert.SerializeObject(deliveryNote.MoveIds);
                    var moveIdArray = JsonConvert.DeserializeObject<JArray>(moveRecord);

                    var deliveryOperations = new List<DeliveryNoteOperation>();
                    foreach (var id in moveIdArray)
                    {
                        var deliveryOperation = deliveryOperationsList.FirstOrDefault(d => d.Id == id.ToString());
                        deliveryOperations.Add(deliveryOperation);
                    }
                    deliveryNote.DeliveryNoteOperation = deliveryOperations;

                    //var vesselRecord = JsonConvert.SerializeObject(deliveryNote.MoveIds);
                    //var vesselArray = JsonConvert.DeserializeObject<JArray>(vesselRecord);
                    
                    //deliveryNote.VesselName = vessel;

                }

                return deliveryNotes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public List<DeliveryNote> GetDeliveryNote(string id)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var searchDomain = new object[]
                {
                    //new object[] { "id", "=", id}
                    new object[] { "name", "ilike", "DN"},
                    new object[] { "origin", "=", id}
                };

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "partner_id", "route_id", "location_id", "scheduled_date", "date_done", "origin", "payment_term_id", 
                            "carrier_id", "carrier_tracking_ref", "weight", "shipping_weight", "move_type", "user_id", "group_id",
                            "airway_no", "signature_name", "fda", "ufi", "batch_id","move_ids"
                        } 
                    }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "stock.picking",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var deliveryNote = new List<DeliveryNote>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine("New Delivery Note:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    var jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<DeliveryNote>(jsonRecord);

                    Console.WriteLine();
                    Console.WriteLine("Delivery Note Operation Items:");

                    if (!item.MoveIds.ToString().Equals("False",StringComparison.InvariantCultureIgnoreCase))
                    {
                        var moveRecords = JsonConvert.SerializeObject(item.MoveIds);
                        var moveArray = JsonConvert.DeserializeObject<JArray>(moveRecords);
                        var deliverNoteOperationList = new List<DeliveryNoteOperation>();

                        var ids = moveArray.ToList().Cast<long>();

                        GetDeliveryNoteOperations(ids.ToArray());
                    
                        foreach (int moveId in moveArray)
                        {
                            var deliveryNoteOperation = GetDeliveryNoteOperations(moveId);

                            deliverNoteOperationList.Add(deliveryNoteOperation);
                        }
                        
                        item.DeliveryNoteOperation = deliverNoteOperationList;
                    }

                    deliveryNote.Add(item);
                    Console.WriteLine("--------------------------------------------");
                }
                Console.WriteLine("Done!");
                
                return deliveryNote;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public List<DeliveryNoteOperation> GetDeliveryNoteOperations( params object[] ids)
        {
            var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
            client.Url = $"{Url}/xmlrpc/2/object";

            var searchDomain = new object[]
            {
                    new object[] { "id", "in", ids }
            };

            var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "product_id", "group_id", "product_uom_qty", "quantity", "end_quantity", "product_uom", "picking_id","move_line_ids","box_no_int"
                        }
                    }
                };

            var result = (object[])client.ExecuteKw(
                DbName,
                User,
                Password,
                "stock.move",
                "search_read",
                new object[] { searchDomain },
                fieldsAndLimit
            );

            var deliveryNoteOp = new List<DeliveryNoteOperation>();
            var moveIdList = new List<Dictionary<string, object>>();
            
            foreach (var record in result)
            {
                var recordData = (XmlRpcStruct)record;
                var jsonRecord = JsonConvert.SerializeObject(record);
                var item = JsonConvert.DeserializeObject<DeliveryNoteOperation>(jsonRecord);
                
                if (!item.MoveLineIds.ToString().Equals("False", StringComparison.InvariantCultureIgnoreCase))
                {
                    var moveRecord = JsonConvert.SerializeObject(item.MoveLineIds);
                    var moveIdArray = JsonConvert.DeserializeObject<JArray>(moveRecord);

                    foreach (int moveId in moveIdArray)
                    {
                        var moveIdDict = new Dictionary<string, object>();
                        moveIdDict.Add(item.Id, moveId);
                        moveIdList.Add(moveIdDict);
                    }
                }
                deliveryNoteOp.Add(item);
            }
            //extract and send all move ids as object to getOperation f(x)

            var moveIdObjects = moveIdList.SelectMany(d => d.Values).ToArray();

            //return list of oerations
            var deliveryOperationsList = new List<DeliveryNoteOperationLine>();
            if (moveIdObjects != null)
            {
                deliveryOperationsList = GetDeliveryNoteOperationLine(moveIdObjects);
            }

            //foreach moveid in list of operaion noteId = id, operationList = moveList
            foreach (var item in deliveryNoteOp)
            {
                var moveRecord = JsonConvert.SerializeObject(item.MoveLineIds);
                var moveIdArray = JsonConvert.DeserializeObject<JArray>(moveRecord);

                var deliveryOperationLines = new List<DeliveryNoteOperationLine>();
                foreach (var id in moveIdArray)
                {
                    var deliveryOperationLine = deliveryOperationsList.FirstOrDefault(d => d.Id == id.ToString());
                    deliveryOperationLines.Add(deliveryOperationLine);
                }
                item.DeliveryNoteOperationLine = deliveryOperationLines;
            }

            return deliveryNoteOp;
        }

        public DeliveryNoteOperation GetDeliveryNoteOperations(int id)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var searchDomain = new object[]
                {
                    new object[] { "id", "=", id }
                };

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "product_id", "group_id", "product_uom_qty", "quantity", "end_quantity", "product_uom", "picking_id","move_line_ids","box_no_int"
                        }
                    }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "stock.move",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var deliveryNoteOp = new DeliveryNoteOperation();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;


                    var jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<DeliveryNoteOperation>(jsonRecord);

                    Console.WriteLine();
                    Console.WriteLine("Delivery Note Operation Line Items:");

                    if (!item.MoveLineIds.ToString().Equals("False", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var moveRecords = JsonConvert.SerializeObject(item.MoveLineIds);
                        var moveArray = JsonConvert.DeserializeObject<JArray>(moveRecords);
                        var deliverNoteOperationLineList = new List<DeliveryNoteOperationLine>();

                        foreach (int movelineId in moveArray)
                        {
                           // var deliveryNoteOperationLine = GetDeliveryNoteOperationLine(movelineId);

                           // deliverNoteOperationLineList.Add(deliveryNoteOperationLine);
                        }

                        item.DeliveryNoteOperationLine = deliverNoteOperationLineList;



                    }


                    deliveryNoteOp = item;
                    Console.WriteLine("--------------------------------------------");
                }
                Console.WriteLine("Done!");
                return deliveryNoteOp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

       

        public List<DeliveryNoteOperationLine> GetDeliveryNoteOperationLine(params object[] ids)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var searchDomain = new object[] 
                {
                    new object[] { "id", "in", ids }
                };

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "picking_id", "origin", "batch_id", "quant_id", "package_id", "date", "result_package_id", 
                            "quantity", "product_uom_id", "product_id", "product_packaging_id", "picking_partner_id"
                        }
                    }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "stock.move.line",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var deliveryNoteOpL = new List<DeliveryNoteOperationLine>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    var jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<DeliveryNoteOperationLine>(jsonRecord);
                    deliveryNoteOpL.Add(item);
                }
                Console.WriteLine("Done!");
                return deliveryNoteOpL;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public List<Customers> GetExportCustomers()
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var searchDomain = new object[]
                {
                    new object[] { "category_id", "in", new object[] { 6 } } // Match records with category_id containing Receivables / Overseas Customer (Fish) Tag ID 6
                };


                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "commercial_company_name", "company_name", "company_type", "category_id", "industry_id", "contact_address"
                        }
                    }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "res.partner",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var exportCustomers = new List<Customers>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine("New Contact:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    var jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<Customers>(jsonRecord);
                    exportCustomers.Add(item);
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------");
                }
                Console.WriteLine("Done!");
                return exportCustomers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public List<Airline> GetAirlines()
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var searchDomain = new object[]
                {
                    new object[] { "company_registry", "in", new object[] { "AIRA01", "AJAG01" } }
                };

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "commercial_company_name", "company_name", "company_type", "category_id", "industry_id", "contact_address"//, "date", "result_package_id",
                            //"quantity", "product_uom_id", "product_id", "product_packaging_id", "picking_partner_id"
                        }
                    }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "res.partner",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var airlines = new List<Airline>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine("New Airline:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    //var jsonRecord = JsonConvert.SerializeObject(record);
                    //var item = JsonConvert.DeserializeObject<Airline>(jsonRecord);
                    //airlines.Add(item);
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------");
                }
                Console.WriteLine("Done!");
                return airlines;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public List<SaleOrder> GetSaleOrder(DateTime start)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var endDateOnly = DateTime.Parse($"{start.ToShortDateString()} 23:59:59");
                var searchDomain = new object[]
                {
                    new object[] { "date_order", ">=", start.ToString() },
                    new object[] { "date_order", "<=", endDateOnly.ToString() }
                    //new object[] { "id", "=", id }
                };

                
                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "partner_id", "partner_invoice_id", "partner_shipping_id", "so_type", "date_order", "pricelist_id",
                            "payment_term_id","client_order_ref", "warehouse_id", "is_yft_export" 
                        }
                    }
                };


                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "sale.order",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var salesOrders = new List<SaleOrder>();
                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine();
                    Console.WriteLine("New Sale Order:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    string jsonRecord = JsonConvert.SerializeObject(record);
                    SaleOrder item = JsonConvert.DeserializeObject<SaleOrder>(jsonRecord);

                    Console.WriteLine();
                    Console.WriteLine("Sale Order Line Items:");

                    var saleOrderLines = GetSaleLineOrder(item.Id);
                    item.Lines = saleOrderLines;

                    salesOrders.Add(item);
                    Console.WriteLine("--------------------------------------------");

                }
                Console.WriteLine("Done!");
                return salesOrders;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public List<SalesOrderLine> GetSaleLineOrder(string saleOrderId)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                //var endDateOnly = DateTime.Parse($"{start.ToShortDateString()} 23:59:59");
                var searchDomain = new object[]
                {
                    new object[] { "order_id", "=", saleOrderId },//saleOrderId },
                };

                /*
                var allFieldsResponse = (XmlRpcStruct)client.ExecuteKw(
                    DbName,
                    userId,
                    Password,
                    "sale.order.line",
                    "fields_get",
                    new object[] { },
                    new XmlRpcStruct()
                    {
                        { "attributes", new string[] { "string" } }
                    }
                );

                var allFields = allFieldsResponse.Keys.Cast<object>().ToArray(); */


                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                             "order_id", "product_template_id", "name", "location_id", "weight", "lot_id", "product_uom_qty", "qty_delivered", "qty_invoiced",
                             "qty_invoiced", "product_uom", "price_unit", "tax_id", "discount", "price_subtotal"
                        }
                    }
                };



                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "sale.order.line",
                    "search_read",
                    new object[] {  },
                    fieldsAndLimit
                );

                var saleOrderLines = new List<SalesOrderLine>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine();
                    Console.WriteLine("New Record:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    string jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<SalesOrderLine>(jsonRecord);
                    saleOrderLines.Add(item);

                    Console.WriteLine("--------------------------");

                }
                Console.WriteLine("Done!");
                return saleOrderLines;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public List<object> GetCustomModel(string model, string[] fieldNames, string searchDomainPre)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var finalSearchDomain = new object[]{};
                var fieldsAndLimit = new XmlRpcStruct
                {
                    { "fields", fieldNames.Cast<object>().ToArray() }
                };

                if (!string.IsNullOrEmpty(searchDomainPre))
                {
                    searchDomainPre = searchDomainPre.Replace(" ", "");
                    var searchDomain = new List<object>();
                    var searchDomainObjects = searchDomainPre.Split('&');

                    foreach (var searchObject in searchDomainObjects)
                    {
                        var searchField = searchObject.Split('=')[0];
                        var searchCat = searchObject.Split('=')[1];

                        object[] searchObjectComplete;

                        if (searchCat.Contains(','))
                        {
                            searchObjectComplete = new object[] { searchField, "in", searchCat.Split(',') };
                        }
                        else
                        {
                            searchObjectComplete = new object[] { searchField, "=", searchCat };
                        }

                        searchDomain.Add(searchObjectComplete);
                    }

                    finalSearchDomain = searchDomain.ToArray();
                }


                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    model,
                    "search_read",
                    new object[] { finalSearchDomain },
                    fieldsAndLimit
                );

                var deliveryNoteOp = new List<object>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine($"New {model}:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    var jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<object>(jsonRecord);

                    Console.WriteLine();


                    deliveryNoteOp.Add(item);
                    Console.WriteLine("--------------------------------------------");
                }
                Console.WriteLine("Done!");
                return deliveryNoteOp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public IEnumerable<object> GetEmployeesByTag(string tagIdentifier)
        {
            var fieldNames = new List<string>();

            fieldNames.Add("category_ids");
            fieldNames.Add("name");

            var searchDomainPre = $"category_ids={tagIdentifier}";

            var results = GetCustomModel("hr.employee", fieldNames.ToArray(), searchDomainPre);
            return results;
        }

        public IEnumerable<Employee> GetEmployeesByTagId(string tagIdentifier)
        {
            var fieldNames = new List<string>();

            fieldNames.Add("category_ids");
            fieldNames.Add("name");

            var searchDomainPre = $"category_ids={tagIdentifier}";

            var results = GetCustomModel("hr.employee", fieldNames.ToArray(), searchDomainPre);


            string jsonRecord = JsonConvert.SerializeObject(results);
            var item = JsonConvert.DeserializeObject<IEnumerable<Employee>>(jsonRecord);

            return item;
        }

        public IEnumerable<Employee> GetConductors(int conductorTag)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "id", "active", "category_ids", "name", "company_id", "department_id", "display_name", "vehicle"
                        }
                    }
                };

                var searchDomain = new object[]
                {
                    new object[] { "category_ids", "=", conductorTag }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "hr.employee",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var conductors = new List<Employee>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine($"New Conductor:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    string jsonRecord = JsonConvert.SerializeObject(result);
                    var conductor = JsonConvert.DeserializeObject<Employee>(jsonRecord);
                    conductors.Add(conductor);

                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------");
                }
                Console.WriteLine("Done!");
                return conductors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<Employee> GetConductorById(int conductorId)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "id", "active", "category_ids", "name", "company_id", "department_id", "display_name", "vehicle"
                        }
                    }
                };

                var searchDomain = new object[]
                {
                    new object[] { "id", "=", conductorId }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "hr.employee",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var conductors = new List<Employee>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    Console.WriteLine($"New Conductor:");

                    foreach (var key in recordData.Keys)
                    {
                        if (recordData[key] is object[] objArray)
                        {
                            Console.WriteLine($"{key} is an object array:");
                            foreach (var obj in objArray)
                            {
                                Console.WriteLine($"  - {obj}");
                            }
                        }
                        else if (recordData[key] is int[] intArray)
                        {
                            Console.WriteLine($"{key} is an int array:");
                            foreach (var num in intArray)
                            {
                                Console.WriteLine($"  - {num}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{key}: {recordData[key]}");
                        }
                    }

                    string jsonRecord = JsonConvert.SerializeObject(result);
                    var conductor = JsonConvert.DeserializeObject<Employee>(jsonRecord);
                    conductors.Add(conductor);

                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------");
                }
                Console.WriteLine("Done!");
                return conductors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public List<Vessel> GetVesselsById(params object[] ids)
        {
            try
            {
                var client = XmlRpcProxyGen.Create<IOdooXmlRpc>();
                client.Url = $"{Url}/xmlrpc/2/object";

                var fieldsAndLimit = new XmlRpcStruct
                {
                    {
                        "fields", new object[]
                        {
                            "id", "sub_contract"
                        }
                    }
                };

                var searchDomain = new object[]
                {
                    new object[] { "id", "in", ids }
                };

                var result = (object[])client.ExecuteKw(
                    DbName,
                    User,
                    Password,
                    "stock.lot",
                    "search_read",
                    new object[] { searchDomain },
                    fieldsAndLimit
                );

                var vessels = new List<Vessel>();

                foreach (var record in result)
                {
                    var recordData = (XmlRpcStruct)record;
                    var jsonRecord = JsonConvert.SerializeObject(record);
                    var item = JsonConvert.DeserializeObject<Vessel>(jsonRecord);
                    
                    vessels.Add(item);
                }
                return vessels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}