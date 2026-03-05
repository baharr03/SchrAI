namespace SchrAI;

/// <summary>
/// Bulanık mantık katmanı: Sıcaklık ve saate göre üçgen üyelik fonksiyonları ile
/// her içecek için üyelik derecesi hesaplar.
/// </summary>
public static class FuzzyLogic
{
    /// <summary>Üçgen üyelik fonksiyonu: a (sol), b (tepe), c (sağ)</summary>
    /// <para>       /\</para>
    /// <para>      /  \</para>
    /// <para>     /    \</para>
    /// <para>    /______\</para>
    /// <para>   a   b   c</para>
    public static double UcgenUyelik(double x, double a, double b, double c)
    {
        if (x <= a || x >= c) return 0;
        if (x == b) return 1;
        if (x < b) return (x - a) / (b - a);
        return (c - x) / (c - b);
    }

    /// <summary>Sıcak hava üyeliği (yazlık/ferahlatıcı içecekler için)</summary>
    public static double SicakHavaUyelik(double temp) =>
        UcgenUyelik(temp, 18, 30, 45);

    /// <summary>Soğuk hava üyeliği (ısınma içecekleri için)</summary>
    public static double SogukHavaUyelik(double temp) =>
        UcgenUyelik(temp, -10, 0, 15);

    /// <summary>Ilık hava üyeliği (neutral içecekler)</summary>
    public static double IlikHavaUyelik(double temp) =>
        UcgenUyelik(temp, 10, 20, 28);

    /// <summary>Sabahtan öğlene kadar (uyarıcı / sıcak içecekler)</summary>
    public static double SabahUyelik(int saat) =>
        UcgenUyelik(saat, 5, 9, 12);

    /// <summary>Öğleden akşama (gün ortası)</summary>
    public static double OgleUyelik(int saat) =>
        UcgenUyelik(saat, 10, 14, 18);

    /// <summary>Akşam (rahatlatıcı içecekler)</summary>
    public static double AksamUyelik(int saat) =>
        UcgenUyelik(saat, 16, 20, 24);

    /// <summary>Gece geç (yatıştırıcı, kafeinsiz)</summary>
    public static double GecUyelik(int saat) =>
        Math.Max(UcgenUyelik(saat, 21, 23, 24), UcgenUyelik(saat, 0, 1, 3));

    /// <summary>Tüm içecekler için sıcaklık ve saate göre üyelik dereceleri hesapla</summary>
    public static double[] HesaplaUyelikDereceleri(double sicaklik, int saat)
    {
        double sicak = SicakHavaUyelik(sicaklik);
        double soguk = SogukHavaUyelik(sicaklik);
        double ilik = IlikHavaUyelik(sicaklik);

        double sabah = SabahUyelik(saat);
        double ogle = OgleUyelik(saat);
        double aksam = AksamUyelik(saat);
        double gec = GecUyelik(saat);

        var dereceler = new double[Superposition.Icecekler.Length];

        // Kahve - sabah/öğle, ılık-soğuk hava
        dereceler[0] = Math.Max(ilik, soguk) * (sabah + ogle) * 0.6 + 0.2;

        // Çay - her zaman, ılık
        dereceler[1] = ilik * 0.7 + sicak * 0.3 + 0.3;

        // Su - sıcak hava, her saat
        dereceler[2] = sicak * 0.8 + 0.5; // Her zaman temel, sıcakta daha fazla

        // Sıcak Çikolata - soğuk hava, akşam
        dereceler[3] = soguk * aksam * 0.9 + 0.1;

        // Salep - soğuk hava, kış
        dereceler[4] = soguk * 0.9 + ilik * 0.3 + 0.1;

        // Limonata - sıcak hava, öğle
        dereceler[5] = sicak * ogle * 0.95 + 0.2;

        // Ayran - sıcak hava, öğle/akşam
        dereceler[6] = sicak * (ogle + aksam) * 0.5 + 0.3;

        // Buzlu Kahve - sıcak hava, öğle
        dereceler[7] = sicak * ogle * 0.9 + 0.2;

        // Soğuk Çay - sıcak hava
        dereceler[8] = sicak * 0.85 + ilik * 0.2 + 0.15;

        // Meyve Suyu - sıcak, öğle
        dereceler[9] = sicak * ogle * 0.8 + 0.25;

        // Smoothie - sıcak, öğle
        dereceler[10] = sicak * ogle * 0.85 + 0.2;

        // Bitki Çayı - akşam, gece, ılık-soğuk
        dereceler[11] = (aksam + gec) * (ilik + soguk) * 0.5 + 0.25;

        // Soda - sıcak, öğle
        dereceler[12] = sicak * ogle * 0.7 + 0.3;

        // Latte - sabah, ılık-soğuk
        dereceler[13] = (sabah + ogle) * (ilik + soguk) * 0.6 + 0.2;

        // Espresso - sabah, her zaman
        dereceler[14] = sabah * 0.9 + ogle * 0.5 + 0.15;

        // Türk Kahvesi - sabah, öğle, ılık
        dereceler[15] = (sabah + ogle) * ilik * 0.7 + 0.2;

        // Chai Latte - akşam, ılık-soğuk
        dereceler[16] = (aksam + sabah) * (ilik + soguk) * 0.6 + 0.2;

        // Kakao - soğuk hava, akşam
        dereceler[17] = soguk * aksam * 0.85 + 0.15;

        // Şalgam - sıcak, öğle/akşam
        dereceler[18] = sicak * (ogle + aksam) * 0.6 + 0.25;

        // Buzlu Çay - sıcak, öğle
        dereceler[19] = sicak * ogle * 0.9 + 0.2;

        // Minimum 0.1 ile sıfır tamamen elenmesin
        for (int i = 0; i < dereceler.Length; i++)
            dereceler[i] = Math.Clamp(dereceler[i], 0.1, 1.0);

        return dereceler;
    }
}
