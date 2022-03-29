using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Implementation.Default;
using PetStore.Interface;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using PetStore.Model;
using Moq.Protected;
using System.Threading;

namespace PetStore.Tests
{
	[TestClass]
	public class PetServiceTests
	{
		[TestMethod]
		public async Task TestSimple()
		{

			var pets = new List<Pet>();
			pets.Add(new Pet
			{
				name = "a",
				id = 1,
				status = PetStatus.pending,
			    Category = new Category
				{
					id = 1,
					name = "test"
				}
			});
			var mockHttpClientFactory = new Mock<IHttpClientFactory>();

			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(JsonConvert.SerializeObject(pets))
				});

			var client = new HttpClient(mockHttpMessageHandler.Object);
			client.BaseAddress = new System.Uri("https://xxxxxx/");

			mockHttpClientFactory.Setup(f => f.CreateClient(string.Empty)).Returns(client);

			var defaultService = new DefaultPetStoreService(mockHttpClientFactory.Object, new DefaultServiceKeyProvider());

			var result = await defaultService.findByStatus(new PetStatus[] { PetStatus.available });

			Assert.IsTrue(result.Success);
			Assert.IsTrue(result.Data.Any());
			Assert.AreEqual(result.Data.First().name, pets.First().name);
		}


		[TestMethod]
		public async Task TestHttp400()
		{
			var mockHttpClientFactory = new Mock<IHttpClientFactory>();

			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.BadRequest,
					Content = new StringContent("Bad request")
				});

			var client = new HttpClient(mockHttpMessageHandler.Object);
			client.BaseAddress = new System.Uri("https://xxxxxx/");

			mockHttpClientFactory.Setup(f => f.CreateClient(string.Empty)).Returns(client);

			var defaultService = new DefaultPetStoreService(mockHttpClientFactory.Object, new DefaultServiceKeyProvider());

			var result = await defaultService.findByStatus(new PetStatus[] { PetStatus.available });

			Assert.IsFalse(result.Success);
			Assert.IsNull(result.Data);
			Assert.AreEqual(result.ErrorMessage, "Bad request");
		}

		[TestMethod]
		public async Task TestEmptyStatus()
		{
			var mockHttpClientFactory = new Mock<IHttpClientFactory>();

			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.BadRequest,
					Content = new StringContent("Bad request")
				});

			var client = new HttpClient(mockHttpMessageHandler.Object);
			client.BaseAddress = new System.Uri("https://xxxxxx/");

			mockHttpClientFactory.Setup(f => f.CreateClient(string.Empty)).Returns(client);

			var defaultService = new DefaultPetStoreService(mockHttpClientFactory.Object, new DefaultServiceKeyProvider());

			var result = await defaultService.findByStatus(new PetStatus[0]);

			Assert.IsFalse(result.Success);
			Assert.IsNull(result.Data);
			Assert.AreEqual(result.ErrorMessage, "Need a status at least");
		}
	}
}
