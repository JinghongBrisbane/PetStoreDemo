using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Implementation.Default;
using PetStore.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Tests
{
	[TestClass]
	public class PetStoreTest
	{
		[TestMethod]
		public void TestSortName()
		{
			var petStore = new DefaultPetStore();

			var pets = new Pet[] {
			new Pet
			{
				name = "a"
			}
			,
			new Pet
			{
				name = "b"
			},
			new Pet
			{
				name = "c"
			}

			};

			var result = petStore.ShowPets(pets, new Model.ShowPetRequest
			{
				CategoryOrder = Model.DisplaySortOrder.Ascending,
				PetNameOrder = Model.DisplaySortOrder.Descending
			});

			Assert.AreEqual("a", result.Last().name);
			Assert.AreEqual("c", result.First().name);
		}

		[TestMethod]
		public void TestDisplayFormat()
		{
			var petStore = new DefaultPetStore();

			var p = new Pet
			{
				name = "a",
				Category = new Model.Category
				{
					name = "b"
				}
			};

			var display = petStore.FormatDisplay(p);

			Assert.AreEqual("a - b", display);
		}
	}
}
