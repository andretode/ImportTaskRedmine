using System;
using System.Linq;
using System.Collections.Specialized;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Aptum.ImportTaskRedmine.Entities;
using System.Collections.Generic;

namespace Aptum.ImportTaskRedmine.Services
{
    public class RedmineServices
    {
        private RedmineManager manager;
        private List<Tracker> listaDeTrackers;
        private List<User> listaDeUsuarios;

        public RedmineServices(string host, string apiKey)
        {
            manager = new RedmineManager(host, apiKey);
        }

        public List<Project> ListarProjetos()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Status", ProjectStatus.Active.ToString());
            return manager.GetObjectList<Project>(parameters).ToList();
        }

        public ExcelIssue CadastrarIssue(ExcelIssue atualExcelIssue)
        {
            var newIssue = new Issue {
                Subject = atualExcelIssue.Subject,
                EstimatedHours = atualExcelIssue.EstimatedHours,
                StartDate = atualExcelIssue.StartDate,
                DueDate = atualExcelIssue.DueDate,
                AssignedTo = new IdentifiableName { Id = atualExcelIssue.AssigneeId },
                Tracker = new IdentifiableName { Id = atualExcelIssue.TrackerId },
                Project = new IdentifiableName { Id = atualExcelIssue.ProjectId }
                };

            if (atualExcelIssue.ParentExcelIssue.IdRedmine != 0)
                newIssue.ParentIssue = new IdentifiableName { Id = atualExcelIssue.ParentExcelIssue.IdRedmine };

            newIssue = manager.CreateObject(newIssue);
            atualExcelIssue.IdRedmine = newIssue.Id;
            return atualExcelIssue;
        }

        /**
         * <summary>Procura no Redmine pelo usuário atribuido na tarefa (issue) e retorna seu ID</summary>
         * <returns>
         * O valor de retorno -1 (menos um) significa que a tarefa (issue) não foi atribuída a ninguem.
         * Diferente do retorno 0 (zero) que significa que foi atribuida a alguém porém este não foi encontrado no Redmine.
         * Qualquer outro retorno representa o ID do usuário no Redmine.
         * </returns>
        **/
        private int UsuarioExiste(ExcelIssue excelIssue)
        {
            if (excelIssue.AssigneeEmail == null)
                return -1;

            if (listaDeUsuarios == null)
                listaDeUsuarios = manager.GetUsers(UserStatus.STATUS_ACTIVE).ToList();

            List<User> usuariosFiltrados = listaDeUsuarios.Where(x => x.Email == excelIssue.AssigneeEmail).ToList();

            if (usuariosFiltrados.Count > 0)
                return usuariosFiltrados[0].Id;
            else
                return 0;
        }

        public List<ExcelIssue> ValidarListaDeUsuarios(List<ExcelIssue> ListExcelIssue)
        {
            foreach(var excelIssue in ListExcelIssue)
                excelIssue.AssigneeId = UsuarioExiste(excelIssue);

            return ListExcelIssue;
        }

        /**
         * <summary>Procura no Redmine pelo Tracker associado a tarefa (issue) e retorna seu ID</summary>
         * <returns>
         * O valor de retorno -1 (menos um) significa que o Tracker não foi atribuída a Issue.
         * Diferente do retorno 0 (zero) que significa que foi atribuida a Issue porém este não foi encontrado no Redmine.
         * Qualquer outro retorno representa o ID do Tracker no Redmine.
         * </returns>
        **/
        private int TrackerExiste(ExcelIssue excelIssue)
        {
            if (excelIssue.Tracker == null || excelIssue.Tracker == "")
                return -1;

            if (listaDeTrackers == null)
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("Name", "*");
                listaDeTrackers = manager.GetObjectList<Tracker>(parameters).ToList();
            }

            List<Tracker> trackersFiltrados = listaDeTrackers.Where(x => x.Name == excelIssue.Tracker).ToList();

            if (trackersFiltrados.Count > 0)
                return trackersFiltrados[0].Id;
            else
                return 0;
        }

        public List<ExcelIssue> ValidarListaDeTrackers(List<ExcelIssue> ListExcelIssue)
        {
            foreach (var excelIssue in ListExcelIssue)
                excelIssue.TrackerId = TrackerExiste(excelIssue);

            return ListExcelIssue;
        }

        //private int RetornarParentTask(ExcelIssue excelIssue)
        //{
        //    excelIssue
        //    return 0;
        //}
    }
}
