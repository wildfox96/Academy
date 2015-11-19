using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AUPPRB.Common.Utils.ExcellWorker
{
    /// <summary>
    /// Класс для работы с Excell
    /// </summary>
    public class ExcellWorker
    {
        #region Properties

        private int _indexOfBorderCell=0;

        /// <summary>
        /// Путь к текущему файлу
        /// </summary>
        public string CurrentFilePath
        {
            get { return _currentFilePath; }
        }

        /// <summary>
        /// Путь к документу с которым ведется работа
        /// </summary>
        private readonly string _currentFilePath;

        /// <summary>
        /// Удалять ли файл после удалении обьекта
        /// </summary>
        public readonly bool RemoveAfterDestroy;

        /// <summary>
        /// Текущий документ
        /// </summary>
        private SpreadsheetDocument _currentDocument;

        /// <summary>
        /// Текущая книга для работы (особенность Excell 7 и выше)
        /// </summary>
        private WorkbookPart _currentWorkBookPart;

        /// <summary>
        /// Текущий лист для работы
        /// </summary>
        private Sheet _currentSheet; 

        #endregion

        #region Constructors

        /// <summary>
        /// Создает экземпляр класса для работы с текущим файлом
        /// </summary>
        /// <param name="filePath">Путь к документу</param>
        /// <param name="removeAfterDestroy">Удалять ли файл после окончания работы с ним</param>
        public ExcellWorker(string filePath, bool removeAfterDestroy)
        {
            _currentFilePath = filePath;
            _currentDocument = SpreadsheetDocument.Open(filePath, true);
            _currentWorkBookPart = _currentDocument.WorkbookPart;
            _currentSheet = _currentWorkBookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
            RemoveAfterDestroy = removeAfterDestroy;
        }


        /// <summary>
        /// Создает экземпляр класса для формирования нового файла по шаблону
        /// </summary>
        /// <param name="templatePath">Путь к документу-шаблону</param>
        /// <param name="newFilePath">Путь к новому файлу</param>
        /// <param name="removeAfterDestroy">Удалять ли файл после окончания работы с ним</param>
        public ExcellWorker(string templatePath, string newFilePath, bool removeAfterDestroy)
        {
            _currentFilePath = newFilePath;
            CopyFile(templatePath, newFilePath);
            _currentDocument = SpreadsheetDocument.Open(newFilePath, true);
            _currentWorkBookPart = _currentDocument.WorkbookPart;
            _currentSheet = _currentWorkBookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
            RemoveAfterDestroy = removeAfterDestroy;
        }

        /// <summary>
        /// Создает экземпляр класса для формирования нового файла по шаблону
        /// </summary>
        /// <param name="templatePath">Путь к документу-шаблону</param>
        /// <param name="newFilePath">Путь к новому файлу</param>
        /// <param name="sheetName">Название активного листа</param>
        /// <param name="removeAfterDestroy">Удалять ли файл после окончания работы с ним</param>
        public ExcellWorker(string templatePath, string newFilePath, string sheetName, bool removeAfterDestroy)
        {
            _currentFilePath = newFilePath;
            CopyFile(templatePath, newFilePath);
            _currentDocument = SpreadsheetDocument.Open(newFilePath, true);
            _currentWorkBookPart = _currentDocument.WorkbookPart;
            _currentSheet = _currentWorkBookPart.Workbook.Descendants<Sheet>().FirstOrDefault(p => p.Name == sheetName);
            RemoveAfterDestroy = removeAfterDestroy;
        }

        /// <summary>
        /// При удалении обьекта удаляем файл
        /// </summary>
        ~ExcellWorker()
        {
            if (RemoveAfterDestroy)
                DeleteFile();
        } 
        #endregion

       

        /// <summary>
        /// Удаление текущего файла
        /// </summary>
        public void DeleteFile()
        {
            CloseFile();
            try
            {
                File.Delete(_currentFilePath);
            }
            catch (Exception)
            {

                //throw;
            }
        }

        /// <summary>
        /// Преобразовывает текущий файл в массив байт
        /// </summary>
        /// <returns></returns>
        public byte[] GetFileBinaryData()
        {
            byte[] bufer;
            CloseFile();
            try
            {
                bufer = File.ReadAllBytes(_currentFilePath);
            }
            catch (Exception)
            {
                bufer = new byte[] { };
            }
            OpenFile();
            return bufer;
        }

       

        /// <summary>
        /// Получить значение ячеки по адресу
        /// </summary>
        /// <param name="addressName"></param>
        /// <returns></returns>
        public string GetCellValue(string addressName)
        {
            var value = "";
            var wsPart = (WorksheetPart)(_currentWorkBookPart.GetPartById(_currentSheet.Id));

            var theCell = wsPart.Worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference == addressName);

            if (theCell == null) return value;

            value = theCell.InnerText;

            if (theCell.DataType == null) return value;

            switch (theCell.DataType.Value)
            {
                case CellValues.SharedString:
                    var stringTable = _currentWorkBookPart.
                        GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    if (stringTable != null)
                    {
                        value = stringTable.SharedStringTable.
                            ElementAt(int.Parse(value)).InnerText;
                    }
                    break;
                case CellValues.Boolean:
                    switch (value)
                    {
                        case "0":
                            value = "FALSE";
                            break;
                        default:
                            value = "TRUE";
                            break;
                    }
                    break;
            }

            return value;
        }

        /// <summary>
        /// Получить значение ячеки по адресу и имени листа
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public string GetCellValue(string addressName, string sheetName)
        {
            var cellValue = string.Empty;

            var sheet = _currentWorkBookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet == null) return cellValue;

            var ws = ((WorksheetPart)(_currentWorkBookPart.GetPartById(sheet.Id))).Worksheet;
            var sheetData = ws.GetFirstChild<SheetData>();
            Cell cell = null;

            UInt32 rowNumber = GetRowIndex(addressName);
            Row row = GetRow(sheetData, rowNumber);

            Cell refCell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == addressName);

            if (refCell != null)
            {
                cell = refCell;
            }
            int id;
            if (Int32.TryParse(cell.InnerText, out id))
            {
                SharedStringItem item = GetSharedStringItemById(id);

                if (item.Text != null)
                {
                    cellValue = item.Text.Text;
                }
                else if (item.InnerText != null)
                {
                    cellValue = item.InnerText;
                }
                else if (item.InnerXml != null)
                {
                    cellValue = item.InnerXml;
                }
            }
            return cellValue;
        }

        /// <summary>
        /// Обновляет значение ячейки в указанном листе
        /// </summary>
        /// <param name="sheetName">Имя листа</param>
        /// <param name="addressName">адрес</param>
        /// <param name="value">значение</param>
        /// <param name="isString"></param>
        /// <returns></returns>
        public bool UpdateValue(string sheetName, string addressName, string value, bool isString = true)
        {
            // Assume failure.
            bool updated = false;

            Sheet sheet = _currentWorkBookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);

            if (sheet == null) return updated;

            var ws = ((WorksheetPart)(_currentWorkBookPart.GetPartById(sheet.Id))).Worksheet;
            Cell cell = InsertCellInWorksheet(ws, addressName);
          
          
            if (isString)
            {
                // Either retrieve the index of an existing string,
                // or insert the string into the shared string table
                // and get the index of the new item.
                int stringIndex = InsertSharedStringItem(_currentWorkBookPart, value);

                cell.CellValue = new CellValue(stringIndex.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            }
            else
            {
                cell.CellValue = new CellValue(value);
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }

            // Save the worksheet.
            ws.Save();
            updated = true;

            return updated;
        }

        /// <summary>
        /// Обновляет значение ячейки текущем листе
        /// </summary>
        /// <param name="addressName">адрес</param>
        /// <param name="value">значение</param>
        /// <param name="isString"></param>
        /// <returns></returns>
        public bool UpdateValue(string addressName, string value, bool isString = true)
        {
            return UpdateValue(_currentSheet.Name, addressName, value, isString);
        }

        #region Inner helpers

        /// <summary>
        /// Закрывает соединение с текущи файлом
        /// </summary>
        private void Dispose()
        {
            try
            {
                _currentDocument.Dispose();
            }
            catch (Exception)
            {
                //throw new Exception("Ошибка разрыва соединения с файлом");
            }
        }


        /// <summary>
        /// Копирут файл 
        /// </summary>
        /// <param name="srcFile">Путь к файлу который требуется скопировать</param>
        /// <param name="targetFile">Путь где будет хранится копия файла</param>
        private static void CopyFile(string srcFile, string targetFile)
        {
            try
            {
                // Overwrites existing files
                File.Copy(srcFile, targetFile, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка копирования файла :" + ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Закрывает текущий документ
        /// </summary>
        public void CloseFile()
        {
            
            try
            {
                _currentDocument.Close();
            }
            catch (Exception)
            {
                //throw new Exception("Ошибка закрытия файла");
            }
            Dispose();
           
        }

        //открывает файл для возобнавления работы
        public void OpenFile()
        {
            CloseFile();
            _currentDocument = SpreadsheetDocument.Open(_currentFilePath, true);
            _currentWorkBookPart = _currentDocument.WorkbookPart;
            _currentSheet = _currentWorkBookPart.Workbook.Descendants<Sheet>().FirstOrDefault(p => p.Name == _currentSheet.Name);
        }

        /// <summary>
        /// Получает индекс строки по адресу ячейки
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static UInt32 GetRowIndex(string address)
        {
            string rowPart;
            UInt32 l;
            UInt32 result = 0;

            for (var i = 0; i < address.Length; i++)
            {
                if (!UInt32.TryParse(address.Substring(i, 1), out l)) continue;
                rowPart = address.Substring(i, address.Length - i);
                if (!UInt32.TryParse(rowPart, out l)) continue;
                result = l;
                break;
            }
            return result;
        }

        /// <summary>
        /// Получает строку по индексу строки (если нет то вставляет строку)
        /// </summary>
        /// <param name="wsData"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private static Row GetRow(SheetData wsData, UInt32 rowIndex)
        {
            var row = wsData.Elements<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex);
            if (row == null)
            {
                row = new Row { RowIndex = rowIndex };
                wsData.Append(row);
            }
            return row;
        }

        private SharedStringItem GetSharedStringItemById(int id)
        {
            return _currentWorkBookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        private Cell InsertCellInWorksheet(Worksheet ws, string addressName)
        {
            var sheetData = ws.GetFirstChild<SheetData>();
            Cell cell = null;

            var rowNumber = GetRowIndex(addressName);
            var row = GetRow(sheetData, rowNumber);

            var refCell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == addressName);
            cell = refCell ?? CreateCell(row, addressName);
            return cell;
        }

        /// <summary>
        /// Вставка ячейки по адресу
        /// </summary>
        /// <param name="row"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private Cell CreateCell(Row row, String address)
        {
            var refCell = row.Elements<Cell>().FirstOrDefault(cell => String.Compare(cell.CellReference.Value, address, StringComparison.OrdinalIgnoreCase) > 0);

            Cell cellResult = new Cell { CellReference = address };

            row.InsertBefore(cellResult, refCell);
            return cellResult;
        }

        /// <summary>
        /// Заполнение ячейки данными
        /// </summary>
        /// <param name="wbPart"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int InsertSharedStringItem(WorkbookPart wbPart, string value)
        {
            var index = 0;
            var found = false;
            var stringTablePart = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault() ??
                                  wbPart.AddNewPart<SharedStringTablePart>();


            var stringTable = stringTablePart.SharedStringTable ?? new SharedStringTable();


            foreach (var item in stringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == value)
                {
                    found = true;
                    break;
                }
                index += 1;
            }

            if (found) return index;
            stringTable.AppendChild(new SharedStringItem(new Text(value)));
            stringTable.Save();

            return index;
        }

        
        #endregion
    }
}
