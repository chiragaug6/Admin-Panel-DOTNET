namespace StartAdminPanel.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        public int CityID { get; set; }

        public int StateID { get; set; }

        public int CountryID { get; set; }

        public string? CityName { get; set; }

        public string? Citycode { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime Modified { get; set; }
    }
}
