using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.Models
{
    public class QueryListCriteria
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Keyword { get; set; }

    }
}
