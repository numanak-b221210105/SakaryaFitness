# 🏋️ Sakarya Fitness App: Spor Salonu Yönetim ve Randevu Sistemi

## 🎯 Proje Amacı ve Konsept

Bu proje, Web Programlama dersi kapsamında ASP.NET Core MVC teknolojileri kullanılarak geliştirilmiştir. Projenin temel amacı, öğrenilen bilgileri gerçek bir probleme uygulayarak, bir spor salonunun yönetim süreçlerini dijitalleştiren ve üyelere kişiselleştirilmiş spor deneyimleri sunan bir web uygulaması geliştirmektir.

Sistem, spor salonlarının sunduğu hizmetleri, antrenörlerin uzmanlık alanlarını, üyelerin randevularını ve yapay zekâ tabanlı egzersiz önerilerini yönetebilecek nitelikte olacaktır.

## ✨ Ana Özellikler (Proje Gereksinimleri)

| Özellik Alanı | Açıklama |
| :--- | :--- |
| **Antrenör & Hizmet Yönetimi** | Salon hizmetleri ve antrenörler sisteme tanımlanır. Antrenörlerin uzmanlık alanları belirtilir. Tüm temel varlıklar için **CRUD** (Oluşturma, Okuma, Güncelleme, Silme) işlemleri tam olarak uygulanmıştır. |
| **Üye ve Randevu Sistemi** | Kullanıcılar, uygun antrenör ve hizmete göre sistem üzerinden randevu alabilir. Sistem, randevu saatlerinin çakışma durumunu kontrol eden onay mekanizmasına sahiptir. |
| **Yapay Zeka (AI) Entegrasyonu** | Kullanıcıların vücut ölçüleri (boy/kilo/yaş) ve hedef bilgisi girilerek, yapay zekâ mantığı ile kendilerine uygun egzersiz ve diyet planı önerileri sunulur. |
| **Raporlama & REST API** | Projede REST API kullanılarak veritabanı ile iletişim sağlanmıştır. API üzerinden **LINQ sorguları** ile Antrenör, Hizmet ve Randevu verileri JSON formatında sunulur. |
| **Yetkilendirme (Auth)** | **Rol bazlı yetkilendirme** (Admin ve Üye) uygulanmıştır. Tüm temel formlarda (Client ve Server Side) veri doğrulama (Data Validation) mevcuttur. |

## 🛠️ Kullanılan Teknolojiler

* **Backend Çatısı:** ASP.NET Core MVC (Güncel LTS Sürümü)
* **Programlama Dili:** C#
* **Veritabanı:** PostgreSQL
* **Veri Yönetimi:** Entity Framework Core (ORM), LINQ
* **Arayüz:** Bootstrap 5, HTML5, CSS3, JavaScript
* **Geliştirme Metodu:** Git & GitHub (Düzenli komitler ile geliştirme süreci kayıt altına alınmıştır.)

## 🔑 Varsayılan Giriş Bilgileri

Sistemde tanımlı Admin rolü, tüm yönetim işlemlerini gerçekleştirebilir.

| Rol | Amaç | Giriş Bilgisi |
| :--- | :--- | :--- |
| **Admin** | Antrenör, Hizmet ve tüm randevuların yönetimi. | **Email:** `ogrencinumarasi@sakarya.edu.tr` / **Şifre:** `sau` |
| **Üye** | Randevu alma, AI Antrenör kullanma. | Kayıt sayfasından oluşturulur. |

## 💻 Projeyi Çalıştırma

1. Projeyi klonlayın.
2. PostgreSQL veritabanınızı kurun ve `appsettings.json` dosyasındaki bağlantı dizesini güncelleyin.
3. Terminalde: `dotnet restore`
4. Terminalde: `dotnet run`
5. Tarayıcıda `http://localhost:5271` adresini ziyaret edin.

---

**Geliştirici:** Numan AK - B221210105
