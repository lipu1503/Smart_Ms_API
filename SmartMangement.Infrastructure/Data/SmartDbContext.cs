using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartManagement.Infrastructure.Extensions;
using SmartMangement.Domain.Configuration;
using SmartMangement.Domain.Models;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SmartManagement.Infrastructure.Data
{
    public  class SmartDbContext: DbContext
    {
        private readonly DbContextOptions<SmartDbContext> _options;
        public virtual DbSet<UserEnity> Users { get; set; }
        public SmartDbContext(DbContextOptions<SmartDbContext> options): base(options) 
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.HasDbFunction(typeof(JsonToSqlFunction).GetMethod("jsonValue")).HasName("JSON_VALUE").IsBuiltIn()
                .HasSchema(null);
            //if(Database.IsSqlServer)
            //{
            //    return;

            //}
            foreach(IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                List<IMutableProperty> list = entityType.GetProperties().Where(delegate (IMutableProperty p)
                   {
                       string name = p.Name;
                       return (name == "ValidFrom" || name == "ValidTo") ? true : false;
                   }).ToList();
                foreach (IMutableProperty item in list)
                {
                    item.SetDefaultValue(DateTime.Now);
                }
            }
            
        }
        public DbContextOptions<SmartDbContext> GetDbContextOptions()
        {
            return _options;
        }
        public EntityEntry<T> Update<T>(T entity, List<string> properties) where T : class
        {
            Entry(entity).State = EntityState.Unchanged;
            foreach (var property in properties)
            {
                Entry(entity).Property(property).IsModified = true;
            }
            return base.Update(entity);
        }

        public sealed override async Task<int> SaveChangesAsync(CancellationToken token)
        {
            int entries = await base.SaveChangesAsync(token);
            ChangeTracker.Clear();
            return entries;
        }
        public sealed override int SaveChanges()
        {
            int result = base.SaveChanges();
            ChangeTracker.Clear();
            return result;
        }
        public DataTable ExecuteNonQuery(string query)
        {
            using DbCommand dbCommand = Database.GetDbConnection().CreateCommand();
            dbCommand.CommandText = query;
            Database.OpenConnection();
            using DbDataReader reader = dbCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            Database.CloseConnection();
            return dataTable;
        }

        public DataSet ExecuteStoredProcedure(string sprocName, List<SqlParameter> sqlParameters)
        {
            using DbCommand dbCommand = Database.GetDbConnection().CreateCommand();
            dbCommand.CommandText = sprocName;
            dbCommand.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter sqlParameter in sqlParameters)
            {
                DbParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = sqlParameter.ParameterName;
                dbParameter.Value = sqlParameter.Value;
                dbCommand.Parameters.Add(dbParameter);
            }
            DataSet dataset = new DataSet();
            Database.OpenConnection();
            try
            {
                using DbDataReader dbDataReader = dbCommand.ExecuteReader();
                do
                {
                    DataTable dataTable = new();
                    dataTable.Load(dbDataReader);
                    dataset.Tables.Add(dataTable);
                }
                while (!dbDataReader.IsClosed);
            }
            finally
            {
                Database.CloseConnection();
            }
            
            return dataset;
        }
        public async Task<DataSet> ExecuteStoredProcedureAsync(string sprocName, List<SqlParameter> sqlParameters, CancellationToken token)
        {
            using DbCommand dbCommand = Database.GetDbConnection().CreateCommand();
            dbCommand.CommandText = sprocName;
            dbCommand.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter sqlParameter in sqlParameters)
            {
                DbParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = sqlParameter.ParameterName;
                dbParameter.Value = sqlParameter.Value;
                dbCommand.Parameters.Add(dbParameter);
            }
            DataSet dataset = new DataSet();
            await Database.OpenConnectionAsync(token);
            try
            {
                using DbDataReader dbDataReader = await dbCommand.ExecuteReaderAsync(token);
                do
                {
                    DataTable dataTable = new();
                    dataTable.Load(dbDataReader);
                    dataset.Tables.Add(dataTable);
                }
                while (!dbDataReader.IsClosed);
            }
            finally
            {
                await Database.CloseConnectionAsync();
            }

            return dataset;
        }
        public DataSet ExecuteStoredProcedureDataSet(string sprocName, List<SqlParameter> sqlParameters)
        {
            return ExecuteStoredProcedure(sprocName, sqlParameters);
        }
        public async Task<DataSet> ExecuteStoredProcedureDataSetAsync(string sprocName, List<SqlParameter> sqlParameters, CancellationToken token)
        {
            return await ExecuteStoredProcedureAsync(sprocName, sqlParameters, token);
        }
        public DataTable ExecuteStoredProcedureDataTable(string sprocName, List<SqlParameter> sqlParameters)
        {
            DataSet dataset = ExecuteStoredProcedure(sprocName, sqlParameters);
            if (dataset.Tables.Count > 0)
            {
                return dataset.Tables[0];
            }
            return new DataTable();
        }
        public string ExecuteStoredProcedureString(string sprocName, List<SqlParameter> sqlParameters)
        {
            StringBuilder stringBuilder = new StringBuilder();
            DataSet dataset = ExecuteStoredProcedure(sprocName, sqlParameters);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataTable table in dataset.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        stringBuilder.Append(Convert.ToString(row[0]));
                    }
                }
            }
            return stringBuilder.ToString();
        }
        public async Task<string> ExecuteStoredProcedureStringAsync(string sprocName, List<SqlParameter> sqlParameters, CancellationToken token)
        {
            StringBuilder stringBuilder = new StringBuilder();
            DataSet dataset = await ExecuteStoredProcedureAsync(sprocName, sqlParameters, token);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataTable table in dataset.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        stringBuilder.Append(Convert.ToString(row[0]));
                    }
                }
            }
            return stringBuilder.ToString();
        }
        public async Task<List<string>> ExecuteStoredProcedureListStringAsync(string sprocName, List<SqlParameter> sqlParameters, CancellationToken token)
        {
            List<string> list = new List<string>();
            DataSet dataset = await ExecuteStoredProcedureAsync(sprocName, sqlParameters, token);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataTable table in dataset.Tables)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    if (table is not null && table.Rows.Count > 0)
                    {

                        foreach (DataRow row in table.Rows)
                        {
                            stringBuilder.Append(Convert.ToString(row[0]));
                        }
                    }
                    list.Add(stringBuilder.ToString());
                }
            }
            return list;
        }
        public List<string> ExecuteStoredProcedureListString(string sprocName, List<SqlParameter> sqlParameters)
        {
            List<string> list = new List<string>();
            DataSet dataset =  ExecuteStoredProcedure(sprocName, sqlParameters);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataTable table in dataset.Tables)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    if (table is not null && table.Rows.Count > 0)
                    {

                        foreach (DataRow row in table.Rows)
                        {
                            stringBuilder.Append(Convert.ToString(row[0]));
                        }
                    }
                    list.Add(stringBuilder.ToString());
                }
            }
            return list;
        }
    }
}
