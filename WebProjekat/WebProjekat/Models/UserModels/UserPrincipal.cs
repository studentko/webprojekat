using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using WebProjekat.Repositories;
using WebProjekat.Repositories.UnitOfWork;

namespace WebProjekat.Models.UserModels
{
    public class UserPrincipal : IPrincipal, IIdentity
    {
        public User User { get; set; }
        bool authenticated = false;

        //UnitOfWork uow = new UnitOfWork();

        public bool Blocked
        {
            get
            {
                GenericUserRepository ur = new GenericUserRepository();
                return ur.GetByID(User.Id).Blocked;
            }
        }

        public IIdentity Identity
        {
            get
            {
                return this;
            }
        }

        public string Name
        {
            get
            {
                return User.Username;
            }
        }

        public string AuthenticationType
        {
            get
            {
                return "password";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return authenticated && !Blocked;
            }
        }

        public bool IsInRole(string role)
        {
            return User.Role.ToString().Equals(role);
        }

        public UserPrincipal(User u, bool authenticated)
        {
            User = u;
            this.authenticated = authenticated;
        }

        public void Logout()
        {
            authenticated = false;
        }

        public static UserPrincipal CurrentPrincipal
        {
            get
            {
                //return Thread.CurrentPrincipal as UserPrincipal;
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Session["user"] as UserPrincipal;
                }
                return null;
            }
        }

        public static bool IsLoged
        {
            get
            {
                if (CurrentPrincipal != null)
                {
                    return CurrentPrincipal.IsAuthenticated;
                }
                return false;
            }
        }

        public static bool IsCustomer
        {
            get
            {
                return IsLoged && CurrentPrincipal.User.Role == Role.Customer;
            }
        }

        public static bool IsDriver
        {
            get
            {
                return IsLoged && CurrentPrincipal.User.Role == Role.Driver;
            }
        }

        public static bool IsDispatcher
        {
            get
            {
                return IsLoged && CurrentPrincipal.User.Role == Role.Dispatcher;
            }
        }

        public static User CurrentUser
        {
            get
            {
                return CurrentPrincipal == null ? null : CurrentPrincipal.User;
            }
        }

        
    }
}