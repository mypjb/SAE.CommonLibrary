using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDBConnectionFactory
    {
        Task<IDbConnection> GetAsync(string name);
    }
}
