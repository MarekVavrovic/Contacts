using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;

namespace TestContacts
{
    public class CounriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CounriesServiceTest()
        {
            var countriesInitialData = new List<Country>() { };

            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(x => x.Countries, countriesInitialData);

            _countriesService = new CountriesService(dbContext);
        }


        #region AddCountry method tests
        //1. CountryAddRequest is null, then it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullRequest()
        {
            // Arrange
            CountryAddRequest? request = null;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>( () =>  _countriesService.AddCountry(request));

        }

        //2. CountryAddRequest.CountryName is null or empty, then it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _countriesService.AddCountry(request));

        }
        //3. CountryName is already exists, then it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameAlreadyExists()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
               await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);

            });
        }
        //4. CountryName is valid and unique
        [Fact]
        public async Task AddCountry_ValidUniqueCountryName()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Canada" };
            // Act 
            CountryResponse response = await _countriesService.AddCountry(request);
            // Assert
            Assert.True(response.CountryID != Guid.Empty);

        }

        #endregion

        #region GetAllCountries method tests


        //list of countries is empty, then it should return an empty list
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            // Act
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
               
            // Assert
            Assert.Empty(countries);
        }
        
        [Fact]
        public async Task GetAllCountries_NonEmptyList()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "Canada" };

            await _countriesService.AddCountry(request1);
            await _countriesService.AddCountry(request2);

            // Act
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

            // Assert
            Assert.Equal(2, countries.Count);
        }
        #endregion

        #region GetCountryByCountryID

        [Fact]
        //If we supply null as CountryID, it should return null as CountryResponse
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countrID = null;

            //Act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByID(countrID);


            //Assert
            Assert.Null(country_response_from_get_method);
        }


        [Fact]
        //If we supply a valid country id, it should return the matching country details as CountryResponse object
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //Act
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByID(country_response_from_add.CountryID);

            //Assert
            Assert.Equal(country_response_from_add, country_response_from_get);
        }
        #endregion

    }
}
