namespace Marchadri.Common.Models
{
	public class BaseListResponseModel<T>
	{
		public int TotalCount { get; set; }

		public T[] Data { get; set; }
	}
}