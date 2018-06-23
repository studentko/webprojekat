using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebProjekat.Models;
using WebProjekat.Models.DTO;
using WebProjekat.Models.UserModels;
using WebProjekat.Repositories.UnitOfWork;

namespace WebProjekat.Controllers
{
    public class UserController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();


        public UserController()
        {
        }

        public IEnumerable<User> Get()
        {
            if (UserPrincipal.IsCustomer || UserPrincipal.IsDriver)
            {
                List<User> users = new List<User>();
                users.Add(UserPrincipal.CurrentPrincipal.User);
                return users;
            } else if(UserPrincipal.IsDispatcher)
            {
                return uow.UserRepository.Get();
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        // GET: api/User/5
        public User Get(int id)
        {
            if (UserPrincipal.IsCustomer || UserPrincipal.IsDriver)
            {
                return UserPrincipal.CurrentPrincipal.User;
            }
            else if (UserPrincipal.IsDispatcher)
            {
                return uow.UserRepository.GetByID(id);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        // POST: api/User
        public RegisterReturnDTO Post([FromBody] DriverCreateDTO driverInfo)
        {
            if (!UserPrincipal.IsDispatcher)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            if (uow.UserRepository.GetByUsername(driverInfo.DriverInfo.Username) != null)
            {
                return new RegisterReturnDTO()
                {
                    IsSuccess = false,
                    Message = $"Registration failed: User with username '{driverInfo.DriverInfo.Username}' allready exist"
                };
            }

            Driver driver = new Driver()
            {
                Email = driverInfo.DriverInfo.Email,
                FirstName = driverInfo.DriverInfo.FirstName,
                Gender = driverInfo.DriverInfo.Gender,
                JMBG = driverInfo.DriverInfo.JMBG,
                LastName = driverInfo.DriverInfo.LastName,
                Password = driverInfo.DriverInfo.Password,
                PhoneNumber = driverInfo.DriverInfo.PhoneNumber,
                Role = Role.Driver,
                Username = driverInfo.DriverInfo.Username,
                Location = new Location()
            };

            driver = uow.UserRepository.Insert(driver) as Driver;

            Car car = new Car()
            {
                CarType = driverInfo.CarType,
                Driver = driver,
                DriverId = driver.Id,
                ModelYear = driverInfo.CarModelYear,
                RegistrationNumber = driverInfo.CarRegistrationNumber
            };

            car = uow.CarRepository.Insert(car);

            driver.Car = car;
            driver.CarId = car.Id;
            uow.UserRepository.Update(driver);

            return new RegisterReturnDTO()
            {
                IsSuccess = true,
                
            };
        }

        [Route("api/user/put/{id}/block")]
        public void Put(int id, [FromBody] bool block)
        {
            if (!UserPrincipal.IsDispatcher)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            User user = uow.UserRepository.GetByID(id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (user.Role == Role.Dispatcher)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            user.Blocked = block;
            uow.UserRepository.Update(user);
        }

        // za apdejt licnih podataka
        public void Put([FromBody]RegisterUserDTO personalData)
        {
            if (!UserPrincipal.IsLoged)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            User user = uow.UserRepository.GetByID(UserPrincipal.CurrentUser.Id);
            user.Email = personalData.Email;
            user.FirstName = personalData.FirstName;
            user.Gender = personalData.Gender;
            user.JMBG = personalData.JMBG;
            user.LastName = personalData.LastName;
            user.Password = personalData.Password;
            user.PhoneNumber = personalData.PhoneNumber;
            uow.UserRepository.Update(user);
            UserPrincipal.CurrentPrincipal.User = user;
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }

        
    }
}
