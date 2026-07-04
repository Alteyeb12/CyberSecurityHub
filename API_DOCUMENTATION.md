# CyberInsight API Documentation

## Base URL

```
https://your-domain.onrender.com/api
```

## Authentication

All API endpoints require authentication via cookies. First, login through the web interface.

## Threat Endpoints

### Get All Threats

```http
GET /api/threat/all
```

**Response:**
```json
[
  {
    "id": 1,
    "userId": 1,
    "threatType": "SQL Injection",
    "description": "Attempted SQL injection on login form",
    "severity": "High",
    "sourceIP": "192.168.1.100",
    "targetAsset": "Web Server",
    "detectedAt": "2026-07-04T10:30:00Z",
    "isResolved": false,
    "alertCount": 5
  }
]
```

### Get Critical Threats

```http
GET /api/threat/critical
```

**Response:**
```json
[
  {
    "id": 2,
    "severity": "Critical",
    "threatType": "Ransomware Attack",
    ...
  }
]
```

### Get Threat Statistics

```http
GET /api/threat/statistics
```

**Response:**
```json
{
  "SQL Injection": 15,
  "Brute Force": 8,
  "DDoS Attack": 3,
  "Malware Detection": 2
}
```

### Create Threat Alert

```http
POST /api/threat/create
Content-Type: application/json
```

**Request Body:**
```json
{
  "threatType": "Brute Force Attack",
  "description": "Multiple failed login attempts detected",
  "severity": "High",
  "sourceIP": "203.0.113.45",
  "targetAsset": "Database Server"
}
```

**Response:**
```json
{
  "id": 3,
  "userId": 1,
  "threatType": "Brute Force Attack",
  "severity": "High",
  "alertCount": 1,
  "detectedAt": "2026-07-04T11:00:00Z"
}
```

## Account Endpoints

### Register

```http
POST /Account/Register
Content-Type: application/x-www-form-urlencoded
```

**Parameters:**
- `email`: User email
- `firstName`: First name
- `lastName`: Last name
- `password`: Password (min 8 characters)
- `confirmPassword`: Confirm password
- `companyName`: Company name

### Login

```http
POST /Account/Login
Content-Type: application/x-www-form-urlencoded
```

**Parameters:**
- `email`: User email
- `password`: User password
- `rememberMe`: Remember login (optional)

### Logout

```http
POST /Account/Logout
```

## Subscription Endpoints

### Get Available Plans

```http
GET /Subscription/Plans
```

**Available Plans:**
- Free: $0/month
- Professional: $99/month
- Enterprise: $299/month

### Checkout

```http
POST /Subscription/Checkout
Content-Type: application/x-www-form-urlencoded
```

**Parameters:**
- `plan`: Plan name (Professional or Enterprise)

**Response:**
- Redirects to Stripe checkout page

### Subscription Success

```http
GET /Subscription/Success
```

Returned after successful payment.

## Error Responses

### 400 Bad Request

```json
{
  "error": "Invalid request parameters",
  "details": "Email is required"
}
```

### 401 Unauthorized

```json
{
  "error": "Authentication required",
  "message": "Please login to continue"
}
```

### 403 Forbidden

```json
{
  "error": "Access denied",
  "message": "You don't have permission to access this resource"
}
```

### 500 Internal Server Error

```json
{
  "error": "Server error",
  "message": "Please try again later"
}
```

## Rate Limiting

- **API Calls**: 1000 requests per hour
- **Login Attempts**: 5 attempts per 15 minutes
- **Payment Requests**: 100 requests per hour

## Best Practices

1. **Always use HTTPS**: Never send sensitive data over HTTP
2. **Handle Errors**: Implement proper error handling
3. **Retry Logic**: Implement exponential backoff for retries
4. **Cache Results**: Cache threat data when appropriate
5. **Monitor Usage**: Track API usage for optimization

## Example Usage

### JavaScript/Fetch

```javascript
// Get all threats
fetch('/api/threat/all', {
  method: 'GET',
  credentials: 'include'
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.error('Error:', error));

// Create threat alert
fetch('/api/threat/create', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  credentials: 'include',
  body: JSON.stringify({
    threatType: 'SQL Injection',
    description: 'Detected SQL injection attempt',
    severity: 'High',
    sourceIP: '192.168.1.1',
    targetAsset: 'Web Server'
  })
})
.then(response => response.json())
.then(data => console.log('Threat created:', data));
```

### Python/Requests

```python
import requests

url = 'https://cyberinsight.onrender.com/api'
session = requests.Session()

# Login first
login_data = {
    'email': 'user@example.com',
    'password': 'password123'
}
session.post(f'{url}/../Account/Login', data=login_data)

# Get threats
response = session.get(f'{url}/threat/all')
threats = response.json()
print(threats)
```

---

**Last Updated**: July 4, 2026
**API Version**: 1.0