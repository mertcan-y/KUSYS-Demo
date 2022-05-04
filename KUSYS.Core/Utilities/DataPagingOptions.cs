using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Core.Utilities
{
    public class DataPagingOptions
    {
        public int? PageSize { get; set; } = 100;
        public int? PageNumber { get; set; } = 0;
    }
}
