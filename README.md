# 📊 LinkedIn Analytics Dashboard

A modern full-stack application for visualizing LinkedIn organization analytics data. Built with ASP.NET Core Web API and Angular 18, featuring real-time data visualization, responsive design, and robust error handling.

## ✨ Features

- 📈 **Real-time LinkedIn Analytics**: Page views, follower growth, and engagement metrics
- 🎨 **Modern UI**: Material Design with dark/light theme support
- 📱 **Responsive Design**: Works seamlessly on desktop, tablet, and mobile
- ⚡ **Performance Optimized**: Efficient data loading with caching capabilities
- 🛡️ **Resilient Architecture**: Built-in retry logic and circuit breakers for API reliability
- 🔧 **Mock Mode**: Development-friendly mock data for testing without LinkedIn API
- 📊 **Data Visualization Ready**: Chart.js integration for advanced analytics
- 🚀 **Production Ready**: Comprehensive error handling and logging

## 🏗️ Architecture

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   Angular 18    │────│  ASP.NET Core    │────│  LinkedIn API   │
│   Frontend      │    │   Web API        │    │                 │
│                 │    │                  │    │                 │
│ • Material UI   │    │ • REST Endpoints │    │ • Marketing API │
│ • Reactive Forms│    │ • Polly Policies │    │ • OAuth 2.0     │
│ • HTTP Client   │    │ • DI Container   │    │ • Rate Limiting │
│ • RxJS          │    │ • Logging        │    │                 │
└─────────────────┘    └──────────────────┘    └─────────────────┘
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI](https://cli.angular.io/) (`npm install -g @angular/cli`)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/linkedin-analytics-dashboard.git
   cd linkedin-analytics-dashboard
   ```

2. **Backend Setup**
   ```bash
   cd Toolidol.Api
   dotnet restore
   dotnet run
   ```
   Backend will be available at `http://localhost:5116`

3. **Frontend Setup**
   ```bash
   cd frontend
   npm install
   ng serve
   ```
   Frontend will be available at `http://localhost:4200`

### Development Mode

The application starts in **mock mode** by default, providing sample data for development without requiring LinkedIn API credentials.

## 🔧 Configuration

### Backend Configuration (`appsettings.json`)

```json
{
  "LinkedIn": {
    "ApiBaseUrl": "https://api.linkedin.com/v2",
    "OrganizationId": "your-linkedin-org-id",
    "AccessToken": "your-access-token",
    "Mock": true
  }
}
```

### Environment Variables (Production)

```bash
# LinkedIn Configuration
LINKEDIN__ORGANIZATIONID=123456789
LINKEDIN__ACCESSTOKEN=your-linkedin-access-token
LINKEDIN__MOCK=false
```

## 📋 LinkedIn API Setup

To use real LinkedIn data instead of mock data:

### 1. Create LinkedIn Developer Application

1. Visit [LinkedIn Developer Portal](https://developer.linkedin.com/)
2. Create a new application
3. Request access to **Marketing Developer Platform**
4. Add your organization to the application

### 2. Configure Permissions

Required permissions:
- `r_organization_social`
- `r_basicprofile` 
- `r_ads_reporting`

### 3. Get Organization ID

```bash
# Find your LinkedIn Organization ID
curl -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  "https://api.linkedin.com/v2/organizations?q=administeredBy"
```

### 4. Update Configuration

```json
{
  "LinkedIn": {
    "OrganizationId": "123456789",
    "AccessToken": "YOUR_ACCESS_TOKEN",
    "Mock": false
  }
}
```

## 🛠️ Development

### Project Structure

```
├── Toolidol.Api/                 # ASP.NET Core Web API
│   ├── Controllers/              # API Controllers
│   ├── Services/                 # Business Logic Layer
│   ├── Models/DTOs/              # Data Transfer Objects
│   ├── Options/                  # Configuration Models
│   └── Program.cs                # Application Entry Point
│
├── frontend/                     # Angular Application
│   ├── src/app/
│   │   ├── components/           # UI Components
│   │   ├── services/             # HTTP Services
│   │   └── app.routes.ts         # Routing Configuration
│   ├── proxy.conf.json           # Development Proxy
│   └── angular.json              # Angular CLI Configuration
│
└── README.md                     # This file
```

### Available Scripts

**Backend:**
```bash
dotnet run                        # Start development server
dotnet build                      # Build application
dotnet test                       # Run tests
```

**Frontend:**
```bash
ng serve                          # Start development server
ng build                          # Build for production
ng test                           # Run unit tests
ng lint                           # Run linter
```

## 📊 API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/linkedin-analytics/page-views` | GET | Organization page view statistics |
| `/api/linkedin-analytics/followers` | GET | Follower growth and statistics |
| `/api/linkedin-analytics/engagement` | GET | Post engagement metrics |

### Response Examples

**Page Views Response:**
```json
{
  "elements": [
    {
      "pageViews": 1250,
      "uniquePageViews": 890,
      "timeRange": {
        "start": 1703203200000,
        "end": 1703289600000
      },
      "organizationalEntity": "urn:li:organization:123456"
    }
  ]
}
```

**Engagement Response:**
```json
{
  "elements": [
    {
      "totalShareStatistics": {
        "shareCount": 15,
        "impressionCount": 5420,
        "clickCount": 128,
        "engagement": 0.0826,
        "likeCount": 342,
        "commentCount": 18,
        "shareMentionsCount": 5
      }
    }
  ]
}
```

### Resilience Patterns

- **Retry Policy**: Exponential backoff (5 attempts)
- **Circuit Breaker**: Opens after 5 failures, 1-minute timeout
- **Rate Limiting**: Handles LinkedIn API rate limits (429 responses)
- **Request Logging**: Comprehensive HTTP request/response logging

### UI Components

- **KPI Cards**: Real-time metrics with trend indicators
- **Loading States**: Skeleton screens and progress indicators
- **Error Handling**: User-friendly error messages with retry options
- **Responsive Grid**: Adaptive layout for all screen sizes

### Production Checklist

- [ ] Set `Mock: false` in production configuration
- [ ] Configure LinkedIn API credentials securely
- [ ] Enable HTTPS redirect
- [ ] Configure CORS for production domain
- [ ] Set up application monitoring
- [ ] Configure rate limiting
- [ ] Set up backup and recovery



