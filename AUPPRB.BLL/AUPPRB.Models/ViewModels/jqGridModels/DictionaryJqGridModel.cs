using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Trirand.Web.Mvc;
using TextAlign = Trirand.Web.Mvc.TextAlign;

namespace AUPPRB.Models.ViewModels.jqGridModels
{
    public class DictionaryJqGridModel
    {
        public JQGrid DictionaryGrid { get; set; }

        public DictionaryJqGridModel()
        {
            DictionaryGrid=new JQGrid()
            {
                Columns = new List<JQGridColumn>()
                                             {
                                                 new JQGridColumn
                                                     {
                                                         DataField = "Id",
                                                         PrimaryKey = true,
                                                         Editable = false,
                                                         Searchable = false,
                                                         HeaderText = "Идентификатор",
                                                         TextAlign = TextAlign.Left,
                                                         Visible = false,
                                                     },
                                                     new JQGridColumn
                                                     {
                                                         DataField = "Name",
                                                         Editable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Left,
                                                         HeaderText = "Название справочника",
                                                         Width = 40
                                                     },
                                              
                                                 new JQGridColumn
                                                     {
                                                         Searchable = false,
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "",
                                                         Width = 10,
                                                         Formatter = new CustomFormatter
                                                                         {
                                                                             FormatFunction = "FormatDetails"
                                                                         }
                                                     }
                                             },
                AutoWidth = true,
                Height = Unit.Percentage(100),
                //PagerSettings = new PagerSettings() { PageSizeOptions = "[5,10,15,25,30]" },
                SearchDialogSettings = { MultipleSearch = true, Width = 800 },
                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "Id",
                    InitialSortDirection = Trirand.Web.Mvc.SortDirection.Asc
                },
                ToolBarSettings =
                {
                    ShowSearchButton = true,
                    ShowRefreshButton = true,
                    ShowEditButton = false,
                    ShowAddButton = false,
                    ShowDeleteButton = false
                },
      
            };
        }
    }
}
