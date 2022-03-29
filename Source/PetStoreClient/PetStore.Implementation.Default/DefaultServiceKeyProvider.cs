using PetStore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Implementation.Default
{
	public class DefaultServiceKeyProvider : IServiceKeyProvider
	{
		public string GetKey()
		{
			//need a secure way but just return for now
			return "special-key";
		}
	}
}
