using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
