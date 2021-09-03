using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoOp.Business.IO
{
    public delegate void BarcodeDelegate(Barcode barcode);

    /// <summary>
    /// Schnittstelle zum angeschlossenen Scanner
    /// </summary>
    public class Scanner
    {
        /// <summary>
        /// Wird ausgeführt wenn ein Barcode gefunden wurde
        /// </summary>
        public event BarcodeDelegate BarcodeScanned;
        /// <summary>
        /// ID des angeschlossenen Scanners
        /// </summary>
        public int ScannerID { get; set; }

        public bool IsScanning { get; private set; }

        public Scanner()
        {
            IsScanning = false;
        }
        public void Scan()
        {
            BarcodeScanned.Invoke(new Barcode("Test"));
        }

    }
}
 