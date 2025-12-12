using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SakaryaFitnessApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity Ayarları - Basit Şifre İzni
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3; 
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// GÜVENLİ VERİTABANI OLUŞTURMA VE ADMİN EKLEME
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.Migrate();

        // Rolleri Ekle
        if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("Member")) await roleManager.CreateAsync(new IdentityRole("Member"));

        // Admin Kullanıcısını Ekle (Hata Kontrollü)
        string adminEmail = "b221210105@sakarya.edu.tr"; 
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            
            // Kullanıcıyı oluşturmayı dene
            var result = await userManager.CreateAsync(adminUser, "sau");
            
            // SADECE BAŞARILI OLURSA ROL VER
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine(">>>> ADMİN KULLANICISI BAŞARIYLA OLUŞTURULDU! <<<<");
            }
            else
            {
                // Başarısız olursa nedenini yazdır (Çökme engellenir)
                Console.WriteLine(">>>> ADMİN OLUŞTURULAMADI! SEBEPLER: <<<<");
                foreach (var err in result.Errors)
                {
                    Console.WriteLine($"- {err.Description}");
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Kritik Hata: " + ex.Message);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();