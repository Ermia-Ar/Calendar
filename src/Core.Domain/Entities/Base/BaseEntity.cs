namespace Core.Domain.Entities.Base;

public class BaseEntity
{
	public long Id { get; set; }

	public DateTime CreatedDate { get; init; }

	public DateTime? UpdateDate { get; set; }

	public bool IsActive { get; set; }

	public void MakeUnActive()
	{
		IsActive = false;
	}

}
