namespace JobPortalApi.Database.Models;

public class Reservation : BaseModel
{
    public bool IsFinished { get; set; }

    public double TotalPrice { get; set; }
}