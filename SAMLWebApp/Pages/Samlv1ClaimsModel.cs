using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace SAMLWebApp.Pages
{
    [Authorize]
    public class Samlv1ClaimsModel : PageModel
    {
        private readonly ILogger<Samlv1ClaimsModel> _logger;

        public Samlv1ClaimsModel(ILogger<Samlv1ClaimsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}