using fitness_db.Interfaces;
using fitness_db.Models;
using fitness_Nutrition_service.Dto.Req;
using Microsoft.AspNetCore.Mvc;

namespace fitness_nutrition_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionController : Controller
    {
        private readonly INutritionRepository _nutritionRep;
        public NutritionController(INutritionRepository nutritionRepository)
        {
            _nutritionRep = nutritionRepository;
        }

        [HttpPost]
        public IActionResult CreateNutrition([FromBody] ReqNutritionDto nutritionReq)
        {
            try
            {
                if (nutritionReq == null)
                    return BadRequest(new
                    {
                        status = "failed",
                        message = "Requset not valid"
                    });

                var isNutritionExist = _nutritionRep.GetNutritions()
                    .Where(u => u.NutritionName.Trim().ToLower() == nutritionReq.NutritionName.Trim().ToLower())
                    .FirstOrDefault();

                if (isNutritionExist != null)
                {
                    ModelState.AddModelError("", "Nutrition already exists");
                    if (!ModelState.IsValid)
                    {
                        var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                        return BadRequest(new
                        {
                            status = "failed",
                            errors = errors
                        });
                    }
                }

                var nutrition = new Nutrition
                {
                    NutritionName = nutritionReq.NutritionName,
                    Calories = nutritionReq.Calories,
                    Protein = nutritionReq.Protein,
                    Carbs = nutritionReq.Carbs,
                    Fat = nutritionReq.Fat
                };

                if (!_nutritionRep.CreateNutrition(nutrition))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                return Ok(new
                {
                    status = "success",
                    message = "Nutrition Successfully created"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "failed",
                    message = e.Message
                });
                throw;
            }
        }

        [HttpPut("{nutritionId}")]
        public IActionResult UpdateNutrition(int nutritionId, [FromBody] ReqNutritionDto nutritionReq)
        {
            if (nutritionReq == null)
                return BadRequest(new
                {
                    status = "failed",
                    message = "Requset not valid"
                });

            var isNutritionExist = _nutritionRep.GetNutritions()
                    .Where(u => u.NutritionID == nutritionId)
                    .FirstOrDefault();

            if (isNutritionExist == null)
                return NotFound(new
                {
                    status = "failed",
                    message = "Nutrition not found!"
                });


            isNutritionExist.NutritionID = nutritionId;
            isNutritionExist.NutritionName = nutritionReq.NutritionName;
            isNutritionExist.Calories = nutritionReq.Calories;
            isNutritionExist.Protein = nutritionReq.Protein;
            isNutritionExist.Carbs = nutritionReq.Carbs;
            isNutritionExist.Fat = nutritionReq.Fat;

            var updatedNutrition = _nutritionRep.UpdateNutrition(isNutritionExist);
            if (updatedNutrition == null)
            {
                ModelState.AddModelError("", "Something went wrong updating nutrition");
                return StatusCode(500, new
                {
                    status = "failed",
                    message = "Something went wrong updating nutrition"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Nutrition Successfully updated",
                data = updatedNutrition
            });
        }

        [HttpDelete("{nutritionId}")]
        public IActionResult DeleteNutrition(int nutritionId)
        {
            var isNutritionExist = _nutritionRep.GetNutritions()
                    .Where(u => u.NutritionID == nutritionId)
                    .FirstOrDefault();

            if (isNutritionExist == null)
                return NotFound(new
                {
                    status = "failed",
                    message = "Nutrition not found!"
                });

            if (!_nutritionRep.DeleteNutrition(isNutritionExist))
            {
                return BadRequest(new
                {
                    status = "failed",
                    message = "Something went wrong deleting nutrition!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Nutrition Successfully Deleted"
            });
        }

        [HttpGet]
        public IActionResult GetNutritions()
        {
            var allNutritions = _nutritionRep.GetNutritions();

            if (allNutritions.Count <= 0)
            {
                return NotFound(new
                {
                    status = "failed",
                    message = "Nutrition is empty!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "All Nutrition Successfully fetched",
                data = allNutritions
            });
        }

        [HttpGet("{nutritionId}")]
        public IActionResult GetNutrition(int nutritionId)
        {
            var nutrition = _nutritionRep.GetNutrition(nutritionId);

            if (nutrition == null)
            {
                return NotFound(new
                {
                    status = "failed",
                    message = "Nutrition not found!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Nutrition Successfully fetched",
                data = nutrition
            });
        }
    }
}
