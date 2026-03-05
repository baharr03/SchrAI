# 🐱 SchrAI - Schrödinger Tarzı İçecek Öneri Sistemi

SchrAI, **Schrödinger'in kedisi** mantığıyla çalışan bir **olasılık katmanı** ve **bulanık mantık (fuzzy logic)** kullanarak içecek önerisi yapan hibrit bir sistemdir.

## 🎯 Özellikler

- **🔵 Olasılık Katmanı (SchrAI)**: Sistem gözlemlenmeden önce birden fazla durumda (süperpozisyon) bulunur
- **🟢 Fuzzy Logic Katmanı**: Sıcaklık ve saate göre üyelik dereceleri hesaplar
- **🧠 Öğrenme**: Kullanıcı geri bildirimiyle olasılıkları günceller
- **📊 Hibrit Model**: Fuzzy dereceleriyle olasılıkları ağırlıklandırır
- **☕🍹 Çeşitli İçecekler**: 20 farklı içecek (sıcak, soğuk, ferahlatıcı, enerji veren)

## ☕ İçecek Listesi

| Kategori | İçecekler |
|----------|-----------|
| **Sıcak** | Kahve, Çay, Sıcak Çikolata, Salep, Bitki Çayı, Latte, Espresso, Türk Kahvesi, Chai Latte, Kakao |
| **Soğuk** | Su, Limonata, Ayran, Buzlu Kahve, Soğuk Çay, Meyve Suyu, Smoothie, Soda, Şalgam, Buzlu Çay |

## 🚀 Nasıl Çalıştırılır?

### Gereksinimler
- .NET 8.0 SDK veya üzeri

### Çalıştırma
```bash
dotnet run
```

## 📖 Nasıl Çalışır?

1. **Fuzzy Katman**: Hava sıcaklığı ve saate göre her içecek için üyelik derecesi hesaplanır
   - Örnek: 28°C → "Sıcak hava" üyelik = 0.8
   - Örnek: 14:00 → "Öğle" üyelik = 0.9

2. **Olasılık Katmanı**: Başlangıç olasılıkları fuzzy dereceleriyle çarpılır
   - `P_new[i] = P_old[i] × FuzzyDerece[i]`
   - Sonra normalize edilir (toplam = 1.0)

3. **Gözlem**: Ağırlıklandırılmış olasılıklara göre rastgele bir içecek seçilir (süperpozisyon çökmesi)

4. **Öğrenme**: Kullanıcı geri bildirimiyle olasılıklar güncellenir

## 🎮 Kullanım Örneği

```
🌡 Hava sıcaklığı (°C): 28
🕐 Saat (0-23): 14

🟢 Fuzzy Katman (Üyelik Dereceleri):
   Sıcaklık: 28°C
   Saat: 14:00
   Su: 0.80
   Limonata: 0.75
   Buzlu Çay: 0.72
   ...

🎲 Sistem gözlemlendi!
💡 Önerilen içecek: LİMONATA

Beğendin mi? (e/h/çıkış): e
```

## 📁 Proje Yapısı

- `Superposition.cs` - Olasılık katmanı (SchrAI), 20 içecek tanımı
- `FuzzyLogic.cs` - Bulanık mantık üyelik fonksiyonları
- `HybridModel.cs` - İki katmanı birleştiren hibrit model
- `Program.cs` - Ana uygulama ve kullanıcı etkileşimi

## 🔬 Matematik

### Üçgen Üyelik Fonksiyonu
```
        /\
       /  \
      /    \
     /      \
    /________\
   a    b    c
```

### Ağırlıklandırma
```
P_new[i] = P_old[i] × FuzzyDegree[i]
Sonra normalize et: P_new[i] = P_new[i] / Σ(P_new)
```
