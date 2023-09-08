using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.Contracts;
using Sifh.ReportGenerator.Model;
using System.Globalization;

namespace Sifh.ReportGenerator.DTO
{
    public class PackingListReportView : IPackingList
    {
        public PackingListReportView()
        {
            ReceivingNoteItemIds = new List<int>();
        }

        public PackingListReportView(IPackingList packingList)
        {
            this.PackingListNumber = packingList.PackingListNumber;
            this.BoxNumber = packingList.BoxNumber;
            this.DateCreated = packingList.DateCreated;
            this.InvoiceNumber = packingList.InvoiceNumber;
            this.CustomerID = packingList.CustomerID;
            this.AirlineID = packingList.AirlineID;
            this.PackingListID = packingList.PackingListID;
            this.StatusClassID = packingList.StatusClassID;
            this.ProductionDate = packingList.ProductionDate;
          
        }

        public int PackingListID { get; set; }
        public int ReceivingNoteID { get; set; }
        public int CustomerID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? DateCreated { get; set; }
        public int StatusClassID { get; set; }
        public int AirlineID { get; set; }
        public int? BoxNumber { get; set; }
        public decimal? Weight { get; set; }
        public string BoatName { get; set; }
        public int? ReceivingNoteItemID { get; set; }
        public int PackingListNumber { get; set; }

        public string CustomerName { get; set; }
        public string AirwayBillNumber { get; set; }
        public string ProductionDate { get; set; }


        public List<int> ReceivingNoteItemIds { get; set; }
    }
}
