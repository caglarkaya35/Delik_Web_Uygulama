using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddScoped<Shared.Servisler.DelikHesaplama>();

// Supabase �stemcisini (Client) Tan�ml�yoruz
var supabaseUrl = "https://arovectgqyzzfkztcqwm.supabase.co";
var supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFyb3ZlY3RncXl6emZrenRjcXdtIiwicm9sZSI6ImFub24iLCJpYXQiOjE3ODA2Mzk0NDcsImV4cCI6MjA5NjIxNTQ0N30.DHv-unWDeI-JH_mdUxtmEH-sA95h9kyjuNe0T1SgtfA";

var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = false
};

// Projeye Supabase servis olarak ekleniyor
builder.Services.AddScoped<Supabase.Client>(provider =>
    new Supabase.Client(supabaseUrl, supabaseKey, options));

await builder.Build().RunAsync();
