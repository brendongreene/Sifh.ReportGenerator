using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Model;
using Sifh.ReportGenerator.Contracts;
using System.Data;
using System.Data.SqlClient;

namespace Sifh.ReportGenerator.Repository
{
    public class RepositoryHelper
    {
        public string cn = "data source=mis.sifishhouse.com;initial catalog=sifhmis;user id=bgreene;password=@Kw5408bi;MultipleActiveResultSets=True;App=EntityFramework";

        public IEnumerable<ReportDataView> GetReceivingNotesByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var context = new SifhContext())
            {
                var receivingNotes =
                    context.ReceivingNotes.Where(x => x.InvoiceDate >= startDate && x.InvoiceDate <= endDate);

                return receivingNotes.ToList().Select( t=> new ReportDataView(t)).ToList();
            }
        }

        public int GetLastPackingList()
        {
            using (var context = new SifhContext())
            {
                var packingList = context.PackingLists
                    .OrderByDescending(x => x.PackingListNumber)
                    .FirstOrDefault();

                return packingList.PackingListNumber;
            }
        }

        public int GetPackingListCount()
        {
            using (var context = new SifhContext())
            {
                var count = context.PackingLists.Count();

                return count;
            }
        }


        public IEnumerable<PackingListReportView> GetPackingLists()
        {

            using (var context = new SifhContext())
            {
                var packingList = context.PackingLists.ToList();
                   

                return packingList.Select( c=> new PackingListReportView(c));
            }
        }

        public void CancelPackingList(int packingListId)
        {
            using (var context = new SifhContext())
            {
                var packingList = context.PackingLists.Find(packingListId);
                if (packingList != null)
                {
                    foreach (var item in packingList.ReceivingNoteItems.ToList())
                    {

                        item.PackingListID = null;
                        var entry = context.Entry(item);
                       
                        entry.State = System.Data.Entity.EntityState.Modified;

                        
                    }

                    var deleteEntry = context.Entry(packingList);
                    deleteEntry.State = System.Data.Entity.EntityState.Deleted;

                }


                context.SaveChanges();
            }

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM PackingListDetails WHERE PackingListID = '{packingListId}'";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void createPackingListDetail(PackingListDetails packingListDetails)
        {
           
                using (SqlConnection connection = new SqlConnection(cn))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"INSERT INTO PackingListDetails (PackingListID, ReceivingNoteItemID, BoxNumber) VALUES ( '{packingListDetails.PackingListID}','{packingListDetails.ReceivingNoteItemID}', '{packingListDetails.BoxNumber}')";
                        command.ExecuteNonQuery();
                    }
                }
        }

        public IEnumerable<PackingListReportView> GetOpenPackingLists()
        {

            using (var context = new SifhContext())
            {
                var packingList = context.PackingLists.Where( t=> t.StatusClassID == 10).ToList();


                return packingList.Select(c => new PackingListReportView(c));
            }
        }

        public void AddPackingList(PackingListReportView packingListItem)
        {
            using (var context = new SifhContext())
            {
                var packingListItemAdd = new PackingList
                {
                    AirlineID = 1,
                    StatusClassID = 10,
                    CustomerID = packingListItem.CustomerID,
                    DateCreated = packingListItem.DateCreated,
                    Weight = packingListItem.Weight,
                    BoatName = packingListItem.BoatName,
                    BoxNumber = packingListItem.BoxNumber,
                    ReceivingNoteItemID = packingListItem.ReceivingNoteItemID,
                    PackingListNumber = packingListItem.PackingListNumber,
                    ProductionDate = packingListItem.ProductionDate,
                };


                foreach (var itemId in packingListItem.ReceivingNoteItemIds)
                {
                    var receivingNoteItem = context.ReceivingNoteItems.Find(itemId);

                    if(receivingNoteItem  != null)
                    {
                        packingListItemAdd.ReceivingNoteItems.Add(receivingNoteItem);
                    }


                }
                context.PackingLists.Add(packingListItemAdd);
                context.SaveChanges();

                foreach (var itemId in packingListItem.ReceivingNoteItemIds)
                {
                    var receivingNoteItem = context.ReceivingNoteItems.Find(itemId);

                    if (receivingNoteItem != null)
                    {
                        using (SqlConnection connection = new SqlConnection(cn))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = $"UPDATE PackingListDetails SET PackingListID = '{receivingNoteItem.PackingListID}' WHERE ReceivingNoteItemID = '{receivingNoteItem.ReceivingNoteItemID}'";
                                command.ExecuteNonQuery();
                            }
                        }
                    }


                }

            }
        }

        public void AddPackingListID(int receivingNoteItemID)
        {
            using (var context = new SifhContext())
            {
                var packingList = context.PackingLists
                    .Where(x => x.ReceivingNoteItemID == receivingNoteItemID)
                    .FirstOrDefault();

                var receivingNoteItem = context.ReceivingNoteItems
                    .Where(x => x.ReceivingNoteItemID == receivingNoteItemID)
                    .FirstOrDefault();

                if (receivingNoteItem != null && packingList != null)
                {
                    receivingNoteItem.PackingListID = packingList.PackingListID;
                    receivingNoteItem.PackingListNumber = packingList.PackingListNumber;
                    context.SaveChanges();
                }
            }
        }

        public IEnumerable<IConductor> GetConductors()
        {
            List<IConductor> conductorsList = new List<IConductor>();

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Conductor";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IConductor conductor = new ConductorView();
                            {
                                conductor.ConductorID = (int)reader["ConductorID"];
                                conductor.LastName = (string)reader["LastName"];
                                conductor.FirstName = (string)reader["FirstName"];
                                conductor.Name = (string)reader["FirstName"] + " " + (string)reader["LastName"];
                                conductor.LicenseNumber = (string)reader["LicenseNumber"];
                            };

                            conductorsList.Add(conductor);
                        }
                    }
                }
            }

            return conductorsList;
        }

        public async Task<IConductor> GetConductorByIDAsync(int conductorID)
        {
            IConductor conductor = null;

            using (SqlConnection connection = new SqlConnection(cn))
            {
                await connection.OpenAsync();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM Conductor WHERE ConductorID = {conductorID}";
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            conductor = new ConductorView(); // Create a new instance of ConductorView
                            conductor.ConductorID = (int)reader["ConductorID"];
                            conductor.LastName = (string)reader["LastName"];
                            conductor.FirstName = (string)reader["FirstName"];
                            conductor.Name = (string)reader["FirstName"] + " " + (string)reader["LastName"];
                            conductor.LicenseNumber = (string)reader["LicenseNumber"];
                        }
                    }
                }
            }

            return conductor;
        }

        public IEnumerable<ITruck> GetTrucks()
        {
            List<ITruck> trucksList = new List<ITruck>();

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Truck";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ITruck truck = new TruckView();
                            {
                                truck.TruckID = (int)reader["TruckID"];
                                truck.License = (string)reader["License"];
                            };

                            trucksList.Add(truck);
                        }
                    }
                }
            }

            return trucksList;
        }

        public async Task<ITruck> GetTruckByIDAsync(int truckID)
        {
            ITruck truck = null;

            using (SqlConnection connection = new SqlConnection(cn))
            {
                await connection.OpenAsync();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM Truck WHERE TruckID = {truckID}";
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            truck = new TruckView(); // Create a new instance of ConductorView
                            truck.TruckID = (int)reader["TruckID"];
                            truck.License = (string)reader["License"];
                        }
                    }
                }
            }

            return truck;
        }

        public string GetVesselName(int receivingNoteId)
        {
            using (var context = new SifhContext())
            {
                var vesselName =
                    context.ReceivingNotes.Where(x => x.ReceivingNoteID == receivingNoteId).FirstOrDefault();

                var boatName = vesselName.Vessel.VesselName;

                return boatName;
            }
        }


        public IEnumerable<ICustomer> GetCustomers()
        {
            using (var context = new SifhContext())
            {
                return context.Customers.ToList();
            }
        }
        public IEnumerable<IVesselView> GetVessels()
        {
            using (var context = new SifhContext())
            {
                var vessels = context.Vessels;
                return context.Vessels.ToList().Select( x=> new VesselView(x)).ToList();
            }
        }
    }
}
