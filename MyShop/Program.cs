using Microsoft.EntityFrameworkCore;
using MyShop.Models;
//Oppretter en instans av WebApplicationBuilder som brukes til å konfigurere
//applikasjonen. args er komamandolinje argumenter som sendes inn i applikasjonen
var builder = WebApplication.CreateBuilder(args);
//Builder.Services: Refererer til IServiceCollection, som er en samling av tjenester 
//som applikasjonen bruker
//AddControllersWithViews: Legger støtte for MVC i applikasjonen. Denne metoden
//konfigurerer nødvendige tjenester for å håndtere kontroller og generere HTML-sider
builder.Services. AddControllersWithViews();

builder.Services.AddDbContext<ItemDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ItemDbContextConnection"]);
});

//Når alle tjenester og middleware er konfigurert, bygges applikasjonen ved å kalle Build()
//Dette returnerer en WebApplication instans som kan brukes til å konfigurere
//pipeline-en videre og kjøre applikasjonen
var app = builder.Build();

//Dette skjekker om applikasjonen kjører i et utviklingsmiljø.
if(app.Environment.IsDevelopment())
{
    //Hvis applikasjonen kjører i utviklingsmiljø, legger dette til en side som viser detaljerte feilmeldinger.
    app.UseDeveloperExceptionPage();
    //Calls the seeding method to initialise the database with predefined data.
    DBInit.Seed(app);
}

//Gjør det mulig at applikasjonen kan servere statiske filer som bilder direkte fra
//mappen wwwroot uten noen ekstra kode
app.UseStaticFiles();

//Setter opp en standardrute for MVC-applikasjonen
app.MapDefaultControllerRoute();

app.Run();
