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
var supabaseKey = "sb_publishable_0_lHXtyWgRm-Ee2thHTx9w_A1pOpDFZ";

var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};

// Projeye Supabase servis olarak ekleniyor
builder.Services.AddScoped<Supabase.Client>(provider =>
    new Supabase.Client(supabaseUrl, supabaseKey, options));

await builder.Build().RunAsync();
