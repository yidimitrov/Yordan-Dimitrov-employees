using DataModels.Interfaces;
using DataModels.Reflections.Csv;
using DataProcessing.Interfaces;
using DataProcessing.Process;
using DataRepository.Interfaces;
using DataRepository.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataPresentationUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddScoped<ILoadable, Loader>();
                services.AddSingleton<IParsable, ParserCsv>();
                services.AddScoped<ICsvModelReposity, CsvModelsRepository>();
                services.AddScoped<IEmployeeParticipationRepository, EmployeeParticipationRepository>();
                services.AddScoped<IService, ProcessData>();
            })
            .Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            IService services = serviceProvider.GetRequiredService<IService>();

            //string csvfile = "EmployeeProjectParicipation.csv";

            

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new EmployeesViewer(services));
        }
    }
}