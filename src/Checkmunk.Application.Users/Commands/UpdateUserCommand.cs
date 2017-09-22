using Checkmunk.Contracts.Users.V1.Models;
using MediatR;

namespace Checkmunk.Application.Users.Commands
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public UpdateUserCommand(string emailAddress, UpdateUserModel updateUserModel)
        {
            this.EmailAddress = emailAddress;
            this.UpdateUserModel = updateUserModel;
        }

        public string EmailAddress { get; set; }

        public UpdateUserModel UpdateUserModel { get; set; }
    }
}
