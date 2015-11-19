using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Repository.DB;

namespace AUPPRB.Domain.Services
{
   public  class DisciplineService:IDisciplineService
   {
       private IDataProvider _dataProvider;

       public DisciplineService(IDataProvider dataProvider)
       {
           _dataProvider = dataProvider;
       }

      

       /// <summary>
       /// Выбрать все дисциплины
       /// </summary>
       /// <returns></returns>
       public IQueryable<Discipline> GetAllDisciplines()
       {
           IQueryable<Discipline> allDisciplines = _dataProvider.Disciplines.GetAllNoTracking();
           return allDisciplines;
       }

       /// <summary>
       /// Взять текущую дисциплину
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public Discipline GetDisciplineById(int id)
       {
           return _dataProvider.Disciplines.GetById(id);
       }


       /// <summary>
       /// Save discipline id DB 
       /// </summary>
       /// <param name="discipline"></param>
       public RequestResult SaveDiscipline(Discipline discipline)
       {
         
           try
           {
               _dataProvider.Disciplines.Add(discipline);
               _dataProvider.Save();

               return RequestResult.Ok;
           }
           catch (Exception ex)
           {
               return new RequestResult(1, "Ошибка при сохранении "+ex.Message);
               
           }
                    
       }

       /// <summary>
       /// Update disciline entry id Db
       /// </summary>
       /// <param name="discipline"></param>
       /// <returns></returns>
       public RequestResult UpdateDiscipline(Discipline discipline)
       {
           try
           {
               _dataProvider.Disciplines.Update(discipline);
               _dataProvider.Save();

               return RequestResult.Ok;
           }
           catch (Exception ex)
           {
               return new RequestResult(1, "Ошибка при сохранении " + ex.Message);
            
           }

       }


   }
}
