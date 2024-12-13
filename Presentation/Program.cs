using BLL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration) 
                .AddSingleton<FamilyTreeRepository>()            
                .AddSingleton<FamilyTreeService>()              
                .AddSingleton<FamilyTreeApp>()                  
                .BuildServiceProvider();

            var app = serviceProvider.GetService<FamilyTreeApp>();
            app.Run();
        }
    }
}
