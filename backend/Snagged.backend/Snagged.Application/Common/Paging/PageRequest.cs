using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Snagged.Application.Common.Paging
{
    public class PageRequest
    {
        private const int MaxPageSize = 100;
        public int Page { get; init; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = value <= 0 ? 10 : value > MaxPageSize ? MaxPageSize : value;
        }
        [JsonIgnore]
        public int SkipCount => (Page - 1) * PageSize;
    }
}
