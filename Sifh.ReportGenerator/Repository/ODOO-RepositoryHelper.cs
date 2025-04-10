using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sifh.ReportGenerator.Contracts;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Model;
using SIFH_ERP_ClassLib;

namespace Sifh.ReportGenerator.Repository
{
    public class ODOO_RepositoryHelper : IRepositoryHelper
    {
        OdooModelsGet odooRpcApi;
        private bool isAuthenticated { get; set; }
        public ODOO_RepositoryHelper(string url, string userName, string password, string databaseName)
        {
            odooRpcApi = new OdooModelsGet();
            odooRpcApi.Url = url;

            isAuthenticated = odooRpcApi.Login(userName, password, databaseName);
        }


        public void AddPackingList(PackingListReportView packingListItem)
        {
            throw new NotImplementedException();
        }

        public void AddPackingListID(int receivingNoteItemID)
        {
            throw new NotImplementedException();
        }

        public void CancelPackingList(int packingListId)
        {
            throw new NotImplementedException();
        }

        public void createPackingListDetail(PackingListDetails packingListDetails)
        {
            throw new NotImplementedException();
        }

        public async Task<IConductor> GetConductorByIDAsync(int conductorID)
        {
            var drivers = odooRpcApi.GetConductorById(conductorID);

            foreach (var driver in drivers)
            {
                    var record = new ConductorView
                    {
                        FirstName = driver.Name,
                        Name = driver.Name
                    };

                    return record;
                
            }

            return null; // or throw if you prefer to signal "not found"
        }


        public IEnumerable<IConductor> GetConductors()
        {
            var drivers = odooRpcApi.GetConductors(1); //GetConductors
            //var drivers = odooRpcApi.GetEmployeesByTagId("1"); //GetConductors

            var results = new List<ConductorView>();


            foreach( var driver in drivers)
            {
                var record = new ConductorView();
                record.FirstName = driver.Name;
                record.Name = driver.Name;

                results.Add(record);
            }

            return results;
        }

        public IEnumerable<ICustomer> GetCustomers()
        {
            var customers = odooRpcApi.GetExportCustomers();

            var customerView = new List<CustomerView>();
            foreach(var customer in customers)
            {
                var cv = new CustomerView
                {
                    CustomerID = int.Parse(customer.Id),
                    CustomerName = customer.CommercialCompanyName
                };

                customerView.Add(cv);
            }
            return customerView;
            //throw new NotImplementedException();
        }

        public int GetLastPackingList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PackingListReportView> GetOpenPackingLists()
        {
            throw new NotImplementedException();
        }

        public int GetPackingListCount()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PackingListReportView> GetPackingLists()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReportDataView> GetReceivingNotesByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var exportCustomers = odooRpcApi.GetExportCustomers().Select( x=> Convert.ToInt64(x.Id)).ToArray<long>();

                var response = odooRpcApi.GetDeliveryNoteByDateRange(startDate, endDate, exportCustomers);
                var reportDataView = new List<ReportDataView>();
                var vesselList = new List<Dictionary<string, object>>();
                foreach (var deliveryNote in response)
                {
                    var groupArray = JsonConvert.DeserializeObject<JArray>(deliveryNote.GroupId.ToString());
                    var packingListId = groupArray[0].ToString();
                    var sourceDocument = groupArray[1].ToString();

                    var partnerId = JsonConvert.DeserializeObject<JArray>(deliveryNote.PartnerId.ToString())[0].ToString();

                    var receivingNoteDetails = new List<ReceivingNoteItemView>();

                    foreach (var operation in deliveryNote.DeliveryNoteOperation)
                    {
                        var pID = JsonConvert.DeserializeObject<JArray>(operation.ProductId.ToString())[0].ToString();

                        var item = new ReceivingNoteItem();
                        item.Quantity = Convert.ToDecimal(operation.Quantity);
                        item.ProductID = int.Parse(pID);
                        item.ProductName = operation.ProductId.ToString()[1].ToString();

                        receivingNoteDetails.Add(new ReceivingNoteItemView(item));
                    }

                    var vesselDict = new Dictionary<string, object>();
                    vesselDict.Add(deliveryNote.Id, deliveryNote.LotNumber);
                    vesselList.Add(vesselDict);


                    dynamic vesselInfo = deliveryNote.LotNumber.ToString().Equals("false", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : JsonConvert.DeserializeObject<dynamic>(deliveryNote.LotNumber.ToString());

                    var results = new List<ReportDataView>();

                    foreach (var grp in receivingNoteDetails.GroupBy(x => x.ProductID))
                    {

                        var reportData = new ReportDataView
                        {
                            InvoiceNumber = sourceDocument,
                            ReceivingNoteID = int.Parse(deliveryNote.Id),
                            InvoiceDate = DateTime.Parse(deliveryNote.DateDone),
                            ReferenceNumber = deliveryNote.CarrierTrackingRef,
                            //PackingListID = int.Parse(packingListId),
                            VesselID = vesselInfo is JArray ? vesselInfo[0] : -1,
                            //StatusClassID = ,
                            //TotalPayments = ,
                            //CheckNumber1 = ,
                            //CheckNumber2 = ,
                            DateCreated = DateTime.Parse(deliveryNote.CreateDate),
                            //VesselName = JsonConvert.DeserializeObject<JArray>(deliveryNote.Vessel.ToString())[1].ToString(),
                            //RegistrationNumber = ,

                            Quantity = receivingNoteDetails.Sum(j => j.Quantity),
                            LineItems = deliveryNote.DeliveryNoteOperation.Count(),
                            ProductName = grp.First().ProductName,

                            CustomerName = JsonConvert.DeserializeObject<JArray>(deliveryNote.PartnerId.ToString())[1].ToString(),
                            CustomerID = JsonConvert.DeserializeObject<JArray>(deliveryNote.PartnerId.ToString())[0].ToString(),
                            //AirwayBillNumber = ,
                            ProductionDate = deliveryNote.DateDone,
                            //ReceivingLotIdentifierMRC = ,
                            //BoxNumber = ,
                            //TotalBoxes = ,

                            //ConductorName = ,
                            //ConductorLicense = ,

                            //TruckLicense = ,
                            //VesselIDForLicence = ,

                            ReceivingNoteDetails = grp.ToList()
                        };

                        reportData.NetQuantity = reportData.Quantity * reportData.ConversionToKg;
                        reportData.ReceivingNoteDetails = grp.ToList();
                        reportDataView.Add(reportData);
                    }
                }

                //call getVesselName

                var vesselObjects = vesselList.SelectMany(d => d.Values).ToArray();

                var vessels = new List<SIFH_ERP_ClassLib.ModelClasses.Vessel>();
                if (vesselObjects != null)
                {
                    var idsList = new List<object>();

                    foreach (var vessel in vesselObjects)
                    {
                        if (!vessel.ToString().Equals("false", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var moveRecord = JsonConvert.SerializeObject(vessel);
                            var moveIdArray = JsonConvert.DeserializeObject<JArray>(moveRecord);

                            idsList.Add(int.Parse(moveIdArray.First().ToString()));
                        }
                    }

                    vessels = odooRpcApi.GetVesselsById(idsList.ToArray());
                }

                var vesselDictionary = vessels.ToDictionary(x => Convert.ToInt32(x.Id));

                //foreach deliveryNote in esponse vesselName = getvesselName.result
                foreach (var deliveryNote in reportDataView)
                {
                    var vesselId =  deliveryNote.VesselID;
                    var vesselString = vesselDictionary.ContainsKey(deliveryNote.VesselID) ? vesselDictionary[deliveryNote.VesselID].VesselName : string.Empty;

                    if (!string.IsNullOrEmpty(vesselString.ToString()))
                    {
                        var vessel = ((Newtonsoft.Json.Linq.JContainer)vesselString).Last.ToString();
                        Match vesselName = Regex.Match(vessel, @"F/V.+|FV.+");
                        deliveryNote.VesselName = vesselName.Value.ToString().Replace("/", "").Replace("'", "").Replace('"', ' ').Trim();
                    }
                }

                return reportDataView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public  async Task<ITruck> GetTruckByIDAsync(int truckID)
        {
#if DEBUG
           

            var truck = new TruckView()
            {
                TruckID = -1,
                License = "TEST"
            };

            return truck;

#endif
            throw new NotImplementedException();
        }

        public IEnumerable<ITruck> GetTrucks()
        {
            throw new NotImplementedException();
        }

        public string GetVesselName(int receivingNoteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVesselView> GetVessels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReceivingNoteItem> GetReceivingNoteItems(int packingListID)
        {
            throw new NotImplementedException();
        }

        public Product GetProductName(int productID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReceivingNoteItem> GetReceivingNoteItemsByReceivingNoteID(int receivingNoteID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReceivingNote> GetReceivingNotesDetails(int packingListID)
        {
            throw new NotImplementedException();
        }

        public int GetTotalNumberOfBoxes(int packingListID)
        {
            throw new NotImplementedException();
        }

        public Vessel GetVesselByID(int vesselID)
        {
            throw new NotImplementedException();
        }
    }
}
