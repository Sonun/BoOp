using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.Business.Models
{
    public class BookReference
    {
        private int _rating;

        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                if (value > 5)
                {
                    _rating = 5;
                }
                else if (value < 5)
                {
                    _rating = 0;
                }
            }
        }
        public string RatingText { get; set; }

        public DateTime TimeWritten { get; set; }
    }
}
