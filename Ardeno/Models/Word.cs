using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardeno.Models
{
    public class Word
    {
        public int WordId { get; set; }
        public string CurrentWord { get; set; }
        public string Hint { get; set; }
        public double Done { get; set; }

    }
}
