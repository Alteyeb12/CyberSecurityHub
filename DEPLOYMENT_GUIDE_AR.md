# إرشادات النشر على Render

## الخطوة 1: إنشاء حساب على Render

1. اذهب إلى [render.com](https://render.com)
2. سجل حساباً جديداً أو سجل دخولك
3. ربط حسابك بـ GitHub

## الخطوة 2: إنشاء قاعدة بيانات

### خيار أول: استخدام Azure SQL

```
1. اذهب إلى portal.azure.com
2. إنشء SQL Server جديد
3. انسخ سلسلة الاتصال
```

### خيار ثاني: استخدام قاعدة بيانات مدارة على Render

```
1. من لوحة التحكم على Render
2. اختر "New" → "PostgreSQL"
3. ستحصل على سلسلة اتصال
```

## الخطوة 3: إنشاء Web Service

1. اذهب إلى لوحة التحكم
2. اضغط على "New" → "Web Service"
3. اختر "Build and deploy from a Git repository"
4. ربط المستودع `Alteyeb12/CyberSecurityHub`
5. اختر الفرع `develop`

## الخطوة 4: تكوين الإعدادات

### البيانات الأساسية:
- **Name**: CyberInsight
- **Region**: اختر الأقرب إلى منطقتك
- **Branch**: develop
- **Runtime**: Docker

### الأوامر:
- **Build Command**: `docker build -t cyberinsight .`
- **Start Command**: `dotnet CyberInsight.dll`

## الخطوة 5: متغيرات البيئة

أضف المتغيرات التالية في "Environment":

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
ConnectionStrings__DefaultConnection=Server=your_server;Database=CyberInsightDB;User Id=sa;Password=your_password;TrustServerCertificate=true;
Stripe__SecretKey=sk_test_your_secret_key
Stripe__PublicKey=pk_test_your_public_key
Stripe__WebhookSecret=whsec_your_webhook_secret
```

## الخطوة 6: اختيار الخطة

- اختر **Starter** أو أعلى
- Starter: $7/شهر (مناسب للاختبار)
- Standard: $12/شهر (موصى به للإنتاج)

## الخطوة 7: النشر

1. اضغط "Create Web Service"
2. انتظر بناء المشروع (5-10 دقائق)
3. ستحصل على رابط مثل: `https://cyberinsight-xxxxx.onrender.com`

## استكشاف الأخطاء

### الخطأ: Connection timeout

```
✓ تأكد من سلسلة الاتصال
✓ تحقق من قاعدة البيانات متاحة
✓ جرب الاتصال المحلي أولاً
```

### الخطأ: Service failed to build

```
✓ تحقق من logs في Render
✓ تأكد من وجود Dockerfile
✓ تحقق من إصدار .NET صحيح
```

### الخطأ: Application won't start

```
✓ تحقق من متغيرات البيئة
✓ تأكد من سلسلة الاتصال صحيحة
✓ اطلع على logs للتفاصيل
```

## المراقبة

1. اذهب إلى "Logs" لمراقبة التطبيق
2. استخدم "Metrics" لرؤية الأداء
3. اضبط الموارد إذا لزم الأمر

## التحديثات المستقبلية

بعد كل تحديث:

```bash
# Push إلى الفرع develop
git add .
git commit -m "Update: description"
git push origin develop
```

Render سيعيد النشر تلقائياً!

---

**ملاحظة مهمة**: تأكد من:
- ✅ استخدام HTTPS في الإنتاج
- ✅ تفعيل CORS إذا لزم الأمر
- ✅ حماية مفاتيح API الحساسة
- ✅ النسخ الاحتياطية المنتظمة لقاعدة البيانات