namespace SchrAI;

/// <summary>
/// SchrAI - Schrödinger'in kedisi mantığıyla çalışan olasılık katmanı.
/// Sistem gözlemlenmeden önce birden fazla durumda (süperpozisyon) bulunur.
/// </summary>
public class Superposition
{
    private readonly Random _random = new();
    private double[] _probabilities;
    
    /// <summary>Tüm içecekler (çeşitli: sıcak, soğuk, ferahlatıcı, enerji veren)</summary>
    public static string[] Icecekler { get; } =
    [
        "Kahve",
        "Çay",
        "Su",
        "Sıcak Çikolata",
        "Salep",
        "Limonata",
        "Ayran",
        "Buzlu Kahve",
        "Soğuk Çay",
        "Meyve Suyu",
        "Smoothie",
        "Bitki Çayı",
        "Soda",
        "Latte",
        "Espresso",
        "Türk Kahvesi",
        "Chai Latte",
        "Kakao",
        "Şalgam",
        "Buzlu Çay"
    ];

    public int Count => _probabilities.Length;
    public double GetProbability(int i) => _probabilities[i];

    public Superposition()
    {
        _probabilities = new double[Icecekler.Length];
        ResetProbabilities();
    }

    /// <summary>Başlangıç olasılıklarını eşit dağıt (gözlem öncesi süperpozisyon)</summary>
    public void ResetProbabilities()
    {
        double uniform = 1.0 / Icecekler.Length;
        for (int i = 0; i < _probabilities.Length; i++)
            _probabilities[i] = uniform;
    }

    /// <summary>Fuzzy dereceleriyle olasılıkları ağırlıklandır: P_new[i] = P_old[i] × FuzzyDegree[i]</summary>
    public void Agirliklandir(double[] fuzzyDereceleri)
    {
        if (fuzzyDereceleri.Length != _probabilities.Length)
            throw new ArgumentException("Fuzzy dereceleri sayısı içecek sayısıyla eşleşmeli.");

        double toplam = 0;
        for (int i = 0; i < _probabilities.Length; i++)
        {
            _probabilities[i] *= fuzzyDereceleri[i];
            toplam += _probabilities[i];
        }

        // Normalize et (toplam = 1.0)
        if (toplam > 0)
        {
            for (int i = 0; i < _probabilities.Length; i++)
                _probabilities[i] /= toplam;
        }
    }

    /// <summary>Gözlem: Ağırlıklandırılmış olasılıklara göre rastgele bir içecek seç (süperpozisyon çökmesi)</summary>
    public int Gozlemle()
    {
        double r = _random.NextDouble();
        double kumulatif = 0;

        for (int i = 0; i < _probabilities.Length; i++)
        {
            kumulatif += _probabilities[i];
            if (r <= kumulatif)
                return i;
        }

        return _probabilities.Length - 1; // Yuvarlama hatasına karşı
    }

    /// <summary>Kullanıcı geri bildirimiyle öğrenme: Seçilen içeceğin olasılığını artır</summary>
    public void Ogren(int secilenIndex, double ogrenmeOrani = 0.1)
    {
        _probabilities[secilenIndex] *= (1 + ogrenmeOrani);
        NormalizeEt();
    }

    /// <summary>Beğenilmeyen içeceğin olasılığını azalt</summary>
    public void GeriBildirim(int index, bool begendi)
    {
        if (begendi)
            Ogren(index, 0.15);
        else
        {
            _probabilities[index] *= 0.8;
            NormalizeEt();
        }
    }

    private void NormalizeEt()
    {
        double toplam = _probabilities.Sum();
        if (toplam > 0)
        {
            for (int i = 0; i < _probabilities.Length; i++)
                _probabilities[i] /= toplam;
        }
    }
}
