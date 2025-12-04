using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Commands.AddReview
{
    public class AddReviewCommand : IRequest<int>
    {
        public int ReviewerId { get; set; }
        public int ReviewedUserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
