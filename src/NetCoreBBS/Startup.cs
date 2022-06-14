using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreBBS.Infrastructure;
using NetCoreBBS.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.WebEncoders;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using NetCoreBBS.Entities;
using NetCoreBBS.Infrastructure.Repositorys;
using NetCoreBBS.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using IGeekFan.AspNetCore.Identity.FreeSql;
using FreeSql;

namespace NetCoreBBS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            FreeSqlFactory.Init(Configuration.GetConnectionString("DefaultConnection"));

            services.AddFreeDbContext<DataContext>(options => options.UseFreeSql(FreeSqlFactory.Fsql).UseOptions(new DbContextOptions()
            {
                EnableCascadeSave = true
            }));
            services.AddIdentity<User, Role>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            }).AddFreeSqlStores<DataContext>().AddDefaultTokenProviders(); ;
            // Add framework services.
            services.AddMvc();
            services.AddScoped<IRepository<TopicNode>, Repository<TopicNode>>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITopicReplyRepository, TopicReplyRepository>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IdentityUserClaim<string>, UserClaim>();
            services.AddScoped<UserServices>();
            services.AddMemoryCache();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("Admin", "Allowed");
                    });
            });
            //services.AddAuthentication()
            //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            //{
            //    o.LoginPath = new PathString("/Account/Login");
            //    o.AccessDeniedPath = new PathString("/Error/Forbidden");
            //});

            //文字被编码 https://github.com/aspnet/HttpAbstractions/issues/315
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRequestIPMiddleware();

            InitializeNetCoreBBSDatabase(app.ApplicationServices);
            app.UseDeveloperExceptionPage();

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
                endpoint.MapControllerRoute(name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });
                endpoint.MapControllerRoute(name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeNetCoreBBSDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<DataContext>();
                if (db.TopicNodes.Count() == 0)
                {
                    db.TopicNodes.AddRange(GetTopicNodes());
                    db.SaveChanges();
                }
            }
        }

        IEnumerable<TopicNode> GetTopicNodes()
        {
            return new List<TopicNode>()
            {
                new TopicNode() { Name=".NET Core", NodeName="", ParentId=0, Order=1, CreateOn=DateTime.Now, },
                new TopicNode() { Name=".NET Core", NodeName="netcore", ParentId=1, Order=1, CreateOn=DateTime.Now, },
                new TopicNode() { Name="ASP.NET Core", NodeName="aspnetcore", ParentId=1, Order=1, CreateOn=DateTime.Now, }
            };
        }
    }
}
