using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCore.Logica.BL
{
    public class Projects
    {
        /// <summary>
        ///  GET  PROJECT BY ID OR  TENANT OR USER PROJECT
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Models.DB.Projects> GetProjects(int? id, 
            int? tenantId,
            string userId = null) 
        {
            DAL.Models.ProjectCoreContext _context = new DAL.Models.ProjectCoreContext();

            var listProjectsEF = ( from _projects in _context.Projects
                                select _projects).ToList();

            if(id != null)
            {
                listProjectsEF = listProjectsEF.Where(x => x.Id == id).ToList();
            }
            if(tenantId != null)
            {
                listProjectsEF = listProjectsEF.Where(x => x.TenantId == tenantId).ToList();
            }

            if (!string.IsNullOrEmpty(userId))
            {
                listProjectsEF = (from _projects in listProjectsEF
                                  join _userProjects in _context.UserProjects on _projects.Id equals _userProjects.ProjectId
                                  where _userProjects.UserId.Equals(userId)
                                  select _projects).ToList();
            }

            var listProjects = (from _projects in listProjectsEF
                                select new Models.DB.Projects 
                                {
                                    Id = _projects.Id,
                                    Title = _projects.Title,
                                    Details =_projects.Details,
                                    ExpectedCompletionDate = _projects.ExpectedCompletionDate,
                                    TenantId = _projects.TenantId,
                                    CreatedAt = _projects.CreatedAt,
                                    UpdatedAt = _projects.UpdatedAt


                                }).ToList();
            return listProjects;
        }

        //create project
        public void CreateProjects(string title,
            string details, DateTime? expectedCompletionDate,
            int? tenantId)
        {
            DAL.Models.ProjectCoreContext _context = new DAL.Models.ProjectCoreContext();

            _context.Projects.Add( new DAL.Models.Projects 
            { 
                Title = title,
                Details= details,
                ExpectedCompletionDate = expectedCompletionDate,
                TenantId = tenantId,
                CreatedAt= DateTime.Now



            });

            //aplica todo los cambio en la BD
            _context.SaveChanges();
        }

    }
}
