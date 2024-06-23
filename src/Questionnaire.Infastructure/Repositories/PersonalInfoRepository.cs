using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Questionnaire.Core;
using System;
using System.Net;
using System.Reflection;

namespace Questionnaire.Infastructure;

public class PersonalInfoRepository : IPersonalInfoRepository
{
    private readonly ILogger<PersonalInfoRepository> _logger;
    private readonly IConfiguration _configuration;
    private readonly ITemplateRepository _templateRepository;
    private readonly IProgramInfoRepository _programInfoRepository;


    private readonly string _applicationName = string.Empty; // Default endpoint URL for Cosmos DB Emulator
    private readonly string _endpointUrl = string.Empty; // Default endpoint URL for Cosmos DB Emulator
    private readonly string _authKeyOrResourceToken = string.Empty; // Default primary key for Cosmos DB Emulator
    private readonly string _databaseId = string.Empty; // Replace with your database ID
    private readonly string _containerId = string.Empty; // Replace with your container ID                                                   
    private readonly int _throughPut; // Replace with your container ID            

    // The Cosmos client instance
    private CosmosClient? _cosmosClient; // The Cosmos client instance

    // The database we will create
    private Database? _database;

    // The container we will create.
    private Container? _container;

    public PersonalInfoRepository(ILogger<PersonalInfoRepository> logger,
        IConfiguration configuration,
        ITemplateRepository templateRepository,
        IProgramInfoRepository programInfoRepository)
    {

        _configuration = configuration;
        _logger = logger;
        _templateRepository = templateRepository;
        _programInfoRepository = programInfoRepository;

        _endpointUrl = configuration.GetSection("CosmosDb")["EndpointUri"]!; // Access endpointUrl
        _authKeyOrResourceToken = configuration.GetSection("CosmosDb")["AuthKeyOrResourceToken"]!; // Access authKeyOrResourceToken
        _applicationName = configuration.GetSection("CosmosDb")["ApplicationName"]!; // Access applicationName
        _databaseId = configuration.GetSection("CosmosDb")["DatabaseName"]!; // Access databaseId
        _containerId = configuration.GetSection("CosmosDb")["PersonalInfoContainerName"]!; // containerId
        _throughPut = Convert.ToInt16(configuration.GetSection("CosmosDb")["Throughput"]); // Access Throughput
        _cosmosClient = new CosmosClient(_endpointUrl, _authKeyOrResourceToken, new CosmosClientOptions() { ApplicationName = _applicationName });


        Task.Run(async () =>
        {
            await GetStartedDemoAsync();
        }).Wait();
    }

    public async Task<PersonalInfo> CreateAsync(PersonalInfoRequestDTO personalInfoRequestDTO)
    {
        try
        {
            #region validate personal info
            {

                if (string.IsNullOrWhiteSpace(personalInfoRequestDTO.ProgramId))
                {
                    throw new Exception("CustomeError: Program not specified!");
                }

                var exisitingProgramInfo = await _programInfoRepository.GetAsync(personalInfoRequestDTO.ProgramId);
                if (exisitingProgramInfo == null)
                {
                    throw new Exception("CustomeError: Program not recognized!");
                }
            }
            #endregion


            #region validate number of choices made if question has choices 
            {

                if (personalInfoRequestDTO.PersonalInfoQuestionnaire == null
                    || personalInfoRequestDTO.PersonalInfoQuestionnaire!.Count == 0)
                {
                    throw new Exception("CustomeError: No Answer found!");
                }

                var exisitingTemplate = await _templateRepository.GetAsync();
                if (exisitingTemplate.Any())
                {
                    //check the number of choices made if its a question with choices
                    foreach (PersonalInfoQuestionnaireDTO pInfoQuest in personalInfoRequestDTO.PersonalInfoQuestionnaire!)
                    {
                        var template = exisitingTemplate.Where(x => x.id!.Trim() == pInfoQuest.TemplateId!.Trim());
                        if (template != null && template.Any())
                        {
                            if (template.First().maxChoiceAllowed > 0
                                && pInfoQuest.Answers!.Count() != template.First().maxChoiceAllowed)
                            {
                                throw new Exception("CustomeError: Maximum Choice Allowed violated!");
                            }
                        }
                        else
                        {
                            throw new Exception("CustomeError: Invalid Question Template Detected!");
                        }
                    }
                }
                else
                {
                    throw new Exception("CustomeError: Questions not existing!");
                }
            }
            #endregion

            string guid = Guid.NewGuid().ToString();

            // get all the personalInforQuestionaire
            List<PersonalInfoQuestionnaire> pInforQuesyionaire = new();
            foreach (PersonalInfoQuestionnaireDTO pInfoQuest in personalInfoRequestDTO.PersonalInfoQuestionnaire!)
            {
                pInforQuesyionaire.Add(new PersonalInfoQuestionnaire { 
                    templateId = pInfoQuest.TemplateId,
                    answers =  pInfoQuest.Answers,
                });
            }

            var personalInfo = new PersonalInfo
            {
                id = guid,
                firstName = personalInfoRequestDTO.FirstName,
                lastName = personalInfoRequestDTO.LastName,
                email = personalInfoRequestDTO.Email,
                phone = personalInfoRequestDTO.Phone,
                nationality = personalInfoRequestDTO.Nationality,
                currentResidence = personalInfoRequestDTO.CurrentResidence,
                idNumber = personalInfoRequestDTO.IdNumber,
                dateOfBirth = personalInfoRequestDTO.DateOfBirth,
                gender = personalInfoRequestDTO.Gender,
                programId = personalInfoRequestDTO.ProgramId,
                PersonalInfoQuestionnaire = pInforQuesyionaire
            };


            //method1
            //await _container.UpsertItemAsync(item);
            //method2
            ItemResponse<PersonalInfo> response = await _container!.CreateItemAsync(personalInfo, new PartitionKey(guid));

            //ItemResponse<PersonalInfo> response = await _container.ReadItemAsync<PersonalInfo>(PersonalInfo.id, new PartitionKey(PersonalInfo.id));

            return response.Resource;
        }
        catch (CosmosException cosmosException)
        {
            _logger.LogError(cosmosException, cosmosException.Message, nameof(CreateAsync));
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(CreateAsync));
            throw;
        }
    }
    public async Task<PersonalInfo> UpdateAsync(string id, PersonalInfoRequestDTO personalInfoRequestDTO)
    {
        try
        {

            #region validate program info
            {

                if (string.IsNullOrWhiteSpace(personalInfoRequestDTO.ProgramId))
                {
                    throw new Exception("CustomeError: Program not specified!");
                }

                var exisitingProgramInfo = await _programInfoRepository.GetAsync(personalInfoRequestDTO.ProgramId);
                if (exisitingProgramInfo == null)
                {
                    throw new Exception("CustomeError: Program not recognized!");
                }
            }
            #endregion


            #region validate number of choices made if question has choices 
            {

                if (personalInfoRequestDTO.PersonalInfoQuestionnaire == null
                    || personalInfoRequestDTO.PersonalInfoQuestionnaire!.Count == 0)
                {
                    throw new Exception("CustomeError: No Answer found!");
                }

                var exisitingTemplate = await _templateRepository.GetAsync();
                if (exisitingTemplate.Any())
                {
                    //check the number of choices made if its a question with choices
                    foreach (PersonalInfoQuestionnaireDTO pInfoQuest in personalInfoRequestDTO.PersonalInfoQuestionnaire!)
                    {
                        var template = exisitingTemplate.Where(x => x.id == pInfoQuest.TemplateId);
                        if (template != null && template.Any())
                        {
                            if (template.First().maxChoiceAllowed > 0
                                && pInfoQuest.Answers!.Count() != template.First().maxChoiceAllowed)
                            {
                                throw new Exception("CustomeError: Maximum Choice Allowed violated!");
                            }
                        }
                        else
                        {
                            throw new Exception("CustomeError: Invalid Question Detected!");
                        }
                    }
                }
                else
                {
                    throw new Exception("CustomeError: Questions not existing!");
                }
            }
            #endregion


            // get all the personalInforQuestionaire
            List<PersonalInfoQuestionnaire> pInforQuesyionaire = new();
            foreach (PersonalInfoQuestionnaireDTO pInfoQuest in personalInfoRequestDTO.PersonalInfoQuestionnaire!)
            {
                pInforQuesyionaire.Add(new PersonalInfoQuestionnaire
                {
                    templateId = pInfoQuest.TemplateId,
                    answers = pInfoQuest.Answers,
                });
            }

            var personalInfo = new PersonalInfo
            {
                id = id,
                firstName = personalInfoRequestDTO.FirstName,
                lastName = personalInfoRequestDTO.LastName,
                email = personalInfoRequestDTO.Email,
                phone = personalInfoRequestDTO.Phone,
                nationality = personalInfoRequestDTO.Nationality,
                currentResidence = personalInfoRequestDTO.CurrentResidence,
                idNumber = personalInfoRequestDTO.IdNumber,
                dateOfBirth = personalInfoRequestDTO.DateOfBirth,
                gender = personalInfoRequestDTO.Gender,
                programId = personalInfoRequestDTO.ProgramId,
                PersonalInfoQuestionnaire = pInforQuesyionaire
            };

            ItemResponse<PersonalInfo> response = await _container!.ReplaceItemAsync(personalInfo, id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, ex.Message, typeof(PersonalInfoRepository));
            return null!;
        }
    }
    public async Task<int> DeleteAsync(string id)
    {
        try
        {
            await _container!.DeleteItemAsync<PersonalInfo>(id, new PartitionKey(id));
            return 0;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, ex.Message, typeof(PersonalInfoRepository));

            return -1;
        }
    }
    public async Task<PersonalInfo> GetAsync(string id)
    {
        try
        {
            ItemResponse<PersonalInfo> response = await _container!.ReadItemAsync<PersonalInfo>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, ex.Message, typeof(PersonalInfoRepository));

            return null!;
        }
    }
    public async Task<IEnumerable<PersonalInfo>> GetAsync()
    {
        try
        {
            var query = _container!.GetItemQueryIterator<PersonalInfo>();
            List<PersonalInfo> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException ex)
        {
            _logger.LogError(ex, ex.Message, typeof(PersonalInfoRepository));
            return null!;
        }

    }


   
    private async Task CreateDatabaseAsync()
    {
        _database = await _cosmosClient!.CreateDatabaseIfNotExistsAsync(
                  id: _databaseId,
                  throughput: _throughPut
              );

    }

    /// <summary>
    /// Create the container if it does not exist. 
    /// </summary>
    /// <returns></returns>
    private async Task CreateContainerAsync()
    {
        // Create a new container
        _container = await _database!.CreateContainerIfNotExistsAsync(
           id: _containerId,
           partitionKeyPath: "/id",
           throughput: _throughPut
       );
    }

    /// <summary>
    /// Scale the throughput provisioned on an existing Container.
    /// You can scale the throughput (RU/s) of your container up and down to meet the needs of the workload. Learn more: https://aka.ms/cosmos-request-units
    /// </summary>
    /// <returns></returns>
    private async Task ScaleContainerAsync()
    {
        // Read the current throughput
        int? throughput = await _container!.ReadThroughputAsync();
        if (throughput.HasValue)
        {
            Console.WriteLine("Current provisioned throughput : {0}\n", throughput.Value);
            int newThroughput = throughput.Value + 100;
            // Update throughput
            await _container!.ReplaceThroughputAsync(newThroughput);
            Console.WriteLine("New provisioned throughput : {0}\n", newThroughput);
        }
    }

    /// <summary>
    /// Entry point to call methods that operate on Azure Cosmos DB resources in this sample
    /// </summary>
    public async Task GetStartedDemoAsync()
    {
        // Create a new instance of the Cosmos Client
        await this.CreateDatabaseAsync();
        await this.CreateContainerAsync();
        await this.ScaleContainerAsync();
    }
}
