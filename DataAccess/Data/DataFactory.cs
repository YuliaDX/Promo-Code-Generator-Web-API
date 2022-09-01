using Core;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public static class DataFactory
    {
        public static IEnumerable<Partner> Partners => new List<Partner>()
        {
            new Partner()
            {
                Id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319"),
                Name = "Toys Cash and Carry",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("ef7f299f-d8d5-459f-896e-eb9f14e1a32f"),
                        CreateDate = new DateTime(2022, 2, 25),
                        EndDate = new DateTime(2022,5,25),
                        Limit = 100
                    }
                }
            },
            new Partner()
            {
                Id = Guid.Parse("0da65561-cf56-4942-bff2-22f50cf70d43"),
                Name = "Sportmaster",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("0691bb24-5fd9-4a52-a11c-34bb8bc9364e"),
                        CreateDate = new DateTime(2022, 4, 25),
                        EndDate = new DateTime(2022,7,25),
                        Limit = 10
                    }
                }
            }
        };

        public static IEnumerable<Preference> Preferences => new List<Preference>() { 
            new Preference()
            {
                  Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                  Name = "Sport"
            },
             new Preference()
            {
                  Id = Guid.Parse("ef7f299f-92d7-459f-896e-eb9f14e1a32f"),
                  Name = "Cinema"
            },
              new Preference()
            {
                  Id = Guid.Parse("ef7f299f-d8d5-459f-896e-eb9f14e1a32f"),
                  Name = "Cars"
            }
        };

        public static IEnumerable<Customer> Customers => new List<Customer>()
        {
            new Customer()
            {
                Id = Guid.Parse("151533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                FirstName = "Alex",
                LastName = "Green",
                Email = "ag@somemail.com"
            },
             new Customer()
            {
                Id = Guid.Parse("251533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                FirstName = "John",
                LastName = "Smith",
                Email = "js@somemail.com"
            },
        };
        public static IEnumerable<Employee> Employees => new List<Employee>()
        {
            new Employee()
            {
                Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.com",
                FirstName = "Иван",
                LastName = "Сергеев",
                Roles = new List<Role>()
                {
                    Roles.FirstOrDefault(x => x.Name == "Admin")
                },
                AppliedPromocodesCount = 5
            },
            new Employee()
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.com",
                FirstName = "Петр",
                LastName = "Андреев",
                Roles = new List<Role>()
                {
                    Roles.FirstOrDefault(x => x.Name == "PartnerManager")
                },
                AppliedPromocodesCount = 10
            },
        };

        public static IEnumerable<Role> Roles => new List<Role>()
        {
            new Role()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role()
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        };

    }
}
