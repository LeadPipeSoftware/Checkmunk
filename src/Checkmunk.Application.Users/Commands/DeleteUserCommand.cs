using MediatR;

namespace Checkmunk.Application.Users.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public DeleteUserCommand(string emailAddress)
        {
            this.EmailAddress = emailAddress;
        }

        public string EmailAddress { get; set; }
    }
}
