﻿using Domain.Entities.Concrete;
using Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        int GetNumOfCompanies();
    }
}
