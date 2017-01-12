using MyCompanyName.AbpZeroTemplate.Storage;
using System.Web.Mvc;

namespace MyCompanyName.AbpZeroTemplate.Web.Controllers {
    public class HomeController : AbpZeroTemplateControllerBase {

        private readonly ISqlExecuter _sqlhelper;
        public HomeController(ISqlExecuter sqlhelper) {
            _sqlhelper = sqlhelper;
        }

        public ActionResult Index() {
            return View();
        }

      
    }
}