using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();

            //mock data
            if (initialize)
            {
                _countries.AddRange(new List<Country>() {
        new Country() { CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), CountryName = "USA" },
        new Country() { CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), CountryName = "Canada" },
        new Country() { CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), CountryName = "UK" },
        new Country() { CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), CountryName = "India" },
        new Country() { CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), CountryName = "Australia" }
        });
            }

        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest), "CountryAddRequest cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
            }

            var existingCountry = _countries.FirstOrDefault(c => c.CountryName.Equals(countryAddRequest.CountryName, StringComparison.OrdinalIgnoreCase));

            if (existingCountry != null)
            {
                throw new ArgumentException("Country with the same name already exists.", nameof(countryAddRequest.CountryName));
            }


            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();

            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByID(Guid? countryID)
        {
           if(countryID == null)
            {
                return null;
            }

            Country? country = _countries.FirstOrDefault(c => c.CountryID == countryID);

            if (country == null)            
                return null;
                   
            
                return country.ToCountryResponse();
            }

        
    }
}
