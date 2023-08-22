using CloudinaryDotNet.Actions;

namespace Datingnew.Interfaces
{
	public interface IPhotoService
	{
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task<DeletionResult> DeletePhotoAsync(string publicid);
	}
}
