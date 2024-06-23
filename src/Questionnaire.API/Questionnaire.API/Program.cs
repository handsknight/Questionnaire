using Questionnaire.Core;
using Questionnaire.Infastructure;

namespace Questionnaire.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IQuestionTypeRepository, QuestionTypeRepository>();
        builder.Services.AddSingleton<ITemplateRepository, TemplateRepository>();
        builder.Services.AddSingleton<IPersonalInfoRepository, PersonalInfoRepository>();
        builder.Services.AddSingleton<IProgramInfoRepository, ProgramInfoRepository>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

