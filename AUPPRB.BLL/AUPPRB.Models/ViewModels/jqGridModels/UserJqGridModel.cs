using System.Collections.Generic;
using System.Web.UI.WebControls;
using Trirand.Web.Mvc;
using TextAlign = Trirand.Web.Mvc.TextAlign;

//using TextAlign = System.Web.UI.WebControls.TextAlign;

namespace AUPPRB.Models.ViewModels.jqGridModels
{
  
    public class UserJqGridModel
    {
        public JQGrid UserGrid { get; set; }

        public UserJqGridModel()
        {
            UserGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                                             {
                                                 new JQGridColumn
                                                     {
                                                         DataField = "Id",
                                                         // always set PrimaryKey for Add,Edit,Delete operations
                                                         // if not set, the first column will be assumed as primary key
                                                         PrimaryKey = true,
                                                         Editable = false,
                                                         Searchable = false,
                                                         HeaderText = "Идентификатор",
                                                         TextAlign = TextAlign.Left,
                                                         Visible = false,
                                                     },
                                                      new JQGridColumn
                                                     {   
                                                         
                                                         DataField = "RoleIcon",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Center,
                                                         HeaderText = "Роль",
                                                         Width = 5
                                                         
                                                     },
                                                     new JQGridColumn
                                                     {   
                                                         
                                                         DataField = "Login",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Left,
                                                         HeaderText = "Логин",
                                                         Width = 30
                                                     },
                                                 //new JQGridColumn
                                                 //    {
                                                 //        DataField = "IsBlocked",
                                                 //        Editable = false,
                                                 //        DataType = typeof (string),
                                                 //        TextAlign = TextAlign.Left,
                                                 //        HeaderText = "Заблокирован",
                                                 //        Width = 40,
                                                 //        Visible = false
                                                 //    },
                                                 new JQGridColumn
                                                     {
                                                         DataField = "LastName",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Left,
                                                         HeaderText = "Фамилия",
                                                         Width = 30
                                                     },
                                                 new JQGridColumn
                                                     {
                                                         DataField = "FirstName",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Left,
                                                         HeaderText = "Имя",
                                                         Width = 30
                                                     },
                                                 new JQGridColumn
                                                     {
                                                         DataField = "MiddleName",
                                                         Editable = true,
                                                            Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Left,
                                                         HeaderText = "Отчество",
                                                         Width = 30
                                                     },
                                                     // new JQGridColumn
                                                     //{
                                                     //    DataField = "Company",
                                                     //    Editable = true,
                                                     //    DataType = typeof (string),
                                                     //    TextAlign = TextAlign.Left,
                                                     //    HeaderText = "Структурное подразделение",
                                                     //    Width = 40
                                                     //},
                                                     //  new JQGridColumn
                                                     //{
                                                     //    DataField = "UniqueNumber",
                                                     //    Editable = true,
                                                     //    DataType = typeof (string),
                                                     //    TextAlign = TextAlign.Left,
                                                     //    HeaderText = "Табельный номер",
                                                     //    Width = 40
                                                     //},
                                                 
                                                 new JQGridColumn
                                                     {
                                                         DataField = "Email",
                                                         Editable = true,
                                                         Searchable = true,
                                                         DataType = typeof (string),
                                                         TextAlign = TextAlign.Left,
                                                         HeaderText = "E-mail",
                                                         Width = 20
                                                     },
                                                 //new JQGridColumn
                                                 //    {
                                                 //        DataField = "Roles",
                                                 //        Editable = true,
                                                 //        DataType = typeof (string),
                                                 //        TextAlign = TextAlign.Left,
                                                 //        HeaderText = "Роли",
                                                 //        Width = 40
                                                 //    },
                                                 //new JQGridColumn
                                                 //    {
                                                 //        DataField = "InstitutionSite",
                                                 //        Editable = true,
                                                 //        DataType = typeof (string),
                                                 //        TextAlign = TextAlign.Left,
                                                 //        HeaderText = "Адрес сайта учреждения образования",
                                                 //        Width = 40
                                                 //    },
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
                    ShowDeleteButton = false,
                    ShowSearchToolBar = true
                },
            };
            UserGrid.EditDialogSettings.CloseAfterEditing = true;
            UserGrid.AddDialogSettings.CloseAfterAdding = true;
        }
    }
}