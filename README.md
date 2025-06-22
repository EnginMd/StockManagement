## Proje Yapısı ve Endpoint'ler
- Projede ürün bilgileri in-memory olarak tutulur. Örnek post ve get istekleri .http dosyalarında bulunmaktadır.
- Ürünler aşağıdaki endpoint üzerinden POST tek tek eklenir:

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
 