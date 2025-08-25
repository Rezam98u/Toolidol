# LinkedIn Analytics Dashboard - Process Flow Guide

## Architecture Overview

This is a full-stack application with:
- **Backend**: ASP.NET Core Web API (.NET 8)
- **Frontend**: Angular 18 with Material Design
- **API Integration**: LinkedIn Marketing Developer Platform

## Complete Process Flow

### 1. Application Startup

#### Backend Initialization (`Program.cs`)
1. **Service Registration**:
   - Configures dependency injection container
   - Registers HTTP client factory with "LinkedInClient"
   - Sets up Polly policies for resilience (retry + circuit breaker)
   - Adds logging delegating handler for HTTP requests
   - Binds LinkedIn configuration from appsettings.json

2. **HTTP Client Configuration**:
   - **Retry Policy**: 5 attempts with exponential backoff
   - **Circuit Breaker**: Opens after 5 failures, stays open for 1 minute
   - **Rate Limiting**: Handles 429 (Too Many Requests) responses
   - **Logging**: Tracks all HTTP requests/responses with timing

3. **Memory Cache**: Configured for potential caching of API responses

#### Frontend Initialization
1. **Angular Bootstrap**: Loads `AppComponent` with routing
2. **Service Providers**: HTTP client, animations, routing configured
3. **Proxy Configuration**: Development proxy redirects `/api` to `localhost:5116`

### 2. User Navigation Flow

#### Step 1: Initial Page Load
```
User visits app → Angular Router → Redirects to /dashboard → AnalyticsDashboardComponent loads
```

#### Step 2: Component Initialization (`AnalyticsDashboardComponent`)
1. **OnInit Lifecycle**:
   - Sets `loading = true`
   - Calls `loadAnalytics()`
   - Applies theme settings

2. **Template Rendering**:
   - Shows loading skeleton with animated placeholders
   - Displays spinner with "Loading analytics data..." message

### 3. Data Fetching Process

#### Step 1: Frontend Service Call (`LinkedInAnalyticsService`)
```typescript
// Parallel API calls using forkJoin
forkJoin({
  pageViews: this.api.getPageViews(),     // → /api/linkedin-analytics/page-views
  followers: this.api.getFollowers(),     // → /api/linkedin-analytics/followers  
  engagement: this.api.getEngagement()    // → /api/linkedin-analytics/engagement
})
```

#### Step 2: Angular HTTP Proxy
- Development proxy intercepts `/api` requests
- Forwards to ASP.NET Core API at `http://localhost:5116`

#### Step 3: API Controller Processing (`LinkedInAnalyticsController`)
Each endpoint follows the same pattern:
1. **Route Matching**: ASP.NET Core routes request to appropriate action
2. **Dependency Injection**: Controller receives `ILinkedInAnalyticsService`
3. **Service Delegation**: Controller calls service method
4. **Response Wrapping**: Returns `Ok(result)` with data

#### Step 4: Service Layer Processing (`LinkedInAnalyticsService`)

**Mock Mode (Default - `Mock: true`):**
```csharp
if (IsMock()) {
    return new OrganizationPageViewsResponse {
        Elements = new List<OrganizationPageViewElement> {
            new OrganizationPageViewElement { 
                PageViews = 123, 
                UniquePageViews = 90,
                TimeRange = new TimeRange { /* current time range */ },
                OrganizationalEntity = "urn:li:organization:123"
            }
        }
    };
}
```

**Live Mode (`Mock: false`):**
1. **Token Retrieval**: `await _orgService.GetAccessTokenAsync()`
2. **Organization URN**: `await _orgService.GetOrganizationUrnAsync()`
3. **Validation**: Ensures token and organization ID exist
4. **API Call Construction**:
   ```csharp
   var query = new Dictionary<string, string> {
       ["q"] = "organization",
       ["organization"] = orgUrn
   };
   ```
5. **HTTP Request**: `_httpService.GetFromLinkedInApiAsync<T>()`

#### Step 5: HTTP Service Layer (`HttpService`)
1. **Client Creation**: Gets "LinkedInClient" from factory
2. **URL Construction**: Combines base URL + relative path + query parameters
3. **Request Building**:
   ```csharp
   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
   ```
4. **Polly Execution**: Request goes through retry/circuit breaker policies
5. **Response Processing**: Deserializes JSON to strongly-typed DTOs

#### Step 6: LinkedIn API Integration
**Real API Endpoints** (when not in mock mode):
- `GET /v2/organizationPageStatistics?q=organization&organization={urn}`
- `GET /v2/organizationalEntityFollowerStatistics?q=organizationalEntity&organizationalEntity={urn}`
- `GET /v2/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={urn}`

### 4. Response Processing Flow

#### Step 1: API Response Handling
1. **HTTP Status Check**: `response.EnsureSuccessStatusCode()`
2. **JSON Deserialization**: Maps to typed DTOs:
   - `OrganizationPageViewsResponse`
   - `FollowersResponse` 
   - `OrganizationShareStatisticsResponse`

#### Step 2: Controller Response
- Wraps data in `Ok()` result
- ASP.NET Core serializes to JSON
- Returns HTTP 200 with analytics data

#### Step 3: Frontend Data Processing
1. **Observable Resolution**: `forkJoin` completes when all 3 APIs return
2. **Success Handler**:
   ```typescript
   next: (results) => {
     this.analyticsData = results;
     this.updateCharts();
   }
   ```
3. **Chart Updates**: Processes raw data for visualization
4. **Loading State**: Sets `loading = false`

### 5. Configuration Management

#### LinkedIn Settings (`appsettings.json`)
```json
{
  "LinkedIn": {
    "ApiBaseUrl": "https://api.linkedin.com/v2",
    "OrganizationId": "",           // LinkedIn Company/Organization ID
    "AccessToken": "",              // OAuth 2.0 Bearer Token
    "Mock": true                    // Toggle between mock/real data
  }
}
```

#### Environment-Specific Config
- **Development**: Mock mode enabled, detailed logging
- **Production**: Real LinkedIn API integration required


## Development Workflow

1. **Start Backend**: `dotnet run` (serves on port 5116)
2. **Start Frontend**: `ng serve` (serves on port 4200, proxies API calls)
3. **Mock Data**: Default configuration returns sample data
4. **Real Integration**: Set `Mock: false` and provide LinkedIn credentials

## Production Deployment Considerations

1. **LinkedIn App Registration**: Obtain OAuth credentials
2. **API Permissions**: Ensure proper LinkedIn Marketing API access
3. **Environment Variables**: Secure credential storage
4. **CORS Configuration**: Configure for production domain
5. **HTTPS**: Required for LinkedIn API integration
