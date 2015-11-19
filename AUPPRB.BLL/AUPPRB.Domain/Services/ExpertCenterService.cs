using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.ExpertCenterModels;
using AUPPRB.Repository.DB;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace AUPPRB.Domain.Services
{
    class ExpertCenterService : BaseServices, IExpertCenterService
    {
        public OutputModel PrepodModel(int page)
        {
            return new OutputModel()
               {
                   List = DataProvider.PrepodMetadata.Filter(p => p.Rate != null)
                   .OrderByDescending(p => p.Rate)
                   .Select(p => p.User.UserMeta.FirstOrDefault().LastName)
                   .Skip((page - 1) * 4)
                   .Take(4)
                   .ToList(),
                   PagingInfo = new PagingInfo()
                   {
                       CurrentPage = page,
                       ItemsForPage = 4,
                       TotalItems = DataProvider.PrepodMetadata.Filter(p => p.Rate != null).OrderByDescending(p => p.Rate).Select(p => p.User.UserMeta.FirstOrDefault().LastName).ToList().Count
                   },
                   PrepDispList = DataProvider.PrepodDiscipline.GetAll().OrderByDescending(p => p.Discipline.Rate).Select(p => new ParseDisciplineList
                   {
                       Name = p.Discipline.Name,
                       LastName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().LastName,
                       FirstName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().FirstName,
                       MiddleName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().MiddleName,
                       Photo = p.Prepod_PrepodMeta.Photo,
                       Likes = p.Prepod_PrepodMeta.Prepod_PrepodLikes.Count,
                       IdSotr = p.Prepod_PrepodMeta.Id
                   }).ToList()
               };
        }

        public OutputModel DispModel(int page)
        {
            return new OutputModel()
            {
                List = DataProvider.Disciplines.Filter(p => p.Rate != null).OrderByDescending(p => p.Rate)
                 .Select(p => p.Name)
                 .Skip((page - 1) * 4)
                 .Take(4)
                 .ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsForPage = 4,
                    TotalItems = DataProvider.Disciplines.Filter(p => p.Rate != null).OrderByDescending(p => p.Rate).Select(p => p.Name).ToList().Count
                },

                PrepCabinetList = DataProvider.PrepodDiscipline.GetAll().OrderByDescending(p => p.Prepod_PrepodMeta.Rate)
           .Select(p => new ParsePrepodList
           {
               LastName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().LastName,
               Cabinet = p.Prepod_PrepodMeta.Classroom,
               Discipline = p.Discipline.Name,
               Photo = p.Prepod_PrepodMeta.Photo,
               FirstName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().FirstName,
               MiddleName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().MiddleName,
               Likes = p.Prepod_PrepodMeta.Prepod_PrepodLikes.Count,
               IdSotr = p.Prepod_PrepodMeta.Id,
           }).ToList(),
            };
        }
        public OutputModel StartPageModel()
        {
            return new OutputModel()
            {
                PrepodList = DataProvider.PrepodDiscipline.GetAll().Select(p => p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().LastName.Trim()).Distinct().ToArray(),
                DispList = DataProvider.PrepodDiscipline.GetAll().Select(p => p.Discipline.Name.Trim()).Distinct().ToArray()
            };
        }

        public OutputModel SeacrhModel(string find)
        {
            OutputModel viewList = null;
            string Check, Name;
            if (!String.IsNullOrWhiteSpace(find))
            {
                if (DataProvider.PrepodDiscipline.GetAll().Any(p => p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().LastName == find)
                    || DataProvider.PrepodDiscipline.GetAll().Any(p => p.Discipline.Name == find))
                {
                    if (DataProvider.PrepodDiscipline.GetAll().Any(p => p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().LastName == find))
                    {
                        Check = "Prepod";
                        Name = find;
                        var prepod = DataProvider.PrepodDiscipline.FirstOrDefault(p => p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().LastName == find);
                        if (prepod != null)
                        {
                            var DisciplineNames = DataProvider.PrepodDiscipline
                                .Filter(p => p.IdSotr == prepod.IdSotr)
                                .OrderByDescending(p => p.Discipline.Rate)
                                .Select(p => p.Discipline.Name).ToList();

                            viewList = new OutputModel()
                            {
                                List = DisciplineNames,
                                Check = Check,
                                Name = Name
                            };
                        }
                    }
                    if (DataProvider.PrepodDiscipline.GetAll().Any(p => p.Discipline.Name == find))
                    {
                        Check = "Discipline";
                        Name = find;

                        var discipline = DataProvider.PrepodDiscipline.FirstOrDefault(p => p.Discipline.Name == find);

                        if (discipline != null)
                        {
                            List<ParsePrepodList> PrepCabinetList =
                                  DataProvider.PrepodDiscipline.Filter(p => p.Discipline.Name == discipline.Discipline.Name)
                                .OrderByDescending(p => p.Prepod_PrepodMeta.Rate)
                                .Select(p => new ParsePrepodList
                                {
                                    LastName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().LastName,
                                    FirstName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().FirstName,
                                    MiddleName = p.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault().MiddleName,
                                    Cabinet = p.Prepod_PrepodMeta.Classroom,
                                    Likes = p.Prepod_PrepodMeta.Prepod_PrepodLikes.Count
                                }).ToList();

                            viewList = new OutputModel()
                            {
                                PrepCabinetList = PrepCabinetList,
                                Name = Name,
                                Check = Check
                            };

                        }
                    }
                }
                else
                {
                    viewList = new OutputModel()
                    {
                        Name = find
                    };
                }
            }
            return viewList;
        }
    }
}
