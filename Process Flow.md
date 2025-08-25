Complete Process Flow
1. Application Startup
Backend Initialization (Program.cs)

Service Registration:

Configures dependency injection container
Registers HTTP client factory with "LinkedInClient"
Sets up Polly policies for resilience (retry + circuit breaker)
Adds logging delegating handler for HTTP requests
Binds LinkedIn configuration from appsettings.json


HTTP Client Configuration:

Retry Policy: 5 attempts with exponential backoff
Circuit Breaker: Opens after 5 failures, stays open for 1 minute
Rate Limiting: Handles 429 (Too Many Requests) responses
Logging: Tracks all HTTP requests/responses with timing


Memory Cache: Configured for potential caching of API responses

Frontend Initialization

Angular Bootstrap: Loads AppComponent with routing
Service Providers: HTTP client, animations, routing configured
Proxy Configuration: Development proxy redirects /api to localhost:5116

2. User Navigation Flow
Step 1: Initial Page Load
User visits app → Angular Router → Redirects to /dashboard → AnalyticsDashboardComponent loads
Step 2: Component Initialization (AnalyticsDashboardComponent)

OnInit Lifecycle:

Sets loading = true
Calls loadAnalytics()
Applies theme settings


Template Rendering:

Shows loading skeleton with animated placeholders
Displays spinner with "Loading analytics data..." message



3. Data Fetching Process
Step 1: Frontend Service Call (LinkedInAnalyticsService)
typescript// Parallel API calls using forkJoin
forkJoin({
  pageViews: this.api.getPageViews(),     // → /api/linkedin-analytics/page-views
  followers: this.api.getFollowers(),     // → /api/linkedin-analytics/followers  
  engagement: this.api.getEngagement()    // → /api/linkedin-analytics/engagement
})
Step 2: Angular HTTP Proxy

Development proxy intercepts /api requests
Forwards to ASP.NET Core API at http://localhost:5116

Step 3: API Controller Processing (LinkedInAnalyticsController)
Each endpoint follows the same pattern:

Route Matching: ASP.NET Core routes request to appropriate action
Dependency Injection: Controller receives ILinkedInAnalyticsService
Service Delegation: Controller calls service method
Response Wrapping: Returns Ok(result) with data

Step 4: Service Layer Processing (LinkedInAnalyticsService)
Mock Mode (Default - Mock: true):
csharpif (IsMock()) {
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
Live Mode (Mock: false):

Token Retrieval: await _orgService.GetAccessTokenAsync()
Organization URN: await _orgService.GetOrganizationUrnAsync()
Validation: Ensures token and organization ID exist
API Call Construction:
csharpvar query = new Dictionary<string, string> {
    ["q"] = "organization",
    ["organization"] = orgUrn
};

HTTP Request: _httpService.GetFromLinkedInApiAsync<T>()

Step 5: HTTP Service Layer (HttpService)

Client Creation: Gets "LinkedInClient" from factory
URL Construction: Combines base URL + relative path + query parameters
Request Building:
csharprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

Polly Execution: Request goes through retry/circuit breaker policies
Response Processing: Deserializes JSON to strongly-typed DTOs

Step 6: LinkedIn API Integration
Real API Endpoints (when not in mock mode):

GET /v2/organizationPageStatistics?q=organization&organization={urn}
GET /v2/organizationalEntityFollowerStatistics?q=organizationalEntity&organizationalEntity={urn}
GET /v2/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={urn}

4. Response Processing Flow
Step 1: API Response Handling

HTTP Status Check: response.EnsureSuccessStatusCode()
JSON Deserialization: Maps to typed DTOs:

OrganizationPageViewsResponse
FollowersResponse
OrganizationShareStatisticsResponse



Step 2: Controller Response

Wraps data in Ok() result
ASP.NET Core serializes to JSON
Returns HTTP 200 with analytics data

Step 3: Frontend Data Processing

Observable Resolution: forkJoin completes when all 3 APIs return
Success Handler:
typescriptnext: (results) => {
  this.analyticsData = results;
  this.updateCharts();
}

Chart Updates: Processes raw data for visualization
Loading State: Sets loading = false

5. UI Rendering Process
Step 1: KPI Cards Display
Template processes data through helper methods:
typescript// Page Views Card
{{ formatNumber(getPageViews()) }}        // this.analyticsData.pageViews.elements[0].pageViews
{{ formatNumber(getUniqueVisitors()) }}   // this.analyticsData.pageViews.elements[0].uniquePageViews

// Followers Card  
{{ formatNumber(getTotalFollowers()) }}   // this.analyticsData.followers.elements[0].followerCounts
+{{ getFollowersGained() }}              // this.analyticsData.followers.elements[0].followerGains
-{{ getFollowersLost() }}                // this.analyticsData.followers.elements[0].followerLosses

// Engagement Card
{{ formatPercentage(getEngagementRate()) }} // this.analyticsData.engagement.elements[0].totalShareStatistics.engagement
{{ formatNumber(getImpressions()) }}        // this.analyticsData.engagement.elements[0].totalShareStatistics.impressionCount
Step 2: Chart Visualization (Prepared but not rendered)
The component prepares chart configurations:

Follower Growth: Line chart with time series data
Engagement Metrics: Bar chart with impressions, clicks, likes, etc.
Engagement Breakdown: Doughnut chart for engagement distribution

Step 3: Raw Data Display
Shows formatted JSON of all API responses for debugging/transparency
6. Error Handling Flow
Backend Error Handling

HTTP Policies: Automatic retry on transient failures
Circuit Breaker: Prevents cascading failures
Exception Propagation: Unhandled errors become HTTP 500 responses

Frontend Error Handling
typescripterror: (e) => {
  this.error = e?.message ?? 'Failed to load analytics';
}

Shows error card with retry button
User can click "Retry" to reload data

7. Configuration Management
LinkedIn Settings (appsettings.json)
json{
  "LinkedIn": {
    "ApiBaseUrl": "https://api.linkedin.com/v2",
    "OrganizationId": "",           // LinkedIn Company/Organization ID
    "AccessToken": "",              // OAuth 2.0 Bearer Token
    "Mock": true                    // Toggle between mock/real data
  }
}
Environment-Specific Config

Development: Mock mode enabled, detailed logging
Production: Real LinkedIn API integration required
