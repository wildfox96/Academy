namespace AUPPRB.Models.ViewModels.jqGridModels
{
    public class JqGridSettingsModel
    {
        public JqGridSettingsModel()
        {
            JqGridName = "JqGrid";
            NeedInitializedEvent = true;

            NeedAddWrappingDialog = true;
            NeedEditWrapingDialog = true;

            NeedAddButton = true;
            NeedEditButton = true;
            NeedDeleteButton = true;

            ViewAction = "View";
            EditAction = "Edit";
            DeleteAction = "Delete";

            AddButtonClass = "addButton";
            ViewButtonClass = "viewButton";
            EditButtonClass = "editButton";
            DeleteButtonText = "deleteButton";

            AddButtonText = "Добавить";
            ViewButtonText = "Просмотреть";
            EditButtonText = "Редактировать";
            DeleteButtonText = "Удалить";

            AddDialogTitle = "Добавление";
            EditDialogTitle = "Редактирование";
            ViewDialogTitle = "Просмотр";

            InitializationFunctionName = "gridInitialized";

            ViewDialogWidth = 900;
            EditDialogWidth = 900;

            AfterInitializedEvents = new string[0];
            NeedCloseEditWindowAfterSave = true;

        }



        public string JqGridName { get; set; }

        public bool NeedInitializedEvent { get; set; }

        public bool NeedViewWrappingDialog { get; set; }
        public bool NeedAddWrappingDialog { get; set; }
        public bool NeedEditWrapingDialog { get; set; }

        public bool NeedViewButton { get; set; }
        public bool NeedAddButton { get; set; }
        public bool NeedEditButton { get; set; }
        public bool NeedDeleteButton { get; set; }
        public string AdditionalButtonFormater { get; set; }


        public string ViewAction { get; set; }
        public string EditAction { get; set; }
        public string DeleteAction { get; set; }

        public string AddButtonClass { get; set; }
        public string ViewButtonClass { get; set; }
        public string EditButtonClass { get; set; }


        public string AddButtonText { get; set; }
        public string ViewButtonText { get; set; }
        public string EditButtonText { get; set; }
        public string DeleteButtonText { get; set; }

        public string AddDialogTitle { get; set; }
        public string ViewDialogTitle { get; set; }
        public string EditDialogTitle { get; set; }
        

        public string InitializationFunctionName { get; set; }

        public string Controller { get; set; }
        public string Area { get; set; }
        
        public int ViewDialogWidth { get; set; }
        public int EditDialogWidth { get; set; }

        public string[] AfterInitializedEvents { get; set; }

        public string SubmitClickEventBefore { get; set; }


        public string CustomViewTemplate { get; set; }
        public string CustomEditTemplate { get; set; }

        public bool NeedCloseEditWindowAfterSave { get; set; }
    }
}