using System;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VanillaAPIController : ControllerBase
	{
		//gets all the datas
		[HttpGet]
		public IEnumerable<VillaDTO>GetVillas()
		{
			return VillaStore.villaList;
		}


		[HttpGet("{id:int}")]
		public VillaDTO GetOneVilla(int id)
		{
			return VillaStore.villaList.FirstOrDefault(u => u.Id == id);
		}
	}
}

