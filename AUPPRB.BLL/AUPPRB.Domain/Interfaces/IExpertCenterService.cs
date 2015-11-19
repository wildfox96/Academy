using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.ViewModels.ExpertCenterModels;
namespace AUPPRB.Domain.Interfaces
{
    public interface IExpertCenterService
    {
        /// <param name="page">Номер начальной страницы</param>
        OutputModel PrepodModel(int page);
        OutputModel DispModel(int page);

        OutputModel StartPageModel();
        OutputModel SeacrhModel(string find);
    }
}
