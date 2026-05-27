# Proje: Delik_Web_Uygulama

## Teknoloji
- .NET 8 Blazor WebAssembly
- Sunucu yok, tamamen tarayıcıda çalışır
- GitHub Pages veya Netlify üzerinden yayınlanır

## Amaç
Kullanıcı mobilya dolap modülü ölçülerini girer (en, boy, yükseklik),
sistem aşağıdaki işlemleri yapar:
- Modül parçalarını oluşturur
- Parçalara delik pozisyonlarını hesaplar ve atar
- Parçaların delinmesini simüle eder
- Barkod üretir ve okur
- Modül parçalarını nesting eder
- Nesting edilen parçalara takım atar
- G-code üretir ve .cnc uzantılı dosya olarak indirir

## Klasör Yapısı
```
Delik_Web_Uygulama/
├── Client/        → Blazor arayüzü (sayfalar, bileşenler)
├── Shared/        → İş mantığı (modeller, hesaplama, G-code üretimi)
```

## Kodlama Kuralları
- Değişken ve metod isimleri Türkçe olacak
- Her servis tek bir iş yapacak (DelikHesaplama, GKodUretici ayrı ayrı)

## Delik Sistemi
- Henüz belirlenmedi, ileride eklenecek
- Şimdilik sabit değerler kullanılacak, sonradan değiştirilebilir yapıda olsun

## G-Code Çıktısı
- .cnc uzantılı dosya olarak indirilir
- Dosya adı: [tarih]_[parça adı].cnc formatında olacak
- Parça adı örnekleri: sol_yan, sag_yan, ust_tabla vb.
