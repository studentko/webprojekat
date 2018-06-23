using System;
using System.Collections.Generic;
using System.Device.Location;
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
    public class RideController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();

        public IEnumerable<Ride> Get([FromUri] GetRidesFilterDTO filter)
        {
            if (!UserPrincipal.IsLoged)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            IEnumerable<Ride> rides = null;



            if (UserPrincipal.IsDispatcher)
            {
                if (filter.OnlyAssigned)
                {
                    rides = (uow.UserRepository.GetByID(UserPrincipal.CurrentUser.Id) as Dispatcher).Rides;
                } else
                {
                    rides = uow.RideRepository.Get();
                }
            } else if (UserPrincipal.IsDriver)
            {
                if (filter.OnlyAssigned)
                {
                    rides = (uow.UserRepository.GetByID(UserPrincipal.CurrentUser.Id) as Driver).Rides;
                }
                else
                {
                    rides = uow.RideRepository.Get().Where(r => r.Status == RideStatus.Created);
                }
            } else if (UserPrincipal.IsCustomer)
            {
                rides = (uow.UserRepository.GetByID(UserPrincipal.CurrentUser.Id) as Customer).Rides;
            }

            if (filter.StatusFilter != null)
            {
                RideStatus statusFilter = (RideStatus)filter.StatusFilter;
                rides = rides.Where(r => r.Status == statusFilter);
            }

            DateTime fromDate = filter.FromOrderDate == null ? DateTime.MinValue : (DateTime)filter.FromOrderDate;
            DateTime toDate = filter.ToOrderDate == null ? DateTime.MaxValue : (DateTime)filter.ToOrderDate;

            int fromRate = filter.FromRate == null ? int.MinValue : (int)filter.FromRate;
            int toRate = filter.ToRate == null ? int.MaxValue : (int)filter.ToRate;

            int fromPrice = filter.FromPrice == null ? int.MinValue : (int)filter.FromPrice;
            int toPrice = filter.ToPrice == null ? int.MaxValue : (int)filter.ToPrice;

            rides = rides.Where(r => (r.Date >= fromDate && r.Date <= toDate) &&
                               ((r.Comment != null && r.Comment.Rating >= fromRate && r.Comment.Rating <= toRate) 
                                                || r.Comment == null) &&
                               (r.Amount >= fromPrice && r.Amount <= toPrice));

            GetDispatcherRidesFilterDTO drf = filter as GetDispatcherRidesFilterDTO;
            if (UserPrincipal.IsDispatcher && drf != null)
            {
                if (drf.CustomerName != null)
                {
                    rides = rides.Where(r => r.Customer != null && r.Customer.FirstName.Equals(drf.CustomerName));
                }
                if (drf.CustomerLastName != null)
                {
                    rides = rides.Where(r => r.Customer != null && r.Customer.LastName.Equals(drf.CustomerLastName));
                }

                if (drf.DriverName != null)
                {
                    rides = rides.Where(r => r.Driver != null && r.Driver.FirstName.Equals(drf.DriverName));
                }
                if (drf.DriverLastName != null)
                {
                    rides = rides.Where(r => r.Driver != null && r.Driver.LastName.Equals(drf.DriverLastName));
                }
            }

            if (filter.SortFilter == RideFilterSort.ByDate)
            {
                rides = rides.OrderByDescending(r => r.Date);
            } else if (filter.SortFilter == RideFilterSort.ByRate)
            {
                rides = rides.OrderByDescending(r => r.Comment == null ? int.MaxValue : r.Comment.Rating);
            } else if (filter.SortFilter == RideFilterSort.ByDistance && UserPrincipal.IsDriver)
            {
                Driver driver = uow.UserRepository.GetByID(UserPrincipal.CurrentUser.Id) as Driver;
                if (driver.Location != null)
                {
                    GeoCoordinate cord = new GeoCoordinate(driver.Location.X, driver.Location.Y);
                    rides = rides.OrderBy(r => new GeoCoordinate(r.StartLocation.X, r.StartLocation.Y).GetDistanceTo(cord));
                }
            }

            return rides;
        }

        public RideReturnDTO Post([FromBody] CreateRideDTO rideInfo)
        {
            if (!UserPrincipal.IsCustomer && !UserPrincipal.IsDispatcher)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            Ride ride = new Ride()
            {
                CarType = rideInfo.CarType,
                StartLocation = rideInfo.Location,
                Destination = new Location(),
                Date = DateTime.Now,
                Amount = 0
            };

            
            

            if (UserPrincipal.IsDispatcher)
            {
                Driver driver = uow.UserRepository.GetByID(rideInfo.DriverId) as Driver;
                if (driver == null)
                {
                    return new RideReturnDTO()
                    {
                        IsSuccess = false,
                        Message = $"No driver with {rideInfo.DriverId}"
                    };
                }
                ride.DriverId = driver.Id;
                ride.Driver = driver;

                ride.DispatcherId = UserPrincipal.CurrentUser.Id;

                ride.Status = RideStatus.Formed;
            }
            else if (UserPrincipal.IsCustomer)
            {
                ride.Status = RideStatus.Created;
                ride.CustomerId = UserPrincipal.CurrentUser.Id;
                ride.Customer = uow.UserRepository.GetByID(UserPrincipal.CurrentUser.Id) as Customer;

            }

            ride = uow.RideRepository.Insert(ride);

            return new RideReturnDTO()
            {
                IsSuccess = true,
                Ride = ride
            };

        }

        public RideChangeReturnDTO Put(int id, [FromBody]RideChangeDTO rideChange)
        {
            if (!UserPrincipal.IsLoged)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            Ride ride = uow.RideRepository.GetByID(id);

            if (ride == null)
            {
                return new RideChangeReturnDTO()
                {
                    IsSuccess = false,
                    Message = $"Couldn't find ride with id {id}"
                };
            }

            if (UserPrincipal.IsCustomer)
            {
                CustomerRideChangeDTO crc = rideChange as CustomerRideChangeDTO;
                
                if (ride.CustomerId != UserPrincipal.CurrentUser.Id)
                {
                    return new RideChangeReturnDTO()
                    {
                        IsSuccess = false,
                        Message = $"Ride doesn't belong to user"
                    };
                }

                if (ride.Status != RideStatus.Created)
                {
                    return new RideChangeReturnDTO()
                    {
                        IsSuccess = false,
                        Message = $"Ride can be changed only in state 'Created'"
                    };
                }

                if (crc.Location != null)
                {
                    ride.StartLocation = crc.Location;
                }

                if (crc.CarType != null)
                {
                    ride.CarType = (CarType) crc.CarType;
                }
            }
            else if (UserPrincipal.IsDriver)
            {
                DriverRideChangeDTO drc = rideChange as DriverRideChangeDTO;
                if (ride.Status == RideStatus.Created && drc.NewStatus == RideStatus.Accepted)
                {
                    ride.Status = RideStatus.Accepted;
                    ride.DriverId = UserPrincipal.CurrentUser.Id;

                } else if((ride.Status == RideStatus.Formed || ride.Status == RideStatus.Accepted 
                        || ride.Status == RideStatus.Processed) && drc.NewStatus == RideStatus.Successful)
                {
                    if (ride.DriverId != UserPrincipal.CurrentUser.Id)
                    {
                        return new RideChangeReturnDTO()
                        {
                            IsSuccess = false,
                            Message = $"Ride doesn't belong to driver"
                        };
                    }
                    if (drc.DestinationLocation == null || drc.Amount == 0)
                    {
                        return new RideChangeReturnDTO()
                        {
                            IsSuccess = false,
                            Message = $"Destination and amount must be provided"
                        };
                    }
                    ride.Status = RideStatus.Successful;
                    ride.Destination = drc.DestinationLocation;
                    ride.Amount = drc.Amount;
                } else
                {
                    return new RideChangeReturnDTO()
                    {
                        IsSuccess = false,
                        Message = $"Operation not permited to driver"
                    };
                }
            }
            else if(UserPrincipal.IsDispatcher)
            {
                DispatcherRideChangeDTO drc = rideChange as DispatcherRideChangeDTO;
                Driver driver = uow.UserRepository.GetByID(drc.AssignDriverId) as Driver;
                if (driver == null)
                {
                    return new RideChangeReturnDTO()
                    {
                        IsSuccess = false,
                        Message = $"Couldn't find driver with id: {drc.AssignDriverId}"
                    };
                }

                if (ride.Status != RideStatus.Created)
                {
                    return new RideChangeReturnDTO()
                    {
                        IsSuccess = false,
                        Message = $"Ride is already procesed"
                    };
                }

                ride.Driver = driver;
                ride.DispatcherId = UserPrincipal.CurrentUser.Id;
                ride.Status = RideStatus.Processed;
            }

            uow.RideRepository.Update(ride);

            return new RideChangeReturnDTO()
            {
                IsSuccess = true,
                Message = $"Ride is updated"
            };
        }
        
        
        [Route("api/ride/{id}/comment")]
        public RideChangeReturnDTO Put(int id, [FromBody] Comment comment)
        {
            if (!UserPrincipal.IsCustomer)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            Ride ride = uow.RideRepository.GetByID(id);
            if (ride == null)
            {
                return new RideChangeReturnDTO()
                {
                    IsSuccess = false,
                    Message = $"Ride with doesn't exist with id: {id}"
                };
            }

            if (ride.CustomerId != UserPrincipal.CurrentUser.Id)
            {
                return new RideChangeReturnDTO()
                {
                    IsSuccess = false,
                    Message = $"Ride with id: {id} doesn't belong to the user"
                };
            }

            if (ride.Status != RideStatus.Successful)
            {
                return new RideChangeReturnDTO()
                {
                    IsSuccess = false,
                    Message = $"Ride with id: {id} isn't yet completed"
                };
            }

            if (ride.Comment != null)
            {
                return new RideChangeReturnDTO()
                {
                    IsSuccess = false,
                    Message = $"Ride is already commented"
                };
            }

            Comment pushComment = new Comment()
            {
                CustomerId = UserPrincipal.CurrentUser.Id,
                Date = DateTime.Now,
                Description = comment.Description,
                Rating = comment.Rating,
                RideId = ride.Id
            };

            uow.CommentRepository.Insert(pushComment);

            return new RideChangeReturnDTO()
            {
                IsSuccess = true
            };
        }
        
        //za otkazivanje ili vozac prosledjuje neuspesnu voznju
        [HttpDelete]
        public RideDeleteReturnDTO Delete(int id, [FromBody] String reason)
        {
            if (!(UserPrincipal.IsCustomer || UserPrincipal.IsDriver))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            if (reason == null)
            {
                return new RideDeleteReturnDTO()
                {
                    IsSuccess = false,
                    Message = "Reason must be provided"
                };
            }



            Ride ride = uow.RideRepository.GetByID(id);
            if (ride == null)
            {
                return new RideDeleteReturnDTO()
                {
                    IsSuccess = false,
                    Message = $"No ride with id: {id}"
                };
            }

            Comment comment = new Comment() {
                Date = DateTime.Now,
                Rating = 0,
                Description = reason,
            };

            if (UserPrincipal.IsCustomer)
            {
                
                if (ride.Status == RideStatus.Created)
                {
                    ride.Status = RideStatus.Canceled;
                    comment.CustomerId = UserPrincipal.CurrentUser.Id;
                    comment.RideId = ride.Id;
                }
                else
                {
                    return new RideDeleteReturnDTO()
                    {
                        IsSuccess = false,
                        Message = "Ride can be only canceled if status is 'Created'"
                    };
                }
            }
            else if (UserPrincipal.IsDriver)
            {
                if (ride.DriverId == UserPrincipal.CurrentUser.Id)
                {
                    ride.Status = RideStatus.Unsuccessful;
                    comment.CustomerId = (int)ride.CustomerId;
                    comment.RideId = ride.Id;
                }
                else
                {
                    return new RideDeleteReturnDTO()
                    {
                        IsSuccess = false,
                        Message = "Ride don't belong to driver"
                    };
                }
            }

            comment = uow.CommentRepository.Insert(comment);
            ride.Comment = comment;
            uow.RideRepository.Update(ride);
            return new RideDeleteReturnDTO()
            {
                IsSuccess = true,
            };
        }
    }
}
