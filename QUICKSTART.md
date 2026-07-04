# 🚀 Quick Start Guide - CyberInsight

## Local Installation and Execution

### Prerequisites
```bash
- .NET 8.0 SDK or later
- SQL Server 2019 or Azure SQL
- Git
- Visual Studio 2022 or VS Code (optional)
```

### Step 1: Clone Repository
```bash
git clone https://github.com/Alteyeb12/CyberSecurityHub.git
cd CyberSecurityHub
git checkout develop
```

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Configure Database

Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=CyberInsightDB;Integrated Security=true;TrustServerCertificate=true;"
}
```

### Step 4: Apply Migrations
```bash
dotnet ef database update
```

### Step 5: Run Application
```bash
dotnet run
```

Application will open at: `https://localhost:5001`

---

## Running with Docker

### Prerequisite: Install Docker

### Build and Run:
```bash
# Build Docker image
docker build -t cyberinsight .

# Run container
docker run -p 8080:80 cyberinsight
```

### Using Docker Compose:
```bash
docker-compose -f docker-compose.dev.yml up --build
```

Access application at: `http://localhost:8080`

---

## Basic Usage Steps

### 1️⃣ Create New Account
- Go to Registration page
- Enter account information:
  - Email
  - First and Last Name
  - Company Name
  - Password (minimum 8 characters)

### 2️⃣ Login
- Use your account credentials
- You'll get a free plan by default

### 3️⃣ Explore Dashboard
- View threat statistics
- See latest detected threats
- Filter by severity level

### 4️⃣ Upgrade Plan (Optional)
- Go to "Plans"
- Choose Professional or Enterprise plan
- Complete payment via Stripe

---

## Configure Stripe Keys (For Payments)

### 1. Get API Keys:
```
1. Go to https://stripe.com
2. Create account or login
3. Navigate to Dashboard → API Keys
4. Copy:
   - Publishable Key (pk_test_...)
   - Secret Key (sk_test_...)
```

### 2. Add Keys:

In `appsettings.json`:
```json
"Stripe": {
  "PublicKey": "pk_test_your_key_here",
  "SecretKey": "sk_test_your_key_here"
}
```

---

## API - Usage Examples

### Get All Threats:
```bash
curl -X GET "https://localhost:5001/api/threat/all" \
  -H "Cookie: your_auth_cookie"
```

### Create Threat Alert:
```bash
curl -X POST "https://localhost:5001/api/threat/create" \
  -H "Content-Type: application/json" \
  -H "Cookie: your_auth_cookie" \
  -d '{
    "threatType": "SQL Injection",
    "description": "SQL injection attempt detected",
    "severity": "High",
    "sourceIP": "192.168.1.100",
    "targetAsset": "Web Server"
  }'
```

### Get Statistics:
```bash
curl -X GET "https://localhost:5001/api/threat/statistics" \
  -H "Cookie: your_auth_cookie"
```

---

## Troubleshooting

### ❌ Error: "Connection string not found"
**Solution:**
- Ensure `appsettings.json` exists
- Verify connection string
- Check SQL Server accessibility

### ❌ Error: "Database update error"
**Solution:**
```bash
# Remove old migrations
rm -r Migrations

# Create new migrations
dotnet ef migrations add InitialCreate

# Apply migrations
dotnet ef database update
```

### ❌ Error: "Port already in use"
**Solution:**
```bash
# Change port in launchSettings.json
# Or use different port
dotnet run --urls "https://localhost:5002"
```

### ❌ Error: "Stripe connection failed"
**Solution:**
- Verify Stripe API keys
- Check internet connection
- Test with Stripe test keys

---

## Deploying to Render

### Step 1: Prepare Repository
```bash
git add .
git commit -m "Prepare for Render deployment"
git push origin develop
```

### Step 2: Create Web Service on Render
```
1. Go to https://render.com
2. Click "New" → "Web Service"
3. Select "Build and deploy from a Git repository"
4. Connect your GitHub repository
5. Select develop branch
```

### Step 3: Configure Settings
```
Name: CyberInsight
Region: Choose closest
Runtime: Docker
Build Command: docker build -t cyberinsight .
Start Command: dotnet CyberInsight.dll
```

### Step 4: Environment Variables
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
ConnectionStrings__DefaultConnection=your_connection_string
Stripe__SecretKey=sk_live_your_key
Stripe__PublicKey=pk_live_your_key
Stripe__WebhookSecret=whsec_your_secret
```

### Step 5: Deploy
```
Click "Create Web Service"
Wait for build completion (5-10 minutes)
You'll get a URL like: https://cyberinsight-xxxxx.onrender.com
```

---

## Production Tips

✅ **Security:**
- Use HTTPS only
- Don't share API keys
- Use environment variables for sensitive data
- Update dependencies regularly

✅ **Performance:**
- Use caching for static data
- Optimize database queries
- Use CDN for static files
- Monitor resource usage

✅ **Reliability:**
- Perform regular backups
- Monitor logs
- Test application before launch
- Have a disaster recovery plan

---

## Useful Resources

- 📚 [Microsoft .NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- 🗄️ [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- 💳 [Stripe Documentation](https://stripe.com/docs/api)
- 🐳 [Docker Documentation](https://docs.docker.com/)
- ☁️ [Render Documentation](https://render.com/docs)

---

## Support and Help

- 📧 Email: support@cyberinsight.com
- 💬 Forum: https://github.com/Alteyeb12/CyberSecurityHub/discussions
- 🐛 Report Issues: https://github.com/Alteyeb12/CyberSecurityHub/issues

---

**Created by**: Al-Tayyib Al-Ustura
**Date**: July 4, 2026
**Version**: 1.0.0
