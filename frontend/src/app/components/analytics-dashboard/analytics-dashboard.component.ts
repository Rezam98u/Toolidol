import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LinkedInAnalyticsService } from '../../services/linkedin-analytics.service';
import { forkJoin } from 'rxjs';

@Component({
	selector: 'app-analytics-dashboard',
	standalone: true,
	imports: [CommonModule],
	templateUrl: './analytics-dashboard.component.html',
	styleUrls: ['./analytics-dashboard.component.scss']
})
export class AnalyticsDashboardComponent implements OnInit {
	private readonly api = inject(LinkedInAnalyticsService);

	pageViews?: any;
	followers?: any;
	engagement?: any;
	loading = true;
	error?: string;

	ngOnInit(): void {
		forkJoin([
			this.api.getPageViews(),
			this.api.getFollowers(),
			this.api.getEngagement()
		]).subscribe({
			next: ([pv, fl, en]) => {
				this.pageViews = pv;
				this.followers = fl;
				this.engagement = en;
			},
			error: (e) => {
				this.error = e?.message ?? 'Failed to load analytics';
				this.loading = false;
			},
			complete: () => {
				this.loading = false;
			}
		});
	}
}
