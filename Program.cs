using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SakaryaFitnessApp.Data;
using System.Security.Claims; // Claim için gerekli

var builder = WebApplication.CreateBuilder(args);

// --- 1. ZAMAN DİLİMİ HATA ÇÖZÜMÜ ---
// PostgreSQL'den gelen zaman dilimsiz (Unspecified) tarihleri UTC olarak kabul et.
// Bu, Randevu sistemindeki ArgumentException hatasını çözer.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true); 

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

// --- 2. GÜVENLİ VERİTABANI OLUŞTURMA VE ADMİN EKLEME/GÜNCELLEME ---
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

        // Admin Kullanıcısını Ekle veya Güncelle
        string adminEmail = "b221210105@sakarya.edu.tr"; 
        string adminName = "Yönetici"; 
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            // Yeni oluşturma
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(adminUser, "sau");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                // Yeni oluştururken isim (Claim) ekle
                await userManager.AddClaimAsync(adminUser, new Claim("FullName", adminName));
                Console.WriteLine($">>>> ADMİN '{adminName}' KULLANICISI BAŞARIYLA OLUŞTURULDU! <<<<");
            }
        }
        else
        {
            // ZATEN VARSA: İsim (Claim) kontrolü yap ve yoksa ekle
            var claims = await userManager.GetClaimsAsync(adminUser);
            if (!claims.Any(c => c.Type == "FullName"))
            {
                await userManager.AddClaimAsync(adminUser, new Claim("FullName", adminName));
                Console.WriteLine($">>>> VAR OLAN ADMİN KULLANICISINA '{adminName}' İSMİ EKLENDİ. <<<<");
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