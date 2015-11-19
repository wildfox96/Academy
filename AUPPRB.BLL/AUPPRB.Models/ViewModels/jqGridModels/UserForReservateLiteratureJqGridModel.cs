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
    public class UserForReservateLiteratureJqGridModel
    {
        public JQGrid UsersGrid { get; set; }

        public UserForReservateLiteratureJqGridModel()
        {
            UsersGrid = new JQGrid
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

                        DataField = "LastName",
                        Editable = true,
                        Searchable = true,
                        DataType = typeof (string),
                        TextAlign = TextAlign.Center,
                        HeaderText = "Фамилия",
                        Width = 20

                    },
                    new JQGridColumn
                    {

                        DataField = "FirstName",
                        Editable = true,
                        Searchable = true,
                        DataType = typeof (string),
                        TextAlign = TextAlign.Center,
                        HeaderText = "Имя",
                        Width = 20
                    },
                    new JQGridColumn
                    {

                        DataField = "MiddleName",
                        Editable = true,
                        Searchable = true,
                        DataType = typeof (string),
                        TextAlign = TextAlign.Center,
                        HeaderText = "Отчество",
                        Width = 20
                    },
                    new JQGridColumn
                    {

                        DataField = "Login",
                        Editable = true,
                        Searchable = true,
                        DataType = typeof (string),
                        TextAlign = TextAlign.Center,
                        HeaderText = "Логин",
                        Width = 20
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
                
                SearchDialogSettings = {MultipleSearch = true, Width = 800},
                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "LastName",
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
