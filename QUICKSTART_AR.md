# 🚀 دليل البدء السريع - CyberInsight

## التثبيت والتشغيل المحلي

### المتطلبات الأساسية
```bash
- .NET 8.0 SDK أو أحدث
- SQL Server 2019 أو Azure SQL
- Git
- Visual Studio 2022 أو VS Code (اختياري)
```

### الخطوة 1: استنساخ المستودع
```bash
git clone https://github.com/Alteyeb12/CyberSecurityHub.git
cd CyberSecurityHub
git checkout develop
```

### الخطوة 2: استعادة المتطلبات
```bash
dotnet restore
```

### الخطوة 3: تكوين قاعدة البيانات

عدّل `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=CyberInsightDB;Integrated Security=true;TrustServerCertificate=true;"
}
```

### الخطوة 4: تطبيق التهجيرات
```bash
dotnet ef database update
```

### الخطوة 5: تشغيل التطبيق
```bash
dotnet run
```

سيتم فتح التطبيق على: `https://localhost:5001`

---

## التشغيل باستخدام Docker

### شرط أساسي: تثبيت Docker

### البناء والتشغيل:
```bash
# بناء صورة Docker
docker build -t cyberinsight .

# تشغيل الحاوية
docker run -p 8080:80 cyberinsight
```

### استخدام Docker Compose:
```bash
docker-compose -f docker-compose.dev.yml up --build
```

سيتم الوصول للتطبيق على: `http://localhost:8080`

---

## الخطوات الأساسية للاستخدام

### 1️⃣ إنشاء حساب جديد
- اذهب إلى صفحة التسجيل
- أدخل بيانات الحساب:
  - البريد الإلكتروني
  - الاسم الأول والأخير
  - اسم الشركة
  - كلمة المرور (8 أحرف على الأقل)

### 2️⃣ تسجيل الدخول
- استخدم بيانات الحساب الذي أنشأته
- ستحصل على خطة مجانية افتراضياً

### 3️⃣ استكشاف لوحة التحكم
- عرض إحصائيات التهديدات
- مشاهدة آخر التهديدات المكتشفة
- تصفية حسب مستوى الخطورة

### 4️⃣ ترقية الخطة (اختياري)
- اذهب إلى "الخطط"
- اختر خطة احترافية أو مؤسسية
- أكمل عملية الدفع عبر Stripe

---

## تكوين مفاتيح Stripe (للمدفوعات)

### 1. الحصول على المفاتيح:
```
1. اذهب إلى https://stripe.com
2. أنشئ حسابًا أو سجل دخول
3. توجه إلى Dashboard → API Keys
4. انسخ:
   - Publishable Key (pk_test_...)
   - Secret Key (sk_test_...)
```

### 2. إضافة المفاتيح:

في `appsettings.json`:
```json
"Stripe": {
  "PublicKey": "pk_test_your_key_here",
  "SecretKey": "sk_test_your_key_here"
}
```

---

## API - أمثلة الاستخدام

### الحصول على جميع التهديدات:
```bash
curl -X GET "https://localhost:5001/api/threat/all" \
  -H "Cookie: your_auth_cookie"
```

### إنشاء تنبيه تهديد:
```bash
curl -X POST "https://localhost:5001/api/threat/create" \
  -H "Content-Type: application/json" \
  -H "Cookie: your_auth_cookie" \
  -d '{
    "threatType": "SQL Injection",
    "description": "محاولة حقن SQL على صفحة تسجيل الدخول",
    "severity": "High",
    "sourceIP": "192.168.1.100",
    "targetAsset": "Web Server"
  }'
```

### الحصول على إحصائيات:
```bash
curl -X GET "https://localhost:5001/api/threat/statistics" \
  -H "Cookie: your_auth_cookie"
```

---

## استكشاف الأخطاء

### ❌ خطأ: "Connection string not found"
**الحل:**
- تأكد من وجود `appsettings.json`
- تحقق من سلسلة الاتصال
- تأكد من وصول SQL Server

### ❌ خطأ: "Database update error"
**الحل:**
```bash
# حذف التهجيرات القديمة
rm -r Migrations

# إنشاء تهجيرات جديدة
dotnet ef migrations add InitialCreate

# تطبيق التهجيرات
dotnet ef database update
```

### ❌ خطأ: "Port already in use"
**الحل:**
```bash
# تغيير المنفذ في launchSettings.json
# أو استخدام port مختلف
dotnet run --urls "https://localhost:5002"
```

### ❌ خطأ: "Stripe connection failed"
**الحل:**
- تحقق من صحة مفاتيح Stripe
- تأكد من الاتصال بالإنترنت
- جرب مفاتيح اختبار Stripe

---

## خطوات النشر على Render

### الخطوة 1: تحضير المستودع
```bash
git add .
git commit -m "Prepare for Render deployment"
git push origin develop
```

### الخطوة 2: إنشاء Web Service على Render
```
1. اذهب إلى https://render.com
2. انقر على "New" → "Web Service"
3. اختر "Build and deploy from a Git repository"
4. اربط مستودعك GitHub
5. اختر الفرع develop
```

### الخطوة 3: تكوين الإعدادات
```
Name: CyberInsight
Region: اختر الأقرب
Runtime: Docker
Build Command: docker build -t cyberinsight .
Start Command: dotnet CyberInsight.dll
```

### الخطوة 4: متغيرات البيئة
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
ConnectionStrings__DefaultConnection=your_connection_string
Stripe__SecretKey=sk_live_your_key
Stripe__PublicKey=pk_live_your_key
Stripe__WebhookSecret=whsec_your_secret
```

### الخطوة 5: النشر
```
انقر على "Create Web Service"
انتظر اكتمال البناء (5-10 دقائق)
ستحصل على URL مثل: https://cyberinsight-xxxxx.onrender.com
```

---

## نصائح مهمة للإنتاج

✅ **الأمان:**
- استخدم HTTPS فقط
- لا تشارك مفاتيح API
- استخدم متغيرات البيئة للبيانات الحساسة
- قم بتحديث المتطلبات بانتظام

✅ **الأداء:**
- استخدم caching للبيانات الثابتة
- قم بتحسين استعلامات قاعدة البيانات
- استخدم CDN للملفات الثابتة
- راقب استهلاك الموارد

✅ **الموثوقية:**
- قم بعمل نسخ احتياطية منتظمة
- راقب السجلات (Logs)
- اختبر التطبيق قبل الإطلاق
- لديك خطة للكوارث (Disaster Recovery)

---

## الموارد المفيدة

- 📚 [Microsoft .NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- 🗄️ [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- 💳 [Stripe Documentation](https://stripe.com/docs/api)
- 🐳 [Docker Documentation](https://docs.docker.com/)
- ☁️ [Render Documentation](https://render.com/docs)

---

## الدعم والمساعدة

- 📧 البريد الإلكتروني: support@cyberinsight.com
- 💬 المنتدى: https://github.com/Alteyeb12/CyberSecurityHub/discussions
- 🐛 الإبلاغ عن الأخطاء: https://github.com/Alteyeb12/CyberSecurityHub/issues

---

**تم الإنشاء بواسطة**: Al-Tayyib Al-Ustura
**التاريخ**: 4 يوليو 2026
**الإصدار**: 1.0.0
