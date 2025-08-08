## Uygulama İsterleri
### Görev Senaryosu

Aşağıdaki gibi örnek bir senaryo üzerinden örnek bir proje geliştirilmiştir.

Tedarik zincirinizin teknoloji tarafında görev aldığınızı düşünün. Sorumluluğunuz altında olan sistem, mağazalardaki ürünlerin stok seviyesini takip etmekte ve ürünler kritik stok seviyesinin altına düştüğünde uygun tedarikçiden sipariş oluşturmakla yükümlü. Ancak bu sistem hâlâ gelişmekte; size bazı eksikleri tamamlamak, bazı süreçleri otomatize etmek ve bunu mümkün olan en temiz, genişletilebilir mimariyle yapmak düşüyor.

### Görevler

Aşağıdaki maddeleri dikkatle okuyun. Kodunuz bu maddeleri karşılayacak şekilde yapılandırılmalı:

### 1. Ürün Entegrasyonu ve Sanal Katalog

Şirketiniz, Fake Store API’yi bir dış tedarikçi gibi entegre etmiş. Ancak bu API sadece veri sunmakta; ürün ekleme ya da güncelleme işlevselliği bulunmamakta.

İç sisteminizde (in-memory veya session olabilir), **kendi ürün kataloğunuzu oluşturmalı ve Fake Store’daki ürünlerle eşleştirmelisiniz.**  
Bu nedenle `POST/products` endpoint'i ile kullanıcıdan ürün ismi, eşik stok miktarı ve başlangıç stoğu alınmalı ve ürünler iç sisteminize kayıt edilmelidir.

> **Fake Store API’den gelen ürünlerle bu ürünleri `productCode` gibi bir ortak alanla eşleştirebilirsiniz.**



### 2. Kritik Stok Tespiti

- `GET /products/low-stock` → Eşik değerin altındaki ürünleri listeler.


### 3. Sipariş Otomasyonu

- `POST /orders/check-and-place` → Kritik seviyedeki ürünler için Fake Store’dan en uygun fiyata sipariş oluşturur.
- Fake Store ürünlerinden en uygun fiyatlı olanı seçmeniz, algoritmik mantık kurmanızı gerektirir.

### 4. API Güvenliği
- API katmanında Fixed Window Limit kullanarak gelen istekleri sınırlandırmanız gerekir.
- Kullanıcı formlarında Cross-Site Request Forgery (XSRF/CSRF) saldırılarını engellemeniz
gerekir.
- Request veya rendering kısımlarında Cross-Site Scripting (XSS) saldırılarını engellemeniz
gerekir.

### 5. Bonus Görev – Stokları Roma Rakamı ile Göster
Ürün stok miktarları, kullanıcıya HTML ortamında **Roma rakamı** ile gösterilmelidir.
Bu dönüşümün JavaScript ile yapılmasını bekliyoruz. 

Örnek çıktı: `Stok: IV adet`

## Proje Yapısı ve Endpoint'ler
- Projede ürün bilgileri in-memory olarak tutulmuştur. Örnek post ve get istekleri .http dosyalarında bulunmaktadır.
- Ürünler aşağıdaki endpoint üzerinden POST ile tek tek eklenir:

        http://localhost:5229/products
    Request:
    ```json
    {
    "name": "T-shirt 1",
    "productCode": "Mens Casual Premium Slim Fit T-Shirts ",
    "initialStockCount":40,
    "criticalStockCount": 90
    }
    ```
    Response:
    ```json
    HTTP/1.1 201 Created
    Connection: close
    Content-Type: application/json; charset=utf-8
    Date: Sun, 22 Jun 2025 19:48:24 GMT
    Server: Kestrel
    Location: http://localhost:5229/products/1
    Transfer-Encoding: chunked

    {
    "id": 1,
    "productCode": "Mens Casual Premium Slim Fit T-Shirts ",
    "name": "T-shirt 1",
    "initialStockCount": 40,
    "criticalStockCount": 90
    }
    ```
- ProductCode gibi bir alan FakeStore'da olmadığından Title alanı ile eşleştirilmiştir.
- Ürün eklenirken ProductCode ile FakeStore'da ürün var mı kontrol edilir, yoksa ekleme yapılmaz.
- Aşağıdaki endpoint ile stoğu kritik eşik değerinin altında olan ürünler listelenir.
    
        http://localhost:5229/products/low-stock
  ```json
    HTTP/1.1 200 OK
    Connection: close
    Content-Type: application/json; charset=utf-8
    Date: Sun, 22 Jun 2025 19:43:20 GMT
    Server: Kestrel
    Transfer-Encoding: chunked

    [
    {
        "id": 1,
        "name": "T-shirt 1",
        "productCode": "Mens Casual Premium Slim Fit T-Shirts ",
        "initialStockCount": 40,
        "criticalStockCount": 90
    }
    ]
  ```
- Aşağıdaki endpoint ile tüm ürünler listelenir.

        http://localhost:5229/products

  ```json
    HTTP/1.1 200 OK
    Connection: close
    Content-Type: application/json; charset=utf-8
    Date: Sun, 22 Jun 2025 19:44:58 GMT
    Server: Kestrel
    Transfer-Encoding: chunked

    [
    {
        "id": 1,
        "name": "T-shirt 1",
        "productCode": "Mens Casual Premium Slim Fit T-Shirts ",
        "initialStockCount": 40,
        "criticalStockCount": 90
    },
    {
        "id": 2,
        "name": "T-shirt 2",
        "productCode": "Mens Cotton Jacket",
        "initialStockCount": 10,
        "criticalStockCount": 7
    }
    ]
  ```
- Aşağıdaki endpoint sipariş edilmesi gereken FakeStore'da en uygun fiyatlı ürünleri döner. POST metodu ile çalışır.
  
        http://localhost:5229/orders/check-and-place

    Response:

  ```json
    HTTP/1.1 201 Created
    Connection: close
    Content-Type: application/json; charset=utf-8
    Date: Sun, 22 Jun 2025 19:52:21 GMT
    Server: Kestrel
    Transfer-Encoding: chunked

    [
    {
        "id": 2,
        "title": "Mens Casual Premium Slim Fit T-Shirts ",
        "price": 22.3,
        "category": "men's clothing",
        "image": "https://fakestoreapi.com/img/71-3HjGNDUL._AC_SY879._SX._UX._SY._UY_.jpg"
    }
    ]
  ```

 ## Exception Handling
 .Net Core 8 ile gelen IExceptionHandler ile exception handling yapılmıştır.

 ## Input validation
 Input validation için Data Anotation'lar ve "MinimalApis.Extensions" kullanılmıştır.

 ## API Documantation
Dokümantasyon için "Microsoft.AspNetCore.OpenApi" kullanılmıştır. Doküman url:

    http://localhost:5229/openapi/v1.json

 ## Cross-Site Request Forgery Protection
* Akifleştirmek için ProductsEndpoints.cs dosyasındaki 52. satır uncomment yapılabilir. Testlerde kolaylık olması için commetlenmiştir.
   ```
    //await antiforgery.ValidateRequestAsync(context);
   ```

 ## Eklenen Diğer Özellikler
* Rate Limiter
* AntiXss Middleware

## Client Projesi (Bonus Görev)
- React projesi olarak ayrı bir repoya atılmıştır. Repo linki: 

        https://github.com/EnginMd/stock-management-fe

- Algoritma aşağıdaki tablo baz alınarak kurgulanmıştır. Sayı bu tablodaki 1. elemandan başlanarak kontrol edilir. Eğer ona eşit veya büyükse sembolü yazılır ve sayıdan sembolün değeri çıkarılır. Sayı halen bu değerden büyükse sembol tekrar eklenir ve sayı tekrar eksiltilir. Sayı sembolün değerinden küçük olana kadar bu devam eder. Sayı küçüldükten sonra sonraki sembolün değeri ile aynı kontrol ve işlemler yapılarak devam edilir. İlgili toRoman JavaScript fonksiyonu yukarıdaki repoda, App.jsx dosyasındadır.

  ```javascript
    const romanMap = [
            { value: 1000, symbol: 'M' },
            { value: 900, symbol: 'CM' },
            { value: 500, symbol: 'D' },
            { value: 400, symbol: 'CD' },
            { value: 100, symbol: 'C' },
            { value: 90, symbol: 'XC' },
            { value: 50, symbol: 'L' },
            { value: 40, symbol: 'XL' },
            { value: 10, symbol: 'X' },
            { value: 9, symbol: 'IX' },
            { value: 5, symbol: 'V' },
            { value: 4, symbol: 'IV' },
            { value: 1, symbol: 'I' }
    ];
  ```
 