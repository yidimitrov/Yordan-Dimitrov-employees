//// See https://aka.ms/new-console-template for more information
//using DataModels.Interfaces;
//using DataModels.Reflections.Csv;
//using DataProcessing.Interfaces;
//using DataProcessing.Process;
//using DataRepository.Interfaces;
//using DataRepository.Processing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;


//using IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices((_, services) =>
//    {
//        services.AddScoped<ILoadable, Loader>();
//        services.AddSingleton<IParsable, ParserCsv>();
//        services.AddScoped<ICsvModelReposity, CsvModelsRepository>();
//        services.AddScoped<IEmployeeParticipationRepository, EmployeeParticipationRepository>();
//        services.AddScoped<IService, ProcessData>();
//    })
//    .Build();

//using IServiceScope serviceScope = host.Services.CreateScope();
//IServiceProvider serviceProvider = serviceScope.ServiceProvider;

//IService services = serviceProvider.GetRequiredService<IService>();

//string csvfile = "EmployeeProjectParicipation.csv";

//var employees = services.GetLongestEmployeesParticipation(csvfile);
