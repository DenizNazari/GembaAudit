# Antigravity ile çalışma rehberi (Türkçe, kişisel kullanım)

## Kurulum
1. Bu paketi proje kök klasörüne aç (`AGENTS.md`, `.agents/`, `docs/` kökte olmalı).
2. Antigravity'de klasörü workspace olarak aç.
3. Skill'ler veya `/` komutları görünmezse `.agents` klasörünü `.agent` olarak yeniden adlandırıp tekrar dene (eski sürümler tekil adı kullanır).
4. Terminal PowerShell olmalı. Bash/WSL gerektiren komut yok.

## Günlük döngü
1. **Conductor sohbeti** aç (Argon (high) / en güçlü model, en yüksek reasoning). `/start-phase` yaz.
2. Conductor `docs/TASK_BRIEF.md`'ye görevleri yazar ve sana hangi rolü, hangi tier'la açacağını söyler.
3. Her görev için **yeni bir sohbet** aç, tier'a uygun modeli seç, Conductor'ın verdiği "STARTER PROMPT"u yapıştır.
4. Rol işini bitirince `/write-handoff` yazar; `docs/handoffs/` altında dosya oluşur.
5. Conductor sohbetine dön: "P0-1 bitti" de. Conductor kendisi build/test çalıştırır, `/commit-task` ile commit atar.
6. Faz görevleri bitince sırasıyla: QA → Docs → Reviewer (`/review-phase`) → Gate (`/close-phase`).
7. Conductor'da `/close-phase`. Tablo ve review sonucu seni onaya getirir. Onay vermeden sonraki faza geçilmez.

## Tier'lar
| Tier | Seviye | Modelde ne seç |
|---|---|---|
| Hydrogen | low | En hızlı/ucuz model, düşük reasoning |
| Argon | medium | Dengeli model, orta reasoning |
| Argon | high | En güçlü model, yüksek reasoning |

Yükseltme (Escalation) kuralı:
- `Hydrogen (low)` iki kez başarısız olursa → `Argon (medium)`.
- `Argon (medium)` iki kez başarısız olursa → `Argon (high)`.
- `Argon (high)` iki kez başarısız olursa → kullanıcıya eskale edilir.

## Raporlara güvenme, kanıta bak
- Bir faz "tamam" denince `docs/reviews/phase-N.md` içinde `VERDICT: APPROVED` ve alıntılanmış komut çıktıları var mı bak.
- `dotnet build` çıktısında 0 uyarı, `dotnet test` toplamı tutarlı mı kontrol et.
- Entity sınıfları boş mu, `Program.cs` hâlâ weatherforecast mi, kendin aç bak.

## Sık sorunlar
- Rol başka rolün klasörüne dokunduysa: Conductor commit etmez, ihlal olarak bildirir.
- Güvenlik uyarısı (NU1903 vb.) çıkarsa susturma, paketi yamalı sürüme yükselt.
- Aynı klasörde iki sohbeti paralel çalıştırma; yalnızca yolları ayrı görevleri paralel yap.
