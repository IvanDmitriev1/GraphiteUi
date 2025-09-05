using GraphiteUI.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddGraphiteUi();

await builder.Build().RunAsync();
