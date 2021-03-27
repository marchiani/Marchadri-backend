namespace Marchadri.Common.Models
{
	public class PagingRequestModel
	{
		public int? Limit { get; set; }

		public int? Page { get; set; }

		public PagingRequestModel()
		{
			Limit = -1;
			Page = 1;
		}
    }
}