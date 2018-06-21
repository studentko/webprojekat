using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebProjekat.Models;
using WebProjekat.Repositories.UnitOfWork;

namespace WebProjekat.Repositories
{
    public class GenericCarRepository : GenericRepository<Car>
    {
        public GenericCarRepository(AppDbContext context) : base(context)
        {
        }
    }
}