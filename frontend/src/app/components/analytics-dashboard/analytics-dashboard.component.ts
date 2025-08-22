import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { LinkedInAnalyticsService } from '../../services/linkedin-analytics.service';

@Component({
	selector: 'app-analytics-dashboard',
	standalone: true,
	imports: [CommonModule, HttpClientModule],
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
		Promise.all([
			this.api.getPageViews().toPromise(),
			this.api.getFollowers().toPromise(),
			this.api.getEngagement().toPromise()
		])
			.then(([pv, fl, en]) => {
				this.pageViews = pv;
				this.followers = fl;
				this.engagement = en;
			})
			.catch((e) => {
				this.error = e?.message ?? 'Failed to load analytics';
			})
			.finally(() => {
				this.loading = false;
			});
	}
}
