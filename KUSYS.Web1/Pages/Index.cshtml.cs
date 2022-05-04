using KUSYS.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KUSYS.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        IUserService _userService;

        public IndexModel(ILogger<IndexModel> logger, IUserService userService)
        {
            _logger = logger;

            _userService = userService;
        }

        public void OnGet()
        {
            _userService.GetList(new Core.Utilities.FilterHelper<Entities.Concrete.User>
            {

            });
        }
    }
}