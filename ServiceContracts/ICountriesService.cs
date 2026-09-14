using ServiceContracts.DTO;
namespace ServiceContracts
{
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a new country to the list of countries and returns the added country as a CountryResponse object.
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Return the added country as a CountryResponse object.</returns>
        Task<CountryResponse>AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns a list of all countries as CountryResponse objects.
        /// </summary>
        /// <returns></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country by its ID as a CountryResponse object.
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        Task<CountryResponse?> GetCountryByID(Guid? countryID);
    }
}
