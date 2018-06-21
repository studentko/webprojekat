using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using WebProjekat.Models;
using WebProjekat.Models.DTO;
using WebProjekat.Models.UserModels;
using WebProjekat.Repositories.UnitOfWork;

namespace WebProjekat.Controllers
{

    

    public class AuthenticationController : ApiController
    {

        UnitOfWork uow = new UnitOfWork();

        [Route("api/authentication/login")]
        [HttpPost]
        public LoginReturnDTO Login(LoginInfoDTO loginInfo)
        {
            LoginReturnDTO lrd = new LoginReturnDTO();

            User user = uow.UserRepository.GetByUsername(loginInfo.Username);

            if (user != null && user.Password.Equals(loginInfo.Password))
            {
                lrd.IsSuccess = true;
                lrd.User = user;
                lrd.Message = "Success";


                UserPrincipal principal = new UserPrincipal(user, true);
                SetPrincipal(principal);
            }
            else
            {
                lrd.IsSuccess = false;
                lrd.Message = "Username and/or password are incorect";
            }

            return lrd;
        }

        [Route("api/authentication/register")]
        [HttpPost]
        public RegisterReturnDTO Register(RegisterUserDTO regUser)
        {
            try
            {
                Customer c = new Customer()
                {
                    Email = regUser.Email,
                    FirstName = regUser.FirstName,
                    Gender = regUser.Gender,
                    JMBG = regUser.JMBG,
                    LastName = regUser.LastName,
                    Password = regUser.Password,
                    PhoneNumber = regUser.PhoneNumber,
                    Role = Role.Customer,
                    Username = regUser.Username
                };

                if (uow.UserRepository.GetByUsername(regUser.Username) != null)
                {
                    return new RegisterReturnDTO() { IsSuccess = false,
                        Message = $"Registration failed: User with username '{regUser.Username}' allready exist" };
                }

                uow.UserRepository.Insert(c);

                return new RegisterReturnDTO() { IsSuccess = true };
            
            } catch (Exception e)
            {
                Trace.TraceError($"Registtration failed: {e.Message}\n{e.StackTrace}");
                return new RegisterReturnDTO() { IsSuccess = false, Message = e.Message };
            }
        }

        [Route("api/authentication/logout")]
        [HttpGet]
        [HttpPost]
        public bool Logout()
        {
            if (UserPrincipal.CurrentPrincipal != null)
            {
                UserPrincipal.CurrentPrincipal.Logout();
            }
            SetPrincipal(null);
            return true;
        }

        [Route("api/authentication/isLoged")]
        [HttpGet]
        public bool IsLoged()
        {
            UserPrincipal pric = UserPrincipal.CurrentPrincipal;
            return pric != null && pric.IsAuthenticated;
        }

        [NonAction]
        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            HttpContext.Current.Session["user"] = principal;
        }
    }
}
