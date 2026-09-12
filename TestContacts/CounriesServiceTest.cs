using ServiceContracts;
using ServiceContracts.DTO;
using Services;


namespace TestContacts
{
    public class CounriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CounriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry method tests
        //1. CountryAddRequest is null, then it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullRequest()
        {
            // Arrange
            CountryAddRequest? request = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(request));

        }

        //2. CountryAddRequest.CountryName is null or empty, then it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(request));

        }
        //3. CountryName is already exists, then it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameAlreadyExists()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);

            });
        }
        //4. CountryName is valid and unique
        [Fact]
        public void AddCountry_ValidUniqueCountryName()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Canada" };
            // Act 
            CountryResponse response = _countriesService.AddCountry(request);
            // Assert
            Assert.True(response.CountryID != Guid.Empty);

        }

        #endregion

        #region GetAllCountries method tests


        //list of countries is empty, then it should return an empty list
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            // Act
            List<CountryResponse> countries = _countriesService.GetAllCountries();
               
            // Assert
            Assert.Empty(countries);
        }
        
        [Fact]
        public void GetAllCountries_NonEmptyList()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "Canada" };

            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);

            // Act
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            // Assert
            Assert.Equal(2, countries.Count);
        }
        #endregion

        #region GetCountryByID method tests

        [Fact]
        public void GetCountryByID_NullCountryID()
        {
            // Arrange
            Guid countryID = Guid.Empty;

            //Act
            CountryResponse response = _countriesService.GetCountryByID(countryID);

            // Assert
            Assert.Null(response);

        }

        //Valid countryID, 
        [Fact]
        public void GetCountryByID_ValidCountryID()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse addedCountry = _countriesService.AddCountry(request);
            //Act
            CountryResponse response = _countriesService.GetCountryByID(addedCountry.CountryID);
            // Assert
            Assert.NotNull(response);
        }

        #endregion

    }
}
