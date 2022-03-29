using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Model
{
	public class BaseServiceResponse<T> where T : class 
	{
		public bool Success { get; set; }	
		public string ErrorMessage { get; set; }
		public T Data { get; set; }
	}
}
