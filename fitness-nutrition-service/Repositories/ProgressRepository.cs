using fitness_db.Data;
using fitness_db.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_db.Repositories
{
    public class ProgressRepository : IProgressRepository
    {
        private FitnessContext _fitnessCtx;
        public ProgressRepository(FitnessContext context)
        {
            _fitnessCtx = context;
        }
    }
}
