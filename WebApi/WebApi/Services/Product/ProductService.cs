using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.UIModels.Product;
using WebApi.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using WebApi.DataAccessLayer.Entities.Product;

namespace WebApi.Services.Product
{
    public class ProductService:IProductService
    {
        private DataContext _context;
        private IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        public ProductService(DataContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public List<string> GetPermissions(string userid)
        {
            var role = getRoleByUserId(userid);

            var data=_context.Permissions.Include(p=>p.Menus).Where(p => p.IsActive && p.RoleId == role.Id);

            return data.Select(p => p.Menus.Name).Distinct().ToList();
        }


        public List<TreeviewUIModel> GetMenus(string roleid)
        {
            var data = _context.Menus.Where(p => p.IsActive && !p.IsMandatory).OrderBy(p=>p.SequenceNumber);
            var permids = _context.Permissions.Where(p => p.IsActive && p.RoleId == roleid).Select(p => p.MenuId).ToList();

            return data.Where(p => !p.ParentId.HasValue).Select(p => new TreeviewUIModel
            {
                text = p.Name,
                value = p.Id,
                Checked=permids.Contains(p.Id),
                children = getChilds(data.Where(q =>q.ParentId.HasValue? q.ParentId == p.Id:false).ToList(), data.ToList(), permids)
            }).ToList();
        }

       private List<TreeviewUIModel> getChilds(List<Menu> submenus,List<Menu> menus,List<int> permids)
        {
            return submenus.Select(p => new TreeviewUIModel
            {
                text = p.Name,
                value = p.Id,
                Checked = permids.Contains(p.Id),
                children = getChilds(menus.Where(q=>q.ParentId.HasValue ? q.ParentId == p.Id : false).ToList(), menus, permids)
            }).ToList();
        }

        public List<MenuUIModel> GetUserMenus(string userid)
        {
         

            try
            {
                var role = getRoleByUserId(userid);

                var menuList = _context.Menus.Where(p => p.IsActive && p.IsMandatory && p.IsDisplay).ToList();
                var data = _context.Permissions.Include(p => p.Menus).Where(p => p.IsActive && p.Menus.IsActive &&p.Menus.IsDisplay && p.RoleId == role.Id);

                var menus = data.Select(p => p.Menus).Distinct().ToList();

                menuList.AddRange(data.Select(p => p.Menus).Distinct());

                var userMenus = menuList.Distinct().OrderBy(p => p.SequenceNumber).ToList();

                return userMenus.Where(p => !p.ParentId.HasValue).Select(p => new MenuUIModel
                {

                    name = p.Name,
                    icon = p.Icon,
                    url = p.Routing,
                    badge = new badgeUIModel
                    {
                        text = p.Text,
                        variant = p.Variant
                    },
                    children = getSubMenus(userMenus.Where(q=>q.ParentId.HasValue? q.ParentId.Value == p.Id:false).ToList(), userMenus),
                }).ToList();

            }
            catch(Exception ex)
            {
                var dd = ex;
                return new List<MenuUIModel>();
            }
        }

        private List<MenuUIModel> getSubMenus(List<Menu> subMenus,List<Menu> menus)
        {
         return subMenus.Count()>0? subMenus.OrderBy(p => p.SequenceNumber).Select(p => new MenuUIModel
            {

                name = p.Name,
                icon = p.Icon,
                url = p.Routing,
                badge = new badgeUIModel
                {
                    text = p.Text,
                    variant = p.Variant
                },
             children = getSubMenus(menus.Where(q => q.ParentId.HasValue ? q.ParentId.Value == p.Id : false).ToList(), menus),
         }).ToList():null;
        }
        private Role getRoleByUserId(string userid)
        {

          return  _context.UserRoles.Include(p=>p.Roles).Where(p => p.UserId == userid).FirstOrDefault().Roles;
        }


        public async Task<int> UpdatePermissionsAsync(string roleid, PermissionUIModel request)
        {

            var perms = _context.Permissions.Where(p => p.IsActive && p.RoleId == roleid).Select(p=>p.MenuId).Distinct().ToList();

            var inserted = request.SelectedIds.Except(perms);
            var deleted = perms.Except(request.SelectedIds);

            foreach(var id in inserted)
            {
                var obj = new Permission();
                obj.MenuId = id;
                obj.RoleId = roleid;
                obj.IsActive = true;
                obj.InsertedDate = DateTime.Now;
                obj.InsertedBy = "admin";
                _context.Permissions.Add(obj);
            }
            foreach (var id in deleted)
            {
              var data= _context.Permissions.Where(p => p.IsActive && p.RoleId == roleid && p.MenuId == id);
                if(data!=null && data.Count() > 0)
                {
                    _context.Permissions.Where(p => p.IsActive && p.RoleId == roleid && p.MenuId == id).FirstOrDefault().IsActive = false;
                }
            }
        return   await  _context.SaveChangesAsync();

            }

        }
}
