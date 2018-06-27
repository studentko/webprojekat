using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebProjekat.Models;
using WebProjekat.Models.UserModels;
using WebProjekat.Repositories.UnitOfWork;

namespace WebProjekat.Controllers
{
    public class DriverController : ApiController
    {

        UnitOfWork uow = new UnitOfWork();

        // vraca 5 najblizih slobodnih vozaca u odnosu na lokaciju
        public IEnumerable<Driver> Post([FromBody] Location location)
        {
            if (!UserPrincipal.IsDispatcher)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            List<Tuple<double, Driver>> choosen = new List<Tuple<double, Driver>>();

            GeoCoordinate c1 = new GeoCoordinate(location.X, location.Y);
            
            foreach (var usr in uow.UserRepository.Get())
            {
                if (usr.Role == Role.Driver)
                {
                    Driver drv = usr as Driver;
                    if (drv.Location != null && drv.IsFree())
                    {
                        GeoCoordinate c2 = new GeoCoordinate(drv.Location.X, drv.Location.Y);
                        choosen.Add(new Tuple<double, Driver>(c2.GetDistanceTo(c1), drv));
                    }
                }
            }

            choosen.Sort((u, v) => u.Item1.CompareTo(v.Item1));

            List<Driver> firstFive = new List<Driver>();

            for (int i = 0; i < 5 && i < choosen.Count; ++i)
            {
                firstFive.Add(choosen[i].Item2);
            }

            return firstFive;
        }

        // postavlja lokaciju za ulogovanog drivera
        public void Put([FromBody] Location location)
        {
            if (!UserPrincipal.IsDriver)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            Driver driver = uow.UserRepository.GetByID(UserPrincipal.CurrentUser.Id) as Driver;
            driver.Location = location;
            uow.UserRepository.Update(driver);
        }

        [Route("api/driver/getAllFree")]
        public IEnumerable<User> GetAllFree([FromUri] Location location)
        {
            return uow.UserRepository.Get(x => x.Role == Role.Driver);
        }
    }
}
