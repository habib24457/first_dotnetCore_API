using System;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VanillaAPIController : ControllerBase
	{

		/*Adding a dependency injenction for logger on this controller. To do so,
		 *we need to build a contructor. Shortcut for that is: ctor + double tab.
		 *Logger: can help us to show more information to the console 
		 */
		private readonly ILogger<VanillaAPIController> _logger; //a private variable is usually written with an underscore. e.g _logger
        public VanillaAPIController(ILogger<VanillaAPIController> logger) //this is a constructor
        {

			_logger = logger;
		}
		

					
		

		//gets all the datas
		[HttpGet]
		public ActionResult <IEnumerable<VillaDTO>>GetVillas()
		{
			return Ok(VillaStore.villaList);
		}


		//get only one data
		[HttpGet("{id:int}")]

        /*
		[ProducesResponseType(200)] //If we do not add these, the status codes returns undocumented result.
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
		*/

        //cleaner way of writing it
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

		/**
		 * ActionResult is used when we return something with the statuscode. e.g: Ok(singleVilla);
		 * IActionResult is used when we do not return something inside the statuscode. e.g in the delete request, NoContent(); or NotFound();
		 **/
        public ActionResult<VillaDTO>GetOneVilla(int id)
		{
			if(id == 0)
			{
				_logger.LogError("Get Villa Error with Id" + id);
				return BadRequest();
			}

			var singleVilla = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

			if(singleVilla == null)
			{
				return NotFound();
			}

			return Ok(singleVilla);
		}

		[HttpPost]
		public ActionResult<VillaDTO>CreateVilla([FromBody]VillaDTO villaDTO)
		{
			/*ModelState is checking if the validation(Required/MaxLength) on VillaDTO is valid or not*/
			/*if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}*/

			/*Check if the name is unique or not*/
			if (VillaStore.villaList.FirstOrDefault(u=>u.Name.ToLower()==villaDTO.Name.ToLower())!=null)
			{
				ModelState.AddModelError("CustomError", "Villa already Exists");
				return BadRequest(ModelState);
			}

			if(villaDTO == null)
			{
				return BadRequest(villaDTO);
			}

			if(villaDTO.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
			VillaStore.villaList.Add(villaDTO);

			return Ok(villaDTO);
		}

		/*Delete*/
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}",Name ="DeleteVilla")]
		public IActionResult DeleteVilla(int id)
		{
			if(id == 0)
			{
				return BadRequest();
			}

			var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

			if(villa == null)
			{
				return NotFound();
			}

			VillaStore.villaList.Remove(villa);
			return NoContent();
		}

        /*Update Data*/
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name ="UpdateVilla")]
		public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
		{
			if (villaDTO == null || id != villaDTO.Id)
			{
				return BadRequest();
			}

			var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			villa.Name = villaDTO.Name;
			villa.Sqft = villaDTO.Sqft;
			villa.Occupancy = villaDTO.Occupancy;

			return NoContent();
		}

		/*When we want to update only one property from the object
		 *For details: https://jsonpatch.com/ 
		 *The packages that were installed for this
		 *=> 
		 */
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
		{
			if (patchDTO == null || id == 0)
			{
				return BadRequest();
			}

			var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			if (villa == null)
			{
				return BadRequest();
			}
			patchDTO.ApplyTo(villa, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		


	}
}

