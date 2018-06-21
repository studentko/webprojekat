using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebProjekat.Models;

namespace WebProjekat.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext context = new AppDbContext();
        private GenericUserRepository userRepository;
        private GenericCarRepository carRepository;
        private GenericRideRepository rideRepository;
        private GenericCommentRepository commentRepository;

        public GenericUserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericUserRepository(context);
                }
                return userRepository;
            }
        }

        public GenericCarRepository CarRepository
        {
            get
            {
                if(this.carRepository == null)
                {
                    this.carRepository = new GenericCarRepository(context);
                }
                return carRepository;
            }
        }

        public GenericRideRepository RideRepository
        {
            get
            {
                if (this.rideRepository == null)
                {
                    this.rideRepository = new GenericRideRepository(context);
                }
                return rideRepository;
            }
        }

        public GenericCommentRepository CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new GenericCommentRepository(context);
                }
                return commentRepository;
            }
        }


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}