using verfiyemail.Services;
using verfiyemail.Data;
using verfiyemail.Models;
using verfiyemail.Services;

namespace verfiyemail
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            var smtpSettings = Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            services.AddSingleton(smtpSettings);
            services.AddTransient<EmailService, EmailService>();
            services.AddTransient<TokenService, TokenService>();
            services.AddSingleton<AuthDataAccess>();
            services.AddControllers();
            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors("AllowSpecificOrigin");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
