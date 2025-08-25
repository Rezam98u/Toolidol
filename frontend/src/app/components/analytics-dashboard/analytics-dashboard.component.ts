import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { forkJoin, Subject, takeUntil } from 'rxjs';
import { LinkedInAnalyticsService } from '../../services/linkedin-analytics.service';

interface AnalyticsData {
  pageViews: any;
  followers: any;
  engagement: any;
}

interface DateRange {
  label: string;
  days: number;
}

@Component({
  selector: 'app-analytics-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
  ],
  templateUrl: './analytics-dashboard.component.html',
  styleUrls: ['./analytics-dashboard.component.scss']
})
export class AnalyticsDashboardComponent implements OnInit, OnDestroy {
  private readonly api = inject(LinkedInAnalyticsService);
  private destroy$ = new Subject<void>();

  // Data
  analyticsData?: AnalyticsData;
  loading = true;
  error?: string;

  // Theme
  isDarkTheme = false;

  // Date filters
  selectedDateRange = 7;
  dateRanges: DateRange[] = [
    { label: 'Last 7 days', days: 7 },
    { label: 'Last 30 days', days: 30 },
    { label: 'Last 90 days', days: 90 }
  ];

  // Chart configurations
  followerGrowthChart: any = {
    type: 'line',
    data: {
      labels: [],
      datasets: [{
        data: [],
        label: 'Followers',
        borderColor: '#1976d2',
        backgroundColor: 'rgba(25, 118, 210, 0.1)',
        tension: 0.4,
        fill: true
      }]
    },
    options: {
      responsive: true,
      plugins: {
        legend: { display: false },
        title: { display: false }
      },
      scales: {
        y: { beginAtZero: true }
      }
    }
  };

  engagementChart: any = {
    type: 'bar',
    data: {
      labels: ['Impressions', 'Clicks', 'Likes', 'Comments', 'Shares'],
      datasets: [{
        data: [],
        backgroundColor: [
          '#1976d2',
          '#388e3c',
          '#f57c00',
          '#7b1fa2',
          '#d32f2f'
        ]
      }]
    },
    options: {
      responsive: true,
      plugins: {
        legend: { display: false }
      },
      scales: {
        y: { beginAtZero: true }
      }
    }
  };

  engagementBreakdownChart: any = {
    type: 'doughnut',
    data: {
      labels: ['Likes', 'Comments', 'Shares'],
      datasets: [{
        data: [],
        backgroundColor: ['#4caf50', '#2196f3', '#ff9800'],
        borderWidth: 0
      }]
    },
    options: {
      responsive: true,
      plugins: {
        legend: { position: 'bottom' }
      }
    }
  };

  ngOnInit(): void {
    this.loadAnalytics();
    this.applyTheme();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadAnalytics(): void {
    this.loading = true;
    this.error = undefined;

    forkJoin({
      pageViews: this.api.getPageViews(),
      followers: this.api.getFollowers(),
      engagement: this.api.getEngagement()
    })
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (results) => {
        this.analyticsData = results;
        this.updateCharts();
      },
      error: (e) => {
        this.error = e?.message ?? 'Failed to load analytics';
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  retry(): void {
    this.loadAnalytics();
  }

  toggleTheme(): void {
    this.isDarkTheme = !this.isDarkTheme;
    this.applyTheme();
  }

  onDateRangeChange(): void {
    // In a real app, you'd refetch data with the new date range
    // For now, we'll just update the charts with existing data
    this.updateCharts();
  }

  private applyTheme(): void {
    document.body.classList.toggle('dark-theme', this.isDarkTheme);
  }

  private updateCharts(): void {
    if (!this.analyticsData) return;

    // Update follower growth chart
    const followerData = this.analyticsData.followers?.elements?.[0];
    if (followerData) {
      this.followerGrowthChart.data.labels = this.generateDateLabels(this.selectedDateRange);
      this.followerGrowthChart.data.datasets[0].data = this.generateFollowerData(followerData.followerCounts);
    }

    // Update engagement chart
    const engagementData = this.analyticsData.engagement?.elements?.[0];
    if (engagementData) {
      const stats = engagementData.totalShareStatistics;
      this.engagementChart.data.datasets[0].data = [
        stats.impressionCount,
        stats.clickCount,
        stats.likeCount,
        stats.commentCount,
        stats.shareCount
      ];
    }

    // Update engagement breakdown chart
    if (engagementData) {
      const stats = engagementData.totalShareStatistics;
      const total = stats.likeCount + stats.commentCount + stats.shareCount;
      this.engagementBreakdownChart.data.datasets[0].data = [
        stats.likeCount,
        stats.commentCount,
        stats.shareCount
      ];
    }
  }

  private generateDateLabels(days: number): string[] {
    const labels = [];
    for (let i = days - 1; i >= 0; i--) {
      const date = new Date();
      date.setDate(date.getDate() - i);
      labels.push(date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' }));
    }
    return labels;
  }

  private generateFollowerData(totalFollowers: number): number[] {
    // Generate realistic follower growth data
    const data = [];
    let current = totalFollowers - (Math.random() * 100);
    for (let i = 0; i < this.selectedDateRange; i++) {
      current += Math.random() * 20 - 5; // Random growth/loss
      data.push(Math.max(0, Math.round(current)));
    }
    return data;
  }

  // Helper methods for template
  formatNumber(value: number): string {
    if (value >= 1000000) {
      return (value / 1000000).toFixed(1) + 'M';
    } else if (value >= 1000) {
      return (value / 1000).toFixed(1) + 'K';
    }
    return value.toString();
  }

  formatPercentage(value: number): string {
    return (value * 100).toFixed(1) + '%';
  }

  getPageViews(): number {
    return this.analyticsData?.pageViews?.elements?.[0]?.pageViews || 0;
  }

  getUniqueVisitors(): number {
    return this.analyticsData?.pageViews?.elements?.[0]?.uniquePageViews || 0;
  }

  getTotalFollowers(): number {
    return this.analyticsData?.followers?.elements?.[0]?.followerCounts || 0;
  }

  getFollowersGained(): number {
    return this.analyticsData?.followers?.elements?.[0]?.followerGains || 0;
  }

  getFollowersLost(): number {
    return this.analyticsData?.followers?.elements?.[0]?.followerLosses || 0;
  }

  getEngagementRate(): number {
    return this.analyticsData?.engagement?.elements?.[0]?.totalShareStatistics?.engagement || 0;
  }

  getImpressions(): number {
    return this.analyticsData?.engagement?.elements?.[0]?.totalShareStatistics?.impressionCount || 0;
  }

  getClicks(): number {
    return this.analyticsData?.engagement?.elements?.[0]?.totalShareStatistics?.clickCount || 0;
  }

  getBarWidth(value: number): number {
    const maxValue = Math.max(...this.engagementChart.data.datasets[0].data);
    return maxValue > 0 ? (value / maxValue) * 100 : 0;
  }
}
