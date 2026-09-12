using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Contacts.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {        
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString,
                    string sortBy=nameof(PersonResponse.PersonName), 
                    SortOrderOptions sortOrder = SortOrderOptions.ASC
            )
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
              {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" }
              };

            //Fileter
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons); 
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            ViewBag.Countries = countries.Select(temp => new SelectListItem() 
            {
                Text = temp.CountryName, 
                Value = temp.CountryID.ToString() 
            }
      );

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View();
            }

            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index","Persons");
        }
        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Edit(Guid personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(country =>
            new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryID.ToString(),             
            }
            );

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? existingPerson = _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (existingPerson == null)
            {
                return RedirectToAction("Index");
            }

            if(ModelState.IsValid)
            {
                PersonResponse updatedperson = _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(country =>
                new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryID.ToString(),
                }
                );

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(existingPerson.ToPersonUpdateRequest());
            }
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Delete(Guid? personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse= _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            _personsService.DeletePerson(personUpdateRequest.PersonID);
            return RedirectToAction("Index");
        }
    }
}
