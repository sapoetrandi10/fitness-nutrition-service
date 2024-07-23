using fitness_db.Data;
using fitness_db.Interfaces;
using fitness_db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_db.Repositories
{
    public class NutritionRepository : INutritionRepository
    {
        private FitnessContext _fitnessCtx;
        public NutritionRepository(FitnessContext context)
        {
            _fitnessCtx = context;
        }

        public bool CreateNutrition(Nutrition Nutrition)
        {
            _fitnessCtx.nutritions.Add(Nutrition);
            var saved = _fitnessCtx.SaveChanges();

            return saved > 0 ? true : false;
        }

        public Nutrition UpdateNutrition(Nutrition Nutrition)
        {
            var updated = _fitnessCtx.SaveChanges();

            if (updated < 0)
            {
                return Nutrition = null;
            }

            return Nutrition;
        }


        public bool DeleteNutrition(Nutrition Nutrition)
        {
            _fitnessCtx.Remove(Nutrition);
            var saved = _fitnessCtx.SaveChanges();

            return saved > 0 ? true : false;
        }

        public ICollection<Nutrition> GetNutritions()
        {
            return _fitnessCtx.nutritions.ToList();
        }

        public Nutrition GetNutrition(int nutritionId)
        {
            var nutrition = _fitnessCtx.nutritions.Find(nutritionId);
            if (nutrition == null)
            {
                return null;
            }
            return nutrition;
        }
    }
}
