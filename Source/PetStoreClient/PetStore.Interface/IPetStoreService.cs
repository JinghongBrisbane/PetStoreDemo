using PetStore.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetStore.Interface
{
	public interface IPetStoreService
	{
		Task<BaseServiceResponse<IEnumerable<Pet>>> findByStatus(IEnumerable<PetStatus> status);
	}
}
