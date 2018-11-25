using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.UIModels.Product;
using WebApi.Services.Product;

namespace WebApi.Product.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        // GET api/values
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
             _productService = productService;
        }


        [HttpGet]
        [Route("~/User/Permissions")]
        public IActionResult GetUserPermissions()
        {
            var currentUser = HttpContext.User.Identity.Name;
            var model = _productService.GetPermissions(currentUser);
            return Ok(model);
        }

        [HttpGet]
        [Route("~/User/Menus")]
        public IActionResult GetUserMenus()
        {
            var currentUser = HttpContext.User.Identity.Name;
            var model = _productService.GetUserMenus(currentUser);
            return Ok(model);
        }


        [HttpGet]
        [Route("~/Menus")]
        public IActionResult GetMenus(string roleid)
        {
            var model = _productService.GetMenus(roleid);
            return Ok(model);
        }


        [HttpPut]
        [Route("Permission/Update/{roleid}")]
        public async Task<IActionResult> UpdatePermission(string roleid, [FromBody] PermissionUIModel request)
        {
            try
            {
                var model =await _productService.UpdatePermissionsAsync(roleid, request);
                return Ok(model);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
