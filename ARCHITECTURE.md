# محاضر التطوير

## نموذج البيانات

### User (المستخدم)
```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }              // البريد الإلكتروني (فريد)
    public string FirstName { get; set; }           // الاسم الأول
    public string LastName { get; set; }            // الاسم الأخير
    public string PasswordHash { get; set; }        // كلمة المرور المشفرة
    public string CompanyName { get; set; }         // اسم الشركة
    public DateTime CreatedAt { get; set; }         // تاريخ الإنشاء
    public DateTime? LastLogin { get; set; }        // آخر دخول
    public bool IsActive { get; set; }              // حالة النشاط
}
```

### Subscription (الاشتراك)
```csharp
public class Subscription
{
    public int Id { get; set; }
    public int UserId { get; set; }                 // معرف المستخدم
    public SubscriptionPlan Plan { get; set; }      // نوع الخطة (Free/Professional/Enterprise)
    public SubscriptionStatus Status { get; set; }  // حالة الاشتراك
    public DateTime StartDate { get; set; }         // تاريخ البدء
    public DateTime? EndDate { get; set; }          // تاريخ الانتهاء
    public decimal Price { get; set; }              // السعر
    public string? StripeCustomerId { get; set; }   // معرف العميل على Stripe
    public string? StripeSubscriptionId { get; set; }// معرف الاشتراك على Stripe
}
```

### ThreatAlert (تنبيه التهديد)
```csharp
public class ThreatAlert
{
    public int Id { get; set; }
    public int UserId { get; set; }                 // معرف المستخدم
    public string ThreatType { get; set; }          // نوع التهديد
    public string Description { get; set; }         // الوصف
    public ThreatSeverity Severity { get; set; }    // مستوى الخطورة
    public string SourceIP { get; set; }            // IP المصدر
    public string TargetAsset { get; set; }         // الأصل المستهدف
    public DateTime DetectedAt { get; set; }        // وقت الاكتشاف
    public bool IsResolved { get; set; }            // هل تم حلها
    public int AlertCount { get; set; }             // عدد التنبيهات
}
```

### AuditLog (سجل التدقيق)
```csharp
public class AuditLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }                // معرف المستخدم (اختياري)
    public string Action { get; set; }              // الإجراء
    public string Details { get; set; }             // التفاصيل
    public string? IpAddress { get; set; }          // عنوان IP
    public DateTime CreatedAt { get; set; }         // التاريخ والوقت
}
```

## معمارية التطبيق

### طبقات التطبيق:

1. **Presentation Layer (طبقة العرض)**
   - Controllers
   - Views (Razor Pages)
   - API Endpoints

2. **Business Logic Layer (طبقة منطق الأعمال)**
   - Services
   - Validation
   - Business Rules

3. **Data Access Layer (طبقة الوصول للبيانات)**
   - Entity Framework Core
   - Database Context
   - Migrations

4. **Database Layer (طبقة قاعدة البيانات)**
   - SQL Server
   - Tables & Relationships

## تدفق المصادقة

```
1. المستخدم يدخل بيانات التسجيل
2. التحقق من صحة البيانات
3. تشفير كلمة المرور باستخدام BCrypt
4. حفظ البيانات في قاعدة البيانات
5. إنشاء Claim وتسجيل الدخول
6. حفظ Cookie للجلسة
7. إعادة التوجيه إلى لوحة التحكم
```

## تدفق معالجة التهديدات

```
1. استقبال تنبيه التهديد
2. التحقق من المستخدم
3. البحث عن تهديدات مشابهة نشطة
4. إذا وجدت، زيادة عداد التنبيهات
5. وإلا، إنشاء تنبيه جديد
6. حفظ في قاعدة البيانات
7. إرسال إشعار للمستخدم
```

## تدفق المدفوعات

```
1. المستخدم اختيار الخطة
2. إنشاء جلسة Stripe Checkout
3. إعادة التوجيه إلى صفحة الدفع
4. المستخدم يدخل بيانات البطاقة
5. م��الجة الدفع بواسطة Stripe
6. Webhook يتم استقباله
7. تحديث حالة الاشتراك
8. إرسال تأكيد البريد الإلكتروني
```

## الميزات المخطط تطويرها

- [ ] لوحة تحكم متقدمة مع رسوم بيانية
- [ ] نظام التنبيهات البريدية
- [ ] API متقدم مع معدل حد أقصى
- [ ] تقارير PDF مفصلة
- [ ] تحليل تهديدات متقدم
- [ ] نظام الأدوار والأذونات
- [ ] دعم الفريق متعدد المستخدمين
- [ ] المزامنة الفعلية WebSocket
- [ ] نظام الإخطارات الفورية
- [ ] تكامل مع أدوات أمان خارجية

---

**تم الإنشاء**: 4 يوليو 2026