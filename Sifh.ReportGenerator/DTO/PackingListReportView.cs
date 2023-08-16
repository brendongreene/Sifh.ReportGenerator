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
    public class PackingListReportView : IPackingListReport
    {
        public PackingListReportView()
        {
        }

        public int PackingListId { get; set; }
        public int CustomerId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int StatusClassId { get; set; }
        public int AirlineId { get; set; }
        public int BoxNumber { get; set; }
        public decimal Weight { get; set; }
        public string BoatName { get; set; }
        public int ReceivingNoteItemID { get; set; }
        public int PackingListNumber { get; set; }
    }
}
