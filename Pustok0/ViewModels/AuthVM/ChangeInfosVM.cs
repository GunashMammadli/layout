using System.ComponentModel.DataAnnotations;

namespace Pustok0.ViewModels.AuthVM
{
	public class ChangeInfosVM
	{
		[Required, MaxLength(36)]
		public string? Fullname { get; set; }
		[Required, DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
		[Required, MaxLength(24)]
		public string? Username { get; set; }
		[DataType(DataType.Password), Compare(nameof(ConfirmPassword))]
		public string? Password { get; set; }
		[DataType(DataType.Password)]
		public string? ConfirmPassword { get; set; }
	}
}
