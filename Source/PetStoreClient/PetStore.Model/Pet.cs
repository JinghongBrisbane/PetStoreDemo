using PetStore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Interface
{
	public class Pet
	{
		public long id { get; set; }
		public Category Category { get; set; }
		public string name { get; set; }
		public IEnumerable<string> photoUrls { get; set; }
		public IEnumerable<Tag> tags { get; set; }
		public  PetStatus status { get; set; }
	}
}
