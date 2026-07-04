# CyberInsight - منصة مراقبة التهديدات السيبرانية

## نظرة عامة
CyberInsight هي منصة سحابية متطورة لمراقبة التهديدات السيبرانية في الوقت الفعلي. توفر المنصة حلاً متكاملاً لمراقبة وتحليل الأنشطة المشبوهة مع لوحة تحكم تفاعلية.

## المتطلبات الأساسية
- .NET 8.0 SDK أو أحدث
- SQL Server 2019 أو Azure SQL
- Stripe Account (للمدفوعات)
- Docker (للنشر على Render)

## المميزات الرئيسية
✅ مراقبة حية للتهديدات (Real-time Monitoring)
✅ نظام تسجيل دخول آمن (BCrypt Encryption)
✅ إدارة الاشتراكات (Stripe Integration)
✅ لوحة تحكم تفاعلية
✅ API RESTful
✅ نشر متقدم على السحابة (Docker & Render)

## البنية التقنية
- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server + Entity Framework Core
- **Authentication**: Cookie-based Authentication
- **Security**: BCrypt Password Hashing
- **Payments**: Stripe API
- **Deployment**: Docker & Render Cloud Platform

## التثبيت المحلي

### 1. استنساخ المستودع
```bash
git clone https://github.com/Alteyeb12/CyberSecurityHub.git
cd CyberSecurityHub
```

### 2. تكوين قاعدة البيانات
حرر ملف `appsettings.json` وأضف بيانات اتصالك:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=CyberInsightDB;User Id=sa;Password=your_password;"
}
```

### 3. تطبيق الترحيلات
```bash
dotnet ef database update
```

### 4. تشغيل التطبيق
```bash
dotnet run
```

## تشغيل باستخدام Docker

### باستخدام Docker Compose
```bash
docker-compose up --build
```

سيتم تشغيل:
- SQL Server على المنفذ 1433
- التطبيق على http://localhost:8080

## النشر على Render

### خطوات النشر:

1. **إنشاء حساب على Render**
   - اذهب إلى https://render.com

2. **الربط مع GitHub**
   - ربط حسابك على GitHub

3. **إنشاء Web Service جديد**
   - اختر الفرع `develop`
   - الأمر الأساسي: `dotnet CyberInsight.dll`

4. **تعيين متغيرات البيئة**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ConnectionStrings__DefaultConnection=your_connection_string
   Stripe__SecretKey=your_stripe_key
   Stripe__PublicKey=your_stripe_public_key
   ```

5. **اختيار الخطة**
   - استخدم Starter Plan أو أعلى

6. **نشر التطبيق**
   - اضغط على Deploy

## متغيرات البيئة المطلوبة

| المتغير | الوصف | مثال |
|---------|-------|-------|
| `ConnectionStrings__DefaultConnection` | سلسلة اتصال قاعدة البيانات | `Server=...;Database=CyberInsightDB;...` |
| `Stripe__SecretKey` | مفتاح Stripe السري | `sk_test_...` |
| `Stripe__PublicKey` | مفتاح Stripe العام | `pk_test_...` |
| `ASPNETCORE_ENVIRONMENT` | بيئة التطبيق | `Production` |

## هيكل المشروع
```
CyberSecurityHub/
├── Controllers/           # المتحكمات
│   ├── AccountController.cs
│   ├── DashboardController.cs
│   ├── SubscriptionController.cs
│   └── ApiController.cs
├── Models/               # نماذج البيانات
│   ├── User.cs
│   ├── Subscription.cs
│   ├── ThreatAlert.cs
│   └── AuditLog.cs
├── Services/             # الخدمات
│   ├── AuthService.cs
│   ├── StripeService.cs
│   └── ThreatMonitoringService.cs
├── Data/                 # قاعدة البيانات
│   └── ApplicationDbContext.cs
├── Views/                # العروض
├── Dockerfile            # لملف Docker
├── docker-compose.yml    # تكوين Docker Compose
└── appsettings.json      # إعدادات التطبيق
```

## API Endpoints

### التهديدات
- `GET /api/threat/all` - الحصول على جميع التهديدات
- `GET /api/threat/critical` - الحصول على التهديدات الحرجة
- `GET /api/threat/statistics` - إحصائيات التهديدات
- `POST /api/threat/create` - إنشاء تهديد جديد

### الحسابات
- `POST /Account/Register` - التسجيل
- `POST /Account/Login` - تسجيل الدخول
- `POST /Account/Logout` - تسجيل الخروج

### الاشتراكات
- `GET /Subscription/Plans` - عرض الخطط
- `POST /Subscription/Checkout` - بدء عملية الشراء

## المساهمة
سعيد باستقبال مساهماتك! يرجى:
1. Fork المشروع
2. إنشاء فرع للميزة الجديدة
3. Commit التغييرات
4. Push إلى الفرع
5. فتح Pull Request

## الترخيص
هذا المشروع مرخص تحت MIT License

## الدعم
للحصول على الدعم، يرجى فتح Issue في المستودع.

---

**تم التطوير بواسطة**: Al-Tayyib Al-Ustura
**التاريخ**: 21 مايو 2026
