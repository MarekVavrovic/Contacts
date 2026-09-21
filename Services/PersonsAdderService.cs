using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonsAdderService : IPersonsAdderService
    {        
        private readonly IPersonsRepository _personsRepository;
        
     
        public PersonsAdderService(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
            
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model Validation
            ValidationHelpers.ModelValidation(personAddRequest);

            //Validate PersonName
            if (string.IsNullOrEmpty(personAddRequest.PersonName))
            {
                throw new ArgumentException("PersonName can't be blank");
            }

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            await _personsRepository.AddPerson(person);
            

            //convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

       

    }
}