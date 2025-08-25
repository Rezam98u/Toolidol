# 📊 LinkedIn Analytics Dashboard

A modern full-stack application for visualizing LinkedIn organization analytics data. Built with ASP.NET Core Web API and Angular 18, featuring real-time data visualization, responsive design, and robust error handling.

## ✨ Features

- 📈 **Real-time LinkedIn Analytics**: Page views, follower growth, and engagement metrics
- 🎨 **Modern UI**: Material Design with responsive layout
- 📱 **Cross-Platform**: Works on desktop, tablet, and mobile devices
- ⚡ **Performance Optimized**: Parallel API calls and efficient data loading
- 🛡️ **Resilient Architecture**: Built-in retry logic and error handling
- 🔧 **Mock Mode**: Development-friendly with sample data (no LinkedIn API required)
- 🚀 **Production Ready**: Comprehensive logging and monitoring

## 🏗️ Architecture

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   Angular 18    │────│  ASP.NET Core    │────│  LinkedIn API   │
│   Frontend      │    │   Web API        │    │                 │
│                 │    │                  │    │                 │
│ • Material UI   │    │ • REST Endpoints │    │ • Marketing API │
│ • HTTP Client   │    │ • Dependency DI  │    │ • OAuth 2.0     │
│ • RxJS          │    │ • Error Handling │    │ • Rate Limiting │
│ • Proxy Config  │    │ • Mock Data      │    │                 │
└─────────────────┘    └──────────────────┘    └─────────────────┘
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI](https://cli.angular.io/) (`npm install -g @angular/cli`)

### Installation & Running

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/linkedin-analytics-dashboard.git
   cd linkedin-analytics-dashboard
   ```

2. **Start Backend** (Terminal 1)
   ```bash
   cd Toolidol.Api
   dotnet restore
   dotnet run
   ```
   ✅ Backend runs on `http://localhost:5116`

3. **Start Frontend** (Terminal 2)
   ```bash
   cd frontend
   npm install
   ng serve
   ```
   ✅ Frontend runs on `http://localhost:4200`

4. **Open Application**
   - Navigate to `http://localhost:4200`
   - Dashboard loads with sample LinkedIn analytics data

### 🎯 What You'll See

The dashboard displays three main KPI cards:
- **Page Views**: Total views and unique visitors
- **Followers**: Current count with gains/losses  
- **Engagement**: Rate percentage and impressions

*Note: Currently shows mock data - perfect for development and testing!*

## 📁 Project Structure

```
linkedin-analytics-dashboard/
├── 📂 Toolidol.Api/                    # Backend (.NET 8 Web API)
│   ├── 📂 Controllers/
│   │   └── LinkedInAnalyticsController.cs    # API endpoints
│   ├── 📂 Services/
│   │   ├── LinkedInAnalyticsService.cs       # Business logic
│   │   ├── LinkedInOrganizationService.cs    # Auth & config
│   │   └── HttpService.cs                    # HTTP client wrapper
│   ├── 📂 Models/DTOs/LinkedInAnalytics/
│   │   ├── OrganizationPageViewsResponse.cs  # Page views model
│   │   ├── FollowersResponse.cs              # Followers model
│   │   └── OrganizationShareStatisticsResponse.cs # Engagement model
│   ├── 📂 Options/
│   │   └── LinkedInOptions.cs                # Configuration model
│   ├── appsettings.json                      # App configuration
│   └── Program.cs                            # App startup
│
├── 📂 frontend/                        # Frontend (Angular 18)
│   ├── 📂 src/app/
│   │   ├── 📂 components/analytics-dashboard/
│   │   │   ├── analytics-dashboard.component.ts    # Main dashboard logic
│   │   │   ├── analytics-dashboard.component.html  # Dashboard template
│   │   │   └── analytics-dashboard.component.scss  # Dashboard styles
│   │   ├── 📂 services/
│   │   │   └── linkedin-analytics.service.ts       # API service
│   │   ├── app.routes.ts                           # Routing config
│   │   └── app.config.ts                           # App configuration
│   ├── proxy.conf.json                       # Dev proxy config
│   ├── package.json                          # Dependencies
│   └── angular.json                          # Angular CLI config
│
├── README.md                           # This file
├── PROCESS FLOW.md                     # Detailed technical flow
└── EXECUTIVE_PROCESS_FLOW.md          # Business-focused documentation
```

## 🔄 How It Works (Step by Step)

### 1. **User Opens Dashboard**
```
http://localhost:4200 → Angular Router → /dashboard → AnalyticsDashboardComponent
```

### 2. **Component Loads Data**
```typescript
// Makes 3 parallel API calls
forkJoin({
  pageViews: this.api.getPageViews(),     // → /api/linkedin-analytics/page-views
  followers: this.api.getFollowers(),     // → /api/linkedin-analytics/followers
  engagement: this.api.getEngagement()    // → /api/linkedin-analytics/engagement
})
```

### 3. **Proxy Routes Requests**
```
Frontend: /api/linkedin-analytics/* 
    ↓ (proxy.conf.json)
Backend: http://localhost:5116/api/linkedin-analytics/*
```

### 4. **Backend Processes Requests**
```csharp
[HttpGet("page-views")]
public async Task<IActionResult> GetPageViews() {
    var result = await _analyticsService.GetPageViewsAsync();
    return Ok(result);
}
```

### 5. **Service Returns Data**
```csharp
// Currently in Mock Mode (Mock: true)
if (IsMock()) {
    return new OrganizationPageViewsResponse {
        Elements = new List<OrganizationPageViewElement> {
            new() { PageViews = 123, UniquePageViews = 90, ... }
        }
    };
}
// In production: calls real LinkedIn API
```

### 6. **Frontend Displays Results**
```html
<mat-card class="kpi-card">
  <span class="kpi-number">{{ formatNumber(getPageViews()) }}</span>
  <!-- Shows: 123 (from mock data) -->
</mat-card>
```

## ⚙️ Configuration

### Current Settings (`Toolidol.Api/appsettings.json`)
```json
{
  "LinkedIn": {
    "ApiBaseUrl": "https://api.linkedin.com/v2",
    "OrganizationId": "",        // Not needed for mock mode
    "AccessToken": "",           // Not needed for mock mode
    "Mock": true                 // 🔧 Using sample data
  }
}
```

### For Production (Real LinkedIn Data)
```json
{
  "LinkedIn": {
    "ApiBaseUrl": "https://api.linkedin.com/v2",
    "OrganizationId": "12345678",     // Your LinkedIn company ID
    "AccessToken": "your-token-here", // OAuth 2.0 access token
    "Mock": false                     // 🚀 Use real LinkedIn API
  }
}
```

## 📊 API Endpoints

| Endpoint | Method | Description | Mock Data |
|----------|--------|-------------|-----------|
| `/api/linkedin-analytics/page-views` | GET | Page view statistics | ✅ Available |
| `/api/linkedin-analytics/followers` | GET | Follower growth data | ✅ Available |
| `/api/linkedin-analytics/engagement` | GET | Post engagement metrics | ✅ Available |

### Sample Response (Page Views)
```json
{
  "elements": [
    {
      "pageViews": 123,
      "uniquePageViews": 90,
      "timeRange": {
        "start": 1703203200000,
        "end": 1703289600000
      },
      "organizationalEntity": "urn:li:organization:123"
    }
  ]
}
```

## 🛠️ Development Commands

### Backend (.NET)
```bash
cd Toolidol.Api
dotnet run          # Start development server
dotnet build        # Build application
dotnet watch run    # Auto-restart on changes
```

### Frontend (Angular)
```bash
cd frontend
ng serve            # Start development server
ng build            # Build for production
ng test             # Run unit tests
ng lint             # Run code linter
```

## 🔐 LinkedIn API Setup (For Production)

### 1. Create LinkedIn Developer App
1. Go to [LinkedIn Developer Portal](https://developer.linkedin.com/)
2. Create new application
3. Request **Marketing Developer Platform** access
4. Add your organization

### 2. Required Permissions
- `r_organization_social` - Organization social data
- `r_basicprofile` - Basic profile information
- `r_ads_reporting` - Advertising reporting data

### 3. Get Your Organization ID
```bash
curl -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  "https://api.linkedin.com/v2/organizations?q=administeredBy"
```

### 4. Update Configuration
Set `Mock: false` and add your credentials to `appsettings.json`

## 🚀 Deployment

### Backend Deployment
```bash
dotnet publish -c Release -o ./publish
# Deploy ./publish folder to your server
```

### Frontend Deployment
```bash
ng build --configuration production
# Deploy ./dist/frontend folder to web server
```

## 🧪 Testing

### Manual Testing
1. Start both backend and frontend
2. Open `http://localhost:4200`
3. Verify all three KPI cards show data
4. Check browser console for any errors
5. Test responsive design on mobile

### API Testing
```bash
# Test individual endpoints
curl http://localhost:5116/api/linkedin-analytics/page-views
curl http://localhost:5116/api/linkedin-analytics/followers
curl http://localhost:5116/api/linkedin-analytics/engagement
```
