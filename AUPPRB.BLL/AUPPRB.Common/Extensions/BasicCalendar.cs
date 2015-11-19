using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Common.Extensions
{
   public class BasicCalendar
   {
       public int YearId { get; set; }

       public string MonthName { get; set; }
       public int MonthId { get; set; }

       public bool AllowPrevious { get; set; }
       public bool AllowNext { get; set; }

       int currSpecialistId { get; set; }

       public List<BasicDay> Days { get; set; }

       public BasicCalendar(int month, int year, int currSpecialistId = 1)
       {
           DateTime tempStartDate = new DateTime(year, month, 1);

           int mustSkip = 0;

           int daysInMonth = DateTime.DaysInMonth(year, month);

           DateTime tempEndDate = new DateTime(year, month, daysInMonth);

           int mustAdd = 0;


           YearId = year;
           MonthId = month;
           MonthName = tempStartDate.ToString("MMMM");

           AllowNext = true;
           AllowPrevious = false;

           Days = new List<BasicDay>();

           switch (tempStartDate.DayOfWeek)
           {
               case DayOfWeek.Monday: mustSkip = 0; break;
               case DayOfWeek.Tuesday: mustSkip = 1; break;
               case DayOfWeek.Wednesday: mustSkip = 2; break;
               case DayOfWeek.Thursday: mustSkip = 3; break;
               case DayOfWeek.Friday: mustSkip = 4; break;
               case DayOfWeek.Saturday: mustSkip = 5; break;
               case DayOfWeek.Sunday: mustSkip = 6; break;
           }

           switch (tempEndDate.DayOfWeek)
           {
               case DayOfWeek.Monday: mustAdd = 6; break;
               case DayOfWeek.Tuesday: mustAdd = 5; break;
               case DayOfWeek.Wednesday: mustAdd = 4; break;
               case DayOfWeek.Thursday: mustAdd = 3; break;
               case DayOfWeek.Friday: mustAdd = 2; break;
               case DayOfWeek.Saturday: mustAdd = 1; break;
               case DayOfWeek.Sunday: mustAdd = 0; break;

           }

           //создаем пустые даты
           for (int i = 0; i < mustSkip; i++)
           {
               Days.Add(new BasicDay(tempStartDate, false, "", false));
           }
           //заполняем остальные даты месяца


           while (true)
           {
               //если заканчивается месяц то заканчиваем заполнение дат
               if (tempStartDate.Month != month) break;

               //окончание дня
               DateTime tempStartDateFor23Hours = tempStartDate.AddHours(23);

               
               Days.Add(new BasicDay(tempStartDate, true, "some data", true));
               
              
              

               tempStartDate = tempStartDate.AddDays(1);

           }

           //рисуем пустые даты после последней реальной даты месяца
           for (int i = 0; i < mustAdd; i++)
           {
               Days.Add(new BasicDay(tempEndDate, false, "", false));
           }

           int count = Days.Count;
       }

   }

   public class BasicDay
   {
       public bool IsDraw { get; set; }
       public bool IsEndOfWeek { get; set; }
       public bool IsToday { get; set; }

       public int NumberOfDay { get; set; }
       public string DateString { get; set; }

       public string Description { get; set; }

       public bool HasWorkTime { get; set; }//назначено ли раб время на этот день

       public BasicDay(DateTime date, bool isDraw, string descq, bool hasWorkTime)
       {
           IsDraw = isDraw;
           if (!isDraw) return;
           IsEndOfWeek = date.DayOfWeek == DayOfWeek.Sunday;
           IsToday = date.Date == DateTime.Now.Date;
           NumberOfDay = date.Day;
           DateString = date.ToString("dd-MM-yyyy");
           Description = descq;
           HasWorkTime = hasWorkTime;
       }

   }


}
