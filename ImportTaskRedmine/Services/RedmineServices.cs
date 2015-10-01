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
        private IList<Tracker> listaDeTrackers;
        private IList<ProjectMembership> listaDeUsuarios;

        public RedmineServices(string host, string apiKey)
        {
            manager = new RedmineManager(host, apiKey);
        }

        public List<Project> ListarProjetos()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Status", ProjectStatus.Active.ToString());
            
            try
            {
                return manager.GetObjectList<Project>(parameters).ToList();
            }
            catch (RedmineException rex)
            {
                return null;
            }
        }
        
        public List<ExcelIssue> ValidarIdIssues(List<ExcelIssue> listadeIssueFromExcel)
        {
            IEnumerable<ExcelIssue> listaIssuesEncontrados;
            IEnumerable<ExcelIssue> listaIssuesNaoEncontrados;

            var parameters = new NameValueCollection();
            parameters.Add("project_id", listadeIssueFromExcel[0].ProjectId.ToString());
            var listaIssuesFromRedmine = manager.GetObjectList<Issue>(parameters);

            listaIssuesEncontrados = from first in listadeIssueFromExcel
                                       join second in listaIssuesFromRedmine
                                       on first.IdRedmine equals second.Id
                                       select first;
            listaIssuesNaoEncontrados = listadeIssueFromExcel.Except(listaIssuesEncontrados.ToList());

            return listaIssuesNaoEncontrados.ToList();
        }

        public ExcelIssue CadastrarIssue(ExcelIssue atualExcelIssue, bool atualizarProjetoExistente)
        {
            Issue issueLoaded = null;
            Issue issueNew;
            Issue issueToSave = null;

            issueNew = new Issue
            {
                Id = atualExcelIssue.IdRedmine,
                Subject = atualExcelIssue.Subject,
                EstimatedHours = atualExcelIssue.EstimatedHours,
                StartDate = atualExcelIssue.StartDate,
                DueDate = atualExcelIssue.DueDate,
                AssignedTo = new IdentifiableName { Id = atualExcelIssue.AssigneeId },
                Tracker = new IdentifiableName { Id = atualExcelIssue.TrackerId },
                Project = new IdentifiableName { Id = atualExcelIssue.ProjectId },
                ParentIssue = atualExcelIssue.ParentExcelIssue != null && atualExcelIssue.ParentExcelIssue.IdRedmine != 0 ?
                    new IdentifiableName { Id = atualExcelIssue.ParentExcelIssue.IdRedmine } : null
            };

            if (atualizarProjetoExistente && atualExcelIssue.IdRedmine != 0)
            {
                try
                {
                    issueLoaded = manager.GetObject<Issue>(atualExcelIssue.IdRedmine.ToString(), null);
                    issueToSave = AtualizarSomenteModificacoes(issueLoaded, issueNew);
                }
                catch(RedmineException rex)
                {
                    Console.WriteLine("Mensagem de erro do sistema ao tentar carregar a Issue Id " + atualExcelIssue.IdRedmine.ToString() + ": " + rex.Message);
                    throw new Exception("O ID da Issue/Atividade sendo atualizada não foi encontrada no Redmine!");
                }
            }
            else
                issueToSave = issueNew;

            try
            {
                if (!atualizarProjetoExistente)
                {
                    issueToSave = manager.CreateObject(issueToSave);
                    atualExcelIssue.IdRedmine = issueToSave.Id;
                }
                else if (issueToSave.Id != 0)
                    manager.UpdateObject(issueToSave.Id.ToString(), issueToSave);
            }
            catch(RedmineException rex)
            {
                if (rex.Message.Contains("Tracker is not included in the list"))
                {
                    throw new Exception("Tracker Not Found");
                }
                else {
                    throw rex;
                }
            }

            return atualExcelIssue;
        }

        private Issue AtualizarSomenteModificacoes(Issue issueLoaded, Issue issueNew)
        {
            bool mudou = false;

            if (!issueLoaded.Subject.Equals(issueNew.Subject))
            {
                issueLoaded.Subject = issueNew.Subject;
                mudou = true;
            }

            if (!issueLoaded.EstimatedHours.Equals(issueNew.EstimatedHours))
            {
                issueLoaded.EstimatedHours = issueNew.EstimatedHours;
                mudou = true;
            }

            if (!issueLoaded.StartDate.Value.ToString("ddMMyyyy").Equals(issueNew.StartDate.Value.ToString("ddMMyyyy")))
            {
                issueLoaded.StartDate = issueNew.StartDate;
                mudou = true;
            }

            if (!issueLoaded.DueDate.Value.ToString("ddMMyyyy").Equals(issueNew.DueDate.Value.ToString("ddMMyyyy")))
            {
                issueLoaded.DueDate = issueNew.DueDate;
                mudou = true;
            }

            if (issueNew.AssignedTo.Id != -1)
            {
                if (issueLoaded.AssignedTo == null || !issueLoaded.AssignedTo.Id.Equals(issueNew.AssignedTo.Id))
                {
                    issueLoaded.AssignedTo = issueNew.AssignedTo;
                    mudou = true;
                }
            }

            if (issueNew.Tracker != null && !issueNew.Tracker.Id.Equals(-1) && !issueLoaded.Tracker.Id.Equals(issueNew.Tracker.Id))
            {
                issueLoaded.Tracker = issueNew.Tracker;
                mudou = true;
            }
            
            if (issueNew.ParentIssue != null && !issueLoaded.ParentIssue.Equals(issueNew.ParentIssue))
            {
                issueLoaded.ParentIssue = issueNew.ParentIssue;
                mudou = true;
            }

            if (!mudou)
                issueLoaded.Id = 0;

            return issueLoaded;
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
            if (excelIssue.AssigneeName == null)
                return -1;

            if (listaDeUsuarios == null)
            {
                var parametros = new NameValueCollection();
                parametros.Add("project_id", excelIssue.ProjectId.ToString());
                listaDeUsuarios = manager.GetObjectList<ProjectMembership>(parametros);
            }

            //User user = listaDeMembrosProjeto[1].User;

            List<ProjectMembership> usuariosFiltrados = listaDeUsuarios.Where(x => x.User.Name.Trim() == excelIssue.AssigneeName.Trim()).ToList();

            if (usuariosFiltrados.Count > 0)
                return usuariosFiltrados[0].User.Id;
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
                NameValueCollection parametros = new NameValueCollection();
                parametros.Add("project_id", excelIssue.ProjectId.ToString());
                listaDeTrackers = manager.GetObjectList<Tracker>(parametros);
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
