using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Trirand.Web.Mvc;
using SortDirection = Trirand.Web.Mvc.SortDirection;
using TextAlign = Trirand.Web.Mvc.TextAlign;

namespace AUPPRB.Models.ViewModels.jqGridModels
{
   public  class DisciplineJqGridModel
    {
       public JQGrid DisciplineJqGrid { get; set; }

       public DisciplineJqGridModel()
       {
           DisciplineJqGrid = new JQGrid
           {
               Columns = new List<JQGridColumn>()
               {
                   new JQGridColumn()
                   {
                       DataField = "Id",
                        // always set PrimaryKey for Add,Edit,Delete operations
                       // if not set, the first column will be assumed as primary key
                       PrimaryKey = true,
                       Editable = false,
                       Searchable = true,
                       HeaderText = "Id",
                       TextAlign = TextAlign.Center,
                       Visible = false,
                       Width = 5
                   },

                   new JQGridColumn()
                   {
                       DataField = "FullName",
                       Editable = true,
                       Sortable = true,
                       HeaderText = "Полное название дисциплины",
                       TextAlign = TextAlign.Left,
                       Width = 20
                   },

                   new JQGridColumn()
                   {
                       DataField = "ShortName",
                       Editable = true,
                       Sortable = true,
                       HeaderText = "Сокращенное название",
                       TextAlign = TextAlign.Center,
                       Width = 10
                   },
                   
                   new JQGridColumn()
                   {
                       DataField = "IsActive",
                       Editable = true,
                       Sortable = true,
                       HeaderText = "Активна",
                       TextAlign = TextAlign.Center,
                       Formatter = new CheckBoxFormatter(),
                       Width = 10
                   },
                    new JQGridColumn
                                        {
                                            Searchable = false,
                                            TextAlign = TextAlign.Center,
                                            HeaderText = "Управление",
                                            Width = 10,
                                            Formatter = new CustomFormatter
                                                            {
                                                                FormatFunction = "FormatDetails"
                                                            }
                                        }


               },

               AutoWidth = true,
               Height = Unit.Percentage(100),
               PagerSettings = new Trirand.Web.Mvc.PagerSettings() { PageSizeOptions = "[5,10,30,100000000]" },
               SearchDialogSettings = { MultipleSearch = true, Width = 800 },
               SortSettings = new SortSettings()
               {
                   InitialSortColumn = "Id",
                   InitialSortDirection = SortDirection.Asc
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
               EditDialogSettings = { CloseAfterEditing = true },
               AddDialogSettings = { CloseAfterAdding = true }

           };
       }


    }
}
