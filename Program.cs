using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var sharedOptions = new SharedOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "docs")),
};

app.UseDefaultFiles(new DefaultFilesOptions(sharedOptions));
app.UseStaticFiles(new StaticFileOptions(sharedOptions));

app.Run();
