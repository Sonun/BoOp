using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business.IO
{
    /// <summary>
    /// Barcode der gescannt wird
    /// </summary>
    public class Barcode
    {
        public DateTime ScanDate { get; set; }

        public string Text { get; private set; }

        public Barcode(string text)
        {
            ScanDate = DateTime.Now;
            Text = text;
        }
    }
}
