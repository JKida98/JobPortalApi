using System.ComponentModel.DataAnnotations;

namespace JobPortalApi.Models.Requests
{
    public class OfferForCreation
    {        
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Offer description is required")]
        public string Description { get; set; }
       
        public double HourlyPrice { get; set; }
    }
}
