# CodeNet.Messaging - Mimari ve Kod İncelemesi

Bu doküman, `CodeNet.Messaging` kütüphanesinin mevcut tasarımını ve kod kalitesini değerlendirir.

## Güçlü Yönler

- **Compile/Build ayrımı doğru bir performans yaklaşımıdır.** Template parsing maliyetini bir kez ödeyip yeniden kullanım imkanı sağlıyor.
- **Alt builder yapısı** (`LoopBuilder`, `IfBuilder`, `FuncBuilder`) ile sorumluluklar ayrılmış.
- **GeneratedRegex kullanımı** regex maliyetini azaltır ve AOT uyumluluğu açısından artıdır.

## Mimari Bulgular

### 1) Public API yüzeyinde persistence için risk
`MessageBuilder` nesnesinin iç durumları (`Parameters`, `LoopBuilders`, `FuncBuilders`, `IfBuilders`) `internal` ve settable. Aynı assembly içinde beklenmedik mutasyonlara açık.

**Öneri:**
- Compile sonrası immutable modele geçin (`IReadOnlyList<>`, private set/init).
- DB persistence için ayrı bir DTO/contract (`CompiledTemplateModel`) tanımlayın; runtime builder ile persistence modelini ayırın.

### 2) Reflection tabanlı fonksiyon çözümleme kırılgan
`FuncBuilder.Build` içinde method overload çözümü runtime reflection ile ve parametre tiplerinin birebir eşleşmesine bağlı.

**Risk:**
- Sayısal tip dönüşümlerinde (`int`/`double`/`decimal`) method bulunamama.
- Hata mesajı yerine sessiz boş string dönme.

**Öneri:**
- Fonksiyonları bir registry/dictionary ile kaydedin (`Dictionary<string, Func<object?[], string>>`).
- Method bulunamazsa domain-specific exception fırlatın.

### 3) Template dili nested bloklarda regex ile sınırlı
`$each` ve `$if` pattern'leri body kısmında `[^}]+` kullandığı için iç içe bloklar doğal olarak desteklenmiyor.

**Öneri:**
- Uzun vadede tokenization + parser (stack tabanlı) yaklaşımına geçin.
- Kısa vadede README'de “nested control block desteklenmez” kuralını açıkça belgeleyin.

## Kod Seviyesi Bulgular

### 1) Sayısal parse kültür bağımlı
`double.Parse(number)` kültüre duyarlı. Sunucu kültürü `,` ise `12.34` parse hatası verebilir.

**Öneri:** `double.Parse(number, CultureInfo.InvariantCulture)` kullanın.

### 2) If numeric karşılaştırma bug riski
`IfBuilder` içinde değerler `decimal`e doğrudan cast ediliyor:
- `(decimal)ParamLeft.Value`

Bu, boxed `double` değerlerde `InvalidCastException` üretebilir.

**Öneri:**
- `Convert.ToDecimal(value, CultureInfo.InvariantCulture)` ile normalize edin.
- Destekli numeric türleri `IConvertible` üzerinden yönetin.

### 3) Hatalı exception mesaj eşlemesi
`IfBuilder.Compile` içinde bilinmeyen operator durumunda `LoopItemParam` mesajı atılıyor; semantik olarak yanlış.

**Öneri:** ayrı bir `InvalidOperator` mesajı ekleyin.

### 4) Regex karakter aralığı hatalı
`[A-z]` aralığı ASCII'de beklenmeyen karakterleri de kapsar (`[`, `\`, `]`, `^`, `_`, `` ` ``).

**Öneri:** `[A-Za-z]` kullanılmalı.

### 5) GetValue null güvenliği
`ObjectExtension.GetValue(this object obj, string property)` içinde `obj` null kontrolü yok.

**Öneri:**
- `this object? obj` ve erken null return.
- Dynamic dictionary erişiminde key yoksa `TryGetValue` kullanın.

### 6) String.Replace ile tüm eşleşmeleri toplu değiştirme
`Build` sırasında aynı `builder.Content` birden fazla yerde geçiyorsa global replace çalışır. Bu çoğu senaryoda doğru olsa da, aynı içeriğin farklı context'lerde farklı değer üretmesi gereken edge case'lerde sürpriz yaratabilir.

**Öneri:** parse aşamasında token bazlı konum bilgisi tutulup deterministik render yapılmalı.

## Test Stratejisi Eksikleri

Bu package için ayrı test projesi görünmüyor.

**Eklenmesi önerilen testler:**
1. Culture invariant number parse testleri.
2. `if` numeric karşılaştırmada `int/double/decimal` kombinasyonları.
3. Nested block negatif testleri (beklenen hata/davranış).
4. Function resolution başarısız olduğunda exception testleri.
5. Serialize/deserialize sonrası aynı template'in eşdeğer çıktıyı üretmesi (snapshot test).

## Yol Haritası (Önceliklendirilmiş)

1. **Kritik doğruluk:** numeric cast/parse ve exception düzeltmeleri.
2. **Operasyonel güven:** function resolution hatalarını görünür hale getirme.
3. **Bakım kolaylığı:** immutable compile çıktısı + persistence DTO ayrımı.
4. **Evrimsel mimari:** regex parser'dan AST tabanlı parser'a geçiş.
