using SchrAI;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
Console.WriteLine("║  🐱 SchrAI - Schrödinger Tarzı İçecek Öneri Sistemi     ║");
Console.WriteLine("║  Olasılık + Bulanık Mantık Hibrit Model                 ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
Console.WriteLine();

var model = new HybridModel();

while (true)
{
    Console.WriteLine("─────────────────────────────────────────────────────────");
    
    // Girdi al
    double sicaklik = GirdiAl("🌡 Hava sıcaklığı (°C)", -10, 50, 22);
    int saat = (int)GirdiAl("🕐 Saat (0-23)", 0, 23, 14);

    Console.WriteLine();
    Console.WriteLine("🟢 Fuzzy Katman (Üyelik Dereceleri):");
    Console.WriteLine($"   Sıcaklık: {sicaklik}°C");
    Console.WriteLine($"   Saat: {saat:D2}:00");
    
    double[] fuzzy = FuzzyLogic.HesaplaUyelikDereceleri(sicaklik, saat);
    
    // En yüksek 5 fuzzy değerini göster
    var sirali = fuzzy
        .Select((d, i) => (derece: d, icecek: Superposition.Icecekler[i]))
        .OrderByDescending(x => x.derece)
        .Take(5);
    
    foreach (var (derece, icecek) in sirali)
        Console.WriteLine($"   {icecek}: {derece:F2}");
    
    Console.WriteLine();
    Console.WriteLine("🎲 Sistem gözlemlendi!");
    
    var (index, secilen) = model.OneriAl(sicaklik, saat);
    
    Console.WriteLine($"💡 Önerilen içecek: {secilen}");
    Console.WriteLine();

    // Geri bildirim
    Console.Write("Beğendin mi? (e/h/çıkış): ");
    var cevap = Console.ReadLine()?.Trim().ToLowerInvariant();

    if (cevap == "çıkış" || cevap == "cikis" || cevap == "q")
    {
        Console.WriteLine("🐱 Hoşça kal!");
        break;
    }

    if (cevap == "e" || cevap == "evet")
        model.GeriBildirim(index, true);
    else if (cevap == "h" || cevap == "hayır" || cevap == "hayir")
        model.GeriBildirim(index, false);

    Console.WriteLine();
}

static double GirdiAl(string etiket, double min, double max, double varsayilan)
{
    while (true)
    {
        Console.Write($"{etiket} [{varsayilan}]: ");
        var giris = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrEmpty(giris))
            return varsayilan;

        if (double.TryParse(giris.Replace(',', '.'), 
                System.Globalization.NumberStyles.Any, 
                System.Globalization.CultureInfo.InvariantCulture, 
                out var deger))
        {
            if (deger >= min && deger <= max)
                return deger;
        }

        Console.WriteLine($"   Lütfen {min}-{max} arası bir sayı girin.");
    }
}
