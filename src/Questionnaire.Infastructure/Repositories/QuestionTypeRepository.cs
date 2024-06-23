using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Questionnaire.Core;
using System;
using System.Net;

namespace Questionnaire.Infastructure;

public class QuestionTypeRepository : IQuestionTypeRepository
{
    private readonly ILogger<QuestionTypeRepository> _logger;
    private readonly IConfiguration _configuration;

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

    public QuestionTypeRepository(ILogger<QuestionTypeRepository> logger, IConfiguration configuration)
    {

        _configuration = configuration;
        _logger = logger;

        _endpointUrl = configuration.GetSection("CosmosDb")["EndpointUri"]!; // Access endpointUrl
        _authKeyOrResourceToken = configuration.GetSection("CosmosDb")["AuthKeyOrResourceToken"]!; // Access authKeyOrResourceToken
        _applicationName = configuration.GetSection("CosmosDb")["ApplicationName"]!; // Access applicationName
        _databaseId = configuration.GetSection("CosmosDb")["DatabaseName"]!; // Access databaseId
        _containerId = configuration.GetSection("CosmosDb")["QuestionTypeContainerName"]!; // containerId
        _throughPut = Convert.ToInt16(configuration.GetSection("CosmosDb")["Throughput"]); // Access Throughput
        _cosmosClient = new CosmosClient(_endpointUrl, _authKeyOrResourceToken, new CosmosClientOptions() { ApplicationName = _applicationName });


        Task.Run(async () =>
        {
            await GetStartedDemoAsync();
        }).Wait();
    }

    public async Task<QuestionType> CreateAsync(QuestionTypeRequestDTO questionTypeRequestDTO)
    {

        try
        {
            string guid = Guid.NewGuid().ToString();

            var questionType  = new QuestionType
            {
                id = guid,
                type = questionTypeRequestDTO.Type,
                
            };

            //method1
            //await _container.UpsertItemAsync(item);
            //method2
            ItemResponse<QuestionType> response = await _container.CreateItemAsync(questionType, new PartitionKey(guid));

            //ItemResponse<QuestionType> response = await _container.ReadItemAsync<QuestionType>(questionType.id, new PartitionKey(questionType.id));

            return response.Resource;
        }
        catch (CosmosException cosmosException) when (cosmosException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogError(cosmosException.Message);
            return null!;
        }
    }
    public async Task<QuestionType> UpdateAsync(string id, QuestionTypeRequestDTO questionTypeRequestDTO)
    {
        try
        {
            var questionType = new QuestionType
            {
                id = id,
                type = questionTypeRequestDTO.Type,

            };

            ItemResponse<QuestionType> response = await _container!.ReplaceItemAsync(questionType, id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, ex.Message, typeof(QuestionTypeRepository));
            return null!;
        }
    }
    public async Task<int> DeleteAsync(string id)
    {
        try
        {
            await _container!.DeleteItemAsync<QuestionType>(id, new PartitionKey(id));
            return 0;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, ex.Message, typeof(QuestionTypeRepository));

            return -1;
        }
    }
    public async Task<QuestionType> GetAsync(string id)
    {
        try
        {
            ItemResponse<QuestionType> response = await _container!.ReadItemAsync<QuestionType>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, ex.Message, typeof(QuestionTypeRepository));

            return null!;
        }
    }
    public async Task<IEnumerable<QuestionType>> GetAsync()
    {
        try
        {
            var query = _container!.GetItemQueryIterator<QuestionType>();
            List<QuestionType> results = new();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException ex)
        {
            _logger.LogError(ex, ex.Message, typeof(QuestionTypeRepository));
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


    ////QuestionTemplate
    //public Task<QuestionTemplate> CreateQuestionTemplateAsync(QuestionTemplate questionTemplate)
    //{
    //    throw new NotImplementedException();
    //}
    //public Task<QuestionTemplate> UpdateQuestionTemplateAsync(string id, QuestionTemplate questionTemplate)
    //{
    //    throw new NotImplementedException();
    //}
    //public Task DeleteQuestionTemplate(string id)
    //{
    //    throw new NotImplementedException();
    //}
    //public Task<QuestionTemplate> GetQuestionTemplateAsync(string id)
    //{
    //    throw new NotImplementedException();
    //}
    //public Task<IEnumerable<QuestionTemplate>> GetQuestionTemplatesAsync()
    //{
    //    throw new NotImplementedException();
    //}

    //Application
    //public Task<Application> CreateApplicationAsync(Application application)
    //{
    //    throw new NotImplementedException();
    //}

    //QuestionType

}


//await cosmosClient.DisposeAsync();