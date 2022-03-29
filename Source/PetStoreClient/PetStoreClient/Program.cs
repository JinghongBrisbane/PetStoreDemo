using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Model;
using PetStore.Implementation.Default;
using PetStore.Interface;

namespace PetStoreClient
{
	internal class Program
	{
		private static ServiceProvider m_serviceProvider;
		static void Main(string[] args)
		{
			try
			{
				IConfiguration config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();
				RegisterServices(config);

				Console.WriteLine("Loading data...");
				IServiceScope scope = m_serviceProvider.CreateScope();
				var petService = scope.ServiceProvider.GetRequiredService<IPetStoreService>();

				var response = petService.findByStatus(new PetStatus[] { PetStatus.available }).GetAwaiter().GetResult();
				
				if (response.Success)
				{
					var petStore = scope.ServiceProvider.GetRequiredService<IPetStore>();

					var sortedPets = petStore.ShowPets(response.Data, new ShowPetRequest
					{
						CategoryOrder = DisplaySortOrder.Ascending,
						PetNameOrder = DisplaySortOrder.Descending
					});

					foreach(var p in sortedPets)
					{
						Console.WriteLine(petStore.FormatDisplay(p));
					}
				}
				else
				{
					Console.WriteLine(response.ErrorMessage);
				}

				Console.ReadKey();
				DisposeServices();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		private static void DisposeServices()
		{
			if (m_serviceProvider == null)
			{
				return;
			}
			if (m_serviceProvider is IDisposable)
			{
				((IDisposable)m_serviceProvider).Dispose();
			}
		}

		private static void RegisterServices(IConfiguration config)
		{
			var services = new ServiceCollection();
			services.AddSingleton<IServiceKeyProvider, DefaultServiceKeyProvider>();
			services.AddSingleton<IPetStoreService, DefaultPetStoreService>();
			services.AddSingleton<IPetStore, DefaultPetStore>();

			services.AddHttpClient(string.Empty,client =>
			{
				var url = config["ApiUrl"];
				if (!url.EndsWith("/"))
				{
					url = $"{url}/";
				}
				client.BaseAddress = new Uri(url);
			});
			m_serviceProvider = services.BuildServiceProvider(true);
		}
	}
}
