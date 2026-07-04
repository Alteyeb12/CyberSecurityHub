# متطلبات النظام والإعداد

## المتطلبات الأساسية

### للتطوير المحلي:
- .NET 8.0 SDK أو أحدث
- SQL Server 2019 أو Azure SQL Database
- Visual Studio 2022 أو Visual Studio Code
- Git
- Docker (اختياري للاختبار المحلي)

### للإنتاج (Render):
- حساب على Render.com
- قاعدة بيانات SQL Server خارجية
- مفاتيح Stripe API
- نطاق مخصص (اختياري)

## مفاتيح Stripe

### الحصول على المفاتيح:

1. اذهب إلى [stripe.com](https://stripe.com)
2. أنشئ حساباً أو سجل دخولك
3. اذهب إلى Dashboard → API Keys
4. انسخ:
   - **Publishable Key** (pk_test_...)
   - **Secret Key** (sk_test_...)
5. للإنتاج، استخدم المفاتيح الحية (pk_live_ و sk_live_)

### إضافة المفاتيح:

```json
// appsettings.json (للتطوير)
"Stripe": {
  "PublicKey": "pk_test_your_key",
  "SecretKey": "sk_test_your_key"
}
```

```
// Render Environment Variables (للإنتاج)
Stripe__PublicKey=pk_live_your_key
Stripe__SecretKey=sk_live_your_key
Stripe__WebhookSecret=whsec_your_secret
```

## قاعدة البيانات

### خيارات الاتصال:

#### 1. SQL Server محلي
```
Server=.;Database=CyberInsightDB;Integrated Security=true;TrustServerCertificate=true;
```

#### 2. Azure SQL Database
```
Server=your-server.database.windows.net;Database=CyberInsightDB;User Id=username;Password=password;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;
```

#### 3. AWS RDS SQL Server
```
Server=your-instance.region.rds.amazonaws.com,1433;Database=CyberInsightDB;User Id=admin;Password=password;TrustServerCertificate=true;
```

#### 4. Docker SQL Server
```
Server=db,1433;Database=CyberInsightDB;User Id=sa;Password=YourPassword@123;TrustServerCertificate=true;
```

## إنشاء قاعدة البيانات

### الطريقة الأولى: Entity Framework Migrations

```bash
# تطبيق الترحيلات
dotnet ef database update

# ستتم إنشاء الجداول تلقائياً
```

### الطريقة الثانية: SQL Script

```sql
CREATE DATABASE CyberInsightDB;

USE CyberInsightDB;

-- سيتم إنشاء الجداول بواسطة EF Core
```

## الإعدادات الأمنية

### كلمات المرور:
- الحد الأدنى: 8 أحرف
- يجب أن تحتوي على: أحرف كبيرة وصغيرة وأرقام
- تشفير: BCrypt (Work Factor: 12)

### HTTPS:
```bash
# توليد شهادة SSL محلية
dotnet dev-certs https --trust
```

### CORS:
```csharp
// في Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## متغيرات البيئة

### تطوير محلي (.env)
```
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Server=.;Database=CyberInsightDB;Integrated Security=true;
Stripe__SecretKey=sk_test_xxx
Stripe__PublicKey=pk_test_xxx
```

### إنتاج (Render)
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
ConnectionStrings__DefaultConnection=Server=...
Stripe__SecretKey=sk_live_xxx
Stripe__PublicKey=pk_live_xxx
```

## الترخيص والتوافقية

- **Stripe**: خدمة مدفوعة، نموذج الدفع حسب الاستخدام
- **SQL Server**: ترخيص مشروط
- **.NET Core**: مفتوح المصدر (MIT License)
- **BCrypt.Net-Next**: MIT License

## دعم المتصفحات

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## حد أقصى للملفات

- حجم الطلب: 100 MB
- حجم قاعدة البيانات: غير محدود (اعتمادً على الخطة)
- التخزين على Render: حسب الخطة المختارة

## موارد مفيدة

- [Microsoft .NET 8 Docs](https://learn.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Stripe Documentation](https://stripe.com/docs)
- [Render Documentation](https://render.com/docs)
- [BCrypt Documentation](https://github.com/BcryptNet/bcrypt.net)

---

**آخر تحديث**: 4 يوليو 2026