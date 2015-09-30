using Excel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace Aptum.ImportTaskRedmine.Entities
{
    public class ExcelIssue
    {
        public ExcelIssue()
        {
            this.IdRedmine = 0;
        }

        public int IdRedmine { get; set; }
        public string Subject { get; set; }
        public float EstimatedHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public String AssigneeName { get; set; }
        public int AssigneeId { get; set; }
        public string OutlineNumber { get; set; }
        public string Tracker { get; set; }
        public int TrackerId { get; set; }
        public int ProjectId { get; set; }
        public ExcelIssue ParentExcelIssue { get; set; }

        private static void AssociarIssueFilhaAoPai(List<ExcelIssue> IssueList)
        {
            string outlineNumberParentTask = "";

            foreach(var issue in IssueList)
            {
                int tamanho = issue.OutlineNumber.Length;
                if(tamanho>2)
                    outlineNumberParentTask = issue.OutlineNumber.Remove(tamanho - 2);

                if(IssueList.Where(x => x.OutlineNumber == outlineNumberParentTask).Count() > 0)
                {
                    ExcelIssue parentIssue = IssueList.Where(x => x.OutlineNumber == outlineNumberParentTask).ToArray()[0];
                    issue.ParentExcelIssue = parentIssue;
                }
            }

            //return IssueList;
        }
        
        public static List<ExcelIssue> CarregarIssuesExcel(System.IO.Stream fileStream, int projectId)
        {
            List<ExcelIssue> IssueList = new List<ExcelIssue>();
            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

            //3. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            bool sair = false;
            excelReader.Read(); //pula a primeira linha que contem o nome das colunas
            //4. Data Reader methods
            while (excelReader.Read() && !sair)
            {
                ExcelIssue issue = new ExcelIssue();

                try
                {
                    issue.OutlineNumber = excelReader.GetString(0);
                    issue.Subject = excelReader.GetString(1);
                    issue.EstimatedHours = excelReader.GetFloat(2);
                    issue.StartDate = excelReader.GetDateTime(3);
                    issue.DueDate = excelReader.GetDateTime(4);
                    issue.AssigneeName = excelReader.GetString(5);
                    issue.Tracker = excelReader.GetString(6);
                    issue.IdRedmine = excelReader.GetInt32(7) == -2147483648 ? 0 : excelReader.GetInt32(7);
                    issue.ProjectId = projectId;
                }
                catch (Exception ex)
                {
                    return null;
                }

                if (issue.OutlineNumber == "")
                    sair = true;
                else
                    IssueList.Add(issue);
            }

            //6. Free resources
            excelReader.Close();
            fileStream.Close();

            AssociarIssueFilhaAoPai(IssueList);

            return IssueList;
        }
    }
}
