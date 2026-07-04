# 🚀 Render Deployment Instructions

## Prerequisites

✅ **GitHub Account** - with existing repository
✅ **Render Account** - register at https://render.com
✅ **SQL Server Database** - Azure SQL or external server
✅ **Stripe Account** - for payments

---

## Step 1️⃣: Prepare Repository

### 1. Verify all files
```bash
git status
git add .
git commit -m "Prepare for Render deployment"
git push origin develop
```

### 2. Check `.gitignore`
```bash
# Must ignore:
bin/
obj/
.env
*.local
```

### 3. Verify Dockerfile
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

## Step 2️⃣: Set up Database

### Option 1: Azure SQL Database

```
1. Go to https://portal.azure.com
2. Click "Create a Resource"
3. Search for "SQL Database"
4. Fill in information:
   - Resource Group: Create new
   - Database name: CyberInsightDB
   - Server: Create new
   - Authentication: SQL authentication
   - Username: azureuser
   - Password: YourSecurePassword123!
5. Click "Review + Create"
6. After creation, copy Connection String
```

### Option 2: Existing Database
```
- AWS RDS
- DigitalOcean Managed Database
- Google Cloud SQL
```

---

## Step 3️⃣: Setup Render

### 3.1 Log in to Render
```
1. Go to https://render.com
2. Sign in or create account
3. Connect GitHub Account
```

### 3.2 Create Web Service
```
1. From Dashboard, click "New"
2. Select "Web Service"
3. Choose "Build and deploy from a Git repository"
4. Click "Connect Account" (to link GitHub)
5. Search for "CyberSecurityHub"
6. Select it and click "Connect"
```

### 3.3 Configure Web Service

#### Basic Information:
```
Name: CyberInsight
Branch: develop
Runtime: Docker
Region: Frankfurt (or closest)
```

#### Commands:
```
Build Command: docker build -t cyberinsight .
Start Command: dotnet CyberInsight.dll
```

#### Plan:
- Choose **Starter** ($7/month) or **Standard** ($12/month)

---

## Step 4️⃣: Environment Variables

### Add these in Render:

```
# Environment
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000

# Database
ConnectionStrings__DefaultConnection=Server=tcp:your-server.database.windows.net,1433;Initial Catalog=CyberInsightDB;Persist Security Info=False;User ID=azureuser;Password=YourPassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

# Stripe
Stripe__PublicKey=pk_live_YOUR_PUBLIC_KEY
Stripe__SecretKey=sk_live_YOUR_SECRET_KEY
Stripe__WebhookSecret=whsec_YOUR_WEBHOOK_SECRET
```

### How to Get Stripe Keys:

```
1. Go to https://dashboard.stripe.com
2. Select "Developers" from sidebar
3. Click "API Keys"
4. Copy:
   - Publishable Key (pk_live_...)
   - Secret Key (sk_live_...)
5. Go to "Webhooks"
6. Create new endpoint:
   - URL: https://your-app.onrender.com/stripe/webhook
   - Copy Signing Secret
```

---

## Step 5️⃣: Deploy

### 5.1 Complete Configuration
```
1. Ensure all environment variables are filled
2. Click "Create Web Service"
3. Build will start automatically
```

### 5.2 Monitor Build
```
1. Go to "Logs"
2. Watch build process
3. Expected time: 5-10 minutes
```

### 5.3 Verify Success
```
✅ Should show:
- Build successful
- Service running
- URL like: https://cyberinsight-xxxxx.onrender.com
```

---

## Step 6️⃣: Testing

### 6.1 Access Application
```
1. Go to Render URL
2. Should see home page
```

### 6.2 Test Features
```bash
# Test registration
GET https://your-app.onrender.com/Account/Register

# Test login
GET https://your-app.onrender.com/Account/Login

# Test dashboard
GET https://your-app.onrender.com/Dashboard
```

### 6.3 Test API
```bash
curl -X GET "https://your-app.onrender.com/api/threat/all" \
  -H "Cookie: your_auth_cookie"
```

---

## Troubleshooting

### ❌ Error: Build failed

**Solution:**
```
1. Go to "Logs"
2. Find error message
3. Check:
   - Dockerfile validity
   - All files present
   - Correct .NET version
```

### ❌ Error: Application won't start

**Solution:**
```
1. Check environment variables
2. Verify Connection String
3. Review logs for specific errors
```

### ❌ Error: Database connection failed

**Solution:**
```
1. Check server IP in whitelist
2. Verify Connection String
3. Test locally first
```

### ❌ Error: Stripe not working

**Solution:**
```
1. Verify Stripe key validity
2. Ensure Webhooks enabled
3. Test with Stripe test data
```

---

## Step 7️⃣: Production

### 7.1 Monitoring
```
1. Go to "Metrics"
2. Monitor:
   - CPU Usage
   - Memory Usage
   - Request Count
```

### 7.2 Logs
```
1. Review "Logs" daily
2. Look for errors
3. Make necessary updates
```

### 7.3 Backups
```
1. Schedule daily database backups
2. Store securely
3. Test restore regularly
```

---

## Step 8️⃣: Custom Domain (Optional)

### Link DNS Domain
```
1. In Render, go to "Settings"
2. Select "Custom Domain"
3. Enter: cyberinsight.com
4. Follow DNS setup instructions
5. SSL Certificate will auto-generate
```

---

## Important Tips

✅ **Security:**
- Use Stripe live keys only in production
- Never share secret keys
- Always use HTTPS

✅ **Performance:**
- Monitor resource usage
- Optimize queries
- Use caching when needed

✅ **Reliability:**
- Daily backups
- React quickly to issues
- Keep logs

---

## Next Steps

1. 🔄 **Updates:** Push to `develop` for auto-deployment
2. 📧 **Email:** Add email service (SendGrid, Mailgun)
3. 📊 **Monitoring:** Add New Relic or Datadog
4. 🔔 **Alerts:** Configure Render alerts

---

## Additional Help

- 📖 Render Docs: https://render.com/docs
- 🆘 Render Support: support@render.com
- 💬 Community: https://community.render.com

---

**Success! 🎉 Your app is now live on the internet!**
