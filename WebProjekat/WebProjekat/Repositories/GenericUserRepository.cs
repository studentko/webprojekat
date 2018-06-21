using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebProjekat.Models;
using WebProjekat.Repositories.UnitOfWork;

namespace WebProjekat.Repositories
{
    public class GenericUserRepository: GenericRepository<User>
    {
        public GenericUserRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public GenericUserRepository()
        {
            context = new AppDbContext();
        }

        public virtual IEnumerable<User> Get()
        {
            return context.Users;
        }

        public override User GetByID(int id)
        {
            return context.Users.SingleOrDefault(b => b.Id == id);
        }

        public User GetByUsername(string username)
        {
            return context.Users.SingleOrDefault(b => b.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool Contains(string username)
        {
            return context.Users.Any(u => u.Username == username);
        }
    }
}