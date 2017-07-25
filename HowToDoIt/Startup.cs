using HowToDoIt.Models;
using HowToDoIt.Models.Classes_for_Db;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HowToDoIt.Startup))]
namespace HowToDoIt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //AddCategory();
            //CreateRolesandUsers();
        }

        private void AddCategory()
        {
            using (var context = new ApplicationDbContext())
            {
                Category category1 = new Category();
                Category category2= new Category();
                category1.Name = "Обычная категория";
                category2.Name = "Необычная категория";
                context.Categories.Add(category1);
                context.Categories.Add(category2);
                Tag tag1 = new Tag();
                Tag tag2 = new Tag();
                Tag tag3 = new Tag();
                tag1.Name = "Деньги";
                tag2.Name = "Карты";
                tag3.Name = "Два ствола";
                context.Tags.Add(tag1);
                context.Tags.Add(tag2);
                context.Tags.Add(tag3);
                context.SaveChanges();
            }
        }

        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);

            // создаем пользователей
            var admin = new ApplicationUser { Email = "kostyshutko@gmail.com", UserName = "kostyshutko@gmail.com" };
            string password = "123456";
            var result = userManager.Create(admin, password);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }
        }
    }
}
