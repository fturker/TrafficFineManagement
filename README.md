# Trafik Cezaları Yönetim Sistemi

ASP.NET Core MVC ile geliştirilen bu case uygulaması; kullanıcı ve araç yönetimini, trafik cezası oluşturmayı, iki aşamalı onay sürecini ve işlem geçmişinin takibini kapsar. Vatandaşlar kendi araçlarını ve onayı tamamlanan cezalarını görüntüleyebilir, demo ödeme işlemi gerçekleştirebilir.

## 1. Gereksinimler

| Gereksinim | Açıklama |
| --- | --- |
| .NET 9 SDK | Projeyi derlemek ve çalıştırmak için |
| SQL Server / SQL Server Express | Veritabanı için; SQL Server servisi çalışıyor olmalı |
| Visual Studio | .NET 9 destekleyen sürüm ve ASP.NET/web geliştirme bileşenleri; aşağıdaki migration adımları Visual Studio içindir |
| Git | Repository'yi klonlamak için; ZIP indirilecekse gerekli değil |
| SQL Server Management Studio (isteğe bağlı) | Veritabanını ve tabloları incelemek için |

Kullanılan başlıca teknolojiler: **ASP.NET Core MVC, Entity Framework Core, ASP.NET Core Identity, SQL Server, Razor ve Bootstrap.**

## 2. Projeyi İndirme

### Git ile

```bash
git clone https://github.com/fturker/TrafficFineManagement.git
cd TrafficFineManagement
```

### ZIP olarak

1. GitHub'da proje sayfasını açın.
2. **Code > Download ZIP** seçeneğini kullanın.
3. ZIP dosyasını bir klasöre çıkarın.
4. Visual Studio ile çözüm dosyasını açın.

Repository private ise indirmek için erişim yetkisi gerekir.

## 3. NuGet Paketlerini Yükleme

Çözümün bulunduğu kök klasörde terminal açıp çalıştırın:

```bash
dotnet restore
```

Bu komut, proje dosyalarında tanımlı NuGet paketlerini belirtilen sürümleriyle indirir.

Ardından derlemeyi kontrol edin:

```bash
dotnet build
```

## 4. SQL Server Bağlantısını Ayarlama

`TrafficFineManagement.Web/appsettings.json` dosyasındaki mevcut `SQLServer` bağlantı değerini kendi sunucunuza göre düzenleyin.

Uygulama bağlantıyı `ConnectionStringOptions` üzerinden okur. **Mevcut JSON bölüm adını ve diğer bağlantı parametrelerini değiştirmeyiniz. Yalnızca Server= parametresindeki sunucu adını (localhost\SQLEXPRESS) kendi SQL Server sunucu adınızla değiştiriniz**

Yerel SQL Server Express ve Windows Authentication için örnek alan:

```json
"SQLServer": "Server=localhost\\SQLEXPRESS;Database=TrafficFineManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

| Alan | Anlamı |
| --- | --- |
| `Server` | SQL Server örneğinin adı; örneğin `localhost\\SQLEXPRESS` |
| `Database` | Uygulamanın kullanacağı veritabanı: `TrafficFineManagementDb` |
| `Trusted_Connection=True` | Windows hesabıyla bağlantı kurar |
| `TrustServerCertificate=True` | Yerel geliştirme bağlantısında sunucu sertifikasına güvenilmesini sağlar |

## 5. Veritabanını Oluşturma

1. Visual Studio'da `TrafficFineManagement.Web` projesini **Startup Project** olarak seçin.
2. **Tools > NuGet Package Manager > Package Manager Console** penceresini açın.
3. Console'daki **Default project** alanından `TrafficFineManagement.Web` projesini seçin.
4. Şu komutu çalıştırın:

```powershell
Update-Database
```

## 6. Uygulamayı Çalıştırma

Visual Studio'dan projeyi başlatın veya kök klasörde şu komutu çalıştırın:

```bash
dotnet run --project TrafficFineManagement.Web
```

Giriş ekranı `/login` adresindedir; port numarası seçilen launch profile'a göre değişebilir.

Demo kayıtları için uygulamanın **Development** ortamında çalışması gerekir. `Properties/launchSettings.json` dosyasında kullandığınız profilin ortam değişkenini kontrol edin:

```json
"ASPNETCORE_ENVIRONMENT": "Development"
```

Veritabanı hazırlandıktan sonra uygulama başlangıcındaki `SeedDatabaseAsync` çağrısı rolleri oluşturur; geliştirme ortamında SuperAdmin ve demo seeder'larını çalıştırır.

## 7. Hazır Gelen Demo Veriler

### Kullanıcılar

Temiz veritabanında `DemoDataSeeder` aşağıdaki hesapları oluşturur:

| Giriş bilgisi | Şifre | Kullanıcı | Rol |
| --- | --- | --- | --- |
| `11111111110` | `12345` | Ali Yılmaz | `Citizen` |
| `officer` | `12345` | Demo Memur | `Officer` |
| `admin` | `12345` | Demo Yönetici | `Admin` |
| `financeadmin` | `12345` | Demo Finans | `Finance` |
| `superadmin` | `12345` | Super Admin | `SuperAdmin` |

Bireysel kullanıcılar TC numarasıyla, ticari kullanıcılar vergi numarasıyla, personel kullanıcıları kullanıcı adıyla giriş yapar. Demo veride ticari hesap bulunmaz; kullanıcı ekleme ekranından oluşturulabilir.

### Araç ve Ceza

| Kayıt | Bilgi |
| --- | --- |
| Araç sahibi | Ali Yılmaz (`11111111110`) |
| Plaka | `34DM001` |
| Marka / model | Ford / Focus |
| Ceza sayısı | 1 |
| Ceza tutarı | 1.500,00 TL |
| Cezayı oluşturan | `officer` |
| Açıklama | `Demo: Speed limit exceeded.` |
| Başlangıç durumu | Yönetici onayı bekleniyor (`New`) |
| Ödeme durumu | Ödenmedi |
| İşlem geçmişi | Memurun cezayı oluşturduğu kayıt |

Araç ve ceza ID'leri veritabanı tarafından oluşturulur; sabit değerler değildir. Formlarda ID istenen işlemler için ilgili liste veya detay ekranındaki ID kullanılmalıdır.


## 8. Kullanıcı Rolleri

| Rol | Yapabilecekleri |
| --- | --- |
| `Citizen` | Kendi bilgilerini ve araçlarını görüntüleme; onayı tamamlanan kendi cezalarını görme ve demo ödeme işlemi yapma |
| `Officer` | Ceza oluşturma, kendi yazdığı cezaları ve süreçlerini görüntüleme; kendi cezalarını kuralların izin verdiği durumlarda düzenleme; yalnızca Citizen rolünde kullanıcı oluşturma |
| `Admin` | Kullanıcı ve araç yönetimi; yönetici onayı bekleyen cezaları açıklamayla onaylama veya reddetme |
| `Finance` | Ceza listelerini ve süreçlerini görüntüleme; finans onayı bekleyen cezaları açıklamayla onaylama veya reddetme |
| `SuperAdmin` | Yönetim ekranlarına ve farklı rollerin işlemlerine erişme; işlem durumuna bağlı kurallar yine geçerlidir |

Kullanıcı oluştururken Officer yalnızca Citizen rolü atayabilir; Admin, SuperAdmin dışındaki rolleri atayabilir; SuperAdmin tüm rolleri atayabilir.

## 9. İş Akışları

### Ceza Oluşturma ve Onay

1. Memur araç için bir ceza oluşturur. Durum `New` olur ve oluşturma geçmişi kaydedilir.
2. Yönetici açıklama girerek onaylar. Durum `ManagerApproved` olur.
3. Finans kullanıcısı açıklama girerek onaylar. Durum `Completed` olur.
4. Onay süreci tamamlanan ceza araç sahibinin ceza listesinde görünür.
5. Araç sahibi demo ödeme işlemini tamamlar. Ödeme kaydı oluşturulur ve ceza ödendi olarak işaretlenir.

`Completed`, onay sürecinin tamamlandığını belirtir; tek başına ödeme yapıldığı anlamına gelmez.

### Ret ve Tek Seferlik Yeniden Gönderim

Yönetici veya finans cezayı açıklama belirterek reddedebilir. Reddedilen cezayı yalnızca oluşturan kullanıcı bir kez düzenleyip yeniden yönetici onayına gönderebilir. Durum tekrar `New` olur; önceki işlem geçmişi korunur.

Aynı ceza tekrar reddedilirse ikinci kez yeniden gönderim hakkı verilmez. Yeniden `New` durumuna dönen cezaya, onay öncesindeki normal düzenleme kuralları uygulanır. Ödenmiş cezalar düzenlenemez.

### Hazır Verilerle Deneme

| Adım | Hesap | İşlem |
| --- | --- | --- |
| 1 | `officer` | “Yazdığım Cezalar” ekranında örnek cezayı ve oluşturma geçmişini görüntüleyiniz. |
| 2 | `admin` | Yönetici onayı bekleyenler listesinden açıklama girerek onaylayınız. |
| 3 | `financeadmin` | Finans onayı bekleyenler listesinden açıklama girerek onaylayınız. |
| 4 | `11111111110` | “Cezalarım” ekranında onaylanan cezayı görüntüleyip demo ödeme yapınız. |
| 5 | `11111111110` | Ödeme durumunun “Ödendi” olduğunu kontrol ediniz. |

Onay ekranlarının adresleri `/fine/pending-admin` ve `/fine/pending-finance` şeklindedir. Bu adreslere erişim ilgili rol yetkisini gerektirir.

## 10. Kontroller ve Hata Durumları

| Durum | Uygulamanın davranışı |
| --- | --- |
| Eksik zorunlu alan veya uzunluk sınırının aşılması | Doğrulama mesajı gösterilir. |
| Hatalı giriş bilgileri | Oturum açılmaz; giriş ekranında hata gösterilir. |
| Oturum açmadan korumalı sayfaya erişim | Giriş ekranına yönlendirilir. |
| Rolün izin vermediği işlem | Yetki kontrolüyle engellenir. |
| Araç veya kullanıcı kaydının bulunamaması | İşlem tamamlanmaz; hata sonucu döndürülür. |
| Tekrarlanan plaka, TC veya vergi numarası | Benzersizlik kontrolleri ve veritabanı indeksleriyle engellenir. |
| Açıklamasız onay veya ret | İşlem kabul edilmez. |
| Geçerli ceza aşamasına uymayan karar | İş kuralı kontrolüyle engellenir. |
| Reddedilen cezayı başka kullanıcının düzenlemesi | Engellenir. |
| Reddedilen cezada yeniden gönderim hakkının kullanılmış olması | İkinci yeniden gönderim engellenir. |
| Ceza kaydı bulunan aracın silinmesi | Araç silinmez; bağlı kayıt mesajı gösterilir. |
| Ödenmiş cezayı yeniden ödeme | Ödeme kontrolüyle engellenir. |

Kullanıcı için silme işlemi varsa bağlı araç, ceza, işlem geçmişi ve ödeme kayıtları da korunmalıdır. İşlem geçmişi olan cezalar fiziksel olarak silinmek yerine mevcut onay/ret akışı üzerinden değerlendirilir.

Sayfalama, sayfa başına gösterilecek kayıttan bir fazla kayıt okuyarak sonraki sayfanın varlığını belirler. Ekranda yalnızca istenen kayıt sayısı gösterilir.

## 11. Proje Organizasyonu ve Kapsam

Proje tek bir MVC uygulaması içinde controller, service, repository ve business rule sınıflarına ayrılmıştır. Entity Framework Core veri erişimini, Identity kullanıcı ve rol işlemlerini yönetir. Formlar request DTO'ları, ekranlar response DTO'ları kullanır. Ortak işlem sonuçları `Result<T>` ile taşınır.

Gerçek ödeme kuruluşu entegrasyonu, e-posta doğrulama ve şifre sıfırlama bu case çalışmasında entegre edilmemiştir.

## Geliştirici

Fatih Türker — [GitHub](https://github.com/fturker)
