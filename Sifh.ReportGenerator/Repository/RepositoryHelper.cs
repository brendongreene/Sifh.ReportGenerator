using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Model;

namespace Sifh.ReportGenerator.Repository
{
    public class RepositoryHelper
    {

        public IEnumerable<ReceivingNoteView> GetReceivingNotesByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var context = new SifhContext())
            {
                var receivingNotes =
                    context.ReceivingNotes.Where(x => x.InvoiceDate >= startDate && x.InvoiceDate <= endDate);

                return receivingNotes.ToList().Select( t=> new ReceivingNoteView(t)).ToList();
            }
        }
    }
}
