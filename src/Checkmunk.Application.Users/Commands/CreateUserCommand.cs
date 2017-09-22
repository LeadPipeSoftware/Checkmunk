using Checkmunk.Contracts.Users.V1.Models;
using MediatR;

namespace Checkmunk.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<UserModel>, IRequest
    {
        public CreateUserCommand(CreateUserModel createUserModel)
        {
            this.CreateUserModel = createUserModel;
        }

        public CreateUserModel CreateUserModel { get; set; }
    }
}
