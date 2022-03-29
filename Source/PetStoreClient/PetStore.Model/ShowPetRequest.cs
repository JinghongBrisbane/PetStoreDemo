using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Model
{
	public class ShowPetRequest
	{
		public DisplaySortOrder CategoryOrder { get; set; }
		public DisplaySortOrder PetNameOrder { get; set; }
	}
}
