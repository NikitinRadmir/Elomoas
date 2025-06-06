using MediatR;
using Microsoft.AspNetCore.Http;

namespace Elomoas.Application.Features.Settings.Commands.UpdateAccountInfo
{
    public class UpdateAccountInfoCommand : IRequest<bool>
    {
        public string IdentityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }

        public UpdateAccountInfoCommand(
            string identityId,
            string firstName,
            string lastName,
            string email,
            string description,
            IFormFile imageFile)
        {
            IdentityId = identityId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Description = description;
            ImageFile = imageFile;
        }
    }
} 