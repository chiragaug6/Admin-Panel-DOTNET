namespace StartAdminPanel.Areas.LOC_State.Models
{
    public class LOC_StateModel
    {
        public int StateId { get; set; }

        public string? StateName { get; set; }

        public int CountryId { get; set; }

        public string? StateCode { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
