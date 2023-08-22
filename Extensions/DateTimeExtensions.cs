namespace Datingnew.Extensions
{
	public static class DateTimeExtensions
	{
		public static int CalculateAge(this DateTime Dob)
		{
			var today = DateOnly.FromDateTime(DateTime.UtcNow);

			var age = today.Year - Dob.Year;
			if (DateOnly.FromDateTime(Dob) > today.AddYears(-age)) age--;
			return age;
		}
	}
}
