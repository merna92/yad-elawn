# Yad ElAwn - Full System Workflow

هذا الملف يشرح دورة العمل الكاملة للنظام من أول لحظة يدخل فيها المستخدم لحد ما البيانات تتحفظ وتظهر في الواجهة.

## 1) الصورة الكبيرة للنظام

```mermaid
flowchart LR
    A["User"] --> B["Frontend"]
    B --> C["ASP.NET Core Web API"]
    C --> D["Controllers"]
    D --> E["Services"]
    E --> F["Repositories"]
    F --> G["ApplicationDbContext"]
    G --> H["SQL Server"]
    H --> G
    G --> F
    F --> E
    E --> D
    D --> I["Response to Frontend"]
```

### الفكرة العامة
- المستخدم يتعامل مع الواجهة الأمامية.
- الواجهة تبعت request للـ API.
- الـ Controller يستقبل الطلب.
- الـ Service ينفذ منطق الشغل.
- الـ Repository يتعامل مع قاعدة البيانات.
- الـ DbContext يترجم الكود إلى أوامر SQL.
- SQL Server يحفظ أو يقرأ البيانات.
- النتيجة ترجع في response للفرونت.

---

## 2) دورة المستخدم من البداية للنهاية

```mermaid
flowchart TD
    A["Open website"] --> B["Choose role"]
    B --> C["Register or Login"]
    C --> D["Frontend sends request to API"]
    D --> E["API validates data"]
    E --> F["API talks to database"]
    F --> G["API returns response"]
    G --> H["Frontend shows result"]
```

### ماذا يحدث فعليًا؟
1. المستخدم يدخل على الموقع.
2. يحدد دوره:
   - Donor
   - Charity
   - Beneficiary
   - Admin
3. إذا لم يكن مسجّلًا، يذهب إلى التسجيل.
4. إذا كان مسجّلًا، يذهب إلى تسجيل الدخول.
5. بعد النجاح، يحصل على صلاحيات الاستخدام المناسبة.
6. يبدأ في تنفيذ العمليات حسب نوعه.

---

## 3) Registration Flow

### ملف البداية
- `backend/api/YadElAwn.Api/Controllers/RegistrationsController.cs`

### الفكرة
كل نوع مستخدم له endpoint مستقل:
- `/api/registrations/donor`
- `/api/registrations/charity`
- `/api/registrations/beneficiary`
- `/api/registrations/admin`

```mermaid
flowchart TD
    A["Registration request"] --> B["RegistrationsController"]
    B --> C["RegistrationService"]
    C --> D["Check if email exists"]
    D -->|Yes| E["Return Conflict"]
    D -->|No| F["Hash password using BCrypt"]
    F --> G["Create User record"]
    G --> H["Create role-specific record"]
    H --> I["Use database transaction"]
    I --> J["Commit if success"]
    I --> K["Rollback if failed"]
    J --> L["Return new IDs"]
```

### تفصيل المهم
#### أ) التحقق من الإيميل
- النظام يتأكد أن الإيميل غير مستخدم.
- لو موجود، يرجع `Conflict`.

#### ب) تشفير كلمة السر
- الباسورد لا يتخزن نصًا صريحًا.
- يتم تشفيره بـ BCrypt.

#### ج) إنشاء سجل المستخدم
- يتم حفظ بيانات المستخدم الأساسية في جدول `User`.

#### د) إنشاء سجل النوع
- لو Donor: يتم إنشاء سجل في `Donor`.
- لو Charity: يتم إنشاء سجل في `Charity`.
- لو Beneficiary: يتم إنشاء سجل في `Beneficiary`.
- لو Admin: يتم إنشاء سجل في `Admin`.

#### هـ) Transaction
- لو أي خطوة فشلت، كل العملية ترجع.
- هذا يمنع وجود User بدون Role أو Role بدون User.

---

## 4) Login Flow

### ملف البداية
- `backend/api/YadElAwn.Api/Controllers/AuthController.cs`

### الفكرة
المستخدم يدخل email و password، ثم النظام يتحقق ويولد JWT token.

```mermaid
flowchart TD
    A["Login request"] --> B["AuthController"]
    B --> C["AuthService"]
    C --> D["Get user by email from repository"]
    D -->|Not found| E["Return Unauthorized"]
    D -->|Found| F["Verify password with BCrypt"]
    F -->|Wrong| E
    F -->|Correct| G["Create JWT token"]
    G --> H["Return token and user info"]
```

### تفاصيل إضافية
- الـ token يحتوي على:
  - `UserId`
  - `Email`
  - `UserType`
- هذا token يستخدم بعد ذلك في أي request محمي.

### كيف يستخدمه الفرونت؟
```http
Authorization: Bearer <TOKEN>
```

---

## 5) Donation Creation Flow

### ملف البداية
- `backend/api/YadElAwn.Api/Controllers/DonationsController.cs`

### الفكرة
المتبرع يرسل request لإنشاء donation، والنظام يقرر نوع التبرع ويتعامل معه داخل transaction.

```mermaid
flowchart TD
    A["Create donation request"] --> B["DonationsController"]
    B --> C["DonationService"]
    C --> D["Validate that at least one type exists"]
    D -->|Invalid| E["Return BadRequest"]
    D -->|Valid| F["Start transaction"]
    F --> G["Create Donation record"]
    G --> H["Create subtype record"]
    H --> I["Food / Clothes / Medicine / MedicalSupplies"]
    I --> J["Save changes"]
    J --> K["Commit transaction"]
    K --> L["Return Created response"]
    J -->|Failure| M["Rollback transaction"]
```

### تفاصيل العملية
#### أ) التحقق
- لازم request يحتوي على واحدة على الأقل من الأنواع:
  - Food
  - Clothes
  - Medicine
  - MedicalSupplies

#### ب) إنشاء السجل الرئيسي
- يتم إنشاء صف جديد في جدول `Donation`.

#### ج) إنشاء التفاصيل الفرعية
- إذا كان النوع Food، يتم إنشاء صف في `Food`.
- إذا Clothes، يتم إنشاء صف في `Clothes`.
- إذا Medicine، يتم إنشاء صف في `Medicine`.
- إذا MedicalSupplies، يتم إنشاء صف في `MedicalSupplies`.

#### د) نجاح العملية
- إذا نجحت كل الخطوات، يرجع النظام donation الجديد.

---

## 6) Donation Status Update Flow

```mermaid
flowchart TD
    A["Update donation status request"] --> B["DonationsController"]
    B --> C["DonationService"]
    C --> D["Find donation by ID"]
    D -->|Not found| E["Return NotFound"]
    D -->|Found| F["Update status"]
    F --> G["Save changes"]
    G --> H["Return NoContent"]
```

### ماذا يحدث؟
- يتم البحث عن donation بالـ ID.
- لو غير موجود، يتم إرجاع `NotFound`.
- لو موجود، يتم تحديث الحالة.
- أمثلة على الحالة:
  - Pending
  - Available
  - Matched
  - Completed

---

## 7) Messages Flow

### الفكرة
- المستخدم يرسل رسالة لمستخدم آخر.
- الرسالة تحفظ في قاعدة البيانات.
- الطرف الآخر يمكنه رؤيتها في inbox.

```mermaid
flowchart TD
    A["Send message"] --> B["MessagesController"]
    B --> C["Validate sender and receiver"]
    C --> D["Save message in database"]
    D --> E["Return success"]
    D --> F["Receiver reads inbox"]
```

### ما الذي يحدث؟
1. SenderId و ReceiverId و Content تصل في request.
2. يتم حفظ الرسالة.
3. يمكن عرض الرسائل لاحقًا عبر inbox endpoint.

---

## 8) Notifications Flow

### الفكرة
- النظام ينشئ notification عندما يحدث شيء مهم.
- مثل:
  - تبرع جديد
  - مطابقة
  - رسالة
  - تحديث حالة

```mermaid
flowchart TD
    A["Trigger event"] --> B["NotificationsController"]
    B --> C["Create notification record"]
    C --> D["Save to database"]
    D --> E["Return response"]
    D --> F["User sees notification list"]
```

### الهدف
- إبقاء المستخدم على علم بما يحدث داخل النظام.

---

## 9) Matching Flow

### الفكرة
المطابقة تربط donation بجهة مناسبة مثل charity أو beneficiary.

```mermaid
flowchart TD
    A["Donation available"] --> B["Matching logic"]
    B --> C["Check charity and beneficiary data"]
    C --> D["Create match record"]
    D --> E["Save match"]
    E --> F["Update donation status"]
    F --> G["Send notification"]
```

### ماذا يعني هذا؟
- إذا كان هناك donation مناسب، يتم إنشاء match.
- يتم حفظ العلاقة في جدول `Match`.
- غالبًا يتم تحديث حالة donation بناءً على ذلك.
- قد يتم إرسال notification للطرفين.

---

## 10) Data Access Flow

```mermaid
flowchart LR
    A["Controller"] --> B["Service"]
    B --> C["Repository"]
    C --> D["DbContext"]
    D --> E["SQL Server"]
```

### لماذا هذا التقسيم مهم؟
- `Controller` لا يعرف تفاصيل الداتابيز.
- `Service` لا يكتب queries مباشرة.
- `Repository` هو المسؤول عن التعامل مع البيانات.
- `DbContext` هو طبقة EF Core التي تتعامل مع الجداول.

---

## 11) Error Handling Cycle

```mermaid
flowchart TD
    A["Request"] --> B["Validate input"]
    B -->|Invalid| C["Return error response"]
    B -->|Valid| D["Continue workflow"]
    D --> E["Database operation"]
    E -->|Fail| F["Rollback transaction"]
    E -->|Success| G["Commit transaction"]
```

### أمثلة على الأخطاء
- email مكرر
- بيانات ناقصة
- تسجيل دخول خاطئ
- donation بدون تفاصيل
- record غير موجود

### لماذا هذا مهم؟
- يمنع البيانات الفاسدة.
- يحافظ على سلامة النظام.

---

## 12) Swagger Flow

### كيف يشتغل؟
- `Program.cs` يفعل Swagger.
- Swagger يعرض كل endpoints.
- يمكن اختبار الـ API من المتصفح بدون فرونت.

```mermaid
flowchart TD
    A["Run API"] --> B["UseSwagger"]
    B --> C["UseSwaggerUI"]
    C --> D["Open browser"]
    D --> E["View endpoints"]
    E --> F["Execute requests"]
```

### الرابط
- `http://localhost:5000/`

### ما فائدته؟
- توثيق حي
- تجربة مباشرة
- يسهّل على الفرونت فهم الـ request و response

---

## 13) الملفات التي تشرحينها في المناقشة

- `backend/api/YadElAwn.Api/Program.cs`
- `backend/api/YadElAwn.Api/Data/ApplicationDbContext.cs`
- `backend/api/YadElAwn.Api/Controllers/AuthController.cs`
- `backend/api/YadElAwn.Api/Controllers/RegistrationsController.cs`
- `backend/api/YadElAwn.Api/Controllers/DonationsController.cs`
- `backend/api/YadElAwn.Api/Services/AuthService.cs`
- `backend/api/YadElAwn.Api/Services/RegistrationService.cs`
- `backend/api/YadElAwn.Api/Services/DonationService.cs`
- `backend/api/YadElAwn.Api/Repositories/UserRepository.cs`
- `backend/api/YadElAwn.Api/Repositories/DonationRepository.cs`
- `backend/api/YadElAwn.Api/Dtos/Requests.cs`
- `backend/api/YadElAwn.Api/Models/Entities.cs`

---

## 14) جملة شرح مختصرة جدًا

> النظام يبدأ من `Program.cs`، ثم يمر من الـ Controller إلى Service إلى Repository إلى DbContext ثم SQL Server، وبعدها يرجع الرد للفرونت.  
> التسجيل واللوجين والتبرعات والرسائل والإشعارات والمطابقة كلها ماشية بنفس الدورة، ومعها validation وtransaction وJWT للحماية.

