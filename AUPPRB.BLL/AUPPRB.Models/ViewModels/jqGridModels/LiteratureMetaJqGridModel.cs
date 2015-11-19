using System.Collections.Generic;
using System.Web.UI.WebControls;
using Trirand.Web.Mvc;
using TextAlign = Trirand.Web.Mvc.TextAlign;

namespace AUPPRB.Models.ViewModels.jqGridModels
{
    public class LiteratureMetaJqGridModel
    {
        public JQGrid LiteratureGrid { get; set; }
        public LiteratureMetaJqGridModel()
        {
            LiteratureGrid = new JQGrid
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
                                                         
                                                         DataField = "ShortName",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "Название",
                                                         Width = 20
                                                         
                                                     },
                                                     new JQGridColumn
                                                     {   
                                                         
                                                         DataField = "Author",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "Автор",
                                                         Width = 20
                                                     },   
                                                    new JQGridColumn
                                                     {   
                                                         
                                                         DataField = "Year",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "Год издания",
                                                         Width = 5
                                                     }, 
                                                    new JQGridColumn
                                                     {   
                                                         
                                                         DataField = "TotalCount",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "Всего",
                                                         Width = 5
                                                     },
                                                    new JQGridColumn
                                                     {   
                                                         
                                                         DataField = "FreeCount",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "Свободно",
                                                         Width = 5
                                                     },
                                                 new JQGridColumn
                                                     {
                                                         Searchable = false,
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "Управление",
                                                         Width = 20,
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
                    ShowDeleteButton = false,
                    ShowSearchToolBar = true
                },
            };

        }
    }
}
