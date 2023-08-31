namespace Datingnew.Models
{
	public class UserLike
	{
		public User SourceUser { get; set; }
		public int SourceId { get; set; }
		public User TargetUser { get; set; }
		public int TargetUserId { get; set; }

	}
}
