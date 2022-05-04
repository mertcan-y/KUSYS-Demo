using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Core.Utilities
{
    public class FilterHelper<T>
    {
        public string SearchWord { get; set; }
        public List<string> SearchWordIncludes { get; set; }
        public List<string> SearchWordColumns { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public T Data { get; set; }


        public List<string> Includes { get; set; }
        public DataPagingOptions PagingOptions { get; set; } = new DataPagingOptions();
        public string DateColumnName { get; set; }
        public string OrderBy { get; set; }
        public bool IsDesc { get; set; } = false;



    }
}
