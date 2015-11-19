using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;

namespace AUPPRB.Domain.Interfaces
{
    public interface IDisciplineService
    {
        IQueryable<Discipline> GetAllDisciplines();

        Discipline GetDisciplineById(int id);

        RequestResult SaveDiscipline(Discipline discipline);

        RequestResult UpdateDiscipline(Discipline discipline);

    }
}
