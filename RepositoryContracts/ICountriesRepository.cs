using Entities;
namespace RepositoryContracts
{
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new country to the list of countries and returns the added country as a CountryResponse object.
        /// </summary>
        /// <param name="country"></param>
        /// <returns>Return the added country as a CountryResponse object.</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns a list of all countries as CountryResponse objects.
        /// </summary>
        /// <returns></returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Returns a country by its ID as a CountryResponse object.
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        Task<Country?> GetCountryByID(Guid? countryID);

        /// <summary>
        /// Returns a country object based on the given country name
        /// </summary>
        /// <param name="countryName">Country name to search</param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);


    }
}
