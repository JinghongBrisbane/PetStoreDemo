using Newtonsoft.Json;
using PetStore.Model;
using PetStore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Implementation.Default
{
	public class DefaultPetStoreService : IPetStoreService
	{
		private readonly IHttpClientFactory m_factory;
        private readonly IServiceKeyProvider m_serviceKeyProvider;
		public DefaultPetStoreService(IHttpClientFactory factory, IServiceKeyProvider serviceKeyProvider)
		{
			m_factory = factory;
            m_serviceKeyProvider = serviceKeyProvider;
        }

		public async Task<BaseServiceResponse<IEnumerable<Pet>>> findByStatus(IEnumerable<PetStatus> status)
		{
            var result = new BaseServiceResponse<IEnumerable<Pet>>();

			if (!status.Any())
			{
                result.Success = false;
                result.ErrorMessage = "Need a status at least";
                return result;
			}

            var client = m_factory.CreateClient(String.Empty);

			string reuqestUrl = $"pet/findByStatus?{string.Join("&", status.Select(s => $"status={s}"))}";

            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod("GET");
                request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
                request.Headers.Add("api_key", m_serviceKeyProvider.GetKey());
                request.RequestUri = new Uri(client.BaseAddress + reuqestUrl);

                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var httpStatus = (int)response.StatusCode;
                if (httpStatus == 200)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var pets = JsonConvert.DeserializeObject<IEnumerable<Pet>>(responseJson);

                    result.Success = true;
                    result.Data = pets;
                }
                else
                if (httpStatus == 400)
                {
                    string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync();
                    result.Success = false;
                    result.ErrorMessage = responseText;
                }
                else
                {
                    var responseText = response.Content == null ? null : await response.Content.ReadAsStringAsync();
                    result.Success = false;
                    result.ErrorMessage = responseText;
                }
            }

            return result;
        }
	}
}
