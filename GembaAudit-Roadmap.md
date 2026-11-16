# GembaAudit — Saha Denetim Platformu (5S / İSG)
### Agent'lar için Katman Katman Uygulama Yol Haritası

> **Bu doküman bir AI agent'a (Google Antigravity, Claude Code, Cursor vb.) verilmek üzere yazılmıştır.**
> Agent, dokümanı **baştan sona okuyup** sonra fazları **sırayla** uygulamalıdır.
> Bir faz bitmeden sonrakine geçilmez. Her faz sonunda "Kabul Kriterleri" tamamen sağlanmalıdır.

---

## 0. Agent Çalışma Kuralları (ZORUNLU)

1. **Sırayla ilerle.** Faz N'nin kabul kriterleri geçmeden Faz N+1'e başlama.
2. **Commit'i yalnızca Conductor atar** (görev bazında, faz branch'inde, build+test+QA geçince; kullanıcıya sormadan). Format: Conventional Commits
   (`feat:`, `fix:`, `test:`, `docs:`, `chore:`, `refactor:`).
3. **Her faz sonunda** şunlar yeşil olmalı:
   - `dotnet build` uyarısız/hatasız
   - `dotnet test` tüm testler geçer
   - `docker compose up` ile sistem ayağa kalkar (Faz 6'dan sonra)
4. **Varsayım yapma.** Belirsiz bir nokta varsa dokümandaki "Kararlar" bölümüne bak. Orada yoksa en basit çözümü seç ve handoff raporunda Decisions altında bildir (Conductor `docs/DECISIONS.md`'ye yazar).
5. **Katman bağımlılık kuralı** (ihlal edilemez):
   `Domain` ← `Application` ← `Infrastructure` ← `Api`. Domain hiçbir projeye bağımlı olamaz.
   Application, Infrastructure'a referans veremez (sadece interface tanımlar).
6. **Gizli bilgi (secret) asla commit edilmez.** Connection string, JWT key, API key → `.env` / user-secrets / environment variable. Repo'da yalnızca `.env.example` bulunur.
7. **Her yeni özellik için test yaz** (Domain → unit, Application → unit, Api → integration).
8. **Kod dili İngilizce**, kullanıcıya dönen mesajlar ve README Türkçe/İngilizce olabilir (README İngilizce olmalı, portföy için).
9. **Her faz sonunda** `README.md` ve `docs/PROGRESS.md` güncellenir (ne yapıldı, nasıl çalıştırılır).
10. Takılırsan: hatayı tam metniyle handoff raporunun "Blockers" bölümüne yaz, ardından çalışmaya devam edilebilecek en yakın göreve geç.

---

## 1. Proje Özeti

**Amaç:** Fabrika/depo/ofis gibi sahalarda 5S ve İş Sağlığı Güvenliği denetimlerini dijitalleştirmek.
Denetçi sahada telefonla denetim yapar, uygunsuzluk (finding) açar, fotoğraf ekler; sorumluya düzeltici aksiyon atanır; yönetici onaylar ve dashboard'dan takip eder.

**Portföy hedefi:** Gerçek bir kurumsal problem + temiz mimari + cross-platform + offline + AI katmanı.

**Kullanıcı rolleri**

| Rol | Yetkiler |
|---|---|
| `Admin` | Her şey: kullanıcı, lokasyon, şablon yönetimi |
| `Manager` | Denetimleri onaylar, raporları görür, aksiyon atar |
| `Auditor` | Denetim yapar, uygunsuzluk açar |
| `Worker` | Kendisine atanan aksiyonları görür ve günceller |

---

## 2. Teknoloji Yığını ve Kararlar

| Konu | Karar |
|---|---|
| Runtime | **.NET 10 (LTS)**. Kurulu değilse .NET 9 kullan, `docs/DECISIONS.md`'ye yaz |
| Dil | C# (nullable enabled, `ImplicitUsings` enabled) |
| Mimari | **Clean Architecture** + CQRS-lite |
| Yazma tarafı | **EF Core** + Npgsql |
| Okuma/rapor tarafı | **Dapper** (dashboard ve liste sorguları) |
| Mediator | **MediatR 12.x** sabitle (12.x Apache lisanslı; 13+ ticari lisans gerektirir). Alternatif: kendi basit dispatcher'ı |
| Validasyon | FluentValidation (MediatR pipeline behavior ile) |
| Mapping | Elle mapping (AutoMapper kullanma) |
| DB | **PostgreSQL 16+** (Faz 8'de `pgvector` eklenecek) |
| Auth | ASP.NET Core Identity **kullanma**; JWT (access + refresh token) + kendi `User` entity'si, şifre için `PasswordHasher<T>` |
| API | ASP.NET Core Minimal API **veya** Controller (Controller seç, daha tanıdık) |
| Loglama | Serilog (console + file) |
| API dokümantasyon | OpenAPI + Scalar (veya Swagger UI) |
| Test | xUnit, FluentAssertions, NSubstitute, Testcontainers (PostgreSQL), `WebApplicationFactory` |
| UI (web) | **Blazor Web App** (Interactive WebAssembly) |
| UI (mobil/desktop) | **.NET MAUI Blazor Hybrid**, paylaşılan **Razor Class Library** |
| UI kütüphanesi | MudBlazor |
| Gerçek zamanlı | SignalR |
| Konteyner | Docker + docker-compose |
| CI/CD | GitHub Actions |
| Dosya depolama | Başlangıç: yerel disk (`IFileStorage` arkasında); sonra: S3 uyumlu (MinIO) |
| PDF | QuestPDF (Community lisansı) |

---

## 3. Klasör Yapısı (Hedef)

```
GembaAudit/
├── .github/workflows/ci.yml
├── docs/
│   ├── DECISIONS.md
│   ├── PROGRESS.md
│   ├── ARCHITECTURE.md
│   └── images/
├── src/
│   ├── GembaAudit.Domain/
│   │   ├── Common/            (BaseEntity, AuditableEntity, IDomainEvent, Result)
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── Events/
│   │   └── Exceptions/
│   ├── GembaAudit.Application/
│   │   ├── Common/            (Behaviors, Interfaces, Models, Exceptions)
│   │   ├── Auth/              (Commands/Queries/Dtos)
│   │   ├── Locations/
│   │   ├── Templates/
│   │   ├── Audits/
│   │   ├── Findings/
│   │   ├── Actions/
│   │   ├── Dashboard/
│   │   └── DependencyInjection.cs
│   ├── GembaAudit.Infrastructure/
│   │   ├── Persistence/       (AppDbContext, Configurations, Migrations, Seed)
│   │   ├── Queries/           (Dapper okuma servisleri)
│   │   ├── Identity/          (JwtTokenService, PasswordService)
│   │   ├── Files/             (LocalFileStorage)
│   │   ├── Reports/           (PdfReportService)
│   │   ├── Ai/                (Faz 8)
│   │   └── DependencyInjection.cs
│   ├── GembaAudit.Api/
│   │   ├── Controllers/
│   │   ├── Middleware/        (ExceptionHandling, CorrelationId)
│   │   ├── Hubs/              (NotificationHub)
│   │   └── Program.cs
│   ├── GembaAudit.Contracts/  (Request/Response DTO'ları; UI ve API paylaşır)
│   ├── GembaAudit.UI.Shared/  (Razor Class Library: sayfalar, bileşenler, API client)
│   ├── GembaAudit.Web/        (Blazor Web App)
│   └── GembaAudit.Mobile/     (MAUI Blazor Hybrid)
├── tests/
│   ├── GembaAudit.Domain.Tests/
│   ├── GembaAudit.Application.Tests/
│   └── GembaAudit.Api.IntegrationTests/
├── docker-compose.yml
├── Dockerfile (Api için, src/GembaAudit.Api/Dockerfile)
├── .env.example
├── .editorconfig
├── Directory.Build.props
├── GembaAudit.slnx          (.NET 10 varsayılanı; .sln de kabul)
└── README.md
```

---

## 4. Domain Modeli

### 4.1 Entity'ler

Tüm entity'ler `Guid Id`, `DateTime CreatedAtUtc`, `Guid? CreatedBy`, `DateTime? UpdatedAtUtc`, `Guid? UpdatedBy` taşır (`AuditableEntity`). Silme: **soft delete** (`IsDeleted`, `DeletedAtUtc`, `DeletedBy`).

**User**
`Email (unique)`, `PasswordHash`, `FullName`, `Role (enum)`, `IsActive`, `RefreshTokens (koleksiyon)`

**Location** (saha noktası, hiyerarşik)
`Name`, `Code (unique)`, `ParentId?`, `Description`, `QrToken (unique, rastgele)`

**AuditTemplate**
`Name`, `Type (FiveS | Safety)`, `Version`, `IsActive`, `Items (koleksiyon)`

**TemplateItem**
`TemplateId`, `Category` (5S için: Seiri, Seiton, Seiso, Seiketsu, Shitsuke; İSG için serbest metin), `Question`, `MaxScore (varsayılan 5)`, `Weight`, `Order`, `IsPhotoRequired`

**Audit**
`TemplateId`, `LocationId`, `AuditorId`, `Status (Draft|InProgress|Completed|Approved|Rejected)`, `StartedAtUtc`, `CompletedAtUtc`, `TotalScore`, `Percentage`, `ApprovedBy?`, `ApprovedAtUtc?`, `Notes`, `Answers (koleksiyon)`, `ClientId` (offline için istemcinin ürettiği Guid)

**AuditAnswer**
`AuditId`, `TemplateItemId`, `Score`, `Comment`, `Attachments`

**Finding** (uygunsuzluk)
`AuditId?`, `LocationId`, `Title`, `Description`, `Severity (Low|Medium|High|Critical)`, `Status (Open|Assigned|InProgress|Resolved|Verified|Rejected)`, `ReportedBy`, `AssignedTo?`, `DueDateUtc?`, `ResolvedAtUtc?`, `VerifiedBy?`, `Latitude?`, `Longitude?`, `ClientId`

**CorrectiveAction**
`FindingId`, `Description`, `AssignedTo`, `DueDateUtc`, `Status (Pending|Done)`, `CompletedAtUtc?`

**Attachment**
`OwnerType (Finding|AuditAnswer|Action)`, `OwnerId`, `FileName`, `StoragePath`, `ContentType`, `SizeBytes`

**Comment**
`FindingId`, `AuthorId`, `Text`

**Notification**
`UserId`, `Type`, `Title`, `Body`, `IsRead`, `PayloadJson`

### 4.2 Durum Geçiş Kuralları (Domain içinde metotla zorunlu kıl)

**Audit:** `Draft → InProgress → Completed → Approved | Rejected`; `Rejected → InProgress`.
**Finding:** `Open → Assigned → InProgress → Resolved → Verified`; `Resolved → InProgress` (Manager reddederse).
Geçersiz geçiş → `DomainException`. Bu kurallar **unit test** ile kanıtlanmalı.

### 4.3 Skor Hesabı

`TotalScore = Σ(Score × Weight)`, `Percentage = TotalScore / Σ(MaxScore × Weight) × 100`.
Hesap `Audit.Complete()` metodunda yapılır. Cevaplanmamış zorunlu madde varsa `Complete()` hata fırlatır.

---

## 5. API Sözleşmesi (Özet)

Tüm uçlar `/api/v1` altında. Hata formatı: **RFC 7807 ProblemDetails**. Liste uçları: `?page=1&pageSize=20&search=...`.

| Alan | Uç noktalar |
|---|---|
| Auth | `POST /auth/login`, `POST /auth/refresh`, `POST /auth/logout`, `GET /auth/me` |
| Users (Admin) | `GET/POST /users`, `PUT /users/{id}`, `PATCH /users/{id}/active` |
| Locations | `GET/POST /locations`, `GET/PUT/DELETE /locations/{id}`, `GET /locations/by-qr/{token}` |
| Templates | `GET/POST /templates`, `GET/PUT /templates/{id}`, `POST /templates/{id}/items` |
| Audits | `POST /audits`, `GET /audits`, `GET /audits/{id}`, `PUT /audits/{id}/answers`, `POST /audits/{id}/complete`, `POST /audits/{id}/approve`, `POST /audits/{id}/reject`, `GET /audits/{id}/report.pdf` |
| Findings | `POST /findings`, `GET /findings`, `GET /findings/{id}`, `POST /findings/{id}/assign`, `POST /findings/{id}/status`, `POST /findings/{id}/comments` |
| Attachments | `POST /attachments` (multipart), `GET /attachments/{id}` |
| Dashboard | `GET /dashboard/summary`, `GET /dashboard/trend?days=90`, `GET /dashboard/by-location` |
| Sync (Faz 7) | `POST /sync/push`, `GET /sync/pull?since=...` |
| Health | `GET /health` |

---

# FAZLAR

## FAZ 0 — Repo ve Çözüm İskeleti

**Görevler**
1. Boş `GembaAudit` klasörü oluştur, `git init`, `.gitignore` (dotnet şablonu), `.editorconfig`.
2. `dotnet new sln -n GembaAudit` (.NET 10 `.slnx` üretir; kabul edilir).
3. Projeleri **`src/` ve `tests/` altında** oluştur; kökte proje klasörü olmaz. Örn. `dotnet new classlib -n GembaAudit.Domain -o src/GembaAudit.Domain`. `Domain`, `Application`, `Infrastructure`, `Contracts` (classlib), `Api` (webapi), 3 test projesi (xunit, `tests/` altında). Hepsini `dotnet sln add` ile çözüme ekle.
4. Referansları §0'daki bağımlılık kuralına göre ekle.
5. `Directory.Build.props`: `Nullable=enable`, `TreatWarningsAsErrors=true`, `LangVersion=latest`. **Yalnızca property içerir**; `ProjectReference`/`PackageReference` içermez.
6. `docs/DECISIONS.md`, `docs/PROGRESS.md`, `README.md` iskeletlerini oluştur.
7. `.github/workflows/ci.yml`: restore, build, test (PostgreSQL service container ile).
8. NuGet güvenlik uyarıları (NU1901-NU1904) `TreatWarningsAsErrors` ile hata olur; paketleri yamalı sürüme yükselterek çöz (`dotnet nuget why`, `dotnet list package --vulnerable --include-transitive`). `NoWarn`/`NuGetAuditSuppress` kullanma.

**Kabul Kriterleri**
- [ ] `dotnet build` ve `dotnet test` (boş testlerle) yeşil
- [ ] CI GitHub'da çalışıyor, rozet README'de
- [ ] Domain projesinin `.csproj` dosyasında hiçbir `ProjectReference` yok
- [ ] Tüm projeler `src/` ve `tests/` altında; kökte proje klasörü yok
- [ ] `dotnet build` 0 uyarı; `dotnet list package --vulnerable --include-transitive` temiz
- [ ] `Directory.Build.props` yalnızca property içeriyor

---

## FAZ 1 — Domain Katmanı

**Görevler**
1. `BaseEntity`, `AuditableEntity`, `ISoftDeletable`, `DomainException`.
2. §4.1'deki tüm entity ve enum'ları yaz. Property setter'ları `private set`; değişiklikler metotlarla yapılır (rich domain model).
3. `Audit.Start()`, `Audit.SetAnswer(...)`, `Audit.Complete()`, `Audit.Approve(...)`, `Audit.Reject(...)` yaz.
4. `Finding.Assign(...)`, `Finding.StartWork()`, `Finding.Resolve()`, `Finding.Verify(...)`, `Finding.Reopen()` yaz.
5. Domain Events: `AuditCompleted`, `FindingAssigned`, `FindingResolved` (sadece sınıf tanımı).

**Testler (`Domain.Tests`)**
- Geçerli/geçersiz her durum geçişi
- Skor hesabı: en az 5 senaryo (tüm tam puan, sıfır, ağırlıklı, eksik zorunlu madde)

**Kabul Kriterleri**
- [ ] Domain testleri yeşil, kapsama ≥ %85
- [ ] Domain projesi hiçbir NuGet paketine bağımlı değil (test hariç)

---

## FAZ 2 — Application Katmanı (CQRS + MediatR)

**Görevler**
1. NuGet: `MediatR` (12.x), `FluentValidation`, `FluentValidation.DependencyInjectionExtensions`.
2. Arayüzler (`Common/Interfaces`): `IAppDbContext`, `ICurrentUser`, `IDateTimeProvider`, `IFileStorage`, `ITokenService`, `IPasswordHasher`.
3. **Pipeline behavior'lar** (bu sırayla): `LoggingBehavior` → `ValidationBehavior` → `TransactionBehavior` (yalnızca Command'lar için).
4. Her özellik klasöründe **Command/Query + Handler + Validator + Dto** (aynı klasörde, "vertical slice" gibi).
   - Faz 2'de yazılacaklar: Auth, Locations, Templates, Audits, Findings, Actions, Attachments.
5. Sayfalama için `PagedResult<T>` modeli.
6. `Result<T>` veya exception tabanlı hata yönetimi — **tek yaklaşım seç**, DECISIONS'a yaz (öneri: `NotFoundException`, `ForbiddenException`, `ValidationException` exception'ları + merkezi middleware).

**Kurallar**
- Handler'lar yalnızca `IAppDbContext` (yazma) ve okuma arayüzleri (`IDashboardQueries` vb.) üzerinden çalışır.
- Entity'ler dışarı **asla** dönmez, yalnızca DTO.
- Yetki kontrolü: `ICurrentUser.Role` ile handler içinde veya `AuthorizationBehavior` ile.

**Testler (`Application.Tests`)**
- Her handler için en az: başarılı senaryo + 1 hata senaryosu (InMemory yerine SQLite veya mock kullan; EF Core InMemory ilişkisel davranışı taklit etmez, dikkat).
- Validator testleri.

**Kabul Kriterleri**
- [ ] Tüm handler'lar testli
- [ ] Application projesi `Infrastructure` veya `Api`'ye referans vermiyor

---

## FAZ 3 — Infrastructure (Veritabanı)

**Görevler**
1. NuGet: `Npgsql.EntityFrameworkCore.PostgreSQL`, `Dapper`, `Microsoft.EntityFrameworkCore.Design`.
2. `AppDbContext`: `IAppDbContext`'i implemente et; audit alanlarını (`CreatedAtUtc/By`, `UpdatedAtUtc/By`, `DeletedAtUtc/By`) bir `SaveChangesInterceptor` ile doldur (sync ve async kaydı kapsar), `ISoftDeletable` silmelerini soft delete'e çevir, global query filter ekle.
3. Her entity için `IEntityTypeConfiguration<T>` (fluent API; attribute kullanma). Index'ler: `User.Email`, `Location.Code`, `Location.QrToken`, `Finding.Status`, `Finding.AssignedTo`, `Audit.LocationId + Status`.
4. İlk migration: `InitialCreate`. Migration'lar `Infrastructure/Persistence/Migrations` altında. Uygulanmış migration asla değiştirilmez; şema değişikliği = yeni migration. Zincir boş DB'de ve önceki migration durumundan yükseltmeyle test edilir; `dotnet ef migrations has-pending-model-changes` temiz olmalı.
5. **Seed:** Admin kullanıcı (şifre env'den), örnek 5S şablonu (5 kategori × 3 soru), 3 örnek lokasyon. Seed yalnızca Development ortamında ve idempotent çalışır.
6. Dapper okuma servisleri: `DashboardQueries` (summary, trend, by-location). Ham SQL parametreli olmalı, string birleştirme **yasak**.
7. `LocalFileStorage : IFileStorage` (`./storage` klasörü, dosya adı = Guid).
8. `AddInfrastructure(IConfiguration)` extension.

**Testler**
- Testcontainers ile gerçek PostgreSQL üzerinde: migration uygulanır, repository benzeri sorgular, soft delete filtresi, Dapper sorguları.

**Kabul Kriterleri**
- [ ] `dotnet ef database update` boş DB'de hatasız çalışıyor
- [ ] Testcontainers testleri yeşil

---

## FAZ 4 — API Katmanı

**Görevler**
1. `Program.cs`: Serilog, DI, JWT auth, authorization policy'leri (`AdminOnly`, `ManagerOrAbove`, `AuditorOrAbove`), CORS (config'den), rate limiting (login için), health checks (`/health`), OpenAPI/Scalar.
2. `ExceptionHandlingMiddleware`: Domain/Application exception'larını ProblemDetails'e (400/403/404/409) çevir, beklenmeyenleri 500 + log.
3. `CorrelationIdMiddleware`: `X-Correlation-Id` header.
4. Controller'lar §5'e uygun, **ince** olmalı (sadece `IMediator.Send`).
5. JWT: access token 15 dk, refresh token 7 gün, DB'de hash'li saklanır, rotation uygulanır.
6. `ICurrentUser` implementasyonu (`HttpContext` claim'lerinden).
7. Dosya yükleme: maksimum 10 MB; JPEG/PNG/WebP/PDF. Yalnızca `Content-Type`'a güvenme: **dosya imzasını (magic bytes) doğrula**; saklama adını sunucuda Guid ile üret; path traversal'ı engelle; dosyaları web root dışında sakla; yüklenen dosyayı asla çalıştırma; `nosniff` ile servis et; görsellerde boyut limiti ve decode doğrulaması, PDF'te yapı/sayfa limiti uygula.

**Testler (`Api.IntegrationTests`)**
- `WebApplicationFactory` + Testcontainers.
- Senaryolar: login → denetim oluştur → cevap kaydet → tamamla → manager onayı (uçtan uca), yetkisiz erişim 401/403, validasyon hatası 400, olmayan kayıt 404.

**Kabul Kriterleri**
- [ ] Uçtan uca "denetim yaşam döngüsü" integration testi yeşil
- [ ] Scalar/Swagger'dan tüm uçlar görünür ve denenebilir
- [ ] Admin dışı kullanıcı `/users` uçlarına 403 alıyor

---

## FAZ 5 — Web UI (Blazor + MudBlazor)

**Görevler**
1. `GembaAudit.UI.Shared` Razor Class Library: sayfalar ve bileşenler **burada** olur (MAUI ile paylaşılacak).
2. `GembaAudit.Web` Blazor Web App: yalnızca host; sayfalar UI.Shared'dan gelir.
3. Typed `ApiClient` (HttpClient + `DelegatingHandler` ile token ekleme ve 401'de refresh).
4. Auth state: `AuthenticationStateProvider`, token'ı güvenli sakla (web: memory + httpOnly cookie tercih edilir; basit başlangıç: `ProtectedLocalStorage`).
5. Sayfalar:
   - `/login`
   - `/` Dashboard (kartlar: açık uygunsuzluk, bu ay denetim sayısı, ortalama skor; grafik: skor trendi)
   - `/audits` liste + filtre, `/audits/new`, `/audits/{id}` (denetim formu: madde madde puan + yorum + fotoğraf)
   - `/findings` liste (durum/ciddiyet filtresi), `/findings/{id}` detay (yorumlar, durum geçmişi, aksiyonlar)
   - `/locations`, `/templates`, `/users` (Admin)
   - `/my-actions` (Worker için)
6. Boş durum, yükleniyor ve hata durumları her sayfada ele alınır.
7. Responsive tasarım (telefon genişliğinde kullanılabilir).
8. Koyu/açık tema.

**Kabul Kriterleri**
- [ ] Tarayıcıdan: giriş → denetim yap → uygunsuzluk aç → ata → çöz → onayla akışı çalışıyor
- [ ] Rol bazlı menü gizleme çalışıyor (yalnızca UI kolaylığı; asıl yetki API'de)
- [ ] 375 px genişlikte yatay scroll yok

---

## FAZ 6 — Docker, CI/CD ve Canlıya Alma

**Görevler**
1. `src/GembaAudit.Api/Dockerfile` (multi-stage, non-root kullanıcı, `HEALTHCHECK`).
2. `GembaAudit.Web` için Dockerfile (veya Api içinde host et; DECISIONS'a yaz).
3. `docker-compose.yml`: `postgres` (volume'lu), `api`, `web`, (opsiyonel `pgadmin`). Env değişkenleri `.env` dosyasından.
4. `.env.example` tüm değişkenleri açıklamalı içerir.
5. Migration'ı otomatik uygulama: yalnızca Development'ta açılır; prod için ayrı komut belgele.
6. GitHub Actions: `ci.yml` (build+test), `docker.yml` (main'e push'ta image build, GHCR'a push).
7. Canlı demo: Railway / Render / Fly.io / Azure (biri seçilir). Demo ortamına örnek verili, **salt-okunur demo kullanıcısı** eklenir.
8. README'ye canlı demo linki, test kullanıcısı bilgisi, ekran görüntüleri eklenir.

**Kabul Kriterleri**
- [ ] Temiz bir makinede `cp .env.example .env && docker compose up --build` ile tüm sistem çalışıyor
- [ ] CI ve Docker workflow'ları yeşil
- [ ] Canlı demo erişilebilir

---

## FAZ 7 — Cross-Platform (MAUI Blazor Hybrid) + Offline

**Görevler**
1. `GembaAudit.Mobile` MAUI Blazor Hybrid projesi; `UI.Shared`'ı referans alır. Hedef: Android (öncelik), Windows; iOS opsiyonel (Mac gerekir).
2. Platform servisleri için arayüzler `UI.Shared`'da, implementasyon MAUI'de:
   `ICameraService` (fotoğraf çek/seç), `IQrScanner` (QR okut), `ILocationService` (GPS), `ISecureStorage`.
3. **QR akışı:** QR okut → `GET /locations/by-qr/{token}` → "Bu lokasyonda denetim başlat" ekranı.
4. **Offline-first:**
   - Cihazda SQLite (`sqlite-net-pcl` veya EF Core SQLite).
   - Yazma işlemleri yerel `OutboxItem` tablosuna yazılır (`Id`, `Type`, `PayloadJson`, `CreatedAtUtc`, `Status`).
   - Bağlantı gelince `SyncService` outbox'ı `POST /sync/push` ile gönderir; çakışmada **"last-write-wins + sunucu zaman damgası"** uygulanır (DECISIONS'a yaz).
   - İstemci üretimli `ClientId` (Guid) ile idempotency: aynı kayıt iki kez gönderilse bile tek kayıt oluşur.
   - `GET /sync/pull?since=` ile şablonlar, lokasyonlar, atanmış aksiyonlar indirilir.
   - Fotoğraflar kuyruğa alınır, bağlantıda yüklenir, küçültülür (max 1600 px).
5. UI'da bağlantı durumu göstergesi ve "bekleyen senkronizasyon: N" rozeti.
6. SignalR ile gerçek zamanlı bildirim (yeni atama, durum değişikliği). Mobilde yerel bildirim opsiyonel.
7. `Api`'ye `SyncController` ve ilgili handler'lar eklenir.

**Testler**
- Sync idempotency integration testi (aynı payload 2 kez → 1 kayıt)
- Outbox sıralama ve retry unit testleri

**Kabul Kriterleri**
- [ ] Android emülatör/cihazda uçak modunda denetim doldurulabiliyor, bağlantı gelince sunucuda görünüyor
- [ ] QR okutma çalışıyor
- [ ] Aynı kayıt çift gönderimde duplicate oluşturmuyor

---

## FAZ 8 — AI Katmanı

> Tüm AI çağrıları **arayüz arkasında** olmalı; sağlayıcı değiştirilebilir (Claude API, Azure OpenAI, OpenAI). API key'ler yalnızca sunucuda, asla istemcide.

**8.1 Fotoğraftan uygunsuzluk önerisi**
- `IFindingSuggestionService.SuggestAsync(imageBytes, locationContext)` → `{ Title, Description, Severity, Category, Confidence }`
- Yalnızca **öneri** döner; kullanıcı onaylamadan kayıt oluşmaz (human-in-the-loop).
- Uç: `POST /ai/suggest-finding` (multipart). Rate limit ve günlük kota uygula.
- Çıktı yapılandırılmış JSON olmalı; parse hatasında güvenli fallback.

**8.2 Özetleme**
- `ISummaryService`: bir denetimin veya aylık uygunsuzlukların yönetici özeti.
- Uç: `GET /ai/audits/{id}/summary`.

**8.3 RAG ile doğal dil sorgusu**
- PostgreSQL'e `pgvector` eklentisini kur (compose'da `pgvector/pgvector` imajı).
- `FindingEmbedding` tablosu: `FindingId`, `Embedding vector(1536)` (boyut seçilen modele göre).
- Finding oluşturulunca/güncellenince arka plan işi embedding üretir (`IHostedService` + kanal/queue).
- `POST /ai/ask` : soru → embedding → en benzer N kayıt → LLM'e bağlam olarak ver → kaynak referanslı cevap.
- Cevapta kullanılan Finding ID'leri döndürülür (kaynak gösterimi).

**8.4 Güvenlik ve maliyet**
- Prompt injection önlemi: kullanıcı metni sistem talimatından ayrı mesajda, çıktı şema ile doğrulanır.
- Kişisel veri (e-posta, isim) LLM'e gönderilmeden önce maskelenir.
- Tüm AI çağrıları loglanır (süre, token, hata), kota aşımında 429.

**Testler**
- AI servisleri mock'lanır; parse, fallback ve kota davranışı test edilir.
- Gerçek sağlayıcıya giden test yalnızca `[Trait("Category","Live")]` ile, CI'da çalışmaz.

**Kabul Kriterleri**
- [ ] Fotoğraftan öneri UI'da "Öneriyi kullan" butonuyla forma doldurulabiliyor
- [ ] RAG sorusu kaynak finding'leri listeleyerek cevap veriyor
- [ ] API key yoksa uygulama çökmez, AI özellikleri "devre dışı" olarak gizlenir

---

## FAZ 9 — Klasik ML (Opsiyonel, Data Science Köprüsü)

1. `ml/` klasörü, Python (pandas, scikit-learn) ile Jupyter notebook:
   uygunsuzluk geri-açılma (reopen) riski veya gecikme tahmini.
2. Sentetik veri üretici script (gerçek veri yoksa) — README'de bunu açıkça belirt.
3. Modeli ONNX'e dışa aktar, `Infrastructure`'da `Microsoft.ML.OnnxRuntime` ile yükle: `IRiskPredictor`.
4. Dashboard'a "Yüksek riskli açık uygunsuzluklar" kartı ekle.

**Kabul Kriterleri**
- [ ] Notebook baştan sona çalışıyor, metrikler README'de
- [ ] .NET içinden tahmin dönebiliyor

---

## FAZ 10 — Portföy Cilası

- [ ] README (İngilizce): proje özeti, ekran görüntüleri/GIF, mimari diyagram (Mermaid), teknoloji yığını, çalıştırma adımları, canlı demo, "Neler öğrendim/kararlar" bölümü
- [ ] `docs/ARCHITECTURE.md`: katman diyagramı, sync akışı, auth akışı (Mermaid)
- [ ] Test kapsamı rozeti, CI rozeti
- [ ] `LICENSE` (MIT)
- [ ] GitHub repo: açıklama, topic'ler (`dotnet`, `blazor`, `maui`, `clean-architecture`, `ai`, `rag`), pinned repo
- [ ] 1–2 dk demo videosu veya GIF (denetim akışı + offline + AI önerisi)
- [ ] Release `v1.0.0` ve changelog

---

## 6. Faz Bazlı Özet Takvim (haftada ~20 saat için tahmini)

| Faz | Süre |
|---|---|
| 0–1 İskelet + Domain | 1 hafta |
| 2 Application | 1,5 hafta |
| 3–4 Infrastructure + API | 2 hafta |
| 5 Web UI | 2 hafta |
| 6 Docker/CI/Deploy | 0,5 hafta |
| 7 MAUI + Offline | 3 hafta |
| 8 AI | 2 hafta |
| 9–10 ML + Cila | 1,5 hafta |
| **Toplam** | **~13–14 hafta** |

> İlk **canlıya çıkış** Faz 6 sonunda olmalıdır (~7. hafta). Sonraki fazlar mevcut ürünü genişletir; ürün her zaman çalışır durumda kalır.

---

## 7. Genel Tanım: "Bitti" (Definition of Done)

Bir görev ancak şunlar sağlanırsa bitmiş sayılır:
1. Kod derleniyor, uyarı yok.
2. Anlamlı testler yazıldı ve geçiyor.
3. Katman bağımlılık kuralı ihlal edilmedi.
4. Secret içermiyor.
5. İlgili dokümantasyon güncellendi.
6. Anlamlı bir commit mesajıyla commit'lendi.

---

## 8. Agent İçin Başlangıç Komutu (Kopyala-Yapıştır)

```
Bu repodaki GembaAudit-Roadmap.md dosyasını baştan sona oku.
§0'daki kurallara kesinlikle uy. FAZ 0'dan başla, her faz sonunda kabul
kriterlerini tek tek kontrol et ve docs/PROGRESS.md'yi güncelle.
Bir fazı bitirdiğinde dur, özet ver ve benim onayımı bekle.
```
