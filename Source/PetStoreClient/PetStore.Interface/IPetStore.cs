using PetStore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Interface
{
	public interface IPetStore
	{
		IEnumerable<Pet> ShowPets(IEnumerable<Pet> allPets, ShowPetRequest showPetRequest);
		string FormatDisplay(Pet pet);
	}
}
