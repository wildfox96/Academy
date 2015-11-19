using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Domain.Interfaces
{
    public interface ISyncronizationService
    {
        
        /// <summary>
        /// Тестовый метод для выбора времени занятий с Бд Белодеда
        /// </summary>
        /// <returns></returns>
        List<string> GetDataFrom_t0005_ВремяЗанятия(string connString);
        

    }
}
