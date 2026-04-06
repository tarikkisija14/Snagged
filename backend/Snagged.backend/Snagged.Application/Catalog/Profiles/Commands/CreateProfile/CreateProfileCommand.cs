using MediatR;

namespace Snagged.Application.Catalog.Profiles.Commands.CreateProfile
{
    public class CreateProfileCommand : IRequest<ProfileDto>
    {
        public int UserId { get; set; }
    }
}