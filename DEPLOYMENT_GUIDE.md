# Render Deployment Guide

## Step 1: Create a Render Account

1. Visit [render.com](https://render.com)
2. Sign up or log in
3. Connect your GitHub account

## Step 2: Set up Database

### Option 1: Azure SQL Database

```
1. Go to portal.azure.com
2. Create new SQL Server
3. Copy connection string
```

### Option 2: External SQL Server

```
- Use any managed SQL Server service
- AWS RDS SQL Server
- DigitalOcean Managed Database
```

## Step 3: Create Web Service

1. Go to Render Dashboard
2. Click "New" → "Web Service"
3. Select "Build and deploy from a Git repository"
4. Connect repository: `Alteyeb12/CyberSecurityHub`
5. Select branch: `develop`

## Step 4: Configure Settings

### Basic Information:
- **Name**: CyberInsight
- **Region**: Choose closest to your location
- **Branch**: develop
- **Runtime**: Docker

### Commands:
- **Build Command**: `docker build -t cyberinsight .`
- **Start Command**: `dotnet CyberInsight.dll`

## Step 5: Environment Variables

Add in "Environment" section:

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
ConnectionStrings__DefaultConnection=Server=your_server;Database=CyberInsightDB;User Id=sa;Password=your_password;TrustServerCertificate=true;
Stripe__SecretKey=sk_test_your_secret_key
Stripe__PublicKey=pk_test_your_public_key
Stripe__WebhookSecret=whsec_your_webhook_secret
```

## Step 6: Choose Plan

- **Starter**: $7/month (for testing)
- **Standard**: $12/month (recommended for production)
- **Pro**: $26/month (high traffic)

## Step 7: Deploy

1. Click "Create Web Service"
2. Wait for build to complete (5-10 minutes)
3. Get URL: `https://cyberinsight-xxxxx.onrender.com`

## Step 8: Database Migrations

After deployment:

```bash
# Connect to your application and run:
# Migrations run automatically on startup
# Check logs to confirm
```

## Troubleshooting

### Error: Connection Timeout

```
✓ Verify connection string
✓ Check database is accessible
✓ Test locally first
✓ Whitelist Render IP in firewall
```

### Error: Service Failed to Build

```
✓ Check Render logs
✓ Verify Dockerfile exists
✓ Check .NET version compatibility
✓ Ensure all dependencies are included
```

### Error: Application Won't Start

```
✓ Verify environment variables
✓ Check connection string format
✓ Review application logs
✓ Ensure database migrations run successfully
```

## Monitoring

1. **Logs**: Monitor application output
2. **Metrics**: View CPU, memory, and bandwidth
3. **Alerts**: Set up notifications for issues
4. **Health Checks**: Enable automatic restarts

## Auto-Deploy

Render automatically redeploys when you push to the `develop` branch:

```bash
git add .
git commit -m "Your message"
git push origin develop
```

## Important Security Notes

- ✅ Always use HTTPS in production
- ✅ Never commit sensitive keys
- ✅ Use environment variables for secrets
- ✅ Enable CORS only if needed
- ✅ Implement rate limiting
- ✅ Regular database backups
- ✅ Monitor API usage
- ✅ Update dependencies regularly

---

**For support**: Visit Render documentation or contact support