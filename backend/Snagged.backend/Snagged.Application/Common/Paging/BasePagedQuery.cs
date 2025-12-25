using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Common.Paging
{
    public abstract class BasePagedQuery<TItem> : IRequest<PageResult<TItem>>
    {
        /// <summary>Pagination parameters (page number and page size).</summary>
        public PageRequest Paging { get; init; } = new();
    }
}
