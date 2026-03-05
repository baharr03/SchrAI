namespace SchrAI;

/// <summary>
/// Hibrit Model: Olasılık katmanı (SchrAI) ile Fuzzy Logic katmanını birleştirir.
/// P_new[i] = P_old[i] × FuzzyDegree[i], sonra normalize edilir.
/// </summary>
public class HybridModel
{
    private readonly Superposition _superposition = new();

    public Superposition Superposition => _superposition;

    /// <summary>Hava sıcaklığı ve saate göre öneri üret</summary>
    public (int secilenIndex, string secilenIcecek) OneriAl(double sicaklik, int saat)
    {
        // 1. Fuzzy Katman: Üyelik derecelerini hesapla
        double[] fuzzyDereceleri = FuzzyLogic.HesaplaUyelikDereceleri(sicaklik, saat);

        // 2. Olasılık Katmanı: Fuzzy dereceleriyle ağırlıklandır
        _superposition.Agirliklandir(fuzzyDereceleri);

        // 3. Gözlem: Süperpozisyon çökmesi - rastgele (olasılıklı) seçim
        int index = _superposition.Gozlemle();

        return (index, Superposition.Icecekler[index]);
    }

    /// <summary>Kullanıcı geri bildirimiyle öğren</summary>
    public void GeriBildirim(int secilenIndex, bool begendi)
    {
        _superposition.GeriBildirim(secilenIndex, begendi);
    }
}
