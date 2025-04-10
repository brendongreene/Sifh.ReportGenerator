using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sifh.ReportGenerator.Contracts;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Model;

namespace Sifh.ReportGenerator.Repository
{
    public interface IRepositoryHelper
    {
        void AddPackingList(PackingListReportView packingListItem);
        void AddPackingListID(int receivingNoteItemID);
        void CancelPackingList(int packingListId);
        void createPackingListDetail(PackingListDetails packingListDetails);
        Task<IConductor> GetConductorByIDAsync(int conductorID);
        IEnumerable<IConductor> GetConductors();
        IEnumerable<ICustomer> GetCustomers();
        int GetLastPackingList();
        IEnumerable<PackingListReportView> GetOpenPackingLists();
        int GetPackingListCount();
        IEnumerable<PackingListReportView> GetPackingLists();
        IEnumerable<ReportDataView> GetReceivingNotesByDateRange(DateTime startDate, DateTime endDate);
        Task<ITruck> GetTruckByIDAsync(int truckID);
        IEnumerable<ITruck> GetTrucks();
        string GetVesselName(int receivingNoteId);
        IEnumerable<IVesselView> GetVessels();
    }
}