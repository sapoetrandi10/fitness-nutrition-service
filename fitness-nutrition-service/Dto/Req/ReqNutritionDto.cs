namespace fitness_Nutrition_service.Dto.Req
{
    public class ReqNutritionDto
    {
        public int NutritionID { get; set; }
        public string NutritionName { get; set; }
        public float Calories { get; set; } = 0;
        public float Protein { get; set; } = 0;
        public float Carbs { get; set; } = 0;
        public float Fat { get; set; } = 0;
    }
}
