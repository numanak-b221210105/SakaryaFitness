# 🏋️ Sakarya Fitness App: Spor Salonu Yönetim ve Randevu Sistemi

## 🎯 Proje Amacı ve Konsept

[cite_start]Bu proje, Web Programlama dersi kapsamında [cite: 4] [cite_start]ASP.NET Core MVC teknolojileri kullanılarak geliştirilmiştir. [cite: 6, 36] [cite_start]Projenin temel amacı, bir spor salonunun (Fitness Center) yönetim süreçlerini dijitalleştirmek, [cite: 6] [cite_start]antrenör-üye iletişimini kolaylaştırmak ve yapay zekâ destekli kişiselleştirilmiş spor deneyimleri sunmaktır. [cite: 7, 8]

## ✨ Ana Özellikler (Proje Gereksinimleri)

| Özellik Alanı | Açıklama | Ödev Maddesi |
| :--- | :--- | :--- |
| **Antrenör & Hizmet Yönetimi** | Salon hizmetleri (fitness, yoga, pilates vb.) ve antrenörler sisteme tanımlanır. [cite_start]Tüm temel varlıklar için **CRUD** işlemleri tam olarak uygulanmıştır. [cite: 12, 15, 45] | 1 & 2 |
| **Üye ve Randevu Sistemi** | [cite_start]Üyeler, uygun antrenör ve hizmete göre sistem üzerinden randevu alabilir. [cite: 19] [cite_start]Randevu çakışma kontrol mekanizması mevcuttur. [cite: 20] | 3 |
| **Yetkilendirme (Auth)** | **Rol bazlı yetkilendirme** uygulanmıştır. [cite_start]Minimum iki rol (Admin ve Üye) bulunmaktadır. [cite: 49] | 4 |
| **REST API ve Raporlama** | [cite_start]Projenin en az bir bölümünde REST API kullanılmıştır. [cite: 24] API üzerinden Antrenör, Hizmet ve Randevu verileri JSON formatında sunulur. [cite_start]**LINQ sorguları** ile filtreleme gerçekleştirilmiştir. [cite: 25, 54] | 5 |
| **Yapay Zeka (AI) Entegrasyonu** | [cite_start]Kullanıcıların boy/kilo ve hedef bilgisi girerek kendilerine özel egzersiz ve diyet planı önerileri alabilmeleri sağlanmıştır. [cite: 31, 32] | 6 |

## 🛠️ Kullanılan Teknolojiler

* [cite_start]**Backend:** ASP.NET Core MVC (Güncel LTS), C# [cite: 36, 37]
* [cite_start]**Veritabanı:** PostgreSQL [cite: 38]
* [cite_start]**ORM:** Entity Framework Core (EF Core), LINQ [cite: 39]
* [cite_start]**Arayüz:** HTML5, CSS3, JavaScript, jQuery [cite: 41]
* [cite_start]**Tasarım:** Bootstrap 5 (Responsive ve modern tema) [cite: 40]
* [cite_start]**Versiyon Kontrol:** Git & GitHub (Düzenli commitler ile projenin gelişimi takip edilmiştir.) [cite: 62, 65]

## 🔑 Varsayılan Giriş Bilgileri

Sistemde tanımlı iki rol mevcuttur.

| Rol | Amaç | Giriş Bilgisi |
| :--- | :--- | :--- |
| **Admin** | [cite_start]Antrenör ve Hizmet yönetimi, tüm randevuları görme. [cite: 47, 51] | [cite_start]**Email:** `ogrencinumarasi@sakarya.edu.tr` / **Şifre:** `sau` [cite: 51] |
| **Üye** | [cite_start]Randevu alma, AI Antrenör kullanma, kendi randevularını görme. [cite: 52] | Kayıt sayfasından oluşturulur. |

## 💻 Projeyi Çalıştırma

1.  Projeyi klonlayın.
2.  PostgreSQL veritabanınızı kurun ve `appsettings.json` dosyasındaki bağlantı dizesini güncelleyin.
3.  Terminalde: `dotnet restore`
4.  Terminalde: `dotnet run`
5.  Tarayıcıda `http://localhost:5271` adresini ziyaret edin.

---
**Geliştirici:** [Öğrenci Adı Soyadı] - [Öğrenci Numarası]
