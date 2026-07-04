# 🚀 تعليمات النشر على Render

## المتطلبات قبل البدء

✅ **GitHub Account** - مع مستودع موجود
✅ **Render Account** - سجل على https://render.com
✅ **SQL Server Database** - Azure SQL أو خادم خارجي
✅ **Stripe Account** - للمدفوعات

---

## الخطوة 1️⃣: تحضير المستودع

### 1. تأكد من أن جميع الملفات موجودة
```bash
git status
git add .
git commit -m "Prepare for Render deployment"
git push origin develop
```

### 2. تحقق من ملف `.gitignore`
```bash
# يجب أن يتجاهل:
bin/
obj/
.env
*.local
```

### 3. تأكد من Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CyberInsight.csproj", "./"]
RUN dotnet restore "CyberInsight.csproj"
COPY . .
RUN dotnet build "CyberInsight.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CyberInsight.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "CyberInsight.dll"]
```

---

## الخطوة 2️⃣: إنشاء قاعدة البيانات

### خيار أول: Azure SQL Database

```
1. اذهب إلى https://portal.azure.com
2. اختر "Create a Resource"
3. ابحث عن "SQL Database"
4. أملأ المعلومات:
   - Resource Group: جديد
   - Database name: CyberInsightDB
   - Server: إنشاء جديد
   - Authentication: SQL authentication
   - Username: azureuser
   - Password: YourSecurePassword123!
5. انقر "Review + Create"
6. بعد الإنشاء، انسخ Connection String
```

### خيار ثاني: قاعدة بيانات موجودة
```
- AWS RDS
- DigitalOcean Managed Database
- Google Cloud SQL
```

---

## الخطوة 3️⃣: إعداد Render

### 3.1 تسجيل الدخول على Render
```
1. اذهب إلى https://render.com
2. سجل الدخول أو أنشئ حساب
3. ربط GitHub Account
```

### 3.2 إنشاء Web Service
```
1. من Dashboard، انقر "New"
2. اختر "Web Service"
3. اختر "Build and deploy from a Git repository"
4. انقر "Connect Account" (لربط GitHub)
5. ابحث عن "CyberSecurityHub"
6. اختره وانقر "Connect"
```

### 3.3 تكوين Web Service

#### المعلومات الأساسية:
```
Name: CyberInsight
Branch: develop
Runtime: Docker
Region: Frankfurt (أو أقرب منطقة)
```

#### الأوامر:
```
Build Command: docker build -t cyberinsight .
Start Command: dotnet CyberInsight.dll
```

#### خطة الدفع:
- اختر **Starter** ($7/شهر) أو **Standard** ($12/شهر)

---

## الخطوة 4️⃣: متغيرات البيئة

### أضف هذه المتغيرات في Render:

```
# البيئة
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000

# قاعدة البيانات
ConnectionStrings__DefaultConnection=Server=tcp:your-server.database.windows.net,1433;Initial Catalog=CyberInsightDB;Persist Security Info=False;User ID=azureuser;Password=YourPassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

# Stripe
Stripe__PublicKey=pk_live_YOUR_PUBLIC_KEY
Stripe__SecretKey=sk_live_YOUR_SECRET_KEY
Stripe__WebhookSecret=whsec_YOUR_WEBHOOK_SECRET
```

### كيفية الحصول على مفاتيح Stripe:

```
1. اذهب إلى https://dashboard.stripe.com
2. اختر "Developers" من الشريط الجانبي
3. اختر "API Keys"
4. انسخ:
   - Publishable Key (pk_live_...)
   - Secret Key (sk_live_...)
5. اذهب إلى "Webhooks"
6. أنشئ endpoint جديد:
   - URL: https://your-app.onrender.com/stripe/webhook
   - انسخ Signing Secret
```

---

## الخطوة 5️⃣: النشر

### 5.1 إكمال الإعدادات
```
1. تأكد من ملء جميع متغيرات البيئة
2. انقر "Create Web Service"
3. سيبدأ البناء تلقائياً
```

### 5.2 متابعة البناء
```
1. اذهب إلى "Logs"
2. راقب عملية البناء
3. الوقت المتوقع: 5-10 دقائق
```

### 5.3 التحقق من النجاح
```
✅ يجب أن يظهر:
- Build successful
- Service running
- رابط URL مثل: https://cyberinsight-xxxxx.onrender.com
```

---

## الخطوة 6️⃣: الاختبار

### 6.1 الوصول للتطبيق
```
1. اذهب إلى رابط Render
2. يجب أن تشاهد الصفحة الرئيسية
```

### 6.2 اختبار المميزات
```bash
# اختبر التسجيل
GET https://your-app.onrender.com/Account/Register

# اختبر تسجيل الدخول
GET https://your-app.onrender.com/Account/Login

# اختبر لوحة التحكم
GET https://your-app.onrender.com/Dashboard
```

### 6.3 اختبر API
```bash
curl -X GET "https://your-app.onrender.com/api/threat/all" \
  -H "Cookie: your_auth_cookie"
```

---

## استكشاف الأخطاء

### ❌ خطأ: Build failed

**الحل:**
```
1. اذهب إلى "Logs"
2. ابحث عن رسالة الخطأ
3. تحقق من:
   - صحة الـ Dockerfile
   - وجود جميع الملفات
   - إصدار .NET صحيح
```

### ❌ خطأ: Application won't start

**الحل:**
```
1. تحقق من متغيرات البيئة
2. تأكد من Connection String
3. راجع السجلات للأخطاء المحددة
```

### ❌ خطأ: Database connection failed

**الحل:**
```
1. تحقق من IP الخادم في قائمة السماح
2. تأكد من صحة Connection String
3. اختبر الاتصال محلياً أولاً
```

### ❌ خطأ: Stripe not working

**الحل:**
```
1. تحقق من صحة مفاتيح Stripe
2. تأكد من تفعيل Webhooks
3. اختبر مع بيانات Stripe التجريبية
```

---

## الخطوة 7️⃣: الإنتاجية

### 7.1 المراقبة
```
1. اذهب إلى "Metrics"
2. راقب:
   - CPU Usage
   - Memory Usage
   - Request Count
```

### 7.2 السجلات
```
1. راجع "Logs" يومياً
2. ابحث عن الأخطاء
3. قم بالتحديثات المطلوبة
```

### 7.3 النسخ الاحتياطية
```
1. ضع جدولة يومية لنسخ قاعدة البيانات
2. احفظ النسخ في مكان آمن
3. اختبر الاستعادة بشكل دوري
```

---

## الخطوة 8️⃣: مجال مخصص (اختياري)

### ربط نطاق DNS
```
1. في Render، اذهب إلى "Settings"
2. اختر "Custom Domain"
3. أدخل: cyberinsight.com
4. اتبع التعليمات لربط DNS
5. يجب أن يظهر SSL Certificate تلقائياً
```

---

## نصائح مهمة

✅ **الأمان:**
- استخدم بيانات Stripe الحية فقط في الإنتاج
- لا تشارك مفاتيح سرية
- استخدم HTTPS دائماً

✅ **الأداء:**
- راقب استخدام الموارد
- قم بتحسين الاستعلامات
- استخدم Caching عند الحاجة

✅ **الموثوقية:**
- نسخ احتياطي يومي
- عطل على الفور عند اكتشاف مشاكل
- حافظ على السجلات

---

## الخطوات التالية

1. 🔄 **التحديثات:** ادفع إلى `develop` وسيتم النشر تلقائياً
2. 📧 **البريد:** أضف خدمة بريد (SendGrid، Mailgun)
3. 📊 **المراقبة:** أضف New Relic أو Datadog
4. 🔔 **التنبيهات:** اضبط تنبيهات Render

---

## مساعدة إضافية

- 📖 Render Docs: https://render.com/docs
- 🆘 Render Support: support@render.com
- 💬 Community Forum: https://community.render.com

---

**تم بنجاح! 🎉 تطبيقك الآن مباشر على الإنترنت!**
