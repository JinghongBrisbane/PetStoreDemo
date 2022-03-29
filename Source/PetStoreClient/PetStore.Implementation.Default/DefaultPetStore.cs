using PetStore.Model;
using PetStore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Implementation.Default
{
	public class DefaultPetStore : IPetStore
	{
		public string FormatDisplay(Pet pet)
		{
			return $"{pet?.name} - {pet?.Category?.name}";
		}

		public IEnumerable<Pet> ShowPets(IEnumerable<Pet> allPets,ShowPetRequest showPetRequest)
		{	
			if (showPetRequest.CategoryOrder == DisplaySortOrder.Ascending)
			{
				allPets = allPets.OrderBy(p => p.Category?.name);
			}
			else if (showPetRequest.CategoryOrder == DisplaySortOrder.Descending)
			{
				allPets = allPets.OrderByDescending(p => p.Category?.name);
			}

			if(showPetRequest.CategoryOrder == DisplaySortOrder.None)
			{
				if (showPetRequest.PetNameOrder == DisplaySortOrder.Ascending)
				{
					allPets =  allPets.OrderBy(p => p.name);
				}
				else if (showPetRequest.PetNameOrder == DisplaySortOrder.Descending)
				{
					allPets = allPets.OrderByDescending(p => p.name);
				}
			}
			else
			{
				if (showPetRequest.PetNameOrder == DisplaySortOrder.Ascending)
				{
					allPets = ((IOrderedEnumerable<Pet>)allPets).ThenBy(p => p.name);
				}
				else if (showPetRequest.PetNameOrder == DisplaySortOrder.Descending)
				{
					allPets = ((IOrderedEnumerable<Pet>)allPets).ThenByDescending(p => p.name);
				}
			}

			return allPets;
		}
	}
}
