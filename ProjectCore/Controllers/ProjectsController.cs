using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectCore.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;


        public ProjectsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Logica.BL.Tenants tenants = new Logica.BL.Tenants();
            var tenant = tenants.GetTenants(user.Id).FirstOrDefault();

            Logica.BL.Projects projects = new Logica.BL.Projects();
            var result = await _userManager.IsInRoleAsync(user, "Admin") ?
                projects.GetProjects(null, tenant.Id): 
                projects.GetProjects(null, tenant.Id, user.Id);

            var listProjects = result.Select(x => new Logica.Models.ViewModel.ProjectsIndexViewModel
                { 
                    Id = x.Id,
                    Title = x.Title,
                    Details = x.Details,
                    CreatedAt = x.CreatedAt,
                    ExpectedCompletionDate = x.ExpectedCompletionDate,
                    UpdateAt = x.UpdatedAt

            });

            listProjects = tenant.Plan.Equals("premium") ?
                listProjects :
                listProjects.Take(1).ToList();

            //ViewBag.listProjects = listProjects; more 1 models


            return View(listProjects);// return view has  the same name of action
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(Logica.Models.BindingModel.ProjectsCreateBindingModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                Logica.BL.Tenants tenants = new Logica.BL.Tenants();
                var tenant = tenants.GetTenants(user.Id).FirstOrDefault();

                Logica.BL.Projects projects = new Logica.BL.Projects();
                projects.CreateProjects(model.Title, model.Details, model.ExpectedCompletionDate, tenant.Id);

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}