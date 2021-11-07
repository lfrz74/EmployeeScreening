﻿using EmployeeScreening.DBContext;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace EmployeeScreening.Tests
{
    public class DbContextFactory : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<ApplicationDBContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseSqlite(_connection).Options;
        }

        public ApplicationDBContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();
                using (var context = new ApplicationDBContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new ApplicationDBContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
