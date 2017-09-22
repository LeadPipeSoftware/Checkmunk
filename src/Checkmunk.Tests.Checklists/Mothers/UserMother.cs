using Checkmunk.Domain.Checklists.Entities;

namespace Checkmunk.Tests.Checklists.Mothers
{
	public class UserMother
	{
		public const string ReferenceEmailAddress = "richard@caltech.edu";

		public static User GetReferenceUser()
		{
			return new User(ReferenceEmailAddress);
		}
	}
}
