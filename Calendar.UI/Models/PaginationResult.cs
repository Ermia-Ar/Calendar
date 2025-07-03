namespace Calendar.UI.Models
{
	public class PaginationResult<T>
	{
		public T Data { get; set; }

		public int PageNumber { get; set; }

		public int PageSize { get; set; }

		public int TotalRecords { get; set; }
	}
}
