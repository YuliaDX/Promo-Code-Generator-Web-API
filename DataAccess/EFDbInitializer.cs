using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class EFDbInitializer : IDbInitializer
    {
        readonly DataContext _dataContext;
        public EFDbInitializer(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public void Initialize()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
            _dataContext.AddRange(DataFactory.Employees);
            _dataContext.SaveChanges();

            _dataContext.AddRange(DataFactory.Preferences);
            _dataContext.SaveChanges();

            _dataContext.AddRange(DataFactory.Customers);
            _dataContext.SaveChanges();

            _dataContext.AddRange(DataFactory.Partners);
            _dataContext.SaveChanges();


        }
    }
}
