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
                        status = "Failed",
                        message = "Requset not valid"
                    });

                var isNutritionExist = _nutritionRep.GetNutritions()
                    .Where(u => u.NutritionName.Trim().ToLower() == nutritionReq.NutritionName.Trim().ToLower())
                    .FirstOrDefault();

                if (isNutritionExist != null)
                {
                    return Ok(new
                    {
                        status = "Success",
                        message = "Nutrition already exists",
                        data = isNutritionExist
                    });
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
                    return StatusCode(500, new
                    {
                        status = "Success",
                        message = "Something went wrong while saving",
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "Nutrition Successfully created",
                    data = nutrition
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message
                });
                throw;
            }
        }

        [HttpPut("{nutritionId}")]
        public IActionResult UpdateNutrition(int nutritionId, [FromBody] ReqNutritionDto nutritionReq)
        {
            try
            {
                if (nutritionReq == null)
                    return BadRequest(new
                    {
                        status = "Failed",
                        message = "Requset not valid"
                    });

                var isNutritionExist = _nutritionRep.GetNutritions()
                        .Where(u => u.NutritionID == nutritionId)
                        .FirstOrDefault();

                if (isNutritionExist == null)
                    return Ok(new
                    {
                        status = "Success",
                        message = "Nutrition not found!",
                        data = isNutritionExist
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
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpDelete("{nutritionId}")]
        public IActionResult DeleteNutrition(int nutritionId)
        {
            try
            {
                var isNutritionExist = _nutritionRep.GetNutritions()
                    .Where(u => u.NutritionID == nutritionId)
                    .FirstOrDefault();

                if (isNutritionExist == null)
                    return Ok(new
                    {
                        status = "Success",
                        message = "Nutrition not found!"
                    });

                if (!_nutritionRep.DeleteNutrition(isNutritionExist))
                {
                    return StatusCode(500, new
                    {
                        status = "Failed",
                        message = "Something went wrong deleting nutrition!"
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "Nutrition Successfully Deleted"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpGet]
        public IActionResult GetNutritions()
        {
            try
            {
                var allNutritions = _nutritionRep.GetNutritions();

                if (allNutritions.Count <= 0)
                {
                    return NotFound(new
                    {
                        status = "Success",
                        message = "Nutrition is empty!",
                        data = allNutritions
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "All Nutrition Successfully fetched",
                    data = allNutritions
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpGet("{nutritionId}")]
        public IActionResult GetNutrition(int nutritionId)
        {
            try
            {
                var nutrition = _nutritionRep.GetNutrition(nutritionId);

                if (nutrition == null)
                {
                    return Ok(new
                    {
                        status = "Success",
                        message = "Nutrition not found!",
                        data = nutrition
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "Nutrition Successfully fetched",
                    data = nutrition
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }
    }
}
