
using Microsoft.AspNetCore.OData;
using ODataAPI.ODataEdmModelBuilders;
using Services;

namespace ODataAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddOData(opt =>
            {
                opt.EnableQueryFeatures()
                    .AddRouteComponents("odata", ODataEdmModelBuilder.GetEdmModel());
            });

            var apiBaseAddress = "https://localhost:7185/api/";

            // Add services to the container.
            builder.Services.AddHttpClient<INewsArticleService, NewsArticleService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
