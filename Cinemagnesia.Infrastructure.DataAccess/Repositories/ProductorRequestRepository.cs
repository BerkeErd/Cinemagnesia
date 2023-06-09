﻿using Cinemagnesia.Infrastructure.DataAccess.DbContext;
using Domain.Entities.Concrete;
using Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Repositories
{
    public class ProductorRequestRepository : BaseRepository<ProductorRequest>, IProductorRequestRepository
    {
        public ProductorRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
