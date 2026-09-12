# Sanayi Randevu

Sanayi Randevu, bir oto sanayi işletmesinin müşterilerinden çevrim içi servis randevusu almasını ve işletme yöneticisinin bu randevuları yönetmesini sağlayan ASP.NET Core Razor Pages uygulamasıdır.

## Özellikler

### Müşteri tarafı

- Hesap oluşturma, giriş yapma ve ASP.NET Core Identity ile kullanıcı yönetimi
- Araç ekleme, düzenleme ve silme
- Hizmet listesini ve hizmet detaylarını görüntüleme
- Araç ve hizmet seçerek randevu oluşturma
- Çalışma saatleri, kapalı tarihler ve saat başına kapasiteye göre uygun saatleri görme
- Geçmiş tarih veya başka bir müşteriye ait araçla randevu oluşturmayı engelleme
- Kendi randevularını listeleme ve randevu detaylarını görüntüleme
- Randevu durumu ve admin yanıtını takip etme

### Yönetici tarafı

- Yalnızca `Admin` rolüne açık yönetim alanı
- Randevuları durum ve tarihe göre filtreleme
- Bekleyen, onaylanan ve gün içindeki randevu sayılarını görme
- Randevu durumunu güncelleme ve müşteriye yanıt yazma
- Hizmet ekleme, düzenleme ve silme
- Müşteri listesini, araç/randevu sayılarını görme
- Müşteri parolasını sıfırlama
- Çalışma saatlerini, kapalı tarihleri ve randevu kapasitesini yönetme

## Kullanılan teknolojiler

- .NET 9 ve ASP.NET Core Razor Pages
- C# ve nullable reference types
- Entity Framework Core 9
- SQLite
- ASP.NET Core Identity ve rol tabanlı yetkilendirme
- Bootstrap, jQuery Validation ve unobtrusive validation
- Entity Framework Core migrations

## Proje yapısı

```text
Areas/Admin/       Yönetici sayfaları
Areas/Identity/    Identity giriş, kayıt ve hesap sayfaları
Data/              DbContext ve başlangıç verisi
Helpers/           Görüntüleme yardımcıları
Migrations/        EF Core veritabanı migration dosyaları
Models/            Kullanıcı, araç, hizmet ve randevu modelleri
Pages/             Müşteri ve genel Razor Pages sayfaları
Services/          Randevu uygunluk kontrol servisi
wwwroot/           CSS, JavaScript ve istemci kütüphaneleri
```

## Gereksinimler

- .NET 9 SDK
- Visual Studio 2022 veya VS Code
- Windows, Linux ya da macOS

Kurulu SDK sürümünü kontrol etmek için:

```bash
dotnet --version
```

## Kurulum

1. Depoyu klonlayın ve proje klasörüne girin:

```bash
git clone <GITHUB_DEPO_ADRESI>
cd "sanayı randevu"
```

2. Paketleri geri yükleyin:

```bash
dotnet restore
```

3. Geliştirme ortamını başlatın:

```bash
dotnet run
```

Uygulama ilk açılışta EF Core migration'larını otomatik uygular. SQLite veritabanı kök dizinde `otosanayi.db` adıyla oluşturulur. Bu dosya yerel makineye aittir ve Git'e gönderilmez.

## Admin hesabı oluşturma

Admin parolası kaynak kodda tutulmaz. Uygulama ilk başlatılmadan önce aşağıdaki ortam değişkenlerini tanımlayın.

PowerShell:

```powershell
$env:SeedAdmin__Email = "admin@example.com"
$env:SeedAdmin__Password = "Guclu-Ve-Gizli-Bir-Parola-123!"
dotnet run
```

Linux/macOS:

```bash
export SeedAdmin__Email="admin@example.com"
export SeedAdmin__Password="Guclu-Ve-Gizli-Bir-Parola-123!"
dotnet run
```

Uygulama açılışında roller (`Admin`, `Customer`), çalışma saatleri ve örnek hizmetler seed edilir. Admin hesabı yalnızca `SeedAdmin:Email` ve `SeedAdmin:Password` değerleri sağlandığında oluşturulur. Gerçek projede güçlü, benzersiz bir parola kullanın ve bu değerleri Git'e eklemeyin.

## Veritabanı ve migration işlemleri

Yeni bir migration oluşturmak için:

```bash
dotnet ef migrations add MigrationAdi
```

Migration'ı veritabanına uygulamak için:

```bash
dotnet ef database update
```

`dotnet ef` komutu bulunamazsa EF aracını kurabilirsiniz:

```bash
dotnet tool install --global dotnet-ef
```

## Yapılandırma

Varsayılan SQLite bağlantısı `appsettings.json` içinde aşağıdaki gibidir:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=otosanayi.db"
  }
}
```

Üretim ortamında bağlantı dizesini kaynak koda yazmak yerine ortam değişkeni veya güvenli bir secret yönetimi kullanın:

```powershell
$env:ConnectionStrings__DefaultConnection = "Data Source=C:\data\sanayi-randevu.db"
```

## Güvenlik notları

- `otosanayi.db` müşteri, araç ve randevu verileri içerebilir; GitHub'a gönderilmez.
- Admin parolası, API anahtarı ve üretim bağlantı dizeleri kaynak kodda tutulmamalıdır.
- `.gitignore` `bin`, `obj`, `.vs` ve yerel veritabanı dosyalarını dışarıda bırakır.
- GitHub'a göndermeden önce `git status` ve `git diff --cached` çıktısını kontrol edin.
- Daha önce bir parola veya anahtar commit edildiyse yalnızca dosyayı silmek yeterli değildir; ilgili parolayı/anahtarı hemen yenileyin.

## Üretim için yayınlama

```bash
dotnet publish -c Release -o ./publish
```

Üretimde `ASPNETCORE_ENVIRONMENT=Production` kullanın, HTTPS yapılandırın, güçlü bir admin parolası belirleyin ve SQLite dosyasının yedekleme/erişim politikasını ayrıca planlayın.

## Docker ile çalıştırma

Docker image'ını oluşturmak için:

```bash
docker build -t sanayirandevu .
```

Container'ı çalıştırmak için:

```bash
docker run --rm -p 10000:10000 \
  -e SeedAdmin__Email=admin@example.com \
  -e SeedAdmin__Password="Guclu-Ve-Gizli-Bir-Parola-123!" \
  sanayirandevu
```

Uygulama container içinde `10000` portunu dinler. SQLite verilerinin container yeniden oluşturulduğunda kaybolmaması için üretimde kalıcı disk veya harici bir veritabanı kullanın.

## Render üzerinde yayınlama

1. Render panelinde **New > Web Service** seçin ve GitHub deposunu bağlayın.
2. Environment olarak **Docker** seçin. Render kök dizindeki `Dockerfile` dosyasını otomatik kullanır.
3. Servis portunu `10000` olarak ayarlayın.
4. Aşağıdaki environment variables değerlerini Render servis ayarlarına ekleyin:

```text
ASPNETCORE_ENVIRONMENT=Production
SeedAdmin__Email=admin@example.com
SeedAdmin__Password=<guclu-ve-gizli-parola>
```

5. Deploy işlemini başlatın. Render her deploy sırasında Docker image'ını yeniden oluşturur.

`SeedAdmin__Password` değerini GitHub'a veya Dockerfile'a yazmayın. SQLite kullanıldığı için Render'da kalıcı disk bağlanmazsa servis yeniden deploy edildiğinde uygulama verileri sıfırlanabilir.

## Lisans

Bu proje için henüz bir lisans belirtilmemiştir.
