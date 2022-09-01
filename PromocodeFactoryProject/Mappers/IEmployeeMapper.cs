﻿
using Core;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Mappers
{
	public interface IEmployeeMapper
    {
        Task<Employee> MapFromModelAsync(CreateOrEditEmployeeRequest model,
           Employee employee = null);

    }
}
